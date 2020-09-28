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
    public partial class MelaporkanMenu : ContentPage
    {
        public MelaporkanMenu()
        {
            InitializeComponent();
        }

        private void kejadian(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new MelaporkanKejadian());
        }

        private void pelanggaran(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new MelaporkanView());

        }
    }
}