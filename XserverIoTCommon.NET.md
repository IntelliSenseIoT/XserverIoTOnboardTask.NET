# Authentication class:

    // Log in to Xserver.IoT
    public static async Task<Result> Login(string UserName, string Password, string ServiceIP = "localhost")

    example: var res = await Authentication.Login("operator", "operator", "192.168.2.154");

    // Gets UserId object 
    public static Models.Com.Common.IUserId GetComServiceUserId()

    example:

    IActiveAlarms AlarmRequest = new IActiveAlarms();
        
    AlarmRequest.IUserId = Authentication.GetComServiceUserId();
    AlarmRequest.NumberOfItems = 0; //No Limit

    var resultackalarm = await RestAPI.RestClientPOST("/com/alarms/getactivealarms", ServiceName.Com, AlarmRequest);

# Services methods:

    /// Gets Data service status
    public static async Task<ResultStatus> DataIsInitialized()

    /// Gets Com service status   
    public static async Task<ResultStatus> ComIsInitialized()

    /// Gets Core service status    
    public static async Task<ResultStatus> CoreIsInitialized()

# ProjectInfo:

Properties:

    //Project Information (Project name, Namespace, IoT Device name, Installer company, Description, Creation & Modification date)
    public static SystemDB.Model.ProjectInfo MyProject { get; internal set; } = new SystemDB.Model.ProjectInfo();

Methods:

    // Gets my project information  
    public static async Task<Result> GetProjectInfo()

# RestAPI methods (for Data, Com, Core services):

    Use Xserver.IoT.Connectivity.Interface class REST API methods.
    More technical details are in the Xserver.IoT.Connectivity.Interface documentation.

# RestAPI methods for External services:

    //Important!!! - This code is same as IO.RestClient, Cause of copy is static HttpClient client object.

Properties:

    /// Authentication Username
    public string Username { get; set; }
    /// Authentication Password
    public string Password { get; set; }
    /// Relative or absolute Uri
    public string uriString { get; set; } 
    /// Connection close (Default value = true)
    public bool ConnectionClose { get; set; }

Methods:

    /// Initialize RestClient
    public void RestClientInitialize()

    /// Send a GET request.
    public async Task<Result> RestClientGET(string RequestURI)

    /// Send a PUT request.
    public async Task<Result> RestClientPUT(string RequestURI, object objectcontent)

    /// Send a POST request.
    public async Task<Result> RestClientPOST(string RequestURI, object objectcontent)

# Realtime class and methods:

    public List<ISourceInfo> ListOfSources { get; internal set; }
    public List<ISourceQuantitiesInfo> ListOfQuantities { get; internal set; }

    //Uploads ListOfSources and ListOfQuantities objects from Xserver.Com service
    public async Task<Result> GetSourcesQuantities()

    //Gets SourceId and QuantityId (error return value null)
    public QuantityInfo GetIds(string SourceName, string QuantityName)

    // Gets Source properties (if error or SourceId is missing return null)   
    public async Task<Source> GetSourceProperties (Int16 SourceId)

    // Gets TemplateDevice properties (if error or TemplateDeviceId is missing return null)    
    public async Task<TemplateDevice> GetTemplateDeviceProperties(int TemplateDeviceId)

    // Gets TemplateDevice quantities properties (if error or TemplateDeviceId is missing return null)    
    public async Task<List<TemplateQuantity>> GetTemplateDeviceQuantitiesProperties(int TemplateDeviceId)

    //Gets value of the quantity of the Source (error return value null)
    public async Task<QuantityValueItem> GetValue(string SourceName, string QuantityName)

    //Gets values of the quantities of the Sources (error return value null)
    public async Task<List<QuantityValueItem>> GetValues(List<QuantitiesRequestItem> QuantitiesRequestList)

    //Writes value of the quantity of the Source (error return value null)
    public async Task<QuantityWriteResult> WriteValue(string SourceName, string QuantityName, double WriteValue)

    /// Adds new values to PeriodLog
    public async Task<Result> PeriodicLogAddNewValues(List<LogItem> LogItems)

    /// Adds new values to DifferenceLog    
    public async Task<Result> DifferenceLogAddNewValues(List<EventItem> LogItems)

# EventLogging methods:

    //Adds a new event into the Onboard EventLog
    public static async Task<bool> AddLogMessage(MessageType MessageType, string Message)

# HttpRestServerService methods: 

    /// If true then REST HTTP server is running
    public bool IsStartHttpServer { get; set; }

    /// Start and Initialize Http server
    public async Task<IO.SimpleHttpServer.Result> HttpRESTServerStart()

    /// Stop Http server   
    public async Task<IO.SimpleHttpServer.Result> HttpRESTServerStop()

    /// Send response to client 
    public async Task<IO.SimpleHttpServer.Result> ServerResponse(HTTPStatusCodes HTTPStatus, Windows.Storage.Streams.IOutputStream OStream, string SendData)

# OnboardTask methods:

    /// Gets Onboard Task config    
    public static async Task<Result> GetConfig()
    
    /// Gets Onboard Task properties
    public static async Task<Result> GetProperties()
     
    /// Saves new onboard task config to Onboard Storage
    public static async Task<Result> SaveConfig(string NewConfig)
    
    /// Saves new onboard task properties to Onboard Storage
    public static async Task<Result> SaveProperties(string NewProperties)

# DeviceTwin methods:

    /// Gets Desired properties of Device Twin
    public static async Task<ResultDesiredProperties> GetDesiredProperties()
    
    /// Gets Reported properties of Device Twin
    public static async Task<ResultReportedProperties> GetReportedProperties()
    
    /// Saves new ReportedProperties
    public static async Task<Result> SaveReportedProperties(List<DeviceTwinProperty> NewReportedProperties)

# Blob storage methods:

    /// Get BlobStorage connection info
    public static async Task<ResultBlobStorage> GetConnectionInfo()

# SQLInfo methods:

    /// Get SQL server connection info
    public static async Task<ResultSQL> GetConnectionInfo()

# Serial Port methods:

    /// Gets Serial Port settings  
    public static async Task<ResultSerialPortSettings> GetSettings()

# SMTPInfo methods:

    /// Gets SMTP Settings
    public static async Task<ResultSMTPSettings> GetSMTPSettings()

# TCPIP methods:

    /// Ping command
    public static async Task<Result> Ping(string IPaddress)
