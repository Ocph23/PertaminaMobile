using MobileApp.Models;
using MobileApp.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;



namespace MobileApp.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        private readonly LoginViewModel vm;

        public LoginView()
        {
            InitializeComponent();
            this.BindingContext =vm = new LoginViewModel();
        }

        private void PasswordChange(object sender, TextChangedEventArgs e)
        {
            var pwd = (Entry)sender;
            vm.Password = pwd.Text;
        }
    }


    public class LoginViewModel: MobileApp.ViewModels.BaseViewModel
    {
        public LoginViewModel()
        {
            LoginCommand = new Command(LoginAction, LoginValidate);
            GoogleLoginCommand = new Command(GoogleLoginAction, GoogleLoginValidate);
            url = Helper.Url;

            MessagingCenter.Subscribe<IAuthService, UserProfile>(this, "UserLogin", async (sender, arg) => {
                try
                {
                    await Task.Delay(200);
                    if (arg != null)
                    {
                        Helper.Account = arg;

                        if (arg.Provider == AuthProvider.Google)
                        {
                           await AuthService.GetProfileByProviderId(arg);
                        }
                        Helper.SetMainPage();
                    }
                    else
                    {
                        throw new SystemException("Anda Tidak Memiliki Akses !");
                    }
                }
                catch (Exception ex)
                {
                    Helper.ErrorMessage(ex.Message);
                    IsBusy = false;
                }
            });
        }

        private bool LoginValidate(object arg)
        {
            if (IsBusy || string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                return false;
            }
         
            return true;
        }

        private void LoginAction(object obj)
        {
            IsBusy = true;
            AuthService.UserName = UserName;
            AuthService.Password = Password;
            AuthService.Login(AuthProvider.UserAndPassword);
        }

        private bool GoogleLoginValidate(object arg)
        {
            return IsBusy ? false: true;
        }

        private async void GoogleLoginAction(object obj)
        {
            IsBusy = true;
           await AuthService.Login( AuthProvider.Google);
        }

        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName , value); LoginCommand = new Command(LoginAction, LoginValidate); }
        }

        public string Password
        {
            get { return password; }
            set { SetProperty(ref password , value); LoginCommand = new Command(LoginAction, LoginValidate); }
        }

        private Command loginCommand;

        public Command LoginCommand
        {
            get { return loginCommand; }
            set {SetProperty(ref loginCommand , value); }
        }

        public Command GoogleLoginCommand { get; private set; }

        private string userName;
        private string password;


        string url;

        public string URL
        {
            get { return url; }
            set { SetProperty(ref url, value); Helper.Url = value; }
        }


    }
}