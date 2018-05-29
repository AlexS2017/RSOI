using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Common.CommonCode;
using Common.ServiceMessages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Newtonsoft.Json;
using WebAppIdentity.Controllers;
using WebSitePublic.Common;
using WebSitePublic.Models;

namespace WebSitePublic.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        HttpRequestHelper restCallImg;

        public HomeController()
        {
            restCallImg = new HttpRequestHelper(PublicAppSettings.ImgSrvUrl, "api/PhotoMsg/", PublicAppSettings.AuthSrvUrl);
        }

        public async Task<IActionResult> MyImages()
        {
            GetHomeImageMsg model = await GetLastImages(token.UserId);

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        private async Task<GetHomeImageMsg> GetLastImages(Guid userId)
        {
            HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
            List<Guid> res = await restCallImg.CallRequest<List<Guid>>("getlastimgs", content, null, false, userId.ToString());

            GetHomeImageMsg model = new GetHomeImageMsg() { ImageList = res };
            return model;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            ViewData["UserProfile"] = "Ivan Putin";
            ViewData["CurrentTime"] = DateTime.Now;

            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
            //await HttpContext.SignOutAsync(IdentityServer4.IdentityServerConstants.DefaultCookieAuthenticationScheme);

            return View("Index");
        }

        public async Task<IActionResult> SignIn()
        {
            return RedirectToAction("MyImages");
        }

        [AllowAnonymous]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            //string bearer = ((FrameRequestHeaders)Request.Headers).HeaderAuthorization[0];

            Claim cl = User.Claims.FirstOrDefault(p => p.Type == "sub");
            string sub = cl == null ? "" : cl.Value;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImg(GetHomeImageMsg msg, IFormFile myUplfile)
        {
            if(ModelState.IsValid && myUplfile != null)
            {
                //UploadImage
                AddImageMsg addImgMsg = new AddImageMsg()
                { Description = msg.Description, HashTag = msg.HashTag, ImageTitle = msg.ImageTitle, UserId = token.UserId };
                string jsonToPost = JsonConvert.SerializeObject(addImgMsg);
                HttpContent content = new StringContent(jsonToPost, Encoding.UTF8, "application/json");
                byte[] file = null;
                if (myUplfile.Length > 0)
                {
                    using (Stream fs = myUplfile.OpenReadStream())
                    {
                        file = fs.ReadFully(myUplfile.Length);
                    }
                }
                bool res = await restCallImg.CallRequest<bool>("uploadimg", content, file);

                if(!res)
                {
                    return View("Error");
                }

                msg = await GetLastImages(token.UserId);
            }

            return View("MyImages", msg);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<FileStreamResult> ViewImage(Guid id)
        {
            HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
            GetImageMsg res = await restCallImg.CallRequest<GetImageMsg>("getimg", content,null,false,id.ToString());
            MemoryStream ms = new MemoryStream(res.Image);

            return new FileStreamResult(ms, "image/jpeg");
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public async Task<IActionResult> ImagePage(Guid id)
        {
            AddImageCommentMsg model = new AddImageCommentMsg() { ImageId = id };
            model = await GetComments(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(AddImageCommentMsg addCommentmsg)
        {
            if (ModelState.IsValid)
            {
                addCommentmsg.UserId = token.UserId;
                string jsonToPost = JsonConvert.SerializeObject(addCommentmsg);
                HttpContent content = new StringContent(jsonToPost, Encoding.UTF8, "application/json");
                
                bool res = await restCallImg.CallRequest<bool>("commentimg", content);

                if (!res)
                {
                    return View("Error");
                }

               // msg = await GetLastImages();
            }

            //return View("ImagePage", addCommentmsg);
            return RedirectToAction("ImagePage", new { id = addCommentmsg.ImageId });
        }

        [HttpGet]
        public async Task<AddImageCommentMsg> GetComments(Guid id)
        {
            AddImageCommentMsg model = new AddImageCommentMsg() { ImageId = id };

            GetImgInfoMsg getImgInfo = new GetImgInfoMsg() { ImageId = id, UserId = token == null ? (Guid?)null : token.UserId };
            string jsonToPost = JsonConvert.SerializeObject(getImgInfo);
            HttpContent content = new StringContent(jsonToPost, Encoding.UTF8, "application/json");

            ImagePageInfo res = await restCallImg.CallRequest<ImagePageInfo>("getimginfo", content);
            List<GetComments> comments = res.Comments; 

            StringBuilder sb = new StringBuilder();
            foreach (GetComments item in comments)
            {
                sb.AppendFormat($"[{item.Date.ToString("dd-MM-yy hh:mm")}]  {item.Comment}\r\n");
            }
            model.AllCommentsInfo = sb.ToString();
            model.Rate = res.Rate;
            model.AvgRate = res.AvgRate;

            return model;
        }
    }
}
