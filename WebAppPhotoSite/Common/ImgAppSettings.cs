using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSitePublic.Common
{
    public class ImgAppSettings
    {
        public static string AuthSrvUrl { get; set; }

        public static string AuthSrvTokenUrl { get { return $"{AuthSrvUrl}connect/token"; } }

        public static string StatSrvUrl { get; set; }
    }
}
