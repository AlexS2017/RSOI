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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebSitePublic.Common;
using WebSitePublic.Models;

namespace WebSitePublic.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            ViewData["UserProfile"] = "Ivan Putin";
            ViewData["CurrentTime"] = DateTime.Now;

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImg(AddImageMsg msg, IFormFile myUplfile)
        {
            if(ModelState.IsValid && myUplfile != null)
            {
                //UploadImage
                HttpRequestHelper restCall = new HttpRequestHelper(PublicAppSettings.ImgSrvUrl, "api/PhotoMsg/");
                string jsonToPost = JsonConvert.SerializeObject(msg);
                HttpContent content = new StringContent(jsonToPost, Encoding.UTF8, "application/json");
                byte[] file = null;
                if (myUplfile.Length > 0)
                {
                    using (Stream fs = myUplfile.OpenReadStream())
                    {
                        file = fs.ReadFully(myUplfile.Length);
                    }
                }
                bool res = await restCall.CallRequest("uploadimg", content, file);

                if(!res)
                {
                    return View("Error");
                }
            }

            return View(msg);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
