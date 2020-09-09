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
            Photo = AuthService.Profile.PhotoUrl;
            ProfileName = AuthService.Profile.DisplayName;
            ShowDetailCommand = new Command(ShowDetailAction, x => true);
        }

        private void ShowDetailAction(object obj)
        {
            var param = (TypeProfileView)obj ;
            switch (param)
            {
                case TypeProfileView.ChangeProfile:
                    Application.Current.MainPage.Navigation.PushModalAsync(new Profiles.ChangeProfileView());
                    break;
                case TypeProfileView.ChangePassword:
                    Application.Current.MainPage.Navigation.PushModalAsync(new Profiles.ChangePasswordView());
                    break;
                case TypeProfileView.Absen:
                    Application.Current.MainPage.Navigation.PushModalAsync(new Profiles.AbsenView());
                    break;
                case TypeProfileView.Pelanggaran:
                    break;
                case TypeProfileView.Pelaporan:
                    break;
                case TypeProfileView.Perusahaan:
                    break;
                case TypeProfileView.MutasiPerusahaan:
                    break;
                default:
                    break;
            }
        }

        public Uri Photo { get; set; }
        public string ProfileName { get; set; }
        public Command ShowDetailCommand { get; }
        public Command NotifCommand { get; }
    }
}