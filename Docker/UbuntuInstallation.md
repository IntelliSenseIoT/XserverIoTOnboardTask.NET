# Ubuntu Installation Guide

## Overview

This guide describes how to install Ubuntu Server on a new machine and deploy the **Xserver.IoT.Docker** platform.

The process includes:

- Creating a bootable USB installer  
- Installing Ubuntu Server  

---

## 1. Prepare Bootable USB Drive

Download the Ubuntu Server image from the official website:

🔗 https://ubuntu.com/download/server

Select:

```text
ubuntu-24.04.4-live-server-amd64.iso
```

Use a tool like balenaEtcher to create a bootable USB drive.

Steps:
  1. Open balenaEtcher
  2. Click Flash from file
  3. Select the downloaded ISO file
  4. Select your USB drive
  5. Click Flash


## 2. Install Ubuntu Server

- Insert the USB drive into the target PC
- Boot from USB (BIOS/UEFI boot menu)  
- Start the Ubuntu installation

### Follow the installer steps:

- Select language and keyboard layout
- Configure storage (default is fine in most cases)
- Create a user and password
- Complete installation

### After installation:

- Remove the USB drive
- Reboot the system

