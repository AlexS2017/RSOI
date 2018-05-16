using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Common.CommonCode;
using Common.ServiceMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebSitePublic.Common;
using WebSitePublic.Models;

namespace WebSitePublic.Controllers
{
    public class HomeController : Controller
    {
        HttpRequestHelper restCallImg;

        public HomeController()
        {
            restCallImg = new HttpRequestHelper(PublicAppSettings.ImgSrvUrl, "api/PhotoMsg/");
        }

        public async Task<IActionResult> Index()
        {
            GetHomeImageMsg model = await GetLastImages();

            return View(model);
        }

        private async Task<GetHomeImageMsg> GetLastImages()
        {
            HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
            List<Guid> res = await restCallImg.CallRequest<List<Guid>>("getlastimgs", content, null, false, "");

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

        [Authorize]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImg(GetHomeImageMsg msg, IFormFile myUplfile)
        {
            if(ModelState.IsValid && myUplfile != null)
            {
                //UploadImage
                AddImageMsg addImgMsg = new AddImageMsg() { Description = msg.Description, HashTag = msg.HashTag, ImageTitle = msg.ImageTitle };
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

                msg = await GetLastImages();
            }

            return View("Index", msg);
            //return View("Index");
        }

        [HttpGet]
        public async Task<FileStreamResult> ViewImage(Guid id)
        {
            HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
            GetImageMsg res = await restCallImg.CallRequest<GetImageMsg>("getimg", content,null,false,id.ToString());
            MemoryStream ms = new MemoryStream(res.Image);

            return new FileStreamResult(ms, "image/jpeg");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

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

            HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
            List<GetComments> res = await restCallImg.CallRequest<List<GetComments>>("getcomments", content, null, false, id.ToString());

            StringBuilder sb = new StringBuilder();
            foreach (GetComments item in res)
            {
                sb.AppendFormat($"[{item.Date.ToString("dd-MM-yy hh:mm")}]  {item.Comment}\r\n");
            }
            model.AllCommentsInfo = sb.ToString();

            return model;
        }
    }
}
