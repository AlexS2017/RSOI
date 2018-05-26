using Common.ServiceMessages;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSitePublic.Common;

namespace WebAppAuth.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        [HttpPost("login")]
        public async Task<LoginResponse> Login(LoginRequest login)
        {
            LoginResponse resp = new LoginResponse();
            try
            {
                TokenClient tokenClient = new TokenClient(ImgAppSettings.AuthSrvUrl, "client_imgapp", "secret");

                TokenResponse result = await tokenClient.RequestResourceOwnerPasswordAsync(login.login, login.password, "api_img");
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
