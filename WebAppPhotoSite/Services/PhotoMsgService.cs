using Common;
using Common.DataEntities;
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

        internal async Task<AddImageResponse> UploadImage(AddImageMsg request)
        {
            ImagePostMsg img = new ImagePostMsg()
            {
                DateCreated = DateTime.Now,
                Description = request.Description,
                HashTag = request.HashTag,
                Image = request.Image,
                ImageTitle = request.ImageTitle,
                UserId = request.UserId
            };

            _db.ImagePostMsgs.Add(img);
            await _db.SaveChangesAsync();

            AddImageResponse res = new AddImageResponse() { ImageId=img.Id, IsSuccess = true };
            return res;
        }

        internal async Task<AddImageCommentResponse> AddCommentToImage(AddImageCommentMsg request)
        {
            ImageCommentMsg imgcomment = new ImageCommentMsg()
            {
                DateCreated = DateTime.Now,
                Comment = request.Comment,
                ImagePostMsgId = request.ImageId,
                UserId = request.UserId
            };

            _db.ImageCommentMsgs.Add(imgcomment);
            await _db.SaveChangesAsync();

            await SaveRating(new SaveImgRating() { ImageId = request.ImageId, UserId = request.UserId, Rate = request.Rate });

            AddImageCommentResponse res = new AddImageCommentResponse() { CommentId = imgcomment.Id, IsSuccess = true };
            return res;
        }

        internal async Task<List<GetComments>> GetComments(Guid imgId)
        {
            List<GetComments> commentList = await _db.ImageCommentMsgs.Where(c => c.ImagePostMsgId == imgId).
                OrderBy(c => c.DateCreated).Select(c => new GetComments() { Comment = c.Comment, Date = c.DateCreated, User = "", UserId = c.UserId }).ToListAsync();
            return commentList;
        }

        internal async Task<ImagePageInfo> GetImgInfo(GetImgInfoMsg request)
        {
            ImagePageInfo res = new ImagePageInfo();
            List<GetComments> commentList = await GetComments(request.ImageId);
            res.Comments = commentList;

            List<ImageRating> ratings = await _db.ImageRatings.Where(r => r.ImageId == request.ImageId).ToListAsync();
            res.AvgRate = ratings.Count > 0 ? (decimal)ratings.Average(r=>r.Rate) : 0;

            if (request.UserId.HasValue)
            {
                ImageRating rt = ratings.FirstOrDefault(r => r.UserId == request.UserId);
                if(rt != null)
                {
                    res.Rate = rt.Rate;
                }
            }

            return res;
        }

        internal async Task<bool> SaveRating(SaveImgRating request)
        {
            if (request.UserId.HasValue)
            {
                ImageRating rt = await _db.ImageRatings.FirstOrDefaultAsync(r => r.ImageId == request.ImageId && r.UserId == request.UserId);
                if (rt != null)
                {
                    rt.Rate = request.Rate;
                }
                else
                {
                    ImageRating newRating = new ImageRating()
                    {
                        DateCreated = DateTime.Now,
                        ImageId = request.ImageId,
                        UserId = request.UserId.Value,
                        Rate = request.Rate
                    };
                    _db.ImageRatings.Add(newRating);
                }
                await _db.SaveChangesAsync();
            }

            return false;
        }

        internal async Task<List<Guid>> GetLastImages(Guid userId)
        {
            List<Guid> ids = await _db.ImagePostMsgs.Where(i => i.UserId == userId).OrderByDescending(i => i.DateCreated).Select(i => i.Id).Take(4).ToListAsync();
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
