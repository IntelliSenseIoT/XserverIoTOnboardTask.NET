# WSL and SSH Installation on Windows 11

## 1. Installing WSL on Windows 11

Open PowerShell as Administrator:

``` powershell
wsl --install
```

This installs WSL2 and the default Ubuntu distribution.\
After installation, restart your computer.

Start WSL:

``` powershell
wsl
```

------------------------------------------------------------------------

## 2. Installing SSH Server in WSL

Check if SSH is already installed:

``` bash
which sshd
```

If it is not installed:

``` bash
sudo apt update
sudo apt install openssh-server
```

------------------------------------------------------------------------

## 3. Starting the SSH Service

``` bash
sudo service ssh start
```

Check service status:

``` bash
sudo service ssh status
```

If the status shows **active (running)**, the SSH server is operational.

------------------------------------------------------------------------

## 4. Retrieving the Docker / WSL IP Address

``` bash
ip addr
```

The `inet` address under the `eth0` interface is the current WSL/Docker
IP address.

------------------------------------------------------------------------

## 5. Configuring Port Forwarding on Windows

Run PowerShell as Administrator:

``` powershell
netsh interface portproxy add v4tov4 listenaddress=0.0.0.0 listenport=8001 connectaddress=DOCKER_IP connectport=8001
netsh interface portproxy add v4tov4 listenaddress=0.0.0.0 listenport=8002 connectaddress=DOCKER_IP connectport=8002
netsh interface portproxy add v4tov4 listenaddress=0.0.0.0 listenport=8003 connectaddress=DOCKER_IP connectport=8003
netsh interface portproxy add v4tov4 listenaddress=0.0.0.0 listenport=502  connectaddress=DOCKER_IP connectport=502
```

⚠ Replace `DOCKER_IP` with the actual IP address retrieved from the
`ip addr` command under the `eth0` interface.

------------------------------------------------------------------------

## 6. Verify Firewall Settings

Ensure that the following TCP ports are allowed in Windows Firewall:

```
ComPort - 8001
DataPort - 8002
CorePort - 8003
UDPPort - 8004 (Server)
RemoteUDPPort - 8005
OnboardTaskPort1 - 8006
OnboardTaskPort2 - 8007
OnboardTaskPort3 - 8008
OnboardTaskPort4 - 8009
Modbus TCP/IP - 502
SSH - 22
```

##Power Management Recommendation (WSL Environment)

When running Xserver.IoT.Docker under WSL, the host machine’s sleep mode must be disabled. If the system enters sleep mode, running services will stop, and after resume the WSL system clock may become unsynchronized. This can prevent secure TLS connections to Azure IoT Hub due to time drift. Continuous operation requires disabling sleep mode and ensuring proper time synchronization.

Path: Windows Defender Firewall → Advanced Settings → Inbound Rules

If necessary, create new inbound rules to allow these ports.
