using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;


namespace SeleniumMStestProject
{
    internal class Config
    {
            public static string BaseUrl
        {
            get { return ConfigurationManager.AppSettings["BaseUrl"] ?? "https://seleniumbase.io/demo_page"; }
        }
            
            public static int ImplicitWait
        {
            get { return ImplicitWait; }
        }
        public static int ExplicitWait
        {
            get { return ExplicitWait; }
        }
        
        public static int Timeout
        { get { return Timeout; } }
        public static int TimeoutMilliseconds
        { get { return TimeoutMilliseconds; } }
        public static int TimeoutSeconds
        { get { return TimeoutSeconds; } }
        public static int TimeoutMinutes
        { get { return TimeoutMinutes; } }

    }
}
