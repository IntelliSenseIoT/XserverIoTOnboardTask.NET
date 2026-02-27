# Xserver.IoT.Docker -- Installation Guide

## Overview

Xserver.IoT.Docker is deployed using the official `install.sh` script on a supported Linux system.

The installation is performed via SSH and requires internet
connectivity.

------------------------------------------------------------------------

# 2. Upload the Installer

Copy the official `install.sh` file to the target server.

It is recommended to upload the file to the user's home directory:

```bash
/home/username/install.sh
```

File transfer can be performed using SCP or SFTP.

------------------------------------------------------------------------

# 3. Connect to the Server via SSH

After login, ensure you are in your home directory:

``` bash
cd ~
```

------------------------------------------------------------------------

# 4. Run the Installation Script

Execute the installer from the home directory:

``` bash
sudo bash ./install.sh
```

The installation script will:

-   Install required system dependencies
-   Install and configure Docker (if not already installed)
-   Pull required Docker images
-   Configure runtime environment
-   Start Xserver.IoT.Docker services

Internet connectivity is required during this process.

------------------------------------------------------------------------

# 5. Post-Installation

After installation completes:

-   Verify Docker containers are running:

``` bash
sudo docker ps
```

-   Verify service health endpoints (if required)
-   Ensure firewall and network rules are correctly configured

------------------------------------------------------------------------

# Configuration Recommendation

For full configuration and lifecycle management of Xserver.IoT.Docker,
the use of **XserverIoTCtl** is strongly recommended.

XserverIoTCtl enables complete remote configuration, system management,
and operational control without requiring any additional software
installation on the target server.

This approach ensures:

-   Clean and minimal server environment
-   Reduced attack surface
-   No additional local UI dependencies
-   Centralized administrative control

Using XserverIoTCtl aligns with enterprise security and deployment best
practices.

------------------------------------------------------------------------

# Production Best Practice

For production deployments:

-   Disable host sleep mode (if running under Windows + WSL)
-   Use static IP configuration
-   Ensure time synchronization (NTP)
-   Monitor Docker container health
-   Document firewall and network configuration
