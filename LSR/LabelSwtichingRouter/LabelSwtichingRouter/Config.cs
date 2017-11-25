using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelSwtichingRouter
{
    class Config
    {
        public static String getProperty(String key)
        {
            String value = System.Configuration.ConfigurationSettings.AppSettings[key];
            return value;
        }
    }
}
