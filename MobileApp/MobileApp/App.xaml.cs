using System;
using Xamarin.Forms;
using MobileApp.Services;
using MobileApp.Views;
using MobileApp.Models;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.Threading.Tasks;
using MobileApp.Models.Datas;
using System.Globalization;
using Xamarin.Essentials;

namespace MobileApp
{
    public partial class App : Application, INotification
    {
        public App()
        {
            InitializeComponent();
            CultureInfo requestCulture;
            try
            {
                requestCulture = CultureInfo.CreateSpecificCulture("id-ID");
            }
            catch
            {
                requestCulture = CultureInfo.CurrentCulture;
            }
            System.Threading.Thread.CurrentThread.CurrentCulture = requestCulture;

            MessagingCenter.Subscribe<MessagingCenterAlert>(this, "message", async (message) =>
            {
                await Current.MainPage.DisplayAlert(message.Title, message.Message, message.Cancel);
            });

            //Auth 
            MessagingCenter.Subscribe<IAuthService, bool>(this, "signout", async (sender, result) =>
            {
                if (result)
                {
                    Current.MainPage = new NavigationPage(new LoginView());
                    await Task.Delay(200);

                }
            });

            MessagingCenter.Subscribe<INotification, Notification>(this, "notification", (sender, result) =>
            {
                DependencyService.Get<IDataStore<Notification>>().AddItemAsync(result);
            });

            DependencyService.Register<AuthService>();
            DependencyService.Register<PelanggaranDataStore>();
            DependencyService.Register<PeriodeDataStore>();
            DependencyService.Register<AbsenDataStore>();
            DependencyService.Register<KaryawanDataStore>();
            DependencyService.Register<LevelPelangagranDataStore>();
            DependencyService.Register<NotificationDataStore>();
            DependencyService.Register<KejadianDataStore>();

            AppCenter.Start("2531af1d-fce1-49da-94ba-d603aff80d84",
                   typeof(Analytics), typeof(Crashes));


            string themeString = SecureStorage.GetAsync("Theme").Result;
            Theme theme = Theme.White;
            if (!string.IsNullOrEmpty(themeString))
            {
                theme = (Theme)Enum.Parse(typeof(Theme), themeString);
            }
            else
            {
                SecureStorage.SetAsync("Theme", theme.ToString());
            }

            var isSet =  Helper.SetTheme(theme).Result;

            if (Helper.Account != null)
            {
                MainPage = new Views.MainPage();
            }
            else
            {
                MainPage = new LoginView();

            }
        }


        public async  void SetMainPage()
        {
            await Task.Delay(200);
            MainPage =new Views.MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
