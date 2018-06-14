using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentitySrv.Common
{
    public class AuthAppSettings
    {
        public static string ImgSrvUrl { get; set; }

        public static string AuthSrvUrl { get; set; }

        public static string StatSrvUrl { get; set; }

        public static string AuthSrvTokenUrl { get { return $"{AuthSrvUrl}connect/token"; } }

    }
}
