using MobileApp.Services;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {
        private int pressed;

        public MainPage()
        {
            InitializeComponent();
            pressed = 0;
        }


        protected override bool OnBackButtonPressed()
        {
            if(pressed<1)
            {
                DependencyService.Get<IAuthService>().ToastShow("Tekan sekali lagi untuk keluar");
            }


            if (pressed >= 1)
            {
                return base.OnBackButtonPressed();
            }

            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(2000);
                pressed = 0;
            });
            pressed++;
            return true;
        }
    }



    
}