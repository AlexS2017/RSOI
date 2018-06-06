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
using System.Threading.Tasks;
using WebSitePublic.Common;

namespace WebAppPhotoSiteImages.Controllers
{
    public class BaseController : Controller
    {
        public TokenInfo token;
        HttpRequestHelper restCallUser;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (User.Identity.IsAuthenticated)
            {
                string bearer = ((FrameRequestHeaders)Request.Headers).HeaderAuthorization[0];
                token = GetToken(bearer);

                restCallUser = new HttpRequestHelper(ImgAppSettings.AuthSrvUrl, "api/user/", ImgAppSettings.AuthSrvTokenUrl);
                try
                {
                    GetUserProfileMsg res = restCallUser.CallRequest<GetUserProfileMsg>("getuserbyid", new StringContent(""), null, false, token.sub).Result;
                    token.Email = res.Email;
                }
                catch (Exception)
                {
                    token.Email = "error receiving email";
                }
            }
        }

        protected TokenInfo GetToken(string bearer)
        {
            string token = bearer.Substring(7);

            UserInfoClient info_client = new UserInfoClient($"{ImgAppSettings.AuthSrvUrl}connect/userinfo");
            UserInfoResponse info_result = info_client.GetAsync(token).Result;

            TokenInfo tokenInfo = new TokenInfo
            {
                sub = info_result.Claims.First(p => p.Type == "sub").Value,
            };
            return tokenInfo;
        }
    }
}
