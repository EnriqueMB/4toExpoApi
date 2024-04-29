using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Helpers
{
    public static class HoraHelper
    {
        public static DateTime GetHora(string culture)
        {
            if (culture == "mx")
                return DateTime.UtcNow.AddHours(-6);

            return DateTime.UtcNow;
        }
    }
}
