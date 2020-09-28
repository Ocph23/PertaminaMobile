using MobileApp.Models.Datas;
using MobileApp.ViewModels;
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
            var image = (Image)sender;
            var fn = image.Source.ToString().Split(' ')[1];
            var imageData = dataModel.Files.Where(x => x.ThumbView == fn).FirstOrDefault();
            
            var source = imageData==null?image.Source: new Uri(imageData.FileView);
            var page = new MobileApp.Views.Profiles.ImageView(source);
            Navigation.PushModalAsync(page);
        }
    }

    public class PelaggaranDetailViewModel : BaseViewModel
    {
        public Pelanggaran Data { get; set; }

        public PelaggaranDetailViewModel(Pelanggaran data)
        {
            this.Data = data;
        }
    }
}