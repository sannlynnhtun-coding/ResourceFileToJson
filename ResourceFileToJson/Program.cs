﻿using Newtonsoft.Json;
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
            string path = @"MessageResource.en-GB.resx";
            var xml = File.ReadAllText(path);

            var result = new
            {
                Texts = XElement.Parse(xml)
                    .Elements("data")
                    .Select(el => new
                    {
                        id = el.Attribute("name").Value,
                        text = el.Element("value").Value.Trim()
                    })
                    .ToList()
            };

            string res = "{";
            var obj = result.Texts.Select(x =>
            {
                string item = $"\"{x.id}\":\"{x.text.Replace("\r", " ").Replace("\n", " ")}\"";
                return item;
            }).ToList();
            string jsonStr = string.Join(",", obj);
            res += jsonStr;
            res += "}";

            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);

            // OR:
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            //string json = jss.Serialize(obj);
            File.WriteAllText(Path.GetFileNameWithoutExtension(path) + ".json", res);
            Console.WriteLine(res);
        }
    }
}