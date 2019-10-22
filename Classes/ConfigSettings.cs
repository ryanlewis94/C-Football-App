using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballApp.Classes
{

    
    public class ConfigSettings
    {
        public string Key = ConfigurationManager.AppSettings["apiKey"];
        public string Secret= ConfigurationManager.AppSettings["apiSecret"];
    }
}
