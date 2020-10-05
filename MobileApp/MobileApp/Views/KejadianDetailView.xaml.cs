using MediaManager;
using MobileApp.Models.Datas;
using MobileApp.ViewModels;
using Plugin.Media.Abstractions;
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
    public partial class KejadianDetailView : ContentPage
    {
        private KejadianDetailViewModel viewModels;
        private Kejadian dataModel;

        public KejadianDetailView(object data)
        {
            InitializeComponent();
            dataModel = data as Kejadian;
            BindingContext = viewModels = new KejadianDetailViewModel(dataModel);
        }

        private void closetap(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void showImage(object sender, EventArgs e)
        {
            viewModels.ShowImage();
        }

        private void CarouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            viewModels.SetSelected = (BuktiKejadian)e.CurrentItem;
        }
    }

    public class KejadianDetailViewModel : BaseViewModel
    {
        public Kejadian Data { get; set; }
        public BuktiKejadian SetSelected { get;  set; }

        public KejadianDetailViewModel(Kejadian data)
        {
            this.Data = data;
        }

        internal void ShowImage()
        {
            if (SetSelected != null)
            {

                if(SetSelected.FileType.ToLower().Contains("image"))
                {
                    var source = new Uri(SetSelected.FileView);
                    var page = new Profiles.ImageView(source);
                    Application.Current.MainPage.Navigation.PushModalAsync(page);
                }else if (SetSelected.FileType.ToLower().Contains("video"))
                {
                    var page = new Profiles.VideosView(SetSelected.FileView);
                    Application.Current.MainPage.Navigation.PushModalAsync(page);
                   
                }

            }
        }
    }
}