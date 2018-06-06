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
        HttpRequestHelper restCallStat;
        //HttpRequestHelper restCallUser;

        public HomeController()
        {
            restCallImg = new HttpRequestHelper(PublicAppSettings.ImgSrvUrl, "api/PhotoMsg/", PublicAppSettings.AuthSrvTokenUrl);
            restCallStat = new HttpRequestHelper(PublicAppSettings.StatSrvUrl, "api/Stat/", PublicAppSettings.AuthSrvTokenUrl);
            if(restCallUser == null)
            {
                restCallUser = new HttpRequestHelper(PublicAppSettings.AuthSrvUrl, "api/user/", PublicAppSettings.AuthSrvTokenUrl);
            }
        }

        public async Task<IActionResult> MyImages()
        {
            GetHomeImageMsg model = await GetLastImages(token.UserId);

            return View(model);
        }

        private async Task<bool> AddStatAction(AddActionMsg msg)
        {
            try
            {
                string jsonToPost = JsonConvert.SerializeObject(msg);
                HttpContent content = new StringContent(jsonToPost, Encoding.UTF8, "application/json");

                bool res = await restCallStat.CallRequest<bool>("addaction", content);
                return res;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            bool resImgSrv = false;
            bool resAuthSrv = false;

            try
            {
                resImgSrv = await restCallImg.CallRequest<bool>("checksrvstatus", new StringContent(""), null, false, "", false);
            }
            catch { }

            try
            {
                resAuthSrv = await restCallUser.CallRequest<bool>("checksrvstatus", new StringContent(""), null, false, "", false);
            }
            catch { }

            string error = "";

            if (!resImgSrv)
            {
                error = "Внимание, в данный момент наблюдаются временные перебои при работе с картинками. Наши специалисты работают над исправлением ситуации. В данный момент функциональность нашего сервиса недоступна, зайдите сюда чуть позже..\n ";
            }

            if (!resAuthSrv)
            {
                error += "В данный момент наблюдаются временные неполадки с доступом к личному кабинету. Наши специалисты работают над исправлением ситуации. В данный момент функциональность нашего сервиса недоступна, зайдите сюда чуть позже..\n ";
            }

            HomePageMsg msg = new HomePageMsg() { ErrorMessage = error };
            return View(msg);
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

        //[AllowAnonymous]
        public async Task<IActionResult> Signout()
        {
            await AddStatAction(new AddActionMsg() { UserId = token.UserId, Action = ActionsEnum.LOGOUT, Client = "website", UserInfo=token.Email });

            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");

            return View("Index");
        }

        public async Task<IActionResult> SignIn()
        {
            await AddStatAction(new AddActionMsg() { UserId = token.UserId, Action = ActionsEnum.LOGIN, Client = "website", UserInfo = token.Email });
            return RedirectToAction("MyImages");
        }

        [AllowAnonymous]
        public IActionResult Contact() //for test
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
                { Description = msg.Description, HashTag = msg.HashTag, ImageTitle = msg.ImageTitle, UserId = token.UserId, Client = "website" };
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
                AddImageResponse res = await restCallImg.CallRequest<AddImageResponse>("uploadimg", content, file);

                if(!res.IsSuccess)
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
                addCommentmsg.Client = "website";
                string jsonToPost = JsonConvert.SerializeObject(addCommentmsg);
                HttpContent content = new StringContent(jsonToPost, Encoding.UTF8, "application/json");

                AddImageCommentResponse res = await restCallImg.CallRequest<AddImageCommentResponse>("commentimg", content);

                if (!res.IsSuccess)
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
