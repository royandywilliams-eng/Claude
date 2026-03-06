using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectSpecGUI.Core
{
    /// <summary>
    /// Handles serialization and deserialization of ProjectConfiguration to/from JSON
    /// Supports round-trip conversion with proper handling of complex types
    /// </summary>
    public class ConfigurationSerializer
    {
        private readonly JsonSerializerOptions _jsonOptions;

        public ConfigurationSerializer()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        /// <summary>
        /// Serialize a ProjectConfiguration to a JSON string
        /// </summary>
        public string Serialize(ProjectConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var configDto = ConvertToDto(config);
            return JsonSerializer.Serialize(configDto, _jsonOptions);
        }

        /// <summary>
        /// Deserialize a JSON string to a ProjectConfiguration
        /// </summary>
        public ProjectConfiguration Deserialize(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("JSON string cannot be null or empty", nameof(json));

            try
            {
                var configDto = JsonSerializer.Deserialize<ConfigurationDto>(json, _jsonOptions);
                if (configDto == null)
                    throw new InvalidOperationException("Failed to deserialize JSON to configuration");

                var config = ConvertFromDto(configDto);

                // Validate after loading
                var validator = new ConfigurationValidator();
                var validationResult = validator.Validate(config);

                // Log validation issues but don't fail - configuration can have warnings
                if (!validationResult.IsValid && validationResult.Errors.Count > 0)
                {
                    throw new InvalidOperationException(
                        $"Configuration validation failed: {string.Join("; ", validationResult.Errors)}");
                }

                return config;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Failed to parse JSON: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Convert ProjectConfiguration to a DTO for JSON serialization
        /// </summary>
        private ConfigurationDto ConvertToDto(ProjectConfiguration config)
        {
            return new ConfigurationDto
            {
                // Wizard Data
                ProjectName = config.ProjectName,
                ProjectDescription = config.ProjectDescription,
                ProjectType = config.ProjectType,
                ComplexityLevel = config.ComplexityLevel,
                TargetAudience = config.TargetAudience,

                // Technology Stack
                PrimaryLanguage = config.PrimaryLanguage,
                Frameworks = config.Frameworks ?? new List<string>(),
                Database = config.Database,
                Libraries = config.Libraries ?? new List<string>(),

                // Key Features
                Features = config.Features ?? new Dictionary<string, bool>(),

                // UI/UX Requirements
                DesignFramework = config.DesignFramework,
                ResponsiveDesign = config.ResponsiveDesign,
                DarkModeSupport = config.DarkModeSupport,
                AccessibilityRequirements = config.AccessibilityRequirements,
                MobileCompatibility = config.MobileCompatibility,

                // Performance & Scalability
                ExpectedUsers = config.ExpectedUsers,
                RealtimeDataNeeds = config.RealtimeDataNeeds,
                CachingStrategy = config.CachingStrategy,
                CDNUsage = config.CDNUsage,
                DatabaseOptimization = config.DatabaseOptimization,

                // Deployment & DevOps
                HostingPlatform = config.HostingPlatform,
                CIPipeline = config.CIPipeline,
                UseDocker = config.UseDocker,
                UseKubernetes = config.UseKubernetes,
                MonitoringTools = config.MonitoringTools,

                // Advanced Configuration
                AdvancedConfig = config.AdvancedConfig ?? new Dictionary<string, object>(),

                // Project Metadata
                Author = config.Author,
                Version = config.Version,
                License = config.License,
                RepositoryUrl = config.RepositoryUrl,
                CreatedDate = config.CreatedDate
            };
        }

        /// <summary>
        /// Convert a DTO back to ProjectConfiguration
        /// </summary>
        private ProjectConfiguration ConvertFromDto(ConfigurationDto dto)
        {
            return new ProjectConfiguration
            {
                // Wizard Data
                ProjectName = dto.ProjectName ?? "",
                ProjectDescription = dto.ProjectDescription ?? "",
                ProjectType = dto.ProjectType ?? "",
                ComplexityLevel = dto.ComplexityLevel ?? "Medium",
                TargetAudience = dto.TargetAudience ?? "",

                // Technology Stack
                PrimaryLanguage = dto.PrimaryLanguage ?? "JavaScript",
                Frameworks = dto.Frameworks ?? new List<string>(),
                Database = dto.Database ?? "None",
                Libraries = dto.Libraries ?? new List<string>(),

                // Key Features
                Features = RestoreFeaturesFromDto(dto.Features),

                // UI/UX Requirements
                DesignFramework = dto.DesignFramework ?? "Bootstrap",
                ResponsiveDesign = dto.ResponsiveDesign,
                DarkModeSupport = dto.DarkModeSupport,
                AccessibilityRequirements = dto.AccessibilityRequirements ?? "WCAG 2.1 AA",
                MobileCompatibility = dto.MobileCompatibility,

                // Performance & Scalability
                ExpectedUsers = dto.ExpectedUsers ?? "1000",
                RealtimeDataNeeds = dto.RealtimeDataNeeds,
                CachingStrategy = dto.CachingStrategy ?? "None",
                CDNUsage = dto.CDNUsage,
                DatabaseOptimization = dto.DatabaseOptimization ?? "Standard indexes",

                // Deployment & DevOps
                HostingPlatform = dto.HostingPlatform ?? "AWS",
                CIPipeline = dto.CIPipeline ?? "GitHub Actions",
                UseDocker = dto.UseDocker,
                UseKubernetes = dto.UseKubernetes,
                MonitoringTools = dto.MonitoringTools ?? "CloudWatch",

                // Advanced Configuration
                AdvancedConfig = dto.AdvancedConfig ?? new Dictionary<string, object>(),

                // Project Metadata
                Author = dto.Author ?? "",
                Version = dto.Version ?? "1.0.0",
                License = dto.License ?? "MIT",
                RepositoryUrl = dto.RepositoryUrl ?? "",
                CreatedDate = dto.CreatedDate == default ? DateTime.Now : dto.CreatedDate
            };
        }

        /// <summary>
        /// Restore the Features dictionary from DTO
        /// Ensures all default feature keys exist even if not in JSON
        /// </summary>
        private Dictionary<string, bool> RestoreFeaturesFromDto(Dictionary<string, bool> dtoFeatures)
        {
            // Start with default features
            var features = new Dictionary<string, bool>
            {
                { "Authentication", false },
                { "Authorization", false },
                { "Database Integration", false },
                { "REST API", false },
                { "Real-time Updates", false },
                { "File Upload/Download", false },
                { "Payment Integration", false },
                { "Email/Notifications", false },
                { "Admin Dashboard", false },
                { "Analytics/Logging", false },
                { "Search Functionality", false },
                { "User Management", false }
            };

            // Update with values from DTO (if present)
            if (dtoFeatures != null)
            {
                foreach (var kvp in dtoFeatures)
                {
                    features[kvp.Key] = kvp.Value;
                }
            }

            return features;
        }
    }

    /// <summary>
    /// DTO for JSON serialization of ProjectConfiguration
    /// Provides a clean representation for JSON output
    /// </summary>
    [Serializable]
    public class ConfigurationDto
    {
        // Wizard Data
        [JsonPropertyName("projectName")]
        public string ProjectName { get; set; }

        [JsonPropertyName("projectDescription")]
        public string ProjectDescription { get; set; }

        [JsonPropertyName("projectType")]
        public string ProjectType { get; set; }

        [JsonPropertyName("complexityLevel")]
        public string ComplexityLevel { get; set; }

        [JsonPropertyName("targetAudience")]
        public string TargetAudience { get; set; }

        // Technology Stack
        [JsonPropertyName("primaryLanguage")]
        public string PrimaryLanguage { get; set; }

        [JsonPropertyName("frameworks")]
        public List<string> Frameworks { get; set; }

        [JsonPropertyName("database")]
        public string Database { get; set; }

        [JsonPropertyName("libraries")]
        public List<string> Libraries { get; set; }

        // Key Features
        [JsonPropertyName("features")]
        public Dictionary<string, bool> Features { get; set; }

        // UI/UX Requirements
        [JsonPropertyName("designFramework")]
        public string DesignFramework { get; set; }

        [JsonPropertyName("responsiveDesign")]
        public bool ResponsiveDesign { get; set; }

        [JsonPropertyName("darkModeSupport")]
        public bool DarkModeSupport { get; set; }

        [JsonPropertyName("accessibilityRequirements")]
        public string AccessibilityRequirements { get; set; }

        [JsonPropertyName("mobileCompatibility")]
        public bool MobileCompatibility { get; set; }

        // Performance & Scalability
        [JsonPropertyName("expectedUsers")]
        public string ExpectedUsers { get; set; }

        [JsonPropertyName("realtimeDataNeeds")]
        public bool RealtimeDataNeeds { get; set; }

        [JsonPropertyName("cachingStrategy")]
        public string CachingStrategy { get; set; }

        [JsonPropertyName("cdnUsage")]
        public bool CDNUsage { get; set; }

        [JsonPropertyName("databaseOptimization")]
        public string DatabaseOptimization { get; set; }

        // Deployment & DevOps
        [JsonPropertyName("hostingPlatform")]
        public string HostingPlatform { get; set; }

        [JsonPropertyName("ciPipeline")]
        public string CIPipeline { get; set; }

        [JsonPropertyName("useDocker")]
        public bool UseDocker { get; set; }

        [JsonPropertyName("useKubernetes")]
        public bool UseKubernetes { get; set; }

        [JsonPropertyName("monitoringTools")]
        public string MonitoringTools { get; set; }

        // Advanced Configuration
        [JsonPropertyName("advancedConfig")]
        public Dictionary<string, object> AdvancedConfig { get; set; }

        // Project Metadata
        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("license")]
        public string License { get; set; }

        [JsonPropertyName("repositoryUrl")]
        public string RepositoryUrl { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }
    }
}
