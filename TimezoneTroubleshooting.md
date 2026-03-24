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

expected:

```
               Local time: Tue 2026-03-24 11:04:30 CET
           Universal time: Tue 2026-03-24 10:04:30 UTC
                 RTC time: Tue 2026-03-24 10:04:30
                Time zone: Europe/Budapest (CET, +0100)
System clock synchronized: yes
              NTP service: active
          RTC in local TZ: no
```


```bash
cat /etc/timezone
```




