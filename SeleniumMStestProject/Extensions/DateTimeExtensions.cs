using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumMStestProject.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToFormattedString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToFormattedString(this DateTime? dateTime)
        {
            return dateTime?.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToFormattedString(this DateTime dateTime, string format)
        {
            return dateTime.ToString(format);
        }

        private static Dictionary<string, string> DateFormatDictionary
        {
            get
            {
                return new Dictionary<string, string>{{"en-us", "yyyy-MM-dd HH:mm:ss"},
                    { "yyyy-MM-dd", "yyyy-MM-dd" },
                    { "en-us","MM/dd/yyyy" },
                    { "dd/MM/yyyy", "dd/MM/yyyy" },
                    { "yyyyMMdd", "yyyyMMdd" },
                    { "ddMMyyyy", "ddMMyyyy" }
                };
            }
        }
    }
}