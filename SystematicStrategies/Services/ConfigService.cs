

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using SystematicStrategies.Models;
using SystematicStrategies.ViewModels;

namespace SystematicStrategies.Services
{
    class ConfigService
    {
        public List<Config> GetAvailableConfigs()
        {
            var res = new List<Config>() { };
            string path = "Configs/config.json";
            string text = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<Config>>(text);
            /*string[] files = Directory.GetFiles("Configs");
            foreach(var file in files)
            {
                res.Add(new ConfigViewModel(file));
            }
            return res;*/
        }
    }
}