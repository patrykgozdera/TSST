﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelSwitchingRouter
{
    class Config
    {
        public static String getProperty(String key)
        {
            String value = ConfigurationManager.AppSettings[key];
            return value;
        }
    }
}
