## XserverIoTCtl Introduction

**xserveriotctl** is a cross-platform command-line interface (CLI) tool designed to configure, manage, and operate **Xserver.IoT** systems programmatically.

It provides direct access to the services of Xserver.IoT devices through authenticated REST APIs, without using the graphical user interface.

### Typical use cases

The CLI is especially useful in scenarios where **automation, repeatability, and scale** are required, such as:

- **Scripting and automation**
  - Integrating Xserver.IoT operations into shell scripts, PowerShell, Python, or CI/CD pipelines
  - Automated health checks, configuration updates, or status queries

- **Mass IoT Server configuration**
  - Applying identical configurations to multiple Xserver.IoT devices
  - Rolling out user, source, gateway, or dataset changes consistently across sites

- **Bulk backup and restore**
  - Creating backups from multiple IoT servers
  - Restoring devices in a standardized and repeatable way

- **Firmware and application management**
  - Downloading and installing firmware versions
  - Managing Onboard Applications (download, install, start, stop) at scale

- **Cloud and local operations**
  - Managing devices connected via **Azure IoT Hub**
  - Operating local or isolated Xserver.IoT installations
  - Controlling **Xserver.IoT.Docker** instances in development or test environments

### Target audience

`xserveriotctl` is primarily intended for:

- **Advanced users**
- **System integrators**
- **Automation engineers**
- **DevOps / IT administrators**

who are familiar with:
- Command-line tools
- Networking concepts
- IoT system operation
- Script-based workflows

For day-to-day operation or single-device configuration, the graphical user interface may be more convenient.  
For **advanced workflows and large-scale operations**, `xserveriotctl` is the recommended tool.

## XserverIoTCtl – Download & Installation

###Download

The latest prebuilt binaries are available for the following platforms:
- Linux x64: [XserverIoTctl_1.0.5._linux-x64.zip](https://1drv.ms/u/c/506260ab1001870b/IQCjTRPBie79QLmUVGGnLdP6ARpQhqgHeJX4JlG56Rk-qE0?e=izkOjT)
- Windows x64: [XserverIoTctl_1.0.5_win-x64.zip](https://1drv.ms/u/c/506260ab1001870b/IQAeG7896EruTKeocQj9e1xyAaDMGd6D4Ow7zjhVM58QK3o?e=vmUDEk)

## XserverIoTCtl – Download & Installation

### Download

The latest prebuilt binaries are available for the following platforms:
- Linux x64: [XserverIoTctl_1.0.5._linux-x64.zip](https://1drv.ms/u/c/506260ab1001870b/IQCjTRPBie79QLmUVGGnLdP6ARpQhqgHeJX4JlG56Rk-qE0?e=izkOjT)
- Windows x64: [XserverIoTctl_1.0.5_win-x64.zip](https://1drv.ms/u/c/506260ab1001870b/IQAeG7896EruTKeocQj9e1xyAaDMGd6D4Ow7zjhVM58QK3o?e=vmUDEk)

### Installation on Linux (x64)

The Linux version is provided as a ZIP archive that contains the executable and all required components.
Extract the archive: unzip xserveriotctl-linux-x64.zip
Grant execute permission to the CLI: chmod +x xserveriotctl

**Note**: Only the xserveriotctl binary requires execute permission.
The included DLL files are loaded automatically at runtime.

### Run the CLI

./xserveriotctl

Running the command without parameters prints the full help output, listing all available commands and subcommands.

### Installation on Windows (x64)

The Windows version is also distributed as a ZIP archive.

- Extract xserveriotctl-win-x64.zip
- The extracted folder contains: xserveriotctl.exe and required .dll files
- No installation is required
- Run from the extracted folder or add it to PATH

xserveriotctl
