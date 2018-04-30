﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Common.CommonCode
{
    public class HttpRequestHelper
    {
        string _serviceUrl = "";
        string _apiControllerRelUrl = "";
        string FullSrvUrl = "";

        public HttpRequestHelper(string srvUrl, string apiControllerUrl)
        {
            _serviceUrl = srvUrl;
            _apiControllerRelUrl = apiControllerUrl;
            FullSrvUrl = $"{_serviceUrl}{_apiControllerRelUrl}";
        }

        public async Task<bool> CallRequest(string methodName, HttpContent param, byte[] file = null, bool isPost = true, string getParams = "")
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{FullSrvUrl}{methodName}";

                try
                {
                    HttpResponseMessage msg = null;
                    if (isPost)
                    {
                        if (file != null)
                        {
                            MultipartFormDataContent request = new MultipartFormDataContent();
                            ByteArrayContent content = new ByteArrayContent(file);
                            content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                            request.Add(content);
                            request.Add(param);

                            msg = await client.PostAsync(url, request);
                        }
                        else
                        {
                            msg = await client.PostAsync(url, param);
                        }
                      
                    }
                    else
                    {
                        if (getParams != "")
                            getParams = "/" + getParams;

                        msg = await client.GetAsync(string.Format("{0}{1}", url, getParams));
                    }

                    if (msg != null && msg.Content != null && msg.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string httpResult = await msg.Content.ReadAsStringAsync();
                        bool requestVal = JsonConvert.DeserializeObject<bool>(httpResult);
                        return requestVal;
                    }
                    else
                    {
                        throw new Exception(string.Format("HttpReponse Content is null: {0}", msg.ReasonPhrase));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Http call error: {ex.Message}");
                }
            }
        }
    }
}
