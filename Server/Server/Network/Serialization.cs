using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
//using System.Text.Json;

namespace Server.Network
{

    class Serialization
    {
        public static string GetSerial(Object obj)
        {
            var format = new JsonSerializerSettings();
            format.TypeNameHandling = (TypeNameHandling)1;

            return JsonConvert.SerializeObject(obj, format);
        }

    }

    
}
