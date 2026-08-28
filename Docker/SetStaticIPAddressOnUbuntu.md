# Set Static IP Address on Ubuntu (Netplan)

This guide shows how to configure a static IP address on Ubuntu using Netplan.

## Identify Network Interface

```bash
ip a
```

## Check Existing Netplan Configuration

```bash
ls -l /etc/netplan/
cat /etc/netplan/*.yaml
```

If 50-cloud-init.yaml exists, it may override your settings.

## Create / Modify Netplan Configuration

```bash
sudo nano /etc/netplan/01-netcfg.yaml
```

Example configuration:

```
network:
  version: 2
  ethernets:
    eth0:
      dhcp4: no
      addresses:
        - 192.168.1.100/24
      routes:
        - to: default
          via: 192.168.1.1
      nameservers:
        addresses:
          - 8.8.8.8
          - 1.1.1.1
```
Replace eth0 (ens32), IP address, gateway, and DNS values as needed.

## Disable Cloud-Init Network Configuration (Recommended)

```bash
sudo nano /etc/cloud/cloud.cfg.d/99-disable-network-config.cfg
```

Add:

```
network: {config: disabled}
```

Rename cloud-init netplan file:

```bash
sudo mv /etc/netplan/50-cloud-init.yaml /etc/netplan/50-cloud-init.yaml.bak
```

## Apply Configuration

```bash
sudo netplan generate
sudo netplan apply
```

## Verify

```bash
ip a
ip route
```
