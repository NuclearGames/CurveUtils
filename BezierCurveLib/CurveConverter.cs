using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Numerics;
using BezierCurveLib;

namespace BezierCurveLib {
    public static class CurveConverter {

        private static JsonSerializerOptions options = new JsonSerializerOptions {
            WriteIndented = true,
            Converters = { new Vector2Converter() }
        };


        public static async void Serialize(Bezier curve) {

            FileStream fileStream = new FileStream(@"Curve.json", FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(fileStream, curve.Points, options);
            fileStream.Close();

        }

        public static async Task<Bezier> Deserialize() {
            FileStream fileStream = new FileStream(@"Curve.json", FileMode.OpenOrCreate);
            Bezier curve = new Bezier();
            curve.Points = await JsonSerializer.DeserializeAsync<List<Vector2>>(fileStream, options);
            fileStream.Close();
            return curve;


        }
    }
}
