# Xserver.IoT.Docker – Useful Commands

This section contains commonly used commands for troubleshooting, service verification in Xserver.IoT.Docker.

---

# 1. Installation & Host Actions

Run the installer from the user home directory:

sudo bash ./install.sh

Shutdown WSL (useful after config changes or network issues):

wsl --shutdown

Switch to the dedicated Docker power user (if used in your environment):

sudo -i -u XserverIoTDockerPowerUser

---

# 2. Container Management (Lifecycle)

List containers (running + stopped):

sudo docker ps -a

Restart the development container:

sudo docker restart xserveriotdocker-dev

Stop the development container:

sudo docker stop xserveriotdocker-dev

Force remove the development container:

sudo docker rm -f xserveriotdocker-dev

---

# 3. Logs & Interactive Debugging

Follow container logs in real time:

sudo docker logs -f xserveriotdocker-dev

Enter container shell as root (interactive):

sudo docker exec -u 0 -it xserveriotdocker-dev /bin/bash

---

# 4. DockerUpdate REST API (Health Check)

Health endpoint (browser or curl):

http://localhost:5261/health

curl -v http://localhost:5261/health


