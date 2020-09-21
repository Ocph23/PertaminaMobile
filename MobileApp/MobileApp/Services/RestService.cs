using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.Services
{
    public class RestService : HttpClient
    {
        public static string DeviceToken { get; set; }
        public static string DeviceID { get; set; }

        public RestService()
        {
            this.MaxResponseContentBufferSize = 52428800;
            this.BaseAddress = new Uri(Helper.Url);
            this.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            if (Helper.Account != null)
            {
                var token = Xamarin.Essentials.SecureStorage.GetAsync("token").Result;
                if(!string.IsNullOrEmpty(token))
                    SetToken(Helper.Account.IdToken);
            }
        }

        public RestService(string apiUrl)
        {
            this.BaseAddress = new Uri(apiUrl);

        }

        public void CekTokenAsync()
        {
        }


        public void SetToken(string token)
        {
            if (token != null)
            {
                //this.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);
                this.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer", token);
            }
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
                if(response.StatusCode== System.Net.HttpStatusCode.Unauthorized)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Xamarin.Forms.Application.Current.MainPage = new MobileApp.Views.LoginView();
                    });
                   throw new SystemException("Silahkan Login Terlebih Dahulu !");
                  
                }

                var content = await response.Content.ReadAsStringAsync();
                if (content.Contains("message"))
                {
                    var result = JsonConvert.DeserializeObject<ErrorMessage>(content);
                    return result.Message;
                }
                else
                {
                    throw new SystemException(content);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }



    public class ErrorMessage
    {
        public string Message { get; set; }
    }


}
