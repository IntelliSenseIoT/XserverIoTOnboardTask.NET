#!/bin/bash

echo -e "XserverIoTDocker Firmware Installer Version: 2026-09-08"
echo -e "XserverIoTDocker Firmware installation started."

set -euo pipefail

# ========= CONFIGURATION =========
LOCAL_USER=XserverIoTDockerPowerUser
LOCAL_USER_PASSWORD=IoTAccess1234
LOCAL_HOME_PATH=/home/$LOCAL_USER

# Public Docker Hub images
DOCKER_REGISTRY=docker.io
DOCKER_NAMESPACE=intellisensexserveriot
DOCKER_IMAGE=xserver-iot
DOCKER_UPDATE_IMAGE=xserver-iot-update

DOCKER_DATABASE_PATH=/app/data/Database
DOCKER_ONBOARDTASK_PATH=/app/OnboardTask
DOCKER_FIRMWARE_PATH=/app/Firmware
SSH_HOST=localhost
SSH_PORT=22
UPDATE_API_PORT=5261
UPDATE_API_DELAY=24

# Requires root or sudo
require_root() {
  if [[ "${EUID}" -ne 0 ]]; then
    echo "This script must be run as root or with sudo." >&2
    exit 1
  fi
}

# Installs required system dependencies
install_system_dependencies() {
  export DEBIAN_FRONTEND=noninteractive

  echo "Updating package list..."
  apt-get update

  # Install net-tools (required by Xserver Core service)
  if ! command -v netstat >/dev/null 2>&1; then
    echo "Installing net-tools (netstat dependency)..."
    apt-get install -y net-tools
  else
    echo "net-tools already installed."
  fi
}

# Sets the default user and its password.
# Furthermore ensures the account has the privileges expected by the current Xserver.IoT architecture.
set_username_and_password() {
  if id -u "$LOCAL_USER" >/dev/null 2>&1; then
    echo "User '$LOCAL_USER' exists. Updating password..."
  else
    echo "User '$LOCAL_USER' does not exist. Creating..."
    if ! getent group "$LOCAL_USER" >/dev/null 2>&1; then
      groupadd "$LOCAL_USER"
    fi
    useradd -m -g "$LOCAL_USER" -s /bin/bash "$LOCAL_USER"
    echo "User '$LOCAL_USER' created."
  fi

  echo "${LOCAL_USER}:${LOCAL_USER_PASSWORD}" | chpasswd
  echo "Password set for user '$LOCAL_USER'."

  usermod -u 0 -o -g 0 "$LOCAL_USER"
  echo "User '$LOCAL_USER' is initialized"

  cp /etc/ssh/sshd_config /etc/ssh/sshd_config.bak
  sed -i -E 's/^#?PermitRootLogin .*/PermitRootLogin yes/' /etc/ssh/sshd_config
  sed -i -E 's/^#?PasswordAuthentication .*/PasswordAuthentication yes/' /etc/ssh/sshd_config
  systemctl restart ssh

  echo "SSH access is granted for user ${LOCAL_USER}"
}

# Installs Docker and Docker Compose
install_docker() {
  export DEBIAN_FRONTEND=noninteractive

  apt-get update
  apt-get install -y --no-install-recommends ca-certificates curl gnupg

  install -m 0755 -d /etc/apt/keyrings
  curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.asc
  chmod a+r /etc/apt/keyrings/docker.asc

  . /etc/os-release
  UBU_CODENAME="${UBUNTU_CODENAME:-${VERSION_CODENAME:-}}"

  echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/ubuntu ${UBU_CODENAME} stable" \
    | tee /etc/apt/sources.list.d/docker.list > /dev/null

  apt-get update
  apt-get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

  echo "Docker repository configured and Docker installed successfully."
}

# Checks whether Docker Engine and Docker Compose are installed
install_docker_if_necessary() {
  if ! command -v docker &> /dev/null; then
    install_docker
  fi

  if command -v docker-compose &> /dev/null; then
    return 0
  elif docker --help 2>/dev/null | grep -q "compose"; then
    return 0
  else
    install_docker
  fi
}

# Pulls the public Docker Hub update image.
# No docker login is required for public repositories.
pull_public_images() {
  echo "Pulling Xserver.IoT.Docker public images from Docker Hub..."
  docker pull "$DOCKER_NAMESPACE/$DOCKER_UPDATE_IMAGE:latest"
}

# Runs the docker compose of the XserverIoTDocker.Update project
run_docker_compose() {
  cd "$LOCAL_HOME_PATH"
  local COMPOSE_FILE="$LOCAL_HOME_PATH/compose.yml"

  cat > "$COMPOSE_FILE" <<EOF
services:
  xserveriotdocker-update:
    image: $DOCKER_NAMESPACE/$DOCKER_UPDATE_IMAGE:latest
    container_name: xserveriotdocker-update
    privileged: true
    network_mode: host
    restart: always
    volumes:
      - /var/run/docker.sock:/var/run/docker.sock
      - /usr/bin/docker:/usr/bin/docker
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=http://localhost:$UPDATE_API_PORT
      - DockerSocketPath=unix:///var/run/docker.sock
      - DockerRegistry=$DOCKER_REGISTRY
      - DockerRegistryAuthMode=anonymous
      - DockerImageName=$DOCKER_NAMESPACE/$DOCKER_IMAGE
      - DockerDatabaseLocalPath=$LOCAL_HOME_PATH/Database
      - DockerDatabasePath=$DOCKER_DATABASE_PATH
      - DockerOnboardtaskLocalPath=$LOCAL_HOME_PATH/OnboardTask
      - DockerOnboardtaskPath=$DOCKER_ONBOARDTASK_PATH
      - DockerFirmwareLocalPath=$LOCAL_HOME_PATH/Firmware
      - DockerFirmwarePath=$DOCKER_FIRMWARE_PATH
      - SshHost=$SSH_HOST
      - SshPort=$SSH_PORT
      - SshUsername=$LOCAL_USER
      - SshPassword=$LOCAL_USER_PASSWORD
      - UpdateMode=manual
      - UpdateDelay=$UPDATE_API_DELAY
      - UpdateApiUrl=http://localhost:$UPDATE_API_PORT
      - WatchTowerUrl=http://localhost:8080
      - WatchTowerApiKey=1234567

  xserveriotdocker-update-watchtower:
    image: nickfedor/watchtower:latest
    container_name: xserveriotdocker-update-watchtower
    privileged: true
    network_mode: host
    environment:
      - WATCHTOWER_CLEANUP=true
      - WATCHTOWER_REMOVE_VOLUMES=true
      - WATCHTOWER_LOG_FORMAT=Pretty
      - WATCHTOWER_POLL_INTERVAL=10
      - WATCHTOWER_DISABLE_CONTAINERS=xserveriotdocker-update-watchtower
      - WATCHTOWER_HTTP_API_UPDATE=true
      - WATCHTOWER_HTTP_API_TOKEN=1234567
    volumes:
      - /var/run/docker.sock:/var/run/docker.sock
    restart: always
EOF

  sed -i $'s/\t/ /g' "$COMPOSE_FILE"
  docker compose -f "$COMPOSE_FILE" up -d
}

# Wait until at least 3 containers exist (min 30s, max 4m), then ask for reboot
wait_for_finish_then_reboot() {
  echo "Waiting for the installation to be finished..."
  start_time=$(date +%s)

  while true; do
    container_count=$(docker ps -aq | wc -l)
    now=$(date +%s)
    elapsed=$((now - start_time))

    if [ "$container_count" -ge 3 ] && [ "$elapsed" -ge 30 ]; then
      break
    fi

    if [ "$elapsed" -ge 240 ]; then
      echo "Unknown error occurred"
      return 1
    fi

    sleep 2
  done

  echo
  echo "============================================================"
  echo "Xserver.IoT.Docker installation completed."
  echo "Local SSH user: $LOCAL_USER"
  echo "Local password: $LOCAL_USER_PASSWORD"
  echo "Please store this password securely."
  echo "============================================================"
  echo

  read -rp "Do you want to reboot the system now? (y/n): " answer
  if [ "$answer" = "y" ] || [ "$answer" = "Y" ]; then
    echo "Rebooting system..."
    reboot
  fi
}

# Business logic of the script
main() {
  require_root
  install_system_dependencies
  set_username_and_password
  install_docker_if_necessary
  pull_public_images
  run_docker_compose
  wait_for_finish_then_reboot
}

# ========= MAIN ENTRY POINT =========
main
