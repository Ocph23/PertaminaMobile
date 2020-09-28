using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Profiles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordView : ContentPage
    {
        public ChangePasswordView()
        {
            InitializeComponent();
            BindingContext = new ChangePasswordViewModel();
        }

        private void closetap(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void eyeOpen(object sender, EventArgs e)
        {
          
        }

        private void tapOldPassword(object sender, EventArgs e)
        {
            var label = sender as Label;

            label.Text = oldPassword.IsPassword ? "\xf06e" : "\xf070";
            oldPassword.IsPassword = !oldPassword.IsPassword;
        }
        private void tapNewPassword(object sender, EventArgs e)
        {
            var label = sender as Label;

            label.Text = newPassword.IsPassword ? "\xf06e" : "\xf070";
            newPassword.IsPassword = !newPassword.IsPassword;
        }
        private void tapConfirmPassword(object sender, EventArgs e)
        {
            var label = sender as Label;

            label.Text = confirmPassword.IsPassword ? "\xf06e" : "\xf070";
            confirmPassword.IsPassword = !confirmPassword.IsPassword;
        }
    }


    public class ChangePasswordViewModel : MobileApp.ViewModels.BaseViewModel
    {


        public ChangePasswordViewModel()
        {
            SaveCommand = new Command(SaveAction, SaveValidation);
        }


        #region Methods

        private async void SaveAction(object obj)
        {

            try
            {
                IsBusy = true;
                var changed = await AuthService.ChangePassword(OldPassword, NewPassword);
                if (changed)
                   await Application.Current.MainPage.Navigation.PopModalAsync();
                
            }
            catch (Exception ex)
            {
                Helper.ErrorMessage(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool SaveValidation(object arg)
        {
            if (string.IsNullOrEmpty(OldPassword) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(ConfirmPassword))
                return false;

            if (NewPassword != ConfirmPassword)
            {
                ErrorMessage = "New Password tidak sama dengan Confirm Password ";
                return false;
            }

            return true;
        }
        private async void OnChangeData()
        {
            await Task.Delay(20);
            ErrorMessage = string.Empty;
            SaveCommand = new Command(SaveAction, SaveValidation);

        }

        #endregion

        #region Properties

        private string oldPassword;

        public string OldPassword
        {
            get { return oldPassword; }
            set { SetProperty(ref oldPassword, value); OnChangeData(); }
        }

        private string newPassword;

        public string NewPassword
        {
            get { return newPassword; }
            set { newPassword = value; OnChangeData(); }
        }


        private string confirmPassword;

        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { confirmPassword = value; OnChangeData(); }
        }



        private string errorMessage;

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage , value); }
        }


        private Command saveCommand;

        public Command SaveCommand
        {
            get { return saveCommand; }
            set { SetProperty(ref saveCommand , value); }
        }



        #endregion

    }
}