
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

            public System.Collections.ObjectModel.ObservableCollection<Absen> Datas { get; set; } = new System.Collections.ObjectModel.ObservableCollection<Absen>();
            public AbsenViewModel()
            {
                LoadCommand = new Command(LoadAction);
                LoadCommand.Execute(null);
            }

            private async void LoadAction(object obj)
            {
                try
                {
                    if (IsBusy)
                        return;


                    DataPeriode = await Periodes.GetItemsAsync();
                    var data = await Absens.GetItemsAsync();
                    Datas.Clear();
                    foreach (var item in data)
                    {
                        Datas.Add(item);
                    }

                    IsBusy = false;
                }
                catch (Exception ex)
                {
                    Helper.ErrorMessage(ex.Message);
                }
            }

            public IEnumerable<Periode> DataPeriode { get; private set; }
            public Command LoadCommand { get; }

         
        }

        private void closetap(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}