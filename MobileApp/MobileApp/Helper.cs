using MobileApp.Controls;
using MobileApp.Models;
using MobileApp.Themes;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileApp
{
    public class Helper
    {
        private static UserProfile _account;
        private static Profile _profile;

        //private static string urlDefault= "http://192.168.1.5";
        private static string urlDefault= "http://epenthinkpapua.com";
        private static string url ;

        public static string Url
        {
            get
            {
                if (string.IsNullOrEmpty(url))
                {
                    url= Xamarin.Essentials.SecureStorage.GetAsync("url").Result;
                    if (string.IsNullOrEmpty(url))
                    {
                        url = urlDefault;
                        Xamarin.Essentials.SecureStorage.SetAsync("url", url);
                    }
                }
                return url;
            }
            set
            {
                url = value;
                Xamarin.Essentials.SecureStorage.SetAsync("url", value);

            }
        }

        internal static  Task<bool> SetTheme(Theme theme)
        {
            Application.Current.Resources.MergedDictionaries.Clear();

            switch (theme)
            {
                case Theme.White:
                    Application.Current.Resources.MergedDictionaries.Add(new WhiteTheme());
                    break;
                case Theme.Red:
                    Application.Current.Resources.MergedDictionaries.Add(new RedTheme());
                    break;
                case Theme.Orange:
                    Application.Current.Resources.MergedDictionaries.Add(new OrangeTheme());
                    break;
                default:
                    Application.Current.Resources.MergedDictionaries.Add(new WhiteTheme());
                    break;
            }

            return Task.FromResult(true);

        }

        public static UserProfile Account { get {
                if (_account == null)
                {
                   var accountString =  Xamarin.Essentials.SecureStorage.GetAsync("account").Result;
                    if (!string.IsNullOrEmpty(accountString))
                    {
                        _account = JsonConvert.DeserializeObject<UserProfile>(accountString);
                    }
                }
                return _account;
            } set {
                _account = value;
                Xamarin.Essentials.SecureStorage.SetAsync("account", JsonConvert.SerializeObject(value));
            
            } 
        }


        public static Profile Profile
        {
            get
            {
                if (_profile == null)
                {
                    var accountString = Xamarin.Essentials.SecureStorage.GetAsync("profile").Result;
                    if (!string.IsNullOrEmpty(accountString))
                    {
                        _profile = JsonConvert.DeserializeObject<Profile>(accountString);
                    }
                }

                return _profile;
            }
            set
            {
                _profile = value;
                Xamarin.Essentials.SecureStorage.SetAsync("profile", JsonConvert.SerializeObject(value));

            }
        }


        public static void ErrorMessage(string message)
        {
            //MessagingCenter.Send(new MessagingCenterAlert { Cancel = "OK", Message = message, Title = "Error" }, "message");
            Application.Current.MainPage.Navigation.PushPopupAsync(new Controls.ErrorAlert(message));
        }

        internal static void ShowLemburInput(InputLembur page)
        {
            Application.Current.MainPage.Navigation.PushPopupAsync(page);
        }

        public static void InfoMessage(string message)
        {
            Application.Current.MainPage.Navigation.PushPopupAsync(new Controls.SuccessAlert(message));

        }


        public static void SetMainPage()
        {
            var app =(App)Application.Current;
            app.SetMainPage();
        }


        public static DateTime ConvertToJayapura(DateTime date)
        {
            var tzs = TimeZoneInfo.GetSystemTimeZones();

            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Asia/Jayapura");

            return TimeZoneInfo.ConvertTime(date, tz);
        }

    }


   
}
