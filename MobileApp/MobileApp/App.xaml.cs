using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileApp.Services;
using MobileApp.Views;
using MobileApp.Models;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace MobileApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<MessagingCenterAlert>(this, "message", async (message) =>
            {
                await Current.MainPage.DisplayAlert(message.Title, message.Message, message.Cancel);
            });
            
            //DataStore Register
            DependencyService.Register<AuthService>();
            DependencyService.Register<PelanggaranDataStore>();
            DependencyService.Register<PeriodeDataStore>();
            DependencyService.Register<AbsenDataStore>();
            DependencyService.Register<KaryawanDataStore>();
            DependencyService.Register<LevelPelangagranDataStore>();
            AppCenter.Start("2531af1d-fce1-49da-94ba-d603aff80d84",
                   typeof(Analytics), typeof(Crashes));

            MainPage = new NavigationPage(new LoginView());
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
