using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ResourceFileToJson
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = @"MessageResource.my-MM.resx";
            if (!File.Exists(path))
            {
                Console.WriteLine("File not found!");
                return;
            }
            var xml = File.ReadAllText(path);

            var result = new
            {
                Data = XElement.Parse(xml)
                    .Elements("data")
                    .Select(el => new
                    {
                        id = el.Attribute("name").Value,
                        text = el.Element("value").Value.Trim()
                    })
                    .ToList()
            };

            string res = "{";
            var obj = result.Data.Select(x =>
            {
                string item = $"\"{x.id}\":\"{x.text.Replace("\r", " ").Replace("\n", " ")}\"";
                return item;
            }).ToList();
            string jsonStr = string.Join(",", obj);
            res += jsonStr;
            res += "}";

            string json = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(res), Formatting.Indented);

            File.WriteAllText(Path.GetFileNameWithoutExtension(path) + ".json", res);
            Console.WriteLine(json);
        }
    }
}
