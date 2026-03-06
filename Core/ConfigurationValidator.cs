using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjectSpecGUI.Core
{
    /// <summary>
    /// Validation result containing success status and error/warning messages
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();

        public ValidationResult()
        {
            IsValid = true;
        }

        public ValidationResult(bool isValid)
        {
            IsValid = isValid;
        }
    }

    /// <summary>
    /// Comprehensive validation engine for ProjectConfiguration
    /// Validates required fields, formats, and business logic
    /// </summary>
    public class ConfigurationValidator
    {
        /// <summary>
        /// Validate the entire project configuration
        /// </summary>
        public ValidationResult Validate(ProjectConfiguration config)
        {
            var result = new ValidationResult();

            if (config == null)
            {
                result.IsValid = false;
                result.Errors.Add("Configuration object is null");
                return result;
            }

            // Validate required fields
            ValidateRequiredFields(config, result);

            // Validate field formats
            ValidateFormats(config, result);

            // Validate business logic
            ValidateBusinessLogic(config, result);

            // Generate warnings
            GenerateWarnings(config, result);

            // Set overall validity
            result.IsValid = result.Errors.Count == 0;

            return result;
        }

        /// <summary>
        /// Validate all required fields
        /// </summary>
        private void ValidateRequiredFields(ProjectConfiguration config, ValidationResult result)
        {
            // Project name is required
            if (string.IsNullOrWhiteSpace(config.ProjectName))
            {
                result.Errors.Add("Project name is required");
            }

            // Project type is required and must be valid
            if (string.IsNullOrWhiteSpace(config.ProjectType))
            {
                result.Errors.Add("Project type is required");
            }
            else if (!AppConstants.PROJECT_TYPES.Contains(config.ProjectType))
            {
                result.Errors.Add($"Project type '{config.ProjectType}' is not valid. Must be one of: {string.Join(", ", AppConstants.PROJECT_TYPES)}");
            }

            // Primary language is required and must be valid
            if (string.IsNullOrWhiteSpace(config.PrimaryLanguage))
            {
                result.Errors.Add("Primary language is required");
            }
            else if (!AppConstants.LANGUAGES.Contains(config.PrimaryLanguage))
            {
                result.Errors.Add($"Primary language '{config.PrimaryLanguage}' is not valid. Must be one of: {string.Join(", ", AppConstants.LANGUAGES)}");
            }

            // At least one framework is required
            if (config.Frameworks == null || config.Frameworks.Count == 0)
            {
                result.Errors.Add("At least one framework must be selected");
            }
        }

        /// <summary>
        /// Validate field formats (URLs, versions, etc.)
        /// </summary>
        private void ValidateFormats(ProjectConfiguration config, ValidationResult result)
        {
            // Validate version format (semantic versioning)
            if (!string.IsNullOrWhiteSpace(config.Version))
            {
                if (!IsValidSemanticVersion(config.Version))
                {
                    result.Errors.Add($"Version '{config.Version}' is not in valid semantic versioning format (e.g., 1.0.0)");
                }
            }

            // Validate repository URL format
            if (!string.IsNullOrWhiteSpace(config.RepositoryUrl))
            {
                if (!IsValidUrl(config.RepositoryUrl))
                {
                    result.Errors.Add($"Repository URL '{config.RepositoryUrl}' is not a valid URL");
                }
            }

            // Validate complexity level if provided
            if (!string.IsNullOrWhiteSpace(config.ComplexityLevel))
            {
                if (!AppConstants.COMPLEXITY_LEVELS.Contains(config.ComplexityLevel))
                {
                    result.Errors.Add($"Complexity level '{config.ComplexityLevel}' is not valid");
                }
            }

            // Validate database selection
            if (!string.IsNullOrWhiteSpace(config.Database))
            {
                if (!AppConstants.DATABASES.Contains(config.Database))
                {
                    result.Errors.Add($"Database '{config.Database}' is not valid");
                }
            }

            // Validate design framework
            if (!string.IsNullOrWhiteSpace(config.DesignFramework))
            {
                if (!AppConstants.DESIGN_FRAMEWORKS.Contains(config.DesignFramework))
                {
                    result.Errors.Add($"Design framework '{config.DesignFramework}' is not valid");
                }
            }

            // Validate hosting platform
            if (!string.IsNullOrWhiteSpace(config.HostingPlatform))
            {
                if (!AppConstants.HOSTING_PLATFORMS.Contains(config.HostingPlatform))
                {
                    result.Errors.Add($"Hosting platform '{config.HostingPlatform}' is not valid");
                }
            }
        }

        /// <summary>
        /// Validate business logic and cross-field constraints
        /// </summary>
        private void ValidateBusinessLogic(ProjectConfiguration config, ValidationResult result)
        {
            // If frameworks are selected, validate they exist for the chosen language
            if (!string.IsNullOrWhiteSpace(config.PrimaryLanguage) &&
                config.Frameworks != null && config.Frameworks.Count > 0)
            {
                if (AppConstants.FRAMEWORKS_BY_LANGUAGE.ContainsKey(config.PrimaryLanguage))
                {
                    var validFrameworks = AppConstants.FRAMEWORKS_BY_LANGUAGE[config.PrimaryLanguage];
                    var invalidFrameworks = config.Frameworks.Where(f => !validFrameworks.Contains(f)).ToList();

                    if (invalidFrameworks.Count > 0)
                    {
                        result.Errors.Add($"Frameworks not available for {config.PrimaryLanguage}: {string.Join(", ", invalidFrameworks)}");
                    }
                }
            }

            // Validate license if provided
            if (!string.IsNullOrWhiteSpace(config.License))
            {
                if (!AppConstants.LICENSES.Contains(config.License))
                {
                    result.Warnings.Add($"License '{config.License}' is not in the standard list but will be accepted");
                }
            }
        }

        /// <summary>
        /// Generate warnings for incomplete or suboptimal configuration
        /// </summary>
        private void GenerateWarnings(ProjectConfiguration config, ValidationResult result)
        {
            // Warn if project description is missing
            if (string.IsNullOrWhiteSpace(config.ProjectDescription))
            {
                result.Warnings.Add("Project description is empty - consider adding one for clarity");
            }

            // Warn if author is not specified
            if (string.IsNullOrWhiteSpace(config.Author))
            {
                result.Warnings.Add("Author is not specified");
            }

            // Warn if repository URL is missing
            if (string.IsNullOrWhiteSpace(config.RepositoryUrl))
            {
                result.Warnings.Add("Repository URL is not specified");
            }

            // Warn if no features are selected
            if (config.Features != null && config.Features.Values.All(v => !v))
            {
                result.Warnings.Add("No key features are selected - consider enabling at least some features");
            }

            // Warn if advanced config is empty
            if (config.AdvancedConfig == null || config.AdvancedConfig.Count == 0)
            {
                result.Warnings.Add("Advanced configuration is empty - detailed settings may not be specified");
            }

            // Warn about scalability if high user count expected but no caching/CDN
            if (config.ExpectedUsers != "1000" && int.TryParse(config.ExpectedUsers, out int expectedUsers))
            {
                if (expectedUsers > 10000 && config.CachingStrategy == "None" && !config.CDNUsage)
                {
                    result.Warnings.Add("High expected user count but no caching strategy or CDN configured - consider enabling these for scalability");
                }
            }
        }

        /// <summary>
        /// Check if a string is a valid semantic version (e.g., 1.0.0)
        /// </summary>
        private bool IsValidSemanticVersion(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
                return true; // Optional field

            var pattern = @"^\d+\.\d+\.\d+(-[a-zA-Z0-9.]+)?(\+[a-zA-Z0-9.]+)?$";
            return Regex.IsMatch(version, pattern);
        }

        /// <summary>
        /// Check if a string is a valid URL
        /// </summary>
        private bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return true; // Optional field

            try
            {
                var uri = new Uri(url);
                return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == "git";
            }
            catch
            {
                return false;
            }
        }
    }
}
