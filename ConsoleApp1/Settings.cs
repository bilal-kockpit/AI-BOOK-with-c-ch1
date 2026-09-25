using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ConsoleApp1
{
    public static class Settings
    {
        public static (string apiKey, string? orgId) LoadFromFile(string configFile = "Config/settings.json")
        {
            // Try looking in the output directory (e.g. bin/Debug/net...)
            string path1 = Path.Combine(AppContext.BaseDirectory, configFile);
            // Try looking in the current working directory (e.g. project folder)
            string path2 = Path.Combine(Directory.GetCurrentDirectory(), configFile);

            string finalPath = File.Exists(path1) ? path1 : (File.Exists(path2) ? path2 : configFile);

            if (!File.Exists(finalPath))
            {
                Console.WriteLine($"Configuration not found. Checked:\n- {path1}\n- {path2}");
                throw new Exception("Configuration not found");
            }
            try
            {
                var options = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };
                var config = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(finalPath), options);
                // check whether config is null
                if (config == null)
                {
                    Console.WriteLine("Configuration is null");
                    throw new Exception("Configuration is null");


                }
                string apiKey = config["apiKey"];
                string? orgId;
                // check whether orgId is in the file
                if (!config.ContainsKey("orgId"))
                {
                    orgId = null;
                }
                else
                {
                    orgId = config["orgId"];
                }
                return (apiKey, orgId);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong: " + e.Message);
                return ("", "");
            }
        }
    }
}
