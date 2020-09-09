
using MobileApp.Models;
using MobileApp.Models.Datas;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Profiles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AbsenView : ContentPage
    {
        private AbsenViewModel viemodel;

        public AbsenView()
        {
            InitializeComponent();
            BindingContext = viemodel = new AbsenViewModel();
        }


        public class AbsenViewModel : MobileApp.ViewModels.BaseViewModel
        {
            public AbsenViewModel()
            {
                Load();
            }

            public IEnumerable<Periode> DataPeriode { get; private set; }

            private async void Load()
            {
                DataPeriode = await Periodes.GetItemsAsync();

            }
        }
    }
}