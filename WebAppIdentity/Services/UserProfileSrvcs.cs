using Common;
using Common.ServiceMessages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppIdentity.Data;

namespace WebAppIdentity.Services
{
    public class UserProfileSrvcs
    {
        ApplicationDbContext _db;

        public UserProfileSrvcs(ApplicationDbContext db)
        {
            _db = db;
        }

        internal async Task<List<GetUserProfileMsg>> GetAllUsers()
        {
            List<GetUserProfileMsg> userList = await _db.UserProfiles.AsNoTracking().
                Select(u => new GetUserProfileMsg() { Email = u.Email, FirstName = u.FirstName, LastName = u.LastName }).ToListAsync();
            return userList;
        }

        internal async Task<GetUserProfileMsg> GetUser(string name)
        {
            GetUserProfileMsg user = await _db.UserProfiles.AsNoTracking().Where(u => u.FirstName == name).
                Select(u => new GetUserProfileMsg() { Email = u.Email, FirstName = u.FirstName, LastName = u.LastName }).FirstOrDefaultAsync();
            return user;
        }

        internal async Task<GetUserProfileMsg> GetUser(Guid id)
        {
            GetUserProfileMsg user = await _db.UserProfiles.AsNoTracking().Where(u => u.Id == id).
                Select(u => new GetUserProfileMsg() { Email = u.Email, FirstName = u.FirstName, LastName = u.LastName }).FirstOrDefaultAsync();
            return user;
        }


        internal async Task<bool> AddUser(AddUserProfileMsg request)
        {
            UserProfile up = new UserProfile()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Id = request.Id,
                Created = DateTime.Now
            };

            _db.UserProfiles.Add(up);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
