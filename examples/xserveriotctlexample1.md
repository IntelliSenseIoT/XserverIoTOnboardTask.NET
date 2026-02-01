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
