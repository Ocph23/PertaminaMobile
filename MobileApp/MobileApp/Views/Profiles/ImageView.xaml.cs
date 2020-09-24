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
    public partial class ImageView : ContentPage
    {
        public ImageView()
        {
            InitializeComponent();
        }

        private void closetap(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}