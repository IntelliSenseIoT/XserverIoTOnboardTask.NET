# Xserver.IoT.Docker

## System Requirements & Network Prerequisites

Xserver.IoT.Docker is a multi-architecture industrial IoT platform
designed for lightweight edge deployment.

------------------------------------------------------------------------

## Supported Architectures

-   x64 (AMD64)
-   ARM64 (Industrial ARM IPC, Raspberry Pi 4/5)

The installer automatically deploys the appropriate image for the
detected platform.

------------------------------------------------------------------------

## Supported Operating Systems

### Recommended for Production

-   **Ubuntu Server 22.04 LTS or newer (x64 or ARM64)**

### Supported for Testing / Small Deployments

-   Windows 11 + WSL2 (Ubuntu 22.04)
-   Windows Server 2022 + WSL2 (Ubuntu 22.04)

WSL environments are recommended for development, testing, and smaller
installations.

------------------------------------------------------------------------

## Hardware Requirements

### Minimum

-   2 CPU cores
-   2--4 GB RAM
-   10 GB available storage
-   1 Ethernet interface

These requirements are comparable to the Xserver.IoT.200 hardware
platform.

### Recommended for Industrial Deployment

-   4 CPU cores
-   4 GB RAM
-   SSD storage
-   Fixed IP address

------------------------------------------------------------------------

## Internet Connectivity

Internet access is required for:

-   Firmware/version validation
-   Application updates
-   Azure IoT Hub communication
-   Azure Blob Storage access

------------------------------------------------------------------------

## External Network Requirements

Xserver.IoT.Docker requires outbound internet connectivity for cloud
communication, version validation, container deployment, and updates.

All communication is outbound only. No inbound cloud ports are required.

------------------------------------------------------------------------

### Azure IoT Hub Connectivity

Outbound access must be allowed according to Microsoft Azure IoT Hub
protocol requirements:

-   TCP 443 (HTTPS)
-   TCP 8883 (MQTT over TLS)
-   TCP 5671 (AMQP over TLS)

Reference:
https://learn.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide-protocols

------------------------------------------------------------------------

### Azure Blob Storage Access

Outbound HTTPS (TCP 443) access required to:

https://intellisenseblob.blob.core.windows.net

This endpoint is used for:

-   Version validation
-   Firmware update checks
-   Application package downloads

Depending on Azure configuration, access to `*.blob.core.windows.net`
may be required.

------------------------------------------------------------------------

### Azure Container Registry (ACR)

Outbound HTTPS (TCP 443) access required for Docker image download from
Azure Container Registry.

------------------------------------------------------------------------

### DNS & Time Synchronization

The system must have:

-   Outbound DNS access (TCP/UDP 53)
-   Proper system time synchronization (NTP or host-based sync)

Accurate system time is required for TLS-based Azure authentication and
secure communication.

------------------------------------------------------------------------

### Summary for IT / Security Review

The deployment requires:

-   Outbound HTTPS connectivity (TCP 443)
-   Outbound secure MQTT or AMQP connectivity (TCP 8883 / 5671)
-   DNS resolution capability
-   System time synchronization
-   No inbound cloud ports required

------------------------------------------------------------------------

## Local Service Ports

The following local service ports are used:

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

When running under Windows + WSL, proper port forwarding configuration
is required.

------------------------------------------------------------------------

## Infrastructure Requirements

-   Dedicated Ethernet endpoint for the IoT server
-   Outbound Azure IoT Hub access
-   Outbound Azure Blob Storage access
-   Firewall rules configured for required ports
-   Documented network configuration

------------------------------------------------------------------------

## Summary

Xserver.IoT.Docker is optimized for lightweight edge environments and
does not require high-end server hardware.

For production deployments, native Ubuntu Server 22.04 LTS or newer is
recommended.
Windows 11 and Windows Server 2022 (WSL2) are supported for development,
testing, and smaller installations.
