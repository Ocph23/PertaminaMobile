using MobileApp.Models;
using MobileApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;



namespace MobileApp.Views
{


    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        private LoginViewModel vm;

        public LoginView()
        {
            InitializeComponent();
            this.BindingContext =vm = new LoginViewModel(Navigation);


        }

        private void passwordChange(object sender, TextChangedEventArgs e)
        {
            var pwd = (Entry)sender;
            vm.Password = pwd.Text;
        }
    }



    public class LoginViewModel: MobileApp.ViewModels.BaseViewModel
    {
        public LoginViewModel(INavigation navigation)
        {
            LoginCommand = new Command(LoginAction, LoginValidate);
            GoogleLoginCommand = new Command(GoogleLoginAction, GoogleLoginValidate);
            _nav = navigation;

            MessagingCenter.Subscribe<IAuthService, UserProfile>(this, "UserLogin", (sender, arg)=> {

                if (arg != null)
                {
                    AuthService.Profile = arg;
                    App.Current.MainPage = new MainPage();
                }
                IsBusy = false;
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

        private INavigation _nav;
        private string userName;
        private string password;

    }
}