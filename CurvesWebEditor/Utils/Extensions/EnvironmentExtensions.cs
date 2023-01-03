using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CurvesWebEditor.Utils.Extensions; 

internal static class EnvironmentExtensions {
    internal static class BaseUrl {
        internal static string Path {
            get {
                Load();
                return _basePath;
            }
        }
        
        internal static string HRef {
            get {
                Load();
                return _baseHRef;
            }
        }

        private static void Load() {
            if (!_wasLoaded) {
                if (!TryLoadValue() && !TryLoadFromSecret()) {
                    _basePath = "/";
                    _baseHRef = "";
                }
                
                _wasLoaded = true;
            }
        }

        private static bool TryLoadValue() {
            var value = Environment.GetEnvironmentVariable("BASE_URL");
            if (string.IsNullOrEmpty(value)) {
                return false;
            }

            _basePath = value;
            _baseHRef = $"{value}/";

            return true;
        }

        private static bool TryLoadFromSecret() {
            var value = Environment.GetEnvironmentVariable("BASE_URL_INFO");
            if (string.IsNullOrEmpty(value)) {
                return false;
            }

            if (File.Exists(value)) {
#if DEBUG
                return false;
#else
                throw new FileNotFoundException(value);
#endif
            }

            value = JsonSerializer.Deserialize<BaseUrlFileContent>(File.ReadAllText(value))?.Value;
            if (string.IsNullOrEmpty(value)) {
                throw new NullReferenceException("Value");
            }

            _basePath = value;
            _baseHRef = $"{value}/";

            return true;
        }
        
        private static bool _wasLoaded;
        private static string _basePath;
        private static string _baseHRef;

        private class BaseUrlFileContent {
            [JsonPropertyName("value")]
            public string Value { get; set; }
        }
    }
}