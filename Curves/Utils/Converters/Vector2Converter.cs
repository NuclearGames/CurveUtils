using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Curves.Utils.Converters {
    [Obsolete]
    internal sealed class Vector2Converter : JsonConverter<Vector2> {
        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            string? parseStr = reader.GetString();

            if (parseStr != null) {
                string[] splitArray = parseStr.Split(";");
                var x = Convert.ToSingle(splitArray[0]);
                var y = Convert.ToSingle(splitArray[1]);

                return new Vector2(x, y);
            }

            return new Vector2();
        }


        public override void Write(Utf8JsonWriter writer, Vector2 vector, JsonSerializerOptions options) {
            writer.WriteStringValue($"{vector.X};{vector.Y}");
        }
    }
}
