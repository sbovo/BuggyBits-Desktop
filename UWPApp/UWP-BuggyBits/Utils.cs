using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_BuggyBits
{
    public static class Utils
    {
        // App Center identifier for submitting analytics and crashes to the corresponding app
        // in AppCenter cloud services
        public static readonly string AppCenterAppSecret = "<<REPLACE_WITH_APPCENTER_APP_SECRET>>";

        // Dictionary used in AppCenter calls to inform about the version of the app:
        // Production, Development, etc.
        public static Dictionary<string, string> AppCenterDictionarySettings = new Dictionary<string, string>()
                {
                     { "Version", "Production-Store"}
                };
    }
}
