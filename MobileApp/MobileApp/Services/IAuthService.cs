using MobileApp.Models;
using MobileApp.Models.Datas;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp.Services
{
    public interface IAuthService
    {
        Task Login(AuthProvider provider);
        Task SignOut();
        void ToastShow(string message);
    }


    public interface IAuthInternalService:IAuthService
    {
        new Task Login(AuthProvider provider);
        Task GetProfileByProviderId(UserProfile profile);

        Task JoinExternalUSer(string key, AuthProvider provider);

        Task<bool> ChangePassword(string oldPassword, string newPassword);

        string UserName { get; set; }
        string Password { get; set; }
    }


    public class AuthService : IAuthInternalService, IMessageInfo
    {
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

                    var result = await client.PostAsync("/api/user/login", client.GenerateHttpContent(userLogin));
                    if (result.IsSuccessStatusCode)
                    {
                        var tokenString = await result.Content.ReadAsStringAsync();
                        var token = JsonConvert.DeserializeObject<AuthToken>(tokenString);
                        client.SetToken(token.Token);

                        var profileResult = await client.GetAsync("/api/user/profile");
                        if (profileResult.IsSuccessStatusCode)
                        {
                            var profileString = await profileResult.Content.ReadAsStringAsync();
                            var profile = JsonConvert.DeserializeObject<Profile>(profileString);

                            if (profile.Karyawan == null)
                            {
                                throw new SystemException();
                            }

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
                            UpdateDeviceToken(profile.Karyawan);
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
                catch 
                {
                    MessagingCenter.Send<IAuthService, UserProfile>(this, "UserLogin", null);
                    
                }
            }
        }

        public async Task SignOut()
        {
            try
            {
                await DependencyService.Get<IAuthService>().SignOut();
                Helper.Account = null;
                Helper.Profile = null;
                MessagingCenter.Send<IAuthService, bool>(this, "signout", true);
            }
            catch (Exception ex)
            {
                Helper.ErrorMessage(ex.Message);
            }
        }




        public async Task GetProfileByProviderId(UserProfile userProfile)
        {
            using (var client = new RestService())
            {
                try
                {
                    var profileResult = await client.GetAsync($"/api/user/ProfilebyproviderId?Id={userProfile.Id}&provider={userProfile.Provider}");
                    if (profileResult.IsSuccessStatusCode)
                    {
                        var profileString = await profileResult.Content.ReadAsStringAsync();
                        var profile = JsonConvert.DeserializeObject<Profile>(profileString);
                        if (profile.Karyawan == null)
                        {
                            throw new SystemException();
                        }

                        Helper.Profile = profile;
                        var datas = new UserProfile
                        {
                            PhotoUrl = new Uri($"{Helper.Url}/Images/profiles/{profile.Karyawan.Photo}"),
                            DisplayName = profile.Karyawan.NamaKaryawan,
                            Email = profile.Karyawan.Email,
                            Id = profile.User.Id,
                            IdToken = profile.Token,
                        };

                        if (string.IsNullOrEmpty(profile.Karyawan.Photo))
                        {
                            datas.PhotoUrl = new Uri($"{userProfile.PhotoUrl}");
                        }
                        Helper.Account = datas;

                        UpdateDeviceToken(profile.Karyawan);

                    }
                    else
                    {
                        throw new SystemException(await client.Error(profileResult));
                    }
                }
                catch 
                {
                    Helper.ErrorMessage("Anda Tidak Memiliki Akses !");
                }


            }
        }

        private async Task UpdateDeviceToken(Karyawan karyawan)
        {
            if (string.IsNullOrEmpty(karyawan.DeviceId))
            {
                using (var client = new RestService())
                {
                    try
                    {
                        var deviceId = await Xamarin.Essentials.SecureStorage.GetAsync("DeviceToken");
                        var resutl = await client.GetAsync($"/api/karyawan/changedevice/{karyawan.Id}/{deviceId}");
                        if (!resutl.IsSuccessStatusCode) { 
                            throw new SystemException(await client.Error(resutl));
                        }
                    }
                    catch
                    {
                        Helper.ErrorMessage("Device Anda Tidak Berhasil Ditambahkan, Coba Ulangi Lagi !");
                    }
                }

            }
        }

        public void ToastShow(string message)
        {
            throw new NotImplementedException();
        }

        public async Task JoinExternalUSer(string key, AuthProvider provider)
        {
            using (var client = new RestService())
            {
                try
                {
                    var resutl = await client.GetAsync($"/api/user/jointexternalUser?key={key}&provider={provider.ToString()}");
                    if (resutl.IsSuccessStatusCode)
                    {
                        var restulString = await resutl.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<bool>(restulString);
                        if (data)
                        {
                            Helper.InfoMessage("Account berhasil dihubungkan !");
                        }
                    }
                    else
                    {
                        throw new SystemException(await client.Error(resutl));
                    }
                }
                catch
                {
                    Helper.ErrorMessage("Tidak Berhasil, Coba Ulangi lagi  !");
                }
            }
        }


        public async Task<bool> ChangePassword(string oldPassword, string newPassword)
        {
            using (var client = new RestService())
            {
                try
                {
                    var model = new { OldPassword = oldPassword, NewPassword = newPassword, UserId = Helper.Account.Id };
                    var resutl = await client.PostAsync($"/api/user/changepassword",client.GenerateHttpContent(model));
                    if (resutl.IsSuccessStatusCode)
                    {
                        var restulString = await resutl.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<bool>(restulString);
                        if (data)
                        {
                            Helper.InfoMessage("Password Berhasil Diubah!");
                            return true;
                        }
                    }
                    throw new SystemException(await client.Error(resutl));
                }
                catch
                {
                    throw new SystemException("Tidak Berhasil, Diubah !");
                }
            }
        }
    }

    public class AuthToken
    {
        public string Token { get; set; }
    }
}
