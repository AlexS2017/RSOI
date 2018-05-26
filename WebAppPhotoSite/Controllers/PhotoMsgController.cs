using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.CommonCode;
using Common.ServiceMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAppPhotoSiteImages.Services;

namespace WebAppPhotoSite.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class PhotoMsgController : Controller
    {

        PhotoMsgService _srv;

        public PhotoMsgController(PhotoMsgService srv)
        {
            _srv = srv;
        }

        // GET api/PhotoMsg
        [HttpGet]
        public async Task<List<UserProfile>> Get()
        {
            List<UserProfile> res = await _srv.GetAllUsers();
            return res;
        }

        // GET api/PhotoMsg/5
        [HttpGet("{name}")]
        public async Task<UserProfile> Get(string name)
        {
            UserProfile res = await _srv.GetUser(name);
            return res;
        }

        [HttpGet("getbylastname/{name}")]
        public async Task<UserProfile> GetLN(string name)
        {
            UserProfile res = await _srv.GetUser(name);
            return res;
        }

        [HttpPost("uploadimg")]
        public async Task<bool> UploadImage([FromForm] AddImageMsg request)
        {
            if(Request != null && Request.Form != null && Request.Form.Files != null)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();

                if (file == null) return false;

                request = JsonConvert.DeserializeObject<AddImageMsg>(Request.Form["json"]);

                using (Stream fs = file.OpenReadStream())
                {
                    request.Image = fs.ReadFully(file.Length);
                }

                return await _srv.UploadImage(request);
            }
            else
            {
                return false;
            }
        }

        [HttpPost("commentimg")]
        public async Task<bool> CommentImage([FromBody] AddImageCommentMsg request)
        {
            return await _srv.AddCommentToImage(request);
        }

        [HttpGet("getcomments/{id}")]
        public async Task<List<GetComments>> GetComments(Guid id)
        {
            List<GetComments> res = await _srv.GetComments(id);
            return res;
        }

        [HttpPost("getimginfo")]
        public async Task<ImagePageInfo> GetImgInfo([FromBody] GetImgInfoMsg request)
        {
            ImagePageInfo res = await _srv.GetImgInfo(request);
            return res;
        }

        [HttpGet("getlastimgs/{userId}")]
        public async Task<List<Guid>> GetLastImages(Guid userId)
        {
            List<Guid> res = await _srv.GetLastImages(userId);
            return res;
        }

        [HttpGet("getimg/{id}")]
        public async Task<GetImageMsg> GetImg(Guid id)
        {
            GetImageMsg res = await _srv.GetImageById(id);
            return res;
        }

        //    // POST api/values
        //    [HttpPost]
        //    public void Post([FromBody]string value)
        //    {
        //    }

        //    // PUT api/values/5
        //    [HttpPut("{id}")]
        //    public void Put(int id, [FromBody]string value)
        //    {
        //    }

        //    // DELETE api/values/5
        //    [HttpDelete("{id}")]
        //    public void Delete(int id)
        //    {
        //    }
    }
}
