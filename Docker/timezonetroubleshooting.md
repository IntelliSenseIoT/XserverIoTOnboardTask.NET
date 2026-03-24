# Docker Container Timezone Troubleshooting Guide

## Overview
This guide ensures that Docker containers use the correct timezone based on the host system.  
Helps avoid issues with logs, reports, and time-based processing.

---

## Goal
Ensure that:
- Host system timezone is correct  
- Container inherits the correct timezone  
- No mismatch between host and container time  

---

## 1. Configure Host Timezone

Set the correct timezone on the host:

```bash
sudo timedatectl set-timezone Europe/Budapest
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

## Restart Container

```bash
 sudo docker restart xserveriotdocker-prod
```

## Verify Inside Container

```bash
sudo docker exec -it xserveriotdocker-prod date
```

Expected:

```
Tue Mar 24 10:19:38 CET 2026
```

## Best Practice

Use host as single source of truth

Configure timezone only on host

Let container inherit via mounted files

Avoid hardcoding timezone inside container
