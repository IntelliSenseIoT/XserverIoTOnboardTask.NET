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
