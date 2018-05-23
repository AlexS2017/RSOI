using Common;
using Common.DataEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppPhotoSiteImages.Database
{
    public class DbMgmt : DbContext
    {
        public DbMgmt(DbContextOptions<DbMgmt> options) : base(options)
        {                
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<TextMessage> TextMessages { get; set; }

        public DbSet<ImagePostMsg> ImagePostMsgs { get; set; }

        public DbSet<ImageCommentMsg> ImageCommentMsgs { get; set; }

        public DbSet<ImageRating> ImageRatings { get; set; }
    }
}
