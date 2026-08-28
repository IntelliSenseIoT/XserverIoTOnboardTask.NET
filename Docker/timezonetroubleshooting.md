# Docker Container Timezone Troubleshooting Guide

## Overview
This guide ensures that Docker containers use the correct timezone based on the host system.  
It helps avoid issues with logs, reports, and time-based processing.

---

## Goal
Ensure that:
- Host system timezone is correct  
- Container inherits the correct timezone  
- No mismatch between host and container time  

---

## Configure Host Timezone

Set the correct timezone on the host:

```bash
sudo timedatectl set-timezone Europe/Budapest
```

## Update the system timezone configuration file

```bash
echo "Europe/Budapest" | sudo tee /etc/timezone
```

Note: Some systems do not automatically update `/etc/timezone`, which can lead to inconsistencies for applications relying on this file.

## Restart Container

```bash
 sudo docker restart xserveriotdocker-prod
```

## Verify Host Configuration

```bash
timedatectl
```

Expected:

```
               Local time: Tue 2026-03-24 11:04:30 CET
           Universal time: Tue 2026-03-24 10:04:30 UTC
                 RTC time: Tue 2026-03-24 10:04:30
                Time zone: Europe/Budapest (CET, +0100)
System clock synchronized: yes
              NTP service: active
          RTC in local TZ: no
```

Check timezone file:

```bash
cat /etc/timezone
```

Expected:

```
Europe/Budapest
```

## Verify Inside Container

```bash
sudo docker exec -it xserveriotdocker-prod date
```

Expected:

```
Tue Mar 24 10:19:38 CET 2026
```

```bash
sudo docker exec -it xserveriotdocker-prod cat /etc/timezone
```

Expected:

```
Europe/Budapest
```

## Best Practice

Use the host as the single source of truth  

Configure the timezone only on the host system  

Let the container inherit the timezone via mounted files  

Avoid modifying the timezone inside the container  

Container-level changes are not persistent and will be lost after updates or container recreation  

Avoid hardcoding the timezone inside the container
