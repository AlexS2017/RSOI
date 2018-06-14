using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ServiceMessages
{
    public class ActionsEnum
    {
        public readonly static string LOGIN = "login";
        public readonly static string LOGOUT = "logout";
        public readonly static string UPLOAD_IMG = "upload_image";
        public readonly static string ADD_COMMENT = "add_comment";
        public readonly static string AUTH_SRV_FAIL = "auth_service_fail";
        public readonly static string AUTH_SRV_UP = "auth_service_ok";
        public readonly static string IMG_SRV_FAIL = "img_service_fail";
        public readonly static string IMG_SRV_UP = "img_service_ok";
        public readonly static string REGISTER = "register";
    }
}
