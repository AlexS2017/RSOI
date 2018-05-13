using Common;
using Common.ServiceMessages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppPhotoSiteImages.Database;

namespace WebAppPhotoSiteImages.Services
{
    public class PhotoMsgService
    {
        DbMgmt _db;

        public PhotoMsgService(DbMgmt dbContext)
        {
            _db = dbContext;
        }

        internal async Task<List<UserProfile>> GetAllUsers()
        {
            List<UserProfile> userList = await _db.UserProfiles.AsNoTracking().ToListAsync();
            return userList;
        }

        internal async Task<UserProfile> GetUser(string name)
        {
            UserProfile user = await _db.UserProfiles.AsNoTracking().Where(u => u.FirstName == name).FirstOrDefaultAsync();
            return user;
        }

        internal async Task<bool> UploadImage(AddImageMsg request)
        {
            ImagePostMsg img = new ImagePostMsg()
            {
                DateCreated = DateTime.UtcNow,
                Description = request.Description,
                HashTag = request.HashTag,
                Image = request.Image,
                ImageTitle = request.ImageTitle
            };

            _db.ImagePostMsgs.Add(img);
            await _db.SaveChangesAsync();

            return true;
        }

        internal async Task<bool> AddCommentToImage(AddImageCommentMsg request)
        {
            ImageCommentMsg imgcomment = new ImageCommentMsg()
            {
                DateCreated = DateTime.UtcNow,
                Comment = request.Comment,
                Rate = request.Rate,
                ImagePostMsgId = request.ImageId
            };

            _db.ImageCommentMsgs.Add(imgcomment);
            await _db.SaveChangesAsync();

            return true;
        }

        internal async Task<List<GetComments>> GetComments(Guid imgId)
        {
            List<GetComments> commentList = await _db.ImageCommentMsgs.Where(c => c.ImagePostMsgId == imgId).
                OrderBy(c => c.DateCreated).Select(c => new GetComments() { Comment = c.Comment, Date = c.DateCreated, User = "", Rate = c.Rate }).ToListAsync();
            return commentList;
        }

        internal async Task<List<Guid>> GetLastImages()
        {
            List<Guid> ids = await _db.ImagePostMsgs.OrderByDescending(i => i.DateCreated).Select(i => i.Id).Take(4).ToListAsync();
            return ids;
        }

        internal async Task<GetImageMsg> GetImageById(Guid id)
        {
            ImagePostMsg img = await _db.ImagePostMsgs.FirstOrDefaultAsync(i => i.Id == id);

            GetImageMsg res = new GetImageMsg();

            if(img != null)
            {
                res.AverageRate = img.AverageRate;
                res.DateCreated = img.DateCreated;
                res.Description = img.Description;
                res.HashTag = img.HashTag;
                res.Image = img.Image;
                res.ImageTitle = img.ImageTitle;
            }

            return res;
        }
    }
}
