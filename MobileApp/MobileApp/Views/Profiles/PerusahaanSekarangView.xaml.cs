using MobileApp.Models.Datas;
using MobileApp.ViewModels;
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
    public partial class PerusahaanSekarangView : ContentPage
    {
        private PerusahaanSekarangViewModel vm;

        public PerusahaanSekarangView()
        {
            InitializeComponent();
            BindingContext = vm = new PerusahaanSekarangViewModel();
        }
    }


    public class PerusahaanSekarangViewModel : BaseViewModel
    {
        public PerusahaanSekarangViewModel()
        {
            Data = Helper.Profile.Karyawan.Perusahaan;
        }

        public PerusahaanKaryawan Data { get; }
    }
}