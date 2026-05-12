![](images/XServerIoT2025.png)

# Releasing TCP Port 502 for Modbus TCP/IP Device Simulation in WSL

When using Xserver.IoT.Docker in a WSL-based Docker environment, the Modbus TCP/IP service typically occupies TCP port `502` via Windows `portproxy` forwarding.

To simulate external Modbus TCP/IP devices locally (for example with Modbus simulators or test tools - ModSim), TCP port `502` must first be released on the Windows host system.

---

## 1. Disable the Modbus TCP/IP Server in Xserver.IoT.Docker

Using IoT Explorer, disable the Modbus TCP/IP Server service running inside Xserver.IoT.Docker.

![](images/XServerIoT2025.png)

This prevents the Docker environment from actively listening on TCP port `502`.

---

## 2. Verify Existing Port Forwarding Rules

Open **PowerShell** with Administrator privileges and execute the following command:

```powershell
netsh interface portproxy show all
```

Example output:

```text
Listen on ipv4:             Connect to ipv4:

Address         Port        Address         Port
--------------- ----------  --------------- ----------
0.0.0.0         8001        172.21.235.82   8001
0.0.0.0         8002        172.21.235.82   8002
0.0.0.0         8003        172.21.235.82   8003
0.0.0.0         502         172.21.235.82   502
```

In this example, TCP port `502` is forwarded from the Windows host to the WSL Docker environment.

---

## 3. Remove the Port 502 Forwarding Rule

Execute the following command to remove the Modbus TCP/IP forwarding rule:

```powershell
netsh interface portproxy delete v4tov4 listenport=502 listenaddress=0.0.0.0
```

---

## 4. Validate the Configuration

Run the verification command again:

```powershell
netsh interface portproxy show all
```

Expected output:

```text
Listen on ipv4:             Connect to ipv4:

Address         Port        Address         Port
--------------- ----------  --------------- ----------
0.0.0.0         8001        172.21.235.82   8001
0.0.0.0         8002        172.21.235.82   8002
0.0.0.0         8003        172.21.235.82   8003
```

The absence of the `502` forwarding rule confirms that the Modbus TCP/IP port has been released successfully.

---

## 5. Start the Modbus TCP/IP Simulator

After releasing TCP port `502`, local Modbus TCP/IP simulation tools can bind to the port and emulate external Modbus devices for development and integration testing purposes.
