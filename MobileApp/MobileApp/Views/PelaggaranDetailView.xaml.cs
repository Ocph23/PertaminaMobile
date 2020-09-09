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
            BindingContext = viewModels = new PelaggaranDetailViewModel(data);
        }
    }

    public class PelaggaranDetailViewModel : BaseViewModel
    {
        private object Data;

        public PelaggaranDetailViewModel(object data)
        {
            this.Data = data;
        }
    }
}