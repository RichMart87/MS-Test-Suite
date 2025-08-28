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
        {
        }

        public CookieNotFoundException(string cookieName)
        {
        }

        public CookieNotFoundException(string cookieName, Exception innerException)
        {
        }
    }
}