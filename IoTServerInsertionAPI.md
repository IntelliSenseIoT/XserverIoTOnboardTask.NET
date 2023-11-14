# IoTServerInsertionAPI.NET to Xserver.IoT.200

IoTServerInsertionAPI.NET helps to easily create custom interfaces to IoT Server for various services (Google Cloud, AWS, IBM Cloud, My SQL, Oracle, REST API services, etc.).<br>

IoTServerInsertionAPI.NET is an add-on to the OnboardTask that makes it easier to connect to different interfaces. [More details about OnboardTask, Click here!](https://github.com/IntelliSenseIoT/XserverIoTOnboardTask.NET/blob/main/OnboardTask%20Architecture%20Overview.md)<br>

The IoTServerInsertionAPI.NET includes:<br>

    -	Read real-time data to cache in memory (LogPuffer)
    -	Save data from LogPuffer to Onboard Storage. (if the connection is not available)
    -	Read back data from Onboard Storage for sending (if the connection works again)
    -	Objects and methods needed to manage data.
<br>

![](/images/IoTServerInsertionAPI.png)

## Prerequisites

- XServerIoTOnboardTaskProject, minimum version: 11.0.1
- Required Xserver.IoT firmware, minimum version: 11.0.29
- Required IoT Explorer, minimum version: 11.0.20

## IoTServerInsertionAPI.NET.LogHelpers

### Logging class

Properties:

    /// Prefetch realtime data (seconds) Default value: 45, Range: 30-55
    public int PreReadSecond

    /// Maximum reading density (minutes) Default value: 1, Range: 1-60 - Do not use more frequent data.
    public int DensityMaxFreq

    /// One communication package size Default value: 100, Range: 1-10000 - Specifies the number of items during data transmission.
    public int PackageSize

    /// Value rounding Default value: 4, Range: 0-10
    public int ValueRounding

Methods:

    /// Reads Realtime values to LogPuffer
    public async Task<Result> ReadRealtimeValues(Realtime FieldDevices)

    /// Add items to LogPuffer    
    public void AddLogPufferItems(List<LogPuffer.LogItem> NewItems)

    /// Returns the number of items in the LogPuffer.
    public int LogPufferCount()

    ///Deletes all items.
    public void ClearLogPuffer()

    /// Gets LogPuffer cloneobject
    public List<LogPuffer.LogItem> GetLogPuffer()

    /// Deletes items from LogPuffer  
    public void DeleteLogPuffer(List<LogPuffer.LogItem> LogItems)

    /// LogPuffer to Onboard storage - Copies the item number specified in PackageSize from the LogPuffer to Onboard storage. LogPuffer is in memory, OnboardStorage is in storage.
    public async Task<Result> LogToOnboardStorage()

    /// Gets the Onboard Storage log file for sending (if error return null)  
    public async Task<IStorage> GetOnboardStorageLogFile()

    /// Gets OnboardStorage logfile number  
    public async Task<int> GetOnboardStorageLogFileNumber()

    /// Deletes a logfile from Onboard storage   
    public async Task<Result> DeleteOnboardStorage(string StorageName)

[Using IoTServerInsertionAPI with Xserver.IoT.100](https://github.com/IntelliSenseIoT/XserverIoTOnboardTask.github.io/blob/master/IoTServerInsertionAPI.md)

## Solutions with IoTServerInsertionAPI:

- [XSERVER.IOT DEXMA EM Platform Connectivity Guide](https://1drv.ms/b/s!AguHARCrYGJQghYI5IXXuLxc5kfy?e=4maIbk)
