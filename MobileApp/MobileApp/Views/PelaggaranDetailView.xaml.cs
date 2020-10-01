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
    public partial class PelaggaranDetailView : ContentPage
    {
        private PelaggaranDetailViewModel viewModels;
        private Pelanggaran dataModel;

        public PelaggaranDetailView(object data)
        {
            InitializeComponent();
            dataModel = data as Pelanggaran;
            BindingContext = viewModels = new PelaggaranDetailViewModel(dataModel);
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
            viewModels.SetSelected = (BuktiPelanggaran)e.CurrentItem;
        }
    }

    public class PelaggaranDetailViewModel : BaseViewModel
    {
        public Pelanggaran Data { get; set; }
        public BuktiPelanggaran SetSelected { get;  set; }

        public PelaggaranDetailViewModel(Pelanggaran data)
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