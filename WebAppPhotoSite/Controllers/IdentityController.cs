using Common.CommonCode;
using Common.ServiceMessages;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppPhotoSiteImages.Common;
using WebAppPhotoSiteImages.Controllers;
using WebSitePublic.Common;

namespace WebAppAuth.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class IdentityController : BaseController
    {
        StatSrvHelper statSrvHelp;

        public IdentityController()
        {
            HttpRequestHelper restCallStat = new HttpRequestHelper(ImgAppSettings.StatSrvUrl, "api/Stat/", ImgAppSettings.AuthSrvTokenUrl);
            statSrvHelp = new StatSrvHelper(restCallStat);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        [HttpPost("login")]
        public async Task<LoginResponse> Login([FromBody] LoginRequest login)
        {
            LoginResponse resp = new LoginResponse();
            try
            {
                TokenClient tokenClient = new TokenClient(ImgAppSettings.AuthSrvTokenUrl, "client_imgapp", "secret");

                TokenResponse result = await tokenClient.RequestResourceOwnerPasswordAsync(login.login, login.password, "api_img openid profile");
                if (result == null || result.IsError)
                {
                    resp.IsSuccess = false;
                    resp.Message = "Authorization failed. Please check your login and password.";
                }
                else
                {
                    TokenData data = new TokenData()
                    {
                        access_token = result.AccessToken,
                        expires_in = result.ExpiresIn,
                        token_type = result.TokenType
                    };
                    resp.IsSuccess = true;
                    resp.Message = "Success";
                    resp.Data = data;
                }

                token = GetToken(resp.Data.access_token, false);
                if (token != null)
                {
                    await statSrvHelp.AddStatAction(new AddActionMsg() { UserId = token.UserId, Action = ActionsEnum.LOGIN, Client = "public_api", UserInfo = token.Email });
                }
            }
            catch(Exception ex)
            {
                resp.IsSuccess = false;
                resp.Message = ex.Message;
            }

            return resp;
        }
    }
}
