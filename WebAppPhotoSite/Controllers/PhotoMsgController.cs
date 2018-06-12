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
using WebAppPhotoSiteImages.Common;
using WebAppPhotoSiteImages.Controllers;
using WebAppPhotoSiteImages.Services;
using WebSitePublic.Common;

namespace WebAppPhotoSite.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class PhotoMsgController : BaseController
    {

        PhotoMsgService _srv;
        StatSrvHelper statSrvHelp;

        public PhotoMsgController(PhotoMsgService srv)
        {
            _srv = srv;
            HttpRequestHelper restCallStat = new HttpRequestHelper(ImgAppSettings.StatSrvUrl, "api/Stat/", ImgAppSettings.AuthSrvTokenUrl);
            statSrvHelp = new StatSrvHelper(restCallStat);
        }

        // GET api/PhotoMsg

        [HttpPost("uploadimg")]
        public async Task<AddImageResponse> UploadImage([FromForm] AddImageMsg request)
        {
            if(Request != null && Request.Form != null && Request.Form.Files != null)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();

                if (file == null) return new AddImageResponse();

                request = JsonConvert.DeserializeObject<AddImageMsg>(Request.Form["json"]);

                using (Stream fs = file.OpenReadStream())
                {
                    request.Image = fs.ReadFully(file.Length);
                }

                AddImageResponse res = await _srv.UploadImage(request);

                if(res.IsSuccess)
                {
                    if (token != null)
                    {
                        await statSrvHelp.AddStatAction(new AddActionMsg() { UserId = token.UserId, Action = ActionsEnum.UPLOAD_IMG, Client = request.Client, UserInfo = token.Email, EntityId = res.ImageId });
                    }
                }

                return res;
            }
            else
            {
                return new AddImageResponse();
            }
        }

        [HttpPost("commentimg")]
        public async Task<AddImageCommentResponse> CommentImage([FromBody] AddImageCommentMsg request)
        {
            AddImageCommentResponse res = await _srv.AddCommentToImage(request);

            if(res.IsSuccess)
            {
                if (token != null)
                {
                    await statSrvHelp.AddStatAction(new AddActionMsg() { UserId = token.UserId, Action = ActionsEnum.ADD_COMMENT, Client = request.Client, UserInfo = token.Email, EntityId = res.CommentId });
                }
            }

            return res;
        }

        [HttpGet("getcomments/{id}")]
        public async Task<List<GetComments>> GetComments(Guid id)
        {
            List<GetComments> res = await _srv.GetComments(id);
            return res;
        }

        [HttpGet("checksrvstatus")]
        [AllowAnonymous]
        public bool CheckSrvStatus()
        {
            return true;
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
