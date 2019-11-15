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
        public string Secret = ConfigurationManager.AppSettings["apiSecret"];

        //foreach (ConnectionStringSettings css in ConfigurationManager.ConnectionStrings)
        //    {
        //        if (css.Name == "apiKey")
        //        {
        //            key = css.ConnectionString;
        //        }
        //        if (css.Name == "apiSecret")
        //        {
        //            secret = css.ConnectionString;
        //        }
        //    }

        //public string GetApi()
        //{
        //    foreach (ConnectionStringSettings css in ConfigurationManager.ConnectionStrings)
        //    {
        //        if (css.Name == "apiKey")
        //        {
        //            return css.ConnectionString;
        //        }
        //        else //if (css.Name == "apiSecret")
        //        {
        //            return css.ConnectionString;
        //        }
        //    }
        //}
    }
    
}
