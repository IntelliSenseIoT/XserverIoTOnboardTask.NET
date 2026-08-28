using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Service.Common.NET;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using XserverIoTCommon;

namespace XserverIoTOnboardTask
{
    public static class InitializeHelpers
    {
        // Globally accessible application settings
        public static GeneralSettings generalSettings { get; set; }

        /// <summary>
        /// Initialize settings from appsettings.json
        /// </summary>
        public static void InitializeAppSettings()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // location of appsettings.json
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            generalSettings = config.GetSection("GeneralSettings").Get<GeneralSettings>();
        }

        /// <summary>
        /// Waits until all required services (Com, Data, Core) are initialized.
        /// Logs status messages during the check.
        /// </summary>
        public static async Task WaitForServicesAsync(ILogger logger,string caller,string serviceDisplayName,CancellationToken ct = default)
        {
            await EventLogging.AddLogMessage(MessageType.Info,$"{caller} - {serviceDisplayName} - Checking services...");
            logger.LogInformation("{Caller} - {Service} - Checking services...", caller, serviceDisplayName);

            while (!ct.IsCancellationRequested)
            {
                var com = await Services.ComIsInitialized();
                var data = await Services.DataIsInitialized();
                var core = await Services.CoreIsInitialized();

                if (com.Initialized && data.Initialized && core.Initialized)
                {
                    await EventLogging.AddLogMessage(MessageType.Info,$"{caller} - {serviceDisplayName} - Services are running.");
                    logger.LogInformation("{Caller} - {Service} - Services are running.", caller, serviceDisplayName);
                    break;
                }

                await Task.Delay(5000, ct);
            }
        }

        /// <summary>
        /// Attempts to login to the Xserver.IoT service until success.
        /// Logs warnings and errors during authentication attempts.
        /// </summary>
        public static async Task<bool> LoginToXserverAsync(ILogger logger,string caller,string serviceDisplayName,string? serviceip = null,CancellationToken ct = default)
        {
            bool firstLoginTried = false;

            if (string.IsNullOrEmpty(serviceip))
            {
                serviceip = "localhost";
            }

            while (!ct.IsCancellationRequested)
            {
                var res = await Authentication.Login(generalSettings.OnboardTaskLoginName,generalSettings.OnboardTaskPassword,serviceip);

                if (!res.Success && !firstLoginTried)
                {
                    logger.LogWarning("{Caller} - {Service} - Authentication error: {Error}",caller, serviceDisplayName, res.ErrorMessage);
                    await EventLogging.AddLogMessage(MessageType.Error,$"{caller} - {serviceDisplayName} - Authentication error: {res.ErrorMessage}");

                    firstLoginTried = true;
                }
                else if (res.Success)
                {
                    logger.LogInformation("{Caller} - {Service} - OnboardTask login was successful.",caller, serviceDisplayName);

                    await EventLogging.AddLogMessage(MessageType.Info,$"{caller} - {serviceDisplayName} - OnboardTask login was successful.");

                    return true;
                }

                await Task.Delay(5000, ct);
            }

            return false;
        }

        /// <summary>
        /// Repeatedly tries to fetch real-time sources and quantities until it succeeds.
        /// Logs error only on the first failure, and logs success when completed.
        /// </summary>
        public static async Task<bool> WaitForRealtimeDataAsync(ILogger logger,string caller,string serviceDisplayName,Realtime realtime,CancellationToken ct = default)
        {
            bool firstErrorLogged = false;

            while (!ct.IsCancellationRequested)
            {
                var rtResult = await realtime.GetSourcesQuantities();
                if (!rtResult.Success)
                {
                    if (!firstErrorLogged)
                    {
                        await EventLogging.AddLogMessage(MessageType.Error,$"{caller} - {serviceDisplayName} - Failed to fetch real-time data: {rtResult.ErrorMessage}");
                        logger.LogError("{Service} - Failed to fetch real-time data: {Err}",serviceDisplayName, rtResult.ErrorMessage);
                        firstErrorLogged = true;
                    }

                    await Task.Delay(5000, ct);
                }
                else
                {
                    await EventLogging.AddLogMessage(MessageType.Info,$"{caller} - {serviceDisplayName} - Real-time data fetch successful.");
                    logger.LogInformation("{Service} - Real-time data fetch successful.",serviceDisplayName);
                    return true;
                }
            }

            return false; // cancelled
        }

        private static string RemoveInvalidJsonControlChars(string s)
        => string.IsNullOrEmpty(s)
           ? s
           : Regex.Replace(s, @"[\x00-\x08\x0B\x0C\x0E-\x1F]", string.Empty);

        public static string MergeConfigIntoRootJson(string originalJson, OnboardTaskConfig cfg, bool indented = true)
        {
            try
            {
                string cleaned = RemoveInvalidJsonControlChars(originalJson);

                JsonNode root = JsonNode.Parse(cleaned) ?? new JsonObject();
                if (root is not JsonObject obj)
                    obj = new JsonObject();

                var props = typeof(OnboardTaskConfig)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite);

                foreach (var p in props)
                {
                    var value = p.GetValue(cfg);
                    obj[p.Name] = System.Text.Json.JsonSerializer.SerializeToNode(value);
                }

                return obj.ToJsonString(new JsonSerializerOptions { WriteIndented = indented });
            }
            catch (Exception)
            {
                return null;
            }  
        }

        public static string? MergePropertiesIntoRootJson(string originalJson, OnboardTaskProperties properties, bool indented = true)
        {
            try
            {
                string cleaned = RemoveInvalidJsonControlChars(originalJson);
                JsonNode root = JsonNode.Parse(cleaned) ?? throw new System.Text.Json.JsonException("Properties JSON root is null.");

                if (root is not JsonObject obj)
                    throw new System.Text.Json.JsonException("Properties JSON root must be an object.");

                var props = typeof(OnboardTaskProperties).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite);

                foreach (var property in props)
                {
                    var value = property.GetValue(properties);
                    obj[property.Name] = System.Text.Json.JsonSerializer.SerializeToNode(value);
                }

                return obj.ToJsonString(new JsonSerializerOptions { WriteIndented = indented });
            }
            catch
            {
                return null;
            }
        }

        public static bool HasMissingOnboardTaskConfigs(string json)
        {
            var targetType = typeof(OnboardTaskConfig);

            var expected = targetType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite)
                .Where(p => p.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>() == null)
                .Where(p => p.GetCustomAttribute<System.Text.Json.Serialization.JsonExtensionDataAttribute>() == null)
                .Select(p => p.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? p.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            try
            {
                using var doc = JsonDocument.Parse(json, new JsonDocumentOptions
                {
                    AllowTrailingCommas = true,
                    CommentHandling = JsonCommentHandling.Skip
                });

                if (doc.RootElement.ValueKind != JsonValueKind.Object)
                    return true;

                var present = doc.RootElement
                    .EnumerateObject()
                    .Select(p => p.Name)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                return expected.Any(name => !present.Contains(name));
            }
            catch
            {
                return true;
            }
        }

        public static bool HasMissingOnboardTaskProps(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json, new JsonDocumentOptions
                {
                    AllowTrailingCommas = true,
                    CommentHandling = JsonCommentHandling.Skip
                });

                if (doc.RootElement.ValueKind != JsonValueKind.Object)
                    return true;

                var expectedRoot = GetSerializableMemberNames(typeof(OnboardTaskProperties));
                var presentRoot = doc.RootElement
                                     .EnumerateObject()
                                     .Select(p => p.Name)
                                     .ToHashSet(StringComparer.OrdinalIgnoreCase);

                if (expectedRoot.Any(name => !presentRoot.Contains(name)))
                    return true;

                return false;
            }
            catch
            {
                return true;
            }
        }

        //Helpers
        private static HashSet<string> GetSerializableMemberNames(Type t)
        {
            var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Public instance PROPERTIES
            foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!p.CanRead) continue;
                if (p.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>() != null) continue;
                if (p.GetCustomAttribute<System.Text.Json.Serialization.JsonExtensionDataAttribute>() != null) continue;

                string name = p.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name
                              ?? p.Name;

                names.Add(name);
            }

            // Public instance FIELDS
            foreach (var f in t.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (f.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>() != null) continue;

                string name = f.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name
                              ?? f.Name;

                names.Add(name);
            }

            return names;
        }

        /// <summary>
        /// Validates and normalizes GeneralSettings values.
        /// Ensures they are within safe ranges; logs corrections if necessary.
        /// </summary>
        public static void ValidateGeneralSettings(string serviceDisplayName, ILogger? logger = null)
        {
            if (generalSettings == null)
                throw new InvalidOperationException("InitializeAppSettings() must be called before ValidateGeneralSettings().");

            // Helper: clamp with logging (Warn + EventLog) so corrections are not silent
            static int ClampWithLog(
                string fieldName,
                int value,
                int minInclusive,
                int maxInclusive,
                string serviceDisplayName,
                ILogger? logger)
            {
                int original = value;
                int clamped = Math.Clamp(value, minInclusive, maxInclusive);

                if (clamped != original)
                {
                    if (original < minInclusive)
                    {
                        logger?.LogWarning("{Svc} - {Field} too small ({Old}) → {New} (min {Min}). Save is recommended.",
                            serviceDisplayName, fieldName, original, clamped, minInclusive);
                    }
                    else
                    {
                        logger?.LogWarning("{Svc} - {Field} too large ({Old}) → {New} (max {Max}). Save is recommended.",
                            serviceDisplayName, fieldName, original, clamped, maxInclusive);
                    }

                    // Fire-and-forget EventLog to avoid changing signature to async
                    _ = EventLogging.AddLogMessage(
                        MessageType.Info,
                        $"{serviceDisplayName} - {fieldName} {original} → {clamped}; save is recommended.");
                }

                return clamped;
            }

            // Periods (ms)
            generalSettings.TaskHandlerPeriod = ClampWithLog("TaskHandlerPeriod", generalSettings.TaskHandlerPeriod, 500, 60000, serviceDisplayName, logger);
        }
    }
}
