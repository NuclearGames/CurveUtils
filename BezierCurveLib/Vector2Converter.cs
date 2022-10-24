using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BezierCurveLib {
    public class Vector2Converter : JsonConverter<Vector2> {
        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            string? parseStr = reader.GetString();
            if(parseStr!= null) {
                string[] splitArray = parseStr.Split(";");
                var X = Convert.ToSingle(splitArray[0]);
                var Y = Convert.ToSingle(splitArray[1]);
                return new Vector2(X,Y);
            }
            return new Vector2();
        }                                                     


        public override void Write(Utf8JsonWriter writer, Vector2 vector, JsonSerializerOptions options) {
            writer.WriteStringValue($"{vector.X};{vector.Y}");
        }
    }
}

