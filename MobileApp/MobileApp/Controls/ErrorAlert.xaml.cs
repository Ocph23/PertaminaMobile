using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ErrorAlert :  PopupPage
    {
        public ErrorAlert(string msg)
        {
            InitializeComponent();
            this.BackgroundColor = new Color(0, 0, 0, 0.5);
            this.message.Text = msg;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void HidePopup()
        {
            await Task.Delay(50);

            if (PopupNavigation.Instance.PopupStack.Contains(this))
                await PopupNavigation.Instance.RemovePageAsync(this);
        }

        private void OnClose(object sender, EventArgs e)
        {
            HidePopup();
        }
    }

}