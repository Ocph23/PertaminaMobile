using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;   

namespace MobileApp.Services
{
    public class RestService : HttpClient
    {
        public static string DeviceToken { get; set; }

        public RestService()
        {
            this.MaxResponseContentBufferSize = 256000;
            this.BaseAddress = new Uri(Helper.Url);
            this.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            if (Helper.Account != null)
            {
                SetToken(Helper.Account.IdToken);
            }
        }

        public RestService(string apiUrl)
        {
            this.BaseAddress = new Uri(apiUrl);

        }

        public void CekTokenAsync()
        {
            //if (Helper.Account != null)
            //{
            //    this.DefaultRequestHeaders.Authorization =
            //       new AuthenticationHeaderValue("", Helper.Account.Token);
            //}
        }


        public void SetToken(string token)
        {
            if (token != null)
            {
                this.DefaultRequestHeaders.TryAddWithoutValidation("Authorization",
                    token);
                this.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer", token);
            }
        }

        internal Task DeleteAsync(string id, StringContent content)
        {

            throw new NotImplementedException();
        }

        public StringContent GenerateHttpContent(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }


        public async Task<string> Error(HttpResponseMessage response)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                if (content.Contains("message"))
                {
                    var result = JsonConvert.DeserializeObject<ErrorMessage>(content);
                    return result.Message;
                }
                else
                {
                    return content;
                }
            }
            catch (Exception)
            {
                return "Maaf Terjadi Kesalahan, Silahkan Ulangi Lagi Nanti";
            }
        }
    }



    public class ErrorMessage
    {
        public string Message { get; set; }
    }


}
