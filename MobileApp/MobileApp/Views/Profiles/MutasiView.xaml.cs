using MobileApp.Models.Datas;
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
    public partial class MutasiView : ContentPage
    {
        public MutasiView()
        {
            InitializeComponent();
            Datas = new System.Collections.ObjectModel.ObservableCollection<PerusahaanKaryawan>(Helper.Profile.Karyawan.Perusahaans);
            BindingContext = this;
        }

        public System.Collections.ObjectModel.ObservableCollection<PerusahaanKaryawan> Datas { get; }

        private void closetap(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var list = (ListView)sender;
            var data = list.SelectedItem as PerusahaanKaryawan;
            Application.Current.MainPage.Navigation.PushModalAsync(new PerusahaanSekarangView(data));
        }
    }
}