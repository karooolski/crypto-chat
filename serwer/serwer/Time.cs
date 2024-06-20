using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serwer
{
    internal class Time
    {
        public static string getCurrentTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt");
        }
    }
}
