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

---

## XserverIoTCtl Documentation

Detailed usage instructions for `xserveriotctl` are available at:

link

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

If the development version is installed (non-final builds):

```bash
sudo docker restart xserveriotdocker-dev
```

If the production (released) version is installed:

```bash
sudo docker restart xserveriotdocker-prod
```

After restart, the license becomes active.
