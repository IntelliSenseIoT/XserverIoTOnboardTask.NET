# Examples

## Factory IP Migration to Project Network

This example demonstrates how to migrate an **Xserver.IoT device from its factory default IP address** (typically `10.10.10.10`) to a **project-specific network IP range**.

The factory IP setup is the **first commissioning step** for a new IoT Server, making the device reachable on the target network and enabling further configuration via `xserveriotctl`.

The script performs the following actions:
- Sets a new static IP address, subnet mask, and gateway using the `factoryip` command
- Waits briefly for the network configuration to take effect
- Prepares the device for further configuration (profile setup)

This workflow is commonly used during **initial device onboarding**, **new project setup**, or **bulk IoT Server deployments**.

> **Note:**  
> After the IP change and waiting period, the IoT Server should be restarted to ensure all services run with the new network settings.

```
    # ---------------------------------------------
    # Xserver.IoT factory IP + initial config script
    # ---------------------------------------------
    
    # Path to xserveriotctl executable
    $xserverIoTCtl = "C:\Tools\xserveriotctl\xserveriotctl.exe"
    
    # --- Configuration parameters ---
    $ProfileName  = "newiot"          # New profile for a new project
    $NewIpAddress = "10.29.2.160"
    $SubnetMask   = "255.255.0.0"
    $Gateway      = "10.29.2.1"
    $UserName     = "admin"
    $Password     = "admin"
    
    # ---------------------------------------------
    # Apply factory IP configuration
    # ---------------------------------------------
    
    Write-Host "Setting factory IP on Xserver.IoT device..."
    Write-Host "New IP     : $NewIpAddress"
    Write-Host "Subnet     : $SubnetMask"
    Write-Host "Gateway    : $Gateway"
    
    & $xserveriotctl core factoryip `
        --newip $NewIpAddress `
        --subnet $SubnetMask `
        --gateway $Gateway
    
    Write-Host "Factory IP configuration command sent."
    Write-Host "Waiting 5 seconds for network settings to take effect..."
    
    Start-Sleep -Seconds 5
    
    # ---------------------------------------------
    # Create initial configuration profile
    # ---------------------------------------------
    
    Write-Host "Creating initial xserveriotctl configuration profile '$ProfileName'..."
    
    & $xserveriotctl config set `
        --profile $ProfileName `
        --local `
        --ip $NewIpAddress `
        --user $UserName `
        --pass $Password
    
    # ---------------------------------------------
    # Final note
    # ---------------------------------------------
    
    Write-Host ""
    Write-Host "--------------------------------------------------"
    Write-Host "5 seconds elapsed."
    Write-Host "Please restart the IoT Server now to ensure"
    Write-Host "all new settings are fully applied."
    Write-Host "--------------------------------------------------"
```

---

## Waiting for Xserver IoT Server startup using xserveriotctl

PowerShell example script that waits until the Xserver IoT Server is fully started.
The script uses **xserveriotctl system waitservices** to check the status of COM, DATA, and CORE services.
If all services are running, the IoT Server is considered operational.

Useful in automated setup, provisioning, and deployment workflows where subsequent steps depend on a fully initialized IoT Server.

```
# ---------------------------------------------
# Wait until IoT Server services are running
# ---------------------------------------------

$xserverIoTCtl = "C:\Tools\xserveriotctl\xserveriotctl.exe"
$TimeoutSec = 120

Write-Host "Waiting for IoT Server services (timeout: $TimeoutSec s)..."

$output = & $xserverIoTCtl system waitservices --timeout $TimeoutSec 2>&1

# Print CLI output
$output | ForEach-Object { Write-Host $_ }

# Check success condition
if ($output -match "All services are running")
{
    Write-Host "IoT Server is up and running." -ForegroundColor Green
    exit 0
}
else
{
    Write-Error "IoT Server did not start properly within timeout."
    exit 1
}
```

---

## Get User Group ID from Xserver IoT Server using xserveriotctl

PowerShell example script that retrieves all user groups from the Xserver IoT Server using xserveriotctl, parses the JSON output, and extracts the ID of a specific user group by name (e.g. IoTExplorerWebPortal).

Useful for automation scenarios where user group IDs are required for further configuration or user management tasks.

```
$xserverIoTCtl = "C:\Tools\xserveriotctl\xserveriotctl.exe"

$json = & $xserverIoTCtl data usergroup getall | ConvertFrom-Json

$IoTExplorerWebPortalGroupId = (
    $json | Where-Object { $_.Name -eq "IoTExplorerWebPortal" }
).Id

Write-Host "IoTExplorerWebPortal Group Id = $IoTExplorerWebPortalGroupId"
```

---

## Create WebUsers group + webuser account on Xserver IoT (PowerShell)

This PowerShell example demonstrates how to provision an **Xserver IoT Server** using `xserveriotctl` and JSON templates in a fully automated and repeatable way. The script configures project-level settings, creates a user group, assigns alarm group permissions, and creates a user associated with the newly created group.

## Workflow

1. **Select IoT Server profile**  
   The script selects the target IoT Server instance using `xserveriotctl config use <profile>`.

2. **Wait for IoT Server readiness**  
   The script waits until all required services (**COM, DATA, CORE**) are running using `xserveriotctl system waitservices --timeout <seconds>`.

3. **Configure ProjectInfo from JSON template**  
   Project-related settings are written into `projectinfo.json` using `json set`, then applied to the IoT Server.

4. **Create a User Group**  
   A new user group (for example **WebUsers**) is configured via `usergroup.json` and created using the `data usergroup applynew` command.

5. **Resolve User Group ID**  
   The script queries all existing user groups and extracts the ID of the newly created group, which is required for subsequent configuration steps.

6. **Assign Alarm Group to User Group**  
   The user group is linked to an alarm group using `usergroupalarmgroup.json` and the appropriate CLI command.

7. **Create a User and assign it to the User Group**  
   A user account is created from `user.json`. The password is encrypted using the `--encrypt` option, and the user is assigned to the previously created user group.

## Result

After the script finishes, the IoT Server contains a configured ProjectInfo, a new user group (for example **WebUsers**), a user account assigned to this group, and alarm group permissions correctly applied. This example is intended for automated provisioning, onboarding, and reproducible IoT Server setup workflows.

```
# (Recommended) Force UTF-8 for proper accented characters
$utf8 = New-Object System.Text.UTF8Encoding($false)
[Console]::OutputEncoding = $utf8
$OutputEncoding = $utf8

#-----------------------------------------------------------------------
# ----------------- IoT Server Settings (edit these) -------------------
#-----------------------------------------------------------------------

$TimeoutSec = 120

# ---- ProjectInfo values ----
$ProjectId              = 1
$ProjectName            = "Your Project Name"
$Namespace              = "Your Namespace"
$IoTGatewayName         = "Xserver.IoT"
$ProjectCreatorCompany  = "Your Company Name"
$ProjectDescription     = "Your Description"
$ProjectCreationDateTimeLT = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ss")
# ---- UserGroup values ----
$UserGroupName		= "WebUsers"
$UserGroupDescription	= "IoT Explorer Web Portal usergroup"
$UserGroupCanControl	= $true
# ---- UserGroup values ----
$UserName		= "webuser"
$UserDescription	= "IoT Explorer Web Portal user"
$UserPassword		= "webuser"
$UserEmail		= "webuser@email.com"
#-----------------------------------------------------------------------

# Path to xserveriotctl executable (adjust if needed)
$xserverIoTCtl = "C:\Tools\xserveriotctl\xserveriotctl.exe"

# ---- Select profile ----
$ProfileName = "newiot"

# ---- Templates ----
$TemplatesDir = Join-Path -Path (Get-Location) -ChildPath "templates"
$ProjectInfoFile = Join-Path -Path $TemplatesDir -ChildPath "projectinfo.json"
$ComSettingsFile = Join-Path -Path $TemplatesDir -ChildPath "comsettings.json"
$DataSettingsFile = Join-Path -Path $TemplatesDir -ChildPath "datasettings.json"
$UserGroupFile = Join-Path -Path $TemplatesDir -ChildPath "usergroup.json"
$UserGroupAlarmGroupFile = Join-Path -Path $TemplatesDir -ChildPath "usergroupalarmgroup.json"
$UserFile = Join-Path -Path $TemplatesDir -ChildPath "user.json"

Write-Host "Using profile: $ProfileName"
& $xserverIoTCtl config use $ProfileName | Out-Null

# ---- Waiting for IoT Server services ----
Write-Host "Waiting for IoT Server services (timeout: $TimeoutSec s)..."
$output = & $xserverIoTCtl system waitservices --timeout $TimeoutSec 2>&1

# Print CLI output
$output | ForEach-Object { Write-Host $_ }

# Check success condition
if ($output -match "All services are running")
{
    Write-Host "IoT Server is up and running." -ForegroundColor Green
}
else
{
    Write-Error "IoT Server did not start properly within timeout."
    exit 1
}

Write-Host "Filling projectinfo.json using xserveriotctl json set..."
& $xserverIoTCtl json set --file $ProjectInfoFile --path Id --value 1 | Out-Null
& $xserverIoTCtl json set --file $ProjectInfoFile --path ProjectName --value $ProjectName | Out-Null
& $xserverIoTCtl json set --file $ProjectInfoFile --path Namespace --value $Namespace | Out-Null
& $xserverIoTCtl json set --file $ProjectInfoFile --path IoTGatewayName --value $IoTGatewayName | Out-Null
& $xserverIoTCtl json set --file $ProjectInfoFile --path ProjectCreatorCompany --value $ProjectCreatorCompany | Out-Null
& $xserverIoTCtl json set --file $ProjectInfoFile --path ProjectDescription --value $ProjectDescription | Out-Null
& $xserverIoTCtl json set --file $ProjectInfoFile --path ProjectCreationDateTimeLT --value $ProjectCreationDateTimeLT | Out-Null

Write-Host "Applying projectinfo settings from JSON template..."
& $xserverIoTCtl data projectinfo apply --file $ProjectInfoFile

Write-Host "Filling usergroup.json using xserveriotctl json set..."
& $xserverIoTCtl json set --file $UserGroupFile --path Name --value $UserGroupName | Out-Null
& $xserverIoTCtl json set --file $UserGroupFile --path Description --value $UserGroupDescription | Out-Null
& $xserverIoTCtl json set --file $UserGroupFile --path CanControl --value $UserGroupCanControl | Out-Null

Write-Host "Applying usergroup settings from JSON template..."
& $xserverIoTCtl data usergroup applynew --file $UserGroupFile

$json = & $xserverIoTCtl data usergroup getall | ConvertFrom-Json
$IoTExplorerWebPortalGroupId = (
    $json | Where-Object { $_.Name -eq "WebUsers" }
).Id
Write-Host "IoTExplorerWebPortal Group Id = $IoTExplorerWebPortalGroupId"

Write-Host "Filling usergroupalarmgroup.json using xserveriotctl json set..."
& $xserverIoTCtl json set --file $UserGroupAlarmGroupFile --path UserGroupId --value $IoTExplorerWebPortalGroupId | Out-Null
& $xserverIoTCtl json set --file $UserGroupAlarmGroupFile --path AlarmGroupId --value 1 | Out-Null

Write-Host "Applying usergroupalarmgroup settings from JSON template..."
& $xserverIoTCtl data usergroupalarmgroup add --file $UserGroupAlarmGroupFile

Write-Host "Filling user.json using xserveriotctl json set..."
& $xserverIoTCtl json set --file $UserFile --path Name --value $UserName | Out-Null
& $xserverIoTCtl json set --file $UserFile --path Description --value $UserDescription | Out-Null
& $xserverIoTCtl json set --file $UserFile --path Password --value $UserPassword --encrypt | Out-Null
& $xserverIoTCtl json set --file $UserFile --path Email --value $UserEmail | Out-Null
& $xserverIoTCtl json set --file $UserFile --path UserGroupId --value $IoTExplorerWebPortalGroupId | Out-Null

Write-Host "Applying user settings from JSON template..."
& $xserverIoTCtl data user applynew --file $UserFile

Write-Host "Done."
```

---

## Read Xserver Core application version using xserveriotctl

This PowerShell example retrieves the Xserver Core application information using xserveriotctl, extracts the ApplicationVersion value from the JSON response, and trims the trailing version segment to obtain a clean semantic version string (e.g. 11.2.48).

The script is useful for version checks, compatibility validation, and automated provisioning workflows.

```
$xserverIoTCtl = "C:\Tools\xserveriotctl\xserveriotctl.exe"

$json = & $xserverIoTCtl core appinfo | ConvertFrom-Json

$CoreVersion = $json.ApplicationVersion -replace '\.0$',''

Write-Host "Core version = $CoreVersion"
```

---

## Download and read firmware version information

This PowerShell example triggers the download of the latest firmware information from the Xserver IoT Server using xserveriotctl, waits briefly to ensure the download is completed, and then queries the firmware information.
The script extracts the firmware Version field from the returned JSON response and stores it in a PowerShell variable.

This example is useful for automated firmware checks, version validation, and provisioning workflows.

```
$xserverIoTCtl = "C:\Tools\xserveriotctl\xserveriotctl.exe"

Write-Host "Starting firmware info download..."
& $xserverIoTCtl core downloadfirmwareinfo latest | Out-Null

Write-Host "Waiting 5 seconds to ensure download is completed..."
Start-Sleep -Seconds 5

Write-Host "Querying firmware info..."
$json = & $xserverIoTCtl core firmwareinfo | ConvertFrom-Json

$FirmwareVersion = $json.Version

Write-Host "Firmware version = $FirmwareVersion"
```

---

## Change the password of the currently logged-in user

This PowerShell example demonstrates how to change the password of the currently logged-in user on an Xserver IoT Server using xserveriotctl.

The script selects the active profile, applies the new password via the data user changepassword command, and validates the operation result.

```
# ---------------------------------------------
# Change current user password on IoT Server
# ---------------------------------------------

$xserverIoTCtl = "C:\Tools\xserveriotctl\xserveriotctl.exe"

# Profile to use
$ProfileName = "newiot"

# New password
$NewPassword = "NewStrongPassword123!"

if ([string]::IsNullOrWhiteSpace($NewPassword)) {
    Write-Error "New password is not set."
    exit 1
}

Write-Host "Using profile: $ProfileName"
& $xserverIoTCtl config use $ProfileName | Out-Null

Write-Host "Changing password for the currently logged-in user..."
& $xserverIoTCtl data user changepassword --password $NewPassword

if ($LASTEXITCODE -ne 0) {
    Write-Error "Password change failed."
    exit 1
}

Write-Host "Password changed successfully." -ForegroundColor Green
```
