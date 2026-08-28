# Reinstalling Xserver.IoT.Docker

This guide describes how to completely remove and reinstall the Xserver.IoT.Docker environment.

## 1. Stop and Remove Containers

```bash
sudo docker rm -f xserveriotdocker-prod
sudo docker rm -f xserveriotdocker-update
sudo docker rm -f xserveriotdocker-update-watchtower
```

## 2. Remove Docker Images

```bash
sudo docker rmi -f iotserverdocker.azurecr.io/xserveriotdocker:prod
sudo docker rmi -f iotserverdocker.azurecr.io/xserveriotdocker-update:latest
sudo docker rmi -f nickfedor/watchtower:latest
```

## 3. Clean Docker Cache and Dangling Resources

```bash
sudo docker system prune -a -f
```

## 4. Verify Cleanup

Check that the containers and images have been removed.

```bash
sudo docker ps -a
sudo docker images
```

If the Xserver.IoT.Docker images are no longer listed, the cleanup was successful.

## 5. Reinstall Xserver.IoT.Docker

Run the installation script again:

```bash
sudo bash ./install.sh
```

## 6. Verify Installation

After installation, verify that the containers are running correctly.

```bash
docker ps
docker images
```

The following containers should normally be running:

- `xserveriotdocker-prod`
- `xserveriotdocker-update`
- `xserveriotdocker-update-watchtower`
