using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileApp.Services;
using MobileApp.Views;
using MobileApp.Models;

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
            DependencyService.Register<PelanggaranDataStore>();
            DependencyService.Register<PeriodeDataStore>();
            DependencyService.Register<AuthService>();


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
