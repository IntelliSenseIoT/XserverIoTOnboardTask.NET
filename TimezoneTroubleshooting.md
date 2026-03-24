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
