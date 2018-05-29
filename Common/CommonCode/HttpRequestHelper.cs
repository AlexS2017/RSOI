using IdentityModel.Client;
using Newtonsoft.Json;
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
        string _authSrvUrl = "";

        public HttpRequestHelper(string srvUrl, string apiControllerUrl, string authUrl)
        {
            _serviceUrl = srvUrl;
            _apiControllerRelUrl = apiControllerUrl;
            FullSrvUrl = $"{_serviceUrl}{_apiControllerRelUrl}";
            _authSrvUrl = authUrl;
        }

        public async Task<returnType> CallRequest<returnType>(string methodName, HttpContent param, byte[] file = null, bool isPost = true, string getParams = "", bool useAuth = true) where returnType : new()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{FullSrvUrl}{methodName}";

                try
                {
                    if (useAuth)
                    {
                        string clientName = "client_imgapp_internal";
                        string secret = "secret123";
                        string api = "api_img_internal";

                        TokenClient tokenClient = new TokenClient(_authSrvUrl, clientName, secret);
                        TokenResponse tokenResponse = await tokenClient.RequestClientCredentialsAsync(api);

                        if (tokenResponse.IsError)
                        {
                            throw new UnauthorizedAccessException($"Could not authorize for internal call: {tokenResponse.Exception?.Message}");
                        }

                        client.SetBearerToken(tokenResponse.AccessToken);
                    }

                    HttpResponseMessage msg = null;
                    if (isPost)
                    {
                        if (file != null)
                        {
                            MultipartFormDataContent request = new MultipartFormDataContent();
                            ByteArrayContent content = new ByteArrayContent(file);
                            //content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                            request.Add(content,"myimage","image.jpeg");
                            request.Add(param, "json");

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
                        returnType requestVal = JsonConvert.DeserializeObject<returnType>(httpResult);
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
