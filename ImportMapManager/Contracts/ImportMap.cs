using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ImportMapManager.Contracts
{
    public class ImportMap
    {
        [JsonPropertyName("imports")]
        public Dictionary<string, string> Imports { get; set; }

        [JsonPropertyName("scopes")]
        public Dictionary<string, Dictionary<string, string>> Scopes { get; set; }
        }
}