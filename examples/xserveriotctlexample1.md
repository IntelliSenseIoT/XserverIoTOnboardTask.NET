# Example

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

