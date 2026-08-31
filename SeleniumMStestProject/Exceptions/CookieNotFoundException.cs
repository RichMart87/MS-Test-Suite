using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumMStestProject.Exceptions
{
    public class CookieNotFoundException : Exception
    {
        public CookieNotFoundException()
            : base("Cookie not found.") { }

        public CookieNotFoundException(string cookieName)
            : base($"Cookie not found: {cookieName}") { }

        public CookieNotFoundException(string cookieName, Exception innerException)
            : base($"Cookie not found: {cookieName}", innerException) { }
    }
}