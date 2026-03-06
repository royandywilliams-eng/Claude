using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ProjectSpecGUI.Core;

namespace ProjectSpecGUI.Integration
{
    /// <summary>
    /// HTTP client for Claude API integration
    /// Handles sending configurations to Claude and parsing responses
    /// </summary>
    public class ClaudeAPIClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly ClaudeSettings _settings;
        private const string ApiEndpoint = "https://api.anthropic.com/v1/messages";
        private const string ApiVersion = "2023-06-01";

        public ClaudeAPIClient(ClaudeSettings settings = null)
        {
            _settings = settings ?? ClaudeSettings.Load();
        }

        /// <summary>
        /// Send a project configuration to Claude API and get generated code/plan
        /// </summary>
        public async Task<ApiResponse> SendConfigurationAsync(ProjectConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            // Validate API key is configured
            if (!_settings.IsConfigured())
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "API key not configured. Please set it in Settings.",
                    ErrorCode = "MISSING_API_KEY"
                };
            }

            try
            {
                // Generate Claude prompt from configuration
                string claudePrompt = config.GenerateClaudePrompt();

                // Build request body
                var requestBody = new
                {
                    model = _settings.SelectedModel,
                    max_tokens = _settings.MaxTokens,
                    temperature = _settings.Temperature,
                    messages = new[]
                    {
                        new { role = "user", content = claudePrompt }
                    }
                };

                string requestJson = JsonSerializer.Serialize(requestBody);

                // Send request to Claude API
                var response = await SendApiRequestAsync(requestJson);

                return response;
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Network error: {ex.Message}",
                    ErrorCode = "NETWORK_ERROR",
                    Timestamp = DateTime.Now
                };
            }
            catch (TaskCanceledException ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Request timeout (>{_settings.TimeoutSeconds}s). Try again with a simpler configuration.",
                    ErrorCode = "TIMEOUT",
                    Timestamp = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Unexpected error: {ex.Message}",
                    ErrorCode = "UNKNOWN_ERROR",
                    Timestamp = DateTime.Now
                };
            }
        }

        /// <summary>
        /// Validate that the API key is valid by making a test request
        /// </summary>
        public async Task<bool> ValidateApiKeyAsync()
        {
            if (!_settings.IsConfigured())
                return false;

            try
            {
                var testBody = new
                {
                    model = _settings.SelectedModel,
                    max_tokens = 100,
                    messages = new[] { new { role = "user", content = "Say 'valid'" } }
                };

                string requestJson = JsonSerializer.Serialize(testBody);
                var response = await SendApiRequestAsync(requestJson, isValidationRequest: true);

                return response.Success;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Send HTTP request to Claude API with error handling
        /// </summary>
        private async Task<ApiResponse> SendApiRequestAsync(string requestJson, bool isValidationRequest = false)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, ApiEndpoint))
            {
                // Set headers
                request.Headers.Add("Authorization", $"Bearer {_settings.ApiKey}");
                request.Headers.Add("anthropic-version", ApiVersion);
                request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                // Set timeout
                using (var cts = new System.Threading.CancellationTokenSource(_settings.TimeoutSeconds * 1000))
                {
                    try
                    {
                        var httpResponse = await _httpClient.SendAsync(request, cts.Token);
                        return await HandleHttpResponseAsync(httpResponse, isValidationRequest);
                    }
                    catch (OperationCanceledException)
                    {
                        throw new TaskCanceledException("API request timed out");
                    }
                }
            }
        }

        /// <summary>
        /// Parse HTTP response from Claude API
        /// </summary>
        private async Task<ApiResponse> HandleHttpResponseAsync(HttpResponseMessage httpResponse, bool isValidationRequest)
        {
            string responseContent = await httpResponse.Content.ReadAsStringAsync();

            // Handle error responses
            if (!httpResponse.IsSuccessStatusCode)
            {
                return HandleErrorResponse(httpResponse.StatusCode, responseContent);
            }

            // Parse successful response
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(responseContent))
                {
                    var root = doc.RootElement;

                    // Extract message content
                    if (!root.TryGetProperty("content", out JsonElement contentArray) ||
                        contentArray.GetArrayLength() == 0)
                    {
                        return new ApiResponse
                        {
                            Success = false,
                            Message = "Invalid response format: no content",
                            ErrorCode = "INVALID_RESPONSE",
                            Timestamp = DateTime.Now
                        };
                    }

                    var firstContent = contentArray[0];
                    if (!firstContent.TryGetProperty("text", out JsonElement textElement))
                    {
                        return new ApiResponse
                        {
                            Success = false,
                            Message = "Invalid response format: no text field",
                            ErrorCode = "INVALID_RESPONSE",
                            Timestamp = DateTime.Now
                        };
                    }

                    // Extract token usage
                    int inputTokens = 0, outputTokens = 0;
                    if (root.TryGetProperty("usage", out JsonElement usage))
                    {
                        if (usage.TryGetProperty("input_tokens", out JsonElement inputTokensElem))
                            inputTokens = inputTokensElem.GetInt32();
                        if (usage.TryGetProperty("output_tokens", out JsonElement outputTokensElem))
                            outputTokens = outputTokensElem.GetInt32();
                    }

                    // Extract model
                    string model = _settings.SelectedModel;
                    if (root.TryGetProperty("model", out JsonElement modelElem))
                        model = modelElem.GetString() ?? model;

                    return new ApiResponse
                    {
                        Success = true,
                        GeneratedCode = textElement.GetString() ?? "",
                        InputTokens = inputTokens,
                        OutputTokens = outputTokens,
                        Model = model,
                        Timestamp = DateTime.Now
                    };
                }
            }
            catch (JsonException ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Failed to parse response: {ex.Message}",
                    ErrorCode = "PARSE_ERROR",
                    Timestamp = DateTime.Now
                };
            }
        }

        /// <summary>
        /// Handle error HTTP responses from Claude API
        /// </summary>
        private ApiResponse HandleErrorResponse(HttpStatusCode statusCode, string content)
        {
            string message = "Unknown error";
            string errorCode = statusCode.ToString();

            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    message = "Invalid API key. Check your credentials in Settings.";
                    errorCode = "UNAUTHORIZED";
                    break;

                case HttpStatusCode.TooManyRequests:
                    message = "Rate limit exceeded. Please wait before trying again.";
                    errorCode = "RATE_LIMIT";
                    break;

                case HttpStatusCode.BadRequest:
                    message = "Invalid request. Check your configuration.";
                    errorCode = "BAD_REQUEST";
                    break;

                case (HttpStatusCode)500:
                case (HttpStatusCode)502:
                case (HttpStatusCode)503:
                    message = "Claude service is temporarily unavailable. Please try again later.";
                    errorCode = "SERVICE_UNAVAILABLE";
                    break;

                default:
                    // Try to extract error message from response
                    try
                    {
                        using (JsonDocument doc = JsonDocument.Parse(content))
                        {
                            var root = doc.RootElement;
                            if (root.TryGetProperty("error", out JsonElement error) &&
                                error.TryGetProperty("message", out JsonElement errorMsg))
                            {
                                message = errorMsg.GetString() ?? message;
                            }
                        }
                    }
                    catch { }
                    break;
            }

            return new ApiResponse
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode,
                Timestamp = DateTime.Now
            };
        }
    }
}
