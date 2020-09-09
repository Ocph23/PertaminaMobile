using MobileApp.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.Services
{
    public interface IAuthService
    {

        Task Login(AuthProvider provider);
        UserProfile Profile { get; set; }
       
    }


    public interface IAuthInternalService:IAuthService
    {
        Task Login(AuthProvider provider);
        string UserName { get; set; }
        string Password { get; set; }
    }


    public class AuthService : IAuthInternalService, IMessageInfo
    {
        AuthProvider Provider { get; set; }
        public UserProfile Profile { get; set; }
        public string UserName { get ; set; }
        public string Password { get ; set ; }

        public async Task Login(AuthProvider provider)
        {
            try
            {
                switch (provider)
                {
                    case AuthProvider.UserAndPassword:
                        LoginUserPassword();
                        break;
                    case AuthProvider.Google:
                        await DependencyService.Get<IAuthService>().Login(AuthProvider.Google);
                        break;
                    case AuthProvider.Facebook:
                        await DependencyService.Get<IAuthService>().Login(AuthProvider.Facebook);
                        break;
                    case AuthProvider.Windows:
                        await DependencyService.Get<IAuthService>().Login(AuthProvider.Windows);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = ex.Message,
                    Cancel = "OK"
                }, "message");

            }
        }

        private async void LoginUserPassword()
        {

            using (var client = new RestService())
            {
                try
                {
                    var userLogin = new { UserName = UserName, Password = Password };

                    var result = await client.PostAsync("api/user/login", client.GenerateHttpContent(userLogin));
                    if (result.IsSuccessStatusCode)
                    {
                        var tokenString = await result.Content.ReadAsStringAsync();
                        var token = JsonConvert.DeserializeObject<AuthToken>(tokenString);
                        client.SetToken(token.Token);

                        var profileResult = await client.GetAsync("api/user/profile");
                        if (profileResult.IsSuccessStatusCode)
                        {
                            var profileString = await profileResult.Content.ReadAsStringAsync();
                            var profile = JsonConvert.DeserializeObject<Profile>(profileString);
                            Helper.Profile = profile;
                            var datas = new UserProfile
                            {
                                PhotoUrl = new Uri($"{Helper.Url}/Images/profiles/{profile.Karyawan.Photo}"),
                                DisplayName = profile.Karyawan.NamaKaryawan,
                                Email = profile.Karyawan.Email,
                                Id = profile.User.Id,
                                IdToken = token.Token,
                            };
                            Helper.Account = datas;
                            MessagingCenter.Send<IAuthService, UserProfile>(this, "UserLogin", datas);
                        }
                        else
                        {
                            throw new SystemException(await client.Error(result));
                        }
                    }
                    else
                    {
                      throw new SystemException(await client.Error(result));
                    }
                }
                catch (Exception ex)
                {
                    Helper.ErrorMessage(ex.Message);
                }
               

            }
           

        }



    }



    public class AuthToken
    {
        public string Token { get; set; }
    }
}
