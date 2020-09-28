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

        public ImageView(ImageSource source)
        {
            InitializeComponent();
            BindingContext = new ImageViewModel { Photo = source };
        }

        private void closetap(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void PinchGestureRecognizer_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {

        }



    }


    public class ImageViewModel : MobileApp.ViewModels.BaseViewModel
    {
        private ImageSource photo;

        public ImageSource Photo
        {
            get { return photo; }
            set {SetProperty(ref photo ,value); }
        }

    }
}