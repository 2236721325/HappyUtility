using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace HappyUtility.Jsons
{
    public static class JsonExtensions
    {
        public static string ToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static string ToJson_Ch(this object obj, bool writeIndented = false)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = writeIndented
            };
            return JsonSerializer.Serialize(obj, options);
        }

        public static T? ToObj<T>(this string str)
        {
            return JsonSerializer.Deserialize<T>(str);
        }
    }
}
