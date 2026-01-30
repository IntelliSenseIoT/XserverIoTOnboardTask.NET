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

---

## XserverIoTCtl – Download & Installation

### Download

The latest prebuilt binaries are available for the following platforms:
- Linux x64: [XserverIoTctl_1.0.5._linux-x64.zip](https://1drv.ms/u/c/506260ab1001870b/IQCjTRPBie79QLmUVGGnLdP6ARpQhqgHeJX4JlG56Rk-qE0?e=izkOjT)
- Windows x64: [XserverIoTctl_1.0.5_win-x64.zip](https://1drv.ms/u/c/506260ab1001870b/IQAeG7896EruTKeocQj9e1xyAaDMGd6D4Ow7zjhVM58QK3o?e=vmUDEk)

### Installation on Linux (x64)

The Linux version is provided as a ZIP archive that contains the executable and all required components.

Extract the archive: **unzip xserveriotctl-linux-x64.zip**

Grant execute permission to the CLI: **chmod +x xserveriotctl**

**Note**: Only the xserveriotctl binary requires execute permission.
The included DLL files are loaded automatically at runtime.

### Run the CLI

**./xserveriotctl**

Running the command without parameters prints the full help output, listing all available commands and subcommands.

### Installation on Windows (x64)

The Windows version is also distributed as a ZIP archive.

- Extract **xserveriotctl-win-x64.zip**
- The extracted folder contains: xserveriotctl.exe and required .dll files
- No installation is required
- Run from the extracted folder or add it to PATH

**xserveriotctl**

---

# xserveriotctl – Command Reference

## Usage:
  xserveriotctl <command> [subcommand] [options | args]

  Config commands:

    config                       - Manage connection profiles (local/cloud)
      list                       - list profiles
      show [name]                - show profile details (defaults to current default)
      set --profile <name> (--local|--remote) [options]
      probe [name]               - detect device type via Core
      use <name>                 - set default profile
      delete <name>              - delete profile
      rename <from> <to>         - rename profile
      path                       - print config.json path
      init                       - create sample local/cloud profiles

## xserveriotctl – Config Command Examples

### List profiles
```
xserveriotctl config list
```
Example output:
```
local
cloud
docker
```

### Show profile details

```
xserveriotctl config show
```

```
xserveriotctl config show cloud
```

The profile configuration is printed as formatted JSON.


### Create or update a local profile

```
xserveriotctl config set   --profile local   --local   --ip 192.168.1.100   --user admin   --pass yourpassword
```

Typical use cases:
- On-site commissioning
- Local maintenance
- Offline or isolated networks


### Create or update a cloud profile (Azure IoT Hub)

```bash
xserveriotctl config set   --profile cloud   --remote   --device-id MyIoTDevice01   --iothub "HostName=my-iothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=REDACTED_KEY"   --user admin   --pass yourpassword
```

Typical use cases:
- Remote access to IoT Servers
- Fleet-wide configuration
- Cloud-based automation

### Create a local Docker (WSL) profile

```
xserveriotctl config set   --profile docker   --local   --ip 127.0.0.1   --user admin   --pass yourpassword
```

### Detect device type (probe)

```
xserveriotctl config probe
```

```
xserveriotctl config probe cloud
```

The detected device type is stored in the profile and used automatically
for device-specific Core commands.

### Set the default profile

```
xserveriotctl config use local
```

### Delete a profile

```
xserveriotctl config delete docker
```

### Rename a profile

```
xserveriotctl config rename local onsite
```

### Show configuration file path

```
xserveriotctl config path
```

### Initialize sample profiles

```
xserveriotctl config init
```

### Recommended workflow

```
xserveriotctl config init
xserveriotctl config set --profile local --local --ip 192.168.1.100 --user admin --pass admin
xserveriotctl config probe
xserveriotctl config use local
```

After these steps, xserveriotctl is ready for use with all Core, Com, Data,
and System commands.

---

## Other commands

```
    com                          - The Com service is responsible for communication with field devices (Modbus RTU, Modbus TCP/IP, and related protocols).
      status                     - Com service status
      appinfo                    - Com service information
      settings                   - Com configuration settings
      updatesources              - Update Sources configuration in Com service
      updatevirtualsources       - Update Virtual Sources configuration in Com service
      reinit-communication       - Reinitializes device communication (Serial and TCP/IP) on the active IoT Server.
      events                     - Com diagnostic events
      eventscheck                - Check Com events for 'Exception error', returns true if none found, false otherwise
      serialcominfo              - Serial (Modbus RTU) communication stats
      tcpipcominfo               - TCP/IP (Modbus TCP) communication stats
      sources                    - List all sources
      quantities                 - List source quantities
      alarmstat                  - Active alarm statistics
      getactivealarms            - List all active alarms
      getvalue                   - Read a realtime value by source and quantity
      writevalue                 - Write a realtime value (double) to a quantity
      virtualsources             - List all Virtual Sources
```

```
    data                         - The Data service handles configuration, and historical datasets.
      status                     - Data service status
      appinfo                    - Data application information
      settings                   - Dataset configuration
      update                     - Update Data service settings
      events                     - Diagnostic events
      eventscheck                - Check Data events for 'Exception error', returns true if none found, false otherwise
      useractivities             - List user activity log
      usersupdate                - Applies user and user group changes by updating user configuration in Core and Com services.
      registertypes              - Register type definitions
      scaleregistertypes         - Scaled register type definitions
      backup
          create --out <file>.IoTBackup   - Create IoT Server backup (prints progress to console)
      restore 
          run --file <file>.IoTBackup [--profile <name>] [--yes]      - Restore IoT Server configuration and data from a backup file.

        Options:
        --file <file>      Path to the .IoTBackup file
        --profile <name>   IoT Server profile to use (default: active profile)
        --yes              Skip confirmation prompts

        Notes:
        - Only admin user can perform restore
        - Existing configuration and data will be removed
        - User passwords will be requested during restore

    comsettings       
        get --out <file>            - Download ComSettings from Data service to JSON file
        template --out <file>       - Save ComSettings template to JSON file
        apply --file <file>         - Apply ComSettings from JSON file. Note: After modifying the ComSettings, the Com service must be restarted.

    datasettings              
        get --out <file>            - Download DataSettings to JSON file
        template --out <file>       - Save DataSettings template to JSON file
        apply --file <file>         - Apply DataSettings from JSON file. Note: After applying DataSettings, run 'xserveriotctl data update' to activate the changes. 

    usergroup
        get --out <file> --id <id>  - Download UserGroup to JSON file
        getall                      - List user groups (prints JSON to console)
        alarmgroups --id <id>       - List AlarmGroups assigned to a UserGroup (prints JSON to console)
        template --out <file>       - Save UserGroup template to JSON file
        applynew --file <file>      - Apply a new UserGroup from JSON file. Note: After applying, run 'xserveriotctl data usersupdate' to activate the changes.
        applyupdate --file <file>   - Apply UserGroup from JSON file. Note: After applying, run 'xserveriotctl data usersupdate' to activate the changes. 
        remove --id <id>            - Remove an existing UserGroup from the system. Note: After removing, run 'xserveriotctl data usersupdate' to activate the changes. 

    user
        get --out <file> --id <id>  - Download User to JSON file
        getall                      - List users (prints JSON to console)
        getallwithgroups            - Display all user settings grouped by UserGroups (prints JSON to console).
        template --out <file>       - Save User template to JSON file
        applynew --file <file>      - Apply a new User from JSON file. Note: After applying, run 'xserveriotctl data usersupdate' to activate the changes.
        applyupdate --file <file>   - Apply User from JSON file. Note: After applying, run 'xserveriotctl data usersupdate' to activate the changes.
        remove --id <id>            - Remove an existing User from the system. After removing, run 'xserveriotctl data usersupdate' to activate the changes.

    alarmgroup
        get --out <file> --id <id>    - Download AlarmGroup to JSON file
        getall                        - List alarm groups (prints JSON to console)
        template --out <file>         - Save AlarmGroup template to JSON file
        applynew --file <file>        - Apply a new AlarmGroup from JSON file. Note: After applying, run 'xserveriotctl data usersupdate' and 'xserveriotctl com updatesources' to activate the changes.
        applyupdate --file <file>     - Apply an existing AlarmGroup from JSON file. Note: After applying, run 'xserveriotctl data usersupdate' and 'xserveriotctl com updatesources' to activate the changes.
        remove --id <id>              - Remove an existing AlarmGroup from the system. Note: After removing, run 'xserveriotctl data usersupdate' and 'xserveriotctl com updatesources' to activate the changes.

    usergroupalarmgroup
        template --out <file>         - Save UsersGroupAlarmGroup template to JSON file
        add --file <file>             - Add a AlarmGroup to UserGroup mapping from JSON file. After adding, run 'xserveriotctl data usersupdate' and 'xserveriotctl com updatesources' to activate the changes.
        remove --file <file>          - Remove one UserGroup-to-AlarmGroup mapping from JSON file. 
                                        After removing, run 'xserveriotctl data usersupdate' and 'xserveriotctl com updatesources' to activate the changes.

    templatedevice
        get --out <file> --id <id>    - Download TemplateDevice to JSON file
        getall                        - List TemplateDevices (prints JSON to console)
        template --out <file>         - Save TemplateDevice template to JSON file
        applynew --file <file>        - Apply a new TemplateDevice from JSON file. Note: After applying, run 'xserveriotctl com updatesources' to activate the changes.
        applyupdate --file <file>     - Apply an existing TemplateDevice from JSON file. Note: After applying, run 'xserveriotctl com updatesources' to activate the changes.
        export --id <id> --out <file>.tempdev   - Export a TemplateDevice driver to a .tempdev file. 
        import --file <file>.tempdev            - Import a TemplateDevice driver from a .tempdev file. Note: After applying, run 'xserveriotctl com updatesources' to activate the changes.
        remove --id <id>              - Remove a TemplateDevice by ID. Note: After removing, run 'xserveriotctl com updatesources' to activate the changes.
        removeall                     - Remove all TemplateDevices. Note: After removing, run 'xserveriotctl com updatesources' to activate the changes.

    templatequantity
        get --out <file> --id <id>    - Download TemplateQuantity to JSON file
        getall --id <id>              - List TemplateQuantities of a TemplateDevice (prints JSON to console)
        template --out <file>         - Save TemplateQuantity template to JSON file
        applynew --file <file>        - Apply a new TemplateQuantity from JSON file. Note: After applying, run 'xserveriotctl com updatesources' to activate the changes.
        applyupdate --file <file>     - Apply an existing TemplateQuantity from JSON file. Note: After applying, run 'xserveriotctl com updatesources' to activate the changes.
        remove --id <id>              - Remove an existing TemplateQuantity from the system. Note: After removing, run 'xserveriotctl com updatesources' to activate the changes.
        removeall --id <id>           - Remove all TemplateQuantities of a TemplateDevice. Note: After removing, run 'xserveriotctl com updatesources' to activate the changes.

    templatealarmsetting
        get --out <file> --id <id>    - Download TemplateAlarmSetting to JSON file
        getall --id <id>              - List all TemplateAlarmSettings of a TemplateDevice (prints JSON to console)
        listall                       - List all TemplateAlarmSettings of all TemplateDevices (prints JSON to console)
        template --out <file>         - Save TemplateAlarmSetting template to JSON file
        applynew --file <file>        - Apply a new TemplateAlarmSetting from JSON file. Note: After applying, run 'xserveriotctl com updatesources' to activate the changes.
        applyupdate --file <file>     - Apply an existing TemplateAlarmSetting from JSON file. Note: After applying, run 'xserveriotctl com updatesources' to activate the changes.
        remove --id <id>              - Remove an existing TemplateAlarmSetting from the system. After removing, run 'xserveriotctl com updatesources' to activate the changes.
        removeall --id <id>           - Remove all TemplateAlarmSettings of a TemplateQuantity. After removing, run 'xserveriotctl com updatesources' to activate the changes.

    gateway
        get --out <file> --id <id>    - Download Gateway to JSON file
        getall                        - List Gateways (prints JSON to console)
        template --out <file>         - Save Gateway template to JSON file
        applynew --file <file>        - Apply a new Gateway from JSON file. Note: After applying, run 'xserveriotctl com updatesources' to activate the changes.
        applyupdate --file <file>     - Apply an existing Gateway from JSON file. Note: After applying, run 'xserveriotctl com updatesources' to activate the changes.
        remove --id <id>              - Remove an existing Gateway from the system. Note: After removing, run 'xserveriotctl com updatesources' to activate the changes.
    
    source
        get --out <file> --id <id>    - Download Source to JSON file
        getall                        - List all sources (prints JSON to console)
        template --out <file>         - Save Source template to JSON file
        applynew --file <file>        - Apply a new Source from JSON file. Note: After applying, run 'xserveriotctl com updatesources' to activate the changes.
        applyupdate --file <file>     - Apply an existing Source from JSON file. Note: After applying, run 'xserveriotctl com updatesources' to activate the changes.
        remove --id <id>              - Remove an existing Source from the system. Note: After removing, run 'xserveriotctl com updatesources' to activate the changes.
        removeall                     - Remove all Sources from the system. Note: After removing, run 'xserveriotctl com updatesources' to activate the changes.
        enable --id <id> | --name <name>    - Enable a Source (sets SourceEnabled=true) by Id or SourceName
        disable --id <id> | --name <name>   - Disable a Source (sets SourceEnabled=false) by Id or SourceName
                                              After applying, run 'xserveriotctl com updatesources' to activate the changes.

    virtualsource
        get --out <file> --id <id>    - Download Virtual Source to JSON file
        getall                        - List Virtual Sources (prints JSON to console)
        sources --id <id>             - List Sources assigned to a Virtual Source (prints JSON to console) 
        template --out <file>         - Save Virtual Source template to JSON file
        applynew --file <file>        - Apply a new Virtual Source from JSON file. 
                                        Note: After applying, run 'xserveriotctl com updatesources' and 'xserveriotctl com updatevirtualsources' to activate the changes.
        applyupdate --file <file>     - Apply an existing Virtual Source from JSON file. 
                                        Note: After applying, run 'xserveriotctl com updatesources' and 'xserveriotctl com updatevirtualsources' to activate the changes.
        remove --id <id>              - Remove an existing Virtual Source from the system. 
                                        Note: After removing, run 'xserveriotctl com updatesources' and 'xserveriotctl com updatevirtualsources' to activate the changes.
                                    
    virtualcalc                       - Used to configure calculation rules for Virtual Sources.
        template --out <file>         - Save SourcesOfVirtualSource template to JSON file
        add --file <file>             - Add a Source-to-VirtualSource mapping from JSON file. 
                                        Note: After adding, run 'xserveriotctl com updatesources' and 'xserveriotctl com updatevirtualsources' to activate the changes.
        remove --file <file>          - Remove one Source-to-VirtualSource mapping from JSON file. 
                                        Note: After removing, run 'xserveriotctl com updatesources' and 'xserveriotctl com updatevirtualsources' to activate the changes.

    systememail                       - Configuration of Maintenance Users responsible for receiving system alerts and maintenance notifications.
        get --out <file> --id <id>    - Download SystemEmail to JSON file
        getall                        - List all SystemEmails (prints JSON to console)
        template --out <file>         - Save SystemEmail template to JSON file
        applynew --file <file>        - Apply a new SystemEmail from JSON file. Note: After adding, the Com service must be restarted.
        applyupdate --file <file>     - Apply an existing SystemEmail from JSON file. Note: After modifying, the Com service must be restarted.
        remove --id <id>              - Remove an existing SystemEmail from the system. Note: After removing, the Com service must be restarted.

    projectinfo
        get --out <file>              - Download ProjectInfo to JSON file
        show                          - Project information (prints JSON to console)
        template --out <file>         - Save ProjectInfo template to JSON file
        apply --file <file>           - Save and update ProjectInfo from JSON file

    onboardtaskconfig
        get --out <file>              - Download Config to JSON file
        show                          - Config (prints JSON to console)
        apply --file <file>           - Save and update Config from JSON file

    onboardtaskproperties
        get --out <file>              - Download Properties to JSON file
        show                          - Properties (prints JSON to console)
        apply --file <file>           - Save and update Properties from JSON file

    heartbeat 
        get                           - Show current heartbeat interval (prints to console)
        set --minutes <minutes>       - Set heartbeat interval in minutes

    json                      - JSON utility commands
      set --file <file>
          --path <propertyPath>
          --value <value>
          [--encrypt]                   - Update a JSON property (optionally encrypt value)
```

```
    core                      - Core service commands (system, network, diagnostics, device information)
     Common (Xserver.IoT.200 + Xserver.IoT.Docker)
       status                  - Core service status
       appinfo                 - Core application information
       events                  - Diagnostic event list
       systeminfo              - System information (device name, OS version, platform, device ID)
       memoryinfo              - System memory information
       processinfo             - System process information (Core, Com, Data services)
       timezone                - Current system timezone
       supportedtimezones      - List supported timezones
       timeinfo                - System time information
       firmwareinfo            - Firmware information
       downloadedapps          - Downloaded Onboard applications
       services                - List core system services and Onboard applications on Xserver.IoT.200; Onboard applications only on Xserver.IoT.Docker
       onboardapps             - List all Onboard applications
       onboardappsinfo         - Onboard application details
       interfaces              - Network interface information
       updateblobsettings      - Apply and update Blob storage settings originating from the data service
       downloadfirmwareinfo [latest|preview|previous] - Download firmware info (latest/preview on Docker, latest/previous/preview on Xserver.IoT.200)
       appstorelist            - List available applications from AppStore (downloads the AppStore content)
       downloadappfromstore <TaskName>  - Download an Onboard application from the store by task name
       downloadapp <TaskName>  - Send download request for an Onboard application from the user's Blob Storage 
                                 (requires Data Service configuration: BlobContainerName and StorageConnectionString)
       checkapp <TaskName>     - Check whether an Onboard App task exists or is available on the device
       installapp <TaskName>   - Send install request for an Onboard application
       uninstallapp <TaskName> - Send uninstall request for an Onboard application (the application must be in stopped state)
       installedappinfo <TaskName>  - Get package information for an Onboard application (name, version, description, release date)
       runapp <TaskName>       - Send run request for an Onboard application
       stopapp <TaskName>      - Send stop request for an Onboard application
       deleteinstallerfiles    - Send request to delete Onboard application installer files from the IoT Server
       ifaceinfo <interface>   - Query network interface settings (DHCP/static, IP/subnet, gateway/DNS, WiFi SSID, MAC)
       ping <ip|hostname>      - Send ICMP ping request from the IoT Server to the specified target
       restart [delaySeconds] [--silent]  - Restart the IoT Server after the specified delay (default: 5s); --silent skips confirmation
       shutdown [delaySeconds] - Shut down the IoT Server after the specified delay (confirmation required)
     Xserver.IoT.200 only
       freespaceinfo           - Disk free space information
       changetimezone <name>   - Change system timezone (restart required)
       setdatetime <yyyy-MM-dd> <hh:mm> - Set system date and time (24-hour, local) (only when NTP is disabled)
       ntpservers              - Configured NTP servers
       ntpstatus               - NTP synchronization status
       ntpenable <true|false>  - Enable or disable NTP time synchronization (restart required)
       setntpservers <servers> - Set NTP servers (semicolon-separated list) (restart required)
       downloadfirmware [latest|previous|preview] - Download firmware to IoT device
       firmwaremanagerstatus   - Check whether a firmware-related process is running (download in progress)
       checkfirmware           - Verify that the downloaded firmware is complete and not corrupted before installation
       installfirmware [--factory] [--silent]  - Install firmware (preserve configuration by default; 
                                                 --factory resets; 
                                                 --silent skips confirmation for preserve configuration)
       factoryip --newip <ip> --subnet <mask> [--gateway <gw>] [--oldip <ip>]
                               - Change device IP via UDP broadcast (default old IP: 10.10.10.10; reboot required)
       setiface <interface> dhcp|static ...    - Set network interface settings on the IoT Server (restart required for changes to take effect)
       wifiscan <interface>    - Scan available WiFi access points on the specified interface
       firewallstatus          - Firewall status
       firewall <enable|disable>        - Enable/disable firewall on the IoT device
       firewall rule <add|del> --ip <ip|Anywhere> --port <n|Anywhere> --action <allow|deny>  - Add/delete a firewall rule
     Xserver.IoT.Docker only
       docker-healthinfo       - Docker update health information
       docker-environment      - Docker environment information (0-Dev,1-Prod)
       docker-mode             - Docker update mode (0-Manual,1-Auto)
       dockerupdatemode <manual|auto>  - Set Docker update mode on the IoT Server (manual or automatic)
       docker-updateavailable  - Check if Docker update is available
```

```
    system 
      waitservices --timeout <seconds> - Wait until all IoT services (COM, DATA, CORE) are running
      licences                  - Manage licences installed on the IoT Server
          show                         - List all installed licences (prints to console)
          add --file <file>            - Import and install a licence from file. The licence must belong to the target DeviceID.
                                         Some Onboard Apps may require a restart for the licence to take effect.
          remove --product <id> --licenceid <id>   - Remove an installed licence from the IoT Server.
      deviceid get              - Prints the unique DeviceID of the IoT Server to the console.
```

### Examples:

```
      xserveriotctl config init
      xserveriotctl config set --profile local --local --ip 192.168.1.100 --user admin --pass admin
      xserveriotctl config set --profile cloud --remote --device-id IoTDevice1 --iothub "HostName=...;..." --user admin --pass admin
      xserveriotctl config probe local
      xserveriotctl config show local
      
      xserveriotctl com status
      xserveriotctl com getvalue --source "Main Energy Meter" --quantity "Active Power"
      xserveriotctl com writevalue --source "TempSensor" --quantity "SetPointValue" --value 20
      
      xserveriotctl data comsettings template --out "comsettings.json"
      xserveriotctl data comsettings get --out "comsettings.json"
      xserveriotctl json set --file "comsettings.json" --path "SerialTimeOut" --value 5000
      xserveriotctl data comsettings apply --file "comsettings.json"
      
      xserveriotctl data datasettings template --out "datasettings.json"
      xserveriotctl data datasettings get --out "datasettings.json"
      xserveriotctl data datasettings apply --file "datasettings.json"
      
      xserveriotctl data usergroup template --out "usergroup.json"
      xserveriotctl data usergroup get --out "usergroup.json" --id 16
      xserveriotctl data usergroup applynew --file "usergroup.json"
      xserveriotctl data usergroup applyupdate --file "usergroup.json"
      xserveriotctl data usergroup remove --id 16
      xserveriotctl data usersupdate
      
      xserveriotctl json set --file "datasettings.json" --path "CloudPeriodLogToCloud" --value true
      xserveriotctl json set --file "datasettings.json" --path "CloudIoTDeviceConnectionString" --value "HostName=myhub.azure-devices.net;DeviceId=IoTDevice1;SharedAccessKey=ABC123==" --encrypt
      
      xserveriotctl data backup create --out "d:\OfficeBuilding.IoTBackup"
      xserveriotctl data restore run --file "d:\OfficeBuilding.IoTBackup"
      
      xserveriotctl core setntpservers "pool.ntp.org;time.google.com;time.windows.com"
      xserveriotctl core setdatetime "2026-01-21" "21:00"
      
      xserveriotctl core setiface eth0 static 192.168.10.50 255.255.255.0 192.168.10.1 8.8.8.8 1.1.1.1
      xserveriotctl core setiface wlan0 dhcp --ssid FactoryWiFi --wifipass StrongPassword123
      
      xserveriotctl core factoryip --newip 192.168.1.50 --subnet 255.255.255.0 --gateway 192.168.1.1
      
      xserveriotctl core firewall rule del --ip Anywhere --port 443 --action allow
      xserveriotctl core firewall rule add --ip 192.168.1.10 --port Anywhere --action deny
      
      xserveriotctl system waitservices --timeout 120
```
