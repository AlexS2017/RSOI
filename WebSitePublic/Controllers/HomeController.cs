using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Common.ServiceMessages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> UploadImg(AddImageMsg msg, IFormFile myuplfile)
        {
            if(ModelState.IsValid && myuplfile != null)
            {
                //UploadImage
            }

            return View(msg);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
