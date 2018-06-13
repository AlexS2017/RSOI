using Common.CommonCode;
using Common.ServiceMessages;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using WebSitePublic.Common;

namespace WebAppPhotoSiteImages.Controllers
{
    public class BaseController : Controller
    {
        public TokenInfo token = null;
        //HttpRequestHelper restCallUser;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (User.Identity.IsAuthenticated)
            {
                Claim cl = User.Claims.FirstOrDefault(p => p.Type == "sub");
                if (cl == null)
                {
                    return;
                }

                string bearer = ((FrameRequestHeaders)Request.Headers).HeaderAuthorization[0];
                token = GetToken(bearer);

            }
        }

        protected TokenInfo GetToken(string bearer, bool isSubstr = true)
        {
            string token = bearer;
            if (isSubstr)
            {
                token = bearer.Substring(7);
            }

            UserInfoClient info_client = new UserInfoClient($"{ImgAppSettings.AuthSrvUrl}connect/userinfo");
            UserInfoResponse info_result = info_client.GetAsync(token).Result;

            Claim cn = info_result.Claims.FirstOrDefault(p => p.Type == "name");
            TokenInfo tokenInfo = new TokenInfo
            {
                sub = info_result.Claims.First(p => p.Type == "sub").Value,
                Email = cn == null ? "unknown" : cn.Value
            };
            return tokenInfo;
        }
    }
}
