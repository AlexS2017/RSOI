using Common.CommonCode;
using Common.ServiceMessages;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using WebSitePublic.Common;

namespace WebAppIdentity.Controllers
{
    public class BaseController : Controller
    {
        public TokenInfo token;
        public HttpRequestHelper restCallUser;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                ViewBag.imgerr = false;
                ViewBag.autherr = false;

                if (User != null
                    && User.Identity != null
                    && User.Identity.IsAuthenticated)
                {
                    Claim cl = User.Claims.FirstOrDefault(p => p.Type == "sub");
                    string sub = cl == null ? "" : cl.Value;

                    token = new TokenInfo()
                    {
                        sub = sub
                    };

                    restCallUser = new HttpRequestHelper(PublicAppSettings.AuthSrvUrl, "api/user/", PublicAppSettings.AuthSrvTokenUrl);
                    try
                    {
                        GetUserProfileMsg res = restCallUser.CallRequest<GetUserProfileMsg>("getuserbyid", new StringContent(""), null, false, sub).Result;
                        token.Email = res.Email;
                    }
                    catch (Exception)
                    {
                        token.Email = "error receiving email";
                    }
                    //token.Email = "";
                }
            }
            catch(Exception ex)
            {
                context.HttpContext.SignOutAsync("Cookies");
                context.HttpContext.SignOutAsync("oidc");               
                return;
            }

            base.OnActionExecuting(context);
        }

        protected TokenInfo GetToken(string bearer, bool isSubstr = true)
        {
            string token = bearer;
            if (isSubstr)
            {
                token = bearer.Substring(7);
            }

            UserInfoClient info_client = new UserInfoClient($"{PublicAppSettings.AuthSrvUrl}connect/userinfo");
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
