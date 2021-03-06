﻿using MobileApp.Controls;
using MobileApp.Models;
using MobileApp.Services;
using MobileApp.ViewModels;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileView : ContentPage
    {
        public ProfileView()
        {
            InitializeComponent();
            string themeString = Xamarin.Essentials.SecureStorage.GetAsync("Theme").Result;
            if(!string.IsNullOrEmpty(themeString))
                themeLabel.Text = $"Theme ({themeString.ToString()})";
            this.BindingContext = new ProfileViewModel();
        }

        private void showTheme(object sender, EventArgs e)
        {
            var source = Enum.GetValues(typeof(Theme));
            themePicker.ItemsSource=source;
            themePicker.Focus();
        }

        private void themePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            if (picker.SelectedItem != null)
            {
                var theme = (Theme)picker.SelectedItem;
                 Xamarin.Essentials.SecureStorage.SetAsync("Theme", theme.ToString());
                Helper.InfoMessage("Silahkan Logout Untuk Mengubah Thema");
                themeLabel.Text = $"Theme ({theme.ToString()})";
            }
               
        }
    }

    public class ProfileViewModel:BaseViewModel
    {
        public ProfileViewModel()
        {
            Photo = Helper.Account.PhotoUrl;
            ProfileName = Helper.Account.DisplayName;
            ShowDetailCommand = new Command(ShowDetailAction, x => true);
            SignoutCommand = new Command(SignoutAction, x => true);
            GoogleCommand = new Command(GooleAccountAction);

            MessagingCenter.Subscribe<UserProfile>(this,"UserLink", async ( arg) => {
                try
                {
                    await Task.Delay(2000);
                    if (arg != null)
                    {
                        Helper.Account = arg;

                        if (arg.Provider == AuthProvider.Google)
                        {
                            await AuthService.JoinExternalUSer(arg.Id, arg.Provider);
                            GoogleAccount = arg.Email;
                        }
                    }
                    else
                    {
                        Helper.ErrorMessage("Account Anda Gagal Di Hubungkan !");
                    }
                }
                catch (Exception ex)
                {
                    Helper.ErrorMessage(ex.Message);
                    IsBusy = false;
                }
            });


        }

        private async void GooleAccountAction(object obj)
        {
            if(GoogleAccount==googleAccount)
                await   DependencyService.Get<IAuthService>().LinkAccount(AuthProvider.Google);
        }

        private async void SignoutAction(object obj)
        {
           var result =  await Application.Current.MainPage.DisplayAlert("Yakin ?", "Anda Ingin Keluar ?", "Ya", "Tidak");
            if (result)
            {
                string themeString = Xamarin.Essentials.SecureStorage.GetAsync("Theme").Result;
                Theme theme = (Theme)Enum.Parse(typeof(Theme), themeString);
               await Helper.SetTheme(theme);
               await AuthService.SignOut();
            }
        }

        private async void ShowDetailAction(object obj)
        {

            IsBusy = true;
           await Task.Delay(300);
            var param = (TypeProfileView)obj ;
            switch (param)
            {
                case TypeProfileView.ChangeProfile:
                   await Application.Current.MainPage.Navigation.PushModalAsync(new Profiles.ChangeProfileView());
                    break;
                case TypeProfileView.ChangePassword:
                   await Application.Current.MainPage.Navigation.PushModalAsync(new Profiles.ChangePasswordView());
                    break;
                case TypeProfileView.Absen:
                   await Application.Current.MainPage.Navigation.PushModalAsync(new Profiles.AbsenView());
                    break;
                case TypeProfileView.Kejadian:
                    var pelanggaranPage = new Views.Profiles.DataKejadianView();
                   await Application.Current.MainPage.Navigation.PushModalAsync(pelanggaranPage);
                    break;
                case TypeProfileView.Pelaporan:
                   await Application.Current.MainPage.Navigation.PushModalAsync(new MobileApp.Views.Profiles.DataMelaporkanView());
                    break;
                case TypeProfileView.Perusahaan:
                   await Application.Current.MainPage.Navigation.PushModalAsync(new Profiles.PerusahaanSekarangView());
                    break;
                case TypeProfileView.MutasiPerusahaan:
                   await Application.Current.MainPage.Navigation.PushModalAsync(new Profiles.MutasiView());
                    break;

                case TypeProfileView.About:
                    await Application.Current.MainPage.Navigation.PushModalAsync(new AboutPage());
                    break;
                default:
                    break;
            }
            IsBusy = false;
        }

        public string Photo { get; set; }
        public string ProfileName { get; set; }
        public Command ShowDetailCommand { get; }
        public Command SignoutCommand { get; }
        public Command GoogleCommand { get; }
        public Command NotifCommand { get; }


        private string googleAccount = "Hubungkan ...!";

        public string GoogleAccount
        {
            get {
                if (Helper.Profile.ExternalLogin != null && Helper.Profile.ExternalLogin.Count>0)
                {
                    return Helper.Profile.ExternalLogin.FirstOrDefault().ProviderKey;
                }
                return googleAccount; }
            set {
                SetProperty(ref  googleAccount , value);
            }
        }

    }
}