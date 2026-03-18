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

This command removes all Docker images stored locally on the system.

```bash
sudo docker rmi -f $(sudo docker images -aq)
```

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

