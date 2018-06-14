using Common.ServiceMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppIdentity.Services;

namespace WebAppIdentity.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserProfileSrvcs _upsrv;

        public UserController(UserProfileSrvcs upsrv)
        {
            _upsrv = upsrv;
        }

        [HttpGet("getusers")]
        public async Task<List<GetUserProfileMsg>> GetUsers()
        {
            List<GetUserProfileMsg> res = await _upsrv.GetAllUsers();
            return res;
        }

        [HttpGet("getuserbylastname/{name}")]
        public async Task<GetUserProfileMsg> GetUserByLastName(string name)
        {
            GetUserProfileMsg res = await _upsrv.GetUser(name);
            return res;
        }

        [HttpGet("getuserbyid/{id}")]
        public async Task<GetUserProfileMsg> GetUserById(Guid id)
        {
            GetUserProfileMsg res = await _upsrv.GetUser(id);
            return res;
        }

        //[HttpPost("adduser")]
        //public async Task<bool> AddUser([FromBody] AddUserProfileMsg request)
        //{
        //    bool res = await _upsrv.AddUser(request);
        //    return res;
        //}

        [HttpGet("checksrvstatus")]
        [AllowAnonymous]
        public bool CheckSrvStatus()
        {
            return true;
        }
    }
}
