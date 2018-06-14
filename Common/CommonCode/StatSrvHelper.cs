using Common.CommonCode;
using Common.ServiceMessages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common.CommonCode
{
    public class StatSrvHelper
    {
        HttpRequestHelper _restCallStat;

        public StatSrvHelper(HttpRequestHelper restCallStat)
        {
            _restCallStat = restCallStat;
        }

        public async Task<bool> AddStatAction(AddActionMsg msg)
        {
            try
            {
                string jsonToPost = JsonConvert.SerializeObject(msg);
                HttpContent content = new StringContent(jsonToPost, Encoding.UTF8, "application/json");

                bool res = await _restCallStat.CallRequest<bool>("addaction", content);
                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
