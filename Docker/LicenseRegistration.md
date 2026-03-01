# Xserver.IoT.Docker – License Registration

After installation, Xserver.IoT.Docker requires license registration – including the Free Tier version.

---

## License Types

### Free Tier
- Intended for evaluation and pilot use
- Must be manually renewed annually

### Enterprise Subscription
- Automatically renewed, provided the system has the required outbound network connectivity
- Network requirements are described in detail in the System Requirements section


For detailed information about licensing options, commercial terms, technical eligibility, and enterprise deployment scenarios, please visit:

**https://www.intellisense-iot.com**

or contact our support team directly at:

**helpdesk@intellisense-iot.com**

---

## XserverIoTCtl Documentation

Detailed usage instructions for `xserveriotctl` are available at: [more details](https://github.com/IntelliSenseIoT/XserverIoTOnboardTask.NET/blob/main/xserveriotctl.md)

---

# License Activation Steps

## 1️⃣ Retrieve the Device ID

First, retrieve the Xserver.IoT.Docker Device ID.  
This identifier is required for license registration.

```bash
xserveriotctl system deviceid get
```

---

## 2️⃣ Install the License

After receiving the license file via email:

1. Save the `LicenseCode.txt` file locally on the machine where `xserveriotctl` is available.
2. Import the license using the following command:

```bash
xserveriotctl system licences add --file "d:\LicenseCode.txt"
```

If the operation is successful, the following message will appear:

```
The license import was successful.
```

---

## 3️⃣ Verify the License

To confirm the active license:

```bash
xserveriotctl system licences show
```

---

## 4️⃣ Restart Xserver.IoT.Docker

After successful license installation, restart Xserver.IoT.Docker to apply the changes.

### Industrial PC / Native Linux Installation (Recommended)

If Xserver.IoT.Docker is running on a dedicated Industrial PC or native Linux host, the recommended method is to restart the entire IoT Server using the CLI:

```bash
xserveriotctl core restart
```

### WSL (Windows Subsystem for Linux) Development Environment

If Xserver.IoT.Docker is running under WSL (for development purposes), restarting the Docker container is usually sufficient.

If the development version is installed (non-final builds):

```bash
sudo docker restart xserveriotdocker-dev
```

If the production (released) version is installed:

```bash
sudo docker restart xserveriotdocker-prod
```

Container restart is practical in WSL environments where Docker Desktop manages the runtime.

### Result

After restart, the license becomes active.
