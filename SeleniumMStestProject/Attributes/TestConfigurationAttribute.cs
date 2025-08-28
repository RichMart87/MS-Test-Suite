using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumMStestProject.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal class TestConfigurationAttribute : Attribute
    {
        public string Browser { get; set; }
        public string Environment { get; set; }

        public TestConfigurationAttribute(string browser, string environment)
        {
            Browser = browser;
            Environment = environment;
        }
    }
}