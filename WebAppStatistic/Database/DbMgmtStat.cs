using Common;
using Common.DataEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppPhotoSiteImages.Database
{
    public class DbMgmtStat : DbContext
    {
        public DbMgmtStat(DbContextOptions<DbMgmtStat> options) : base(options)
        {                
        }

        public DbSet<UserAction> UserActions { get; set; }

    }
}
