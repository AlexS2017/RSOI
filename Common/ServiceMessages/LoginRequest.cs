using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ServiceMessages
{
    public class LoginRequest
    {
        public string appsecret { get; set; }
        public string clientid { get; set; }
        public string login { get; set; }
        public string password { get; set; }
    }
}
