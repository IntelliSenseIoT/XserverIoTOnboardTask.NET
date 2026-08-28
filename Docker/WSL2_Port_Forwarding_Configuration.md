# WSL Port Forwarding Configuration for Xserver.IoT.Docker

## Overview

In WSL-based Docker environments, it is common that Xserver.IoT.Docker services:

- work correctly from the local machine,
- but are not accessible from other devices on the same network.

This issue typically affects:

- IoT Explorer connections
- Modbus TCP/IP communication
- SSH access

The root cause is usually:
- missing or incorrect Windows `portproxy` configuration,
- or the dynamic IP address used by WSL.

---

# Typical Symptoms

## Working

- Localhost connection
- IoT Explorer on the same PC
- Docker containers are running

## Not Working

- IoT Explorer from another PC
- SSH access
- Modbus TCP/IP communication
- Remote access from LAN devices

---

# Root Cause

The Windows host only listens on localhost (`127.0.0.1`).

Example of incorrect configuration:

```powershell
netstat -ano | findstr :8001
```

Output:

```text
TCP    127.0.0.1:8001    LISTENING
```

This means:
- the service is only accessible locally,
- and not from the network.

Correct configuration:

```text
TCP    0.0.0.0:8001    LISTENING
```

This allows network access from other devices.

---

# Get Current WSL IP Address

In the WSL terminal:

```bash
hostname -I
```

Example:

```text
172.21.235.82
```

---

# Configure Port Forwarding

Run the following commands in an **Administrator PowerShell** window.

## SSH (22)

```powershell
netsh interface portproxy add v4tov4 listenaddress=0.0.0.0 listenport=22 connectaddress=172.21.235.82 connectport=22
```

## Modbus TCP/IP (502)

```powershell
netsh interface portproxy add v4tov4 listenaddress=0.0.0.0 listenport=502 connectaddress=172.21.235.82 connectport=502
```

## IoT Explorer Ports

### 8001

```powershell
netsh interface portproxy add v4tov4 listenaddress=0.0.0.0 listenport=8001 connectaddress=172.21.235.82 connectport=8001
```

### 8002

```powershell
netsh interface portproxy add v4tov4 listenaddress=0.0.0.0 listenport=8002 connectaddress=172.21.235.82 connectport=8002
```

### 8003

```powershell
netsh interface portproxy add v4tov4 listenaddress=0.0.0.0 listenport=8003 connectaddress=172.21.235.82 connectport=8003
```

---

# Firewall Configuration

Also verify the Windows Firewall settings and, if necessary, allow the required inbound ports for network communication.

---


# Verify Portproxy Configuration

```powershell
netsh interface portproxy show all
```

Example output:

```text
Listen on ipv4:             Connect to ipv4:

Address         Port        Address         Port
--------------- ----------  --------------- ----------
0.0.0.0         22          172.21.235.82  22
0.0.0.0         502         172.21.235.82  502
0.0.0.0         8001        172.21.235.82  8001
0.0.0.0         8002        172.21.235.82  8002
0.0.0.0         8003        172.21.235.82  8003
```

---

# Verify Listening Ports

```powershell
netstat -ano | findstr :8001
```

Correct result:

```text
TCP    0.0.0.0:8001    LISTENING
```

---

# Test Network Connectivity

From another computer on the same network:

```powershell
Test-NetConnection WINDOWS_HOST_IP -Port 8001
```

Example:

```powershell
Test-NetConnection 192.168.1.50 -Port 8001
```

Successful result:

```text
TcpTestSucceeded : True
```

---

# Verify SSH Service in WSL

In the WSL terminal:

```bash
sudo systemctl status ssh
```

Start SSH service:

```bash
sudo systemctl start ssh
sudo systemctl enable ssh
```

---

# Important Note About WSL

The WSL IP address may change after reboot.

Because of this:
- the `portproxy` rules remain,
- but may point to an outdated IP address.

In this case:
- localhost connections still work,
- but network access fails.

---

# Recommended Long-Term Solution

WSL is an excellent solution for:
- development,
- testing,
- demonstrations.

However, for industrial or 24/7 runtime environments, the following are recommended:

- Ubuntu Linux host
- Hyper-V virtual machine
- dedicated Linux server

These provide more stable networking for industrial IoT applications.
