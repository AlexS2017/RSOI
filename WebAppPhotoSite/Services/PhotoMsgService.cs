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

        public async Task<List<UserProfile>> GetAllUsers()
        {
            List<UserProfile> userList = await _db.UserProfiles.AsNoTracking().ToListAsync();
            return userList;
        }

        public async Task<UserProfile> GetUser(string name)
        {
            UserProfile user = await _db.UserProfiles.AsNoTracking().Where(u => u.FirstName == name).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> UploadImage(AddImageMsg request)
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

        public async Task<bool> AddCommentToImage(AddImageCommentMsg request)
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
    }
}
