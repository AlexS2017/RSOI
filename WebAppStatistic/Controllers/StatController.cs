using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.ServiceMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppStatistic.Services;

namespace WebAppStatistic.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class StatController : Controller
    {
        StatService _srv;

        public StatController(StatService srv)
        {
            _srv = srv;
        }

        [HttpPost("addaction")]
        public async Task<bool> AddAction([FromBody] AddActionMsg request)
        {
            return await _srv.AddAction(request);
        }

        [HttpGet("getallstat")]
        public async Task<List<GetActionStatMsg>> GetAllStat()
        {
            return await _srv.GetAllStat();
        }
        
    }
}
