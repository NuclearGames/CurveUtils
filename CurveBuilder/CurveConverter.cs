using System;
using System.IO;
using System.Text.Json;
using BezierCurveLib;
using BezierCurveLib.Utils.Converters;

namespace CurveBuilder {
    public static class CurveConverter {

        private static JsonSerializerOptions _options = new JsonSerializerOptions {

            WriteIndented = true,
            Converters = { new Vector2Converter() }
        };


        public static void Serialize(BezierCurveSourceModel curve) {
            var stringValue = JsonSerializer.Serialize(curve, _options);
            File.WriteAllText(@"Curve.json", stringValue);
        }

        public static BezierCurveSourceModel Deserialize() {
            var stringValue = File.ReadAllText(@"Curve.json");

            return JsonSerializer.Deserialize<BezierCurveSourceModel>(stringValue, _options) ?? throw new NullReferenceException();

        }
    }
}
