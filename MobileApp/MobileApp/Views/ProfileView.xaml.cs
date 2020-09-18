using MobileApp.Models;
using MobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            this.BindingContext = new ProfileViewModel();
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
        }

        private async void SignoutAction(object obj)
        {
           var result =  await Application.Current.MainPage.DisplayAlert("Yakin ?", "Anda Ingin Keluar ?", "Ya", "Tidak");
            if (result)
            {
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
                case TypeProfileView.Pelanggaran:
                   await Application.Current.MainPage.Navigation.PushModalAsync(new PelanggaranView ());
                    break;
                case TypeProfileView.Pelaporan:
                   await Application.Current.MainPage.Navigation.PushModalAsync(new MelaporkanView());
                    break;
                case TypeProfileView.Perusahaan:
                   await Application.Current.MainPage.Navigation.PushModalAsync(new Profiles.PerusahaanSekarangView());
                    break;
                case TypeProfileView.MutasiPerusahaan:
                   await Application.Current.MainPage.Navigation.PushModalAsync(new Profiles.MutasiView());

                    break;
                default:
                    break;
            }
            IsBusy = false;
        }

        public Uri Photo { get; set; }
        public string ProfileName { get; set; }
        public Command ShowDetailCommand { get; }
        public Command SignoutCommand { get; }
        public Command NotifCommand { get; }
    }
}