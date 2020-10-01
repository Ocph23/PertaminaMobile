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
    public partial class VideosView : ContentPage
    {

        public VideosView(string source)
        {
            InitializeComponent();
            BindingContext = new VideoViewModel { VideoSource =source };
        }

        private void closetap(object sender, EventArgs e)
        {
            if (MediaManager.CrossMediaManager.Current.State == MediaManager.Player.MediaPlayerState.Playing)
            {
                MediaManager.CrossMediaManager.Current.Stop();
            }
            Navigation.PopModalAsync();
        }

        private void PinchGestureRecognizer_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {

        }

        protected override bool OnBackButtonPressed()
        {
            if(MediaManager.CrossMediaManager.Current.State == MediaManager.Player.MediaPlayerState.Playing)
            {
                MediaManager.CrossMediaManager.Current.Stop();
            }
            return base.OnBackButtonPressed();
        }
    }


    public class VideoViewModel : MobileApp.ViewModels.BaseViewModel
    {
        private string videoSource;

        public string VideoSource
        {
            get { return videoSource; }
            set { SetProperty(ref videoSource, value); }
        }

    }
}