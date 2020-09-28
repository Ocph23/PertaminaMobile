using MobileApp.Models.Datas;
using MobileApp.Services;
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
    public partial class InputLembur :  PopupPage
    {
        public InputLembur(Absen absen)
        {
            InitializeComponent();
            this.BackgroundColor = new Color(0, 0, 0, 0.5);
            Model = absen;
            BindingContext = this;
        }

        public Absen Model { get; }

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

        private void CancleClick(object sender, EventArgs e)
        {
            HidePopup();
        }

        private void OkClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Model.Deskripsi))
                Helper.ErrorMessage("Anda Wajib Menginput Alasan Lembur !");
            else
            {
                try
                {
                    var absen = DependencyService.Get<IDataStore<Absen>>();
                    absen.AddItemAsync(Model);
                    HidePopup();
                }
                catch (Exception ex)
                {
                    Helper.ErrorMessage(ex.Message);
                }
            }
        }
    }

}