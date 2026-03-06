using System;
using System.Text.Json.Serialization;

namespace ProjectSpecGUI.Integration
{
    /// <summary>
    /// Response data from Claude API
    /// </summary>
    public class ApiResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = "";

        [JsonPropertyName("generatedCode")]
        public string GeneratedCode { get; set; } = "";

        [JsonPropertyName("inputTokens")]
        public int InputTokens { get; set; }

        [JsonPropertyName("outputTokens")]
        public int OutputTokens { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; } = "";

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; } = "";

        /// <summary>
        /// Convert response to history entry for persistence
        /// </summary>
        public HistoryEntry ToHistoryEntry(Core.ProjectConfiguration config)
        {
            var serializer = new Core.ConfigurationSerializer();
            return new HistoryEntry
            {
                ProjectName = config.ProjectName,
                SentAt = Timestamp,
                ConfigurationJson = serializer.Serialize(config),
                Success = Success,
                TokensUsed = InputTokens + OutputTokens,
                ResponsePreview = GeneratedCode.Length > 500
                    ? GeneratedCode.Substring(0, 500) + "..."
                    : GeneratedCode
            };
        }

        /// <summary>
        /// Format response for display in UI
        /// </summary>
        public override string ToString()
        {
            if (!Success)
                return $"Error ({ErrorCode}): {Message}";

            return $"Success - {Model} - {InputTokens + OutputTokens} tokens used";
        }
    }
}
