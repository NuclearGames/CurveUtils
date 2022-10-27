using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Numerics;
using BezierCurveLib;
using System.IO;

namespace BezierCurveLib {
    public static class CurveConverter {

        private static JsonSerializerOptions _options = new JsonSerializerOptions {

            WriteIndented = true,
            Converters = { new Vector2Converter() }
        };


        public static async void Serialize(BezierNode curve) {

            FileStream fileStream = new FileStream(@"Curve.json", FileMode.Truncate);

            await JsonSerializer.SerializeAsync(fileStream, curve.Nodes, _options);

            fileStream.Close();

        }

        public static async Task<BezierNode> Deserialize() {

            FileStream fileStream = new FileStream(@"Curve.json", FileMode.Open);

            List<Vector2>? points = await JsonSerializer.DeserializeAsync<List<Vector2>>(fileStream, _options);

            BezierNode bezierNode = new BezierNode();
            bezierNode.Nodes.AddRange(points);

            fileStream.Close();
            return bezierNode;


        }
    }
}
