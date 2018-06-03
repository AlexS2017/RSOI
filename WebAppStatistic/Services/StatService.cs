using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.DataEntities;
using Common.ServiceMessages;
using Microsoft.EntityFrameworkCore;
using WebAppPhotoSiteImages.Database;

namespace WebAppStatistic.Services
{
    public class StatService
    {
        DbMgmtStat _db;

        public StatService(DbMgmtStat dbContext)
        {
            _db = dbContext;
        }

        internal async Task<bool> AddAction(AddActionMsg request)
        {
            UserAction ua = new UserAction()
            {
                DateCreated = DateTime.Now,
                Action = request.Action,
                EntityId = request.EntityId,
                UserProfileId = request.UserId,
                UserInfo = request.UserInfo
            };

            _db.UserActions.Add(ua);
            await _db.SaveChangesAsync();

            return true;
        }

        internal async Task<List<GetActionStatMsg>> GetAllStat()
        {
            List<GetActionStatMsg> statlist = await _db.UserActions.OrderByDescending(a => a.DateCreated).Take(100).
                Select(a => new GetActionStatMsg() { Action = a.Action, DateCreated = a.DateCreated, EntityId = a.EntityId, UserId = a.UserProfileId, UserInfo = a.UserInfo}).
                ToListAsync();
            return statlist;
        }
    }
}
