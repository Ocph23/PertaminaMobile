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

        public PelaggaranDetailView(object data)
        {
            InitializeComponent();
            var dataModel = data as Pelanggaran;
            BindingContext = viewModels = new PelaggaranDetailViewModel(dataModel);
        }

        private void closetap(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
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