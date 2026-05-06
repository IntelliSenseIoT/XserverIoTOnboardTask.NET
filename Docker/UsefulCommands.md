# Xserver.IoT.Docker – Useful Commands

This section contains commonly used commands for troubleshooting, service verification in Xserver.IoT.Docker.

---

# 1. Installation & Host Actions

Run the installer from the user home directory:

```bash
sudo bash ./install.sh
```

Shutdown WSL (useful after config changes or network issues):

```
wsl --shutdown
```

Switch to the dedicated Docker power user (if used in your environment):

```bash
sudo -i -u XserverIoTDockerPowerUser
```

---

# 2. Container Management (Lifecycle)

List containers (running + stopped):

```bash
sudo docker ps -a
```

Restart the development container:

```bash
sudo docker restart xserveriotdocker-dev
```

Stop the development container:

```bash
sudo docker stop xserveriotdocker-dev
```

Force remove the development container:

```bash
sudo docker rm -f xserveriotdocker-dev
```

Remove all Docker containers:

```bash
sudo docker rm -f $(sudo docker ps -aq)
```

Remove all local Docker images:

```bash
sudo docker rmi -f $(sudo docker images -aq)
```
This command removes all Docker images stored locally on the system.

Check container system time:

```bash
docker exec -it xserveriotdocker-dev date
```
---

# 3. Logs & Interactive Debugging

Follow container logs in real time:

```bash
sudo docker logs -f xserveriotdocker-dev
```

Enter container shell as root (interactive):

```bash
sudo docker exec -u 0 -it xserveriotdocker-dev /bin/bash
```

---

# 4. DockerUpdate REST API (Health Check)

Health endpoint:

```bash
curl -v http://localhost:5261/health
```

# 5. Set Static IP Address on Ubuntu (Netplan)

This guide shows how to configure a static IP address on Ubuntu using Netplan.

## Identify Network Interface

```bash
ip a
```

## Check Existing Netplan Configuration

```bash
ls -l /etc/netplan/
cat /etc/netplan/*.yaml
```

If 50-cloud-init.yaml exists, it may override your settings.

## Create / Modify Netplan Configuration

```bash
sudo nano /etc/netplan/01-netcfg.yaml
```

Example configuration:

```
network:
  version: 2
  ethernets:
    eth0:
      dhcp4: no
      addresses:
        - 192.168.1.100/24
      routes:
        - to: default
          via: 192.168.1.1
      nameservers:
        addresses:
          - 8.8.8.8
          - 1.1.1.1
```
Replace eth0 (ens32), IP address, gateway, and DNS values as needed.

## Disable Cloud-Init Network Configuration (Recommended)

```bash
sudo nano /etc/cloud/cloud.cfg.d/99-disable-network-config.cfg
```

Add:

```
network: {config: disabled}
```

Rename cloud-init netplan file:

```bash
sudo mv /etc/netplan/50-cloud-init.yaml /etc/netplan/50-cloud-init.yaml.bak
```

## Apply Configuration

```bash
sudo netplan generate
sudo netplan apply
```

## Verify

```bash
ip a
ip route
```
