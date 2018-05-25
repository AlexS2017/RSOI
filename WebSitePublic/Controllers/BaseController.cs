using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAppIdentity.Controllers
{
    public class BaseController : Controller
    {
        public TokenInfo token;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
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
    }

    public class TokenInfo
    {
        public string sub { get; set; }

        public Guid UserId { get { return Guid.Parse(sub); } }

        public string Email { get; set; }
    }
}
