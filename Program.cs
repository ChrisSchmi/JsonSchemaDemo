using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Linq;

namespace JsonSchemaDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            JSchemaGenerator generator = new();
            JSchema schema = generator.Generate(typeof(DeviceMessage));
            Console.WriteLine($"{Environment.NewLine}Schema:" + Environment.NewLine + schema.ToString());

            var deviceMessage = new DeviceMessage()
            {
                DeviceId = "1234",
                MessageId = 1,
                TypeId = 2,    
                CompatibleDeviceIds = [1,2,3]
            };

            var corruptDeviceMessage = new CorruptDeviceMessage()
            {
                MessageId = 1,
                TypeId = 2
            };

            var json = JsonConvert.SerializeObject(deviceMessage);
            var data = JObject.Parse(json);
            var isValidSchema = data.IsValid(schema);
            Console.WriteLine($"{Environment.NewLine}{nameof(deviceMessage)} - schema valid: {isValidSchema}");

            IList<string> errorMessages;
            var corruptJson = JsonConvert.SerializeObject(corruptDeviceMessage);
            var corruptData = JObject.Parse(corruptJson);
            var isValidSchema2 = corruptData.IsValid(schema, out errorMessages);
            Console.WriteLine($"{Environment.NewLine}{nameof(corruptDeviceMessage)} - schema valid: {isValidSchema2}{Environment.NewLine}");

            foreach (string message in errorMessages)
            {
                Console.WriteLine(message);
            }

            Console.WriteLine($"{Environment.NewLine}");
        }    
    }

    public class DeviceMessage
    {   
        [JsonProperty(Required = Required.Always)]
        public int MessageId { get; set; }
        
        [JsonProperty(Required = Required.Always)]
        public int TypeId { get; set; }

        [JsonProperty(Required = Required.Always)]
        public required string DeviceId { get; set; }

        [JsonProperty(Required = Required.Always)]
        public required int[] CompatibleDeviceIds { get; set; }
    }

    public class CorruptDeviceMessage
    {
        public int MessageId { get; set; }
        public int TypeId { get; set; }
    }
}