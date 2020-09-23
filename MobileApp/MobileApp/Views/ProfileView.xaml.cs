using MobileApp.Models;
using MobileApp.Services;
using MobileApp.ViewModels;
using System;
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
            GoogleCommand = new Command(GooleAccountAction);

            MessagingCenter.Subscribe<IAuthService, UserProfile>(this, "UserLink", async (sender, arg) => {
                try
                {
                    await Task.Delay(200);
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

        private void GooleAccountAction(object obj)
        {
            DependencyService.Get<IAuthService>().LinkAccount(AuthProvider.Google);
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
                   await Application.Current.MainPage.Navigation.PushModalAsync(new Views.PelanggaranView());
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


        private string googleAccount = "Hubungkan ...";

        public string GoogleAccount
        {
            get {
                var google= Xamarin.Essentials.SecureStorage.GetAsync("GoogleAccount").Result;
                if (string.IsNullOrEmpty(google))
                    return googleAccount;
                return google; }
            set {
                SetProperty(ref  googleAccount , value);
                Xamarin.Essentials.SecureStorage.SetAsync("GoogleAccount", value);
            }
        }

    }
}