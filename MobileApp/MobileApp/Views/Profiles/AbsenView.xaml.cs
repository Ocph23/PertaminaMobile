
using MobileApp.Models;
using MobileApp.Models.Datas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private void closetap(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }

    public class AbsenViewModel : MobileApp.ViewModels.BaseViewModel
    {

        public ObservableCollection<Absen> Datas { get; set; } = new ObservableCollection<Absen>();
        public ObservableCollection<Periode> DataPeriode { get; set; } = new ObservableCollection<Periode>();
        public List<Absen> _ItemsFiltered { get; set; } = new List<Absen>();
        public AbsenViewModel()
        {
            LoadCommand = new Command(LoadAction, x => !IsBusy);
            LoadCommand.Execute(null);
        }

        public void Filter(Periode value)
        {
            if (value != null)
            {
                Datas.Clear();
                foreach (var item in _ItemsFiltered.Where(x => x.Masuk.Value >= value.Mulai && x.Masuk <= value.Selesai))
                {
                    Datas.Add(item);

                }
            }
        }
    

    private async void LoadAction(object obj)
        {
            try
            {
                var periodes = await Periodes.GetItemsAsync();
                DataPeriode.Clear();
                foreach (var item in periodes.OrderByDescending(x=>x.Mulai))
                {
                    DataPeriode.Add(item);
                }
                var data = await Absens.GetItemsAsync(true);
                _ItemsFiltered = data.ToList();
                SelectedPeriode = DataPeriode.First();
            }
            catch (Exception ex)
            {
                Helper.ErrorMessage(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }


        public Command LoadCommand { get; }


        private Periode selectedPeriode;

        public Periode SelectedPeriode
        {
            get { return selectedPeriode; }
            set
            {
                SetProperty(ref selectedPeriode, value);

                if (value != null)
                {
                    Filter(value);
                }
            }
        }

    }
}