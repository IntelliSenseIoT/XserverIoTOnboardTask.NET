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

-   **Ubuntu Server 22.04 LTS (x64 or ARM64)**

### Supported for Testing / Small Deployments

-   Windows 11 + WSL2 (Ubuntu 22.04)
-   Windows Server 2022 + WSL2 (Ubuntu 22.04)

WSL environments are recommended for development, testing, and smaller
installations.

------------------------------------------------------------------------

## Hardware Requirements

### Minimum

-   2 CPU cores\
-   2--4 GB RAM\
-   10 GB available storage\
-   1 Ethernet interface

These requirements are comparable to the Xserver.IoT.200 hardware
platform.

### Recommended for Industrial Deployment

-   4 CPU cores\
-   4 GB RAM\
-   SSD storage\
-   Fixed IP address

------------------------------------------------------------------------

## Internet Connectivity

Internet access is required for:

-   Docker image download\
-   Firmware/version validation\
-   Application updates\
-   Azure IoT Hub communication\
-   Azure Blob Storage access

------------------------------------------------------------------------

## External Network Requirements

### Azure IoT Hub Connectivity

Outbound access must be allowed according to Microsoft IoT Hub protocol
requirements:

-   TCP 443 (HTTPS)
-   TCP 8883 (MQTT over TLS)
-   TCP 5671 (AMQP over TLS)

Reference:\
https://learn.microsoft.com/en-us/azure/iot-hub/iot-hub-devguide-protocols

------------------------------------------------------------------------

### Azure Blob Storage Access

Outbound HTTPS (TCP 443) access required to:

-   https://intellisenseblob.blob.core.windows.net/xserveriotdockerdev/versioninfo.json\
-   https://intellisenseblob.blob.core.windows.net/xserveriotdockerlatest/versioninfo.json\
-   https://intellisenseblob.blob.core.windows.net/xserveriotdocker/

------------------------------------------------------------------------

## Local Service Ports

The following local service ports are used:

-   8001 -- Data Service\
-   8002 -- Communication Service\
-   8003 -- Core Service\
-   502 -- Modbus TCP

When running under Windows + WSL, proper port forwarding configuration
is required.

------------------------------------------------------------------------

## Infrastructure Requirements

-   Dedicated Ethernet endpoint for the IoT server\
-   Outbound Azure IoT Hub access\
-   Outbound Azure Blob Storage access\
-   Firewall rules configured for required ports\
-   Documented network configuration

------------------------------------------------------------------------

## Summary

Xserver.IoT.Docker is optimized for lightweight edge environments and
does not require high-end server hardware.

For production deployments, native Ubuntu Server 22.04 LTS is
recommended.\
Windows 11 and Windows Server 2022 (WSL2) are supported for development,
testing, and smaller installations.
