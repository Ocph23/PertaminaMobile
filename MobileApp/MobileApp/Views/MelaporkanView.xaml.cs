using MobileApp.Models.Datas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MelaporkanView : ContentPage
    {

        public MelaporkanView()
        {
            InitializeComponent();
            BindingContext = new MelaporkanViewModel();
        }

        private void onSelect(object sender, SelectedItemChangedEventArgs e)
        {
            var list = (ListView)sender; 

            if(list.SelectedItem !=null)
                Navigation.PushModalAsync(new Profiles.AddmelaporkanView(list.SelectedItem));
            list.SelectedItem = null;
        }
    }


    public class MelaporkanViewModel : MobileApp.ViewModels.BaseViewModel
    {
      
        private ObservableCollection<Karyawan> _Items;
        private ObservableCollection<Karyawan> _ItemsFiltered;
        private readonly ObservableCollection<Karyawan> _ItemsUnfiltered = new ObservableCollection<Karyawan>();
        private string _searchText;


        public string SearchText
        {
            get { return _searchText; }
            set
            {
                SetProperty(ref _searchText, value);
            }
        }
        public ObservableCollection<Karyawan> SourceView
        {
            get { return _Items; }
            set
            {
              SetProperty(ref  _Items , value);
            }
        }

        public MelaporkanViewModel()
        {
            SourceView = new ObservableCollection<Karyawan>();
            LoadCommand = new Command(LoadAction);
            SearchCommand = new Command(SearchAction);
            LoadCommand.Execute(null);
        }

        private void SearchAction(object obj)
        {
            if (string.IsNullOrWhiteSpace(this._searchText))
                SourceView = new ObservableCollection<Karyawan>(_ItemsUnfiltered.Where(x=>x.Id != Helper.Profile.Karyawan.Id).ToList());
            else
            {
                _ItemsFiltered = new ObservableCollection<Karyawan>(
                    _ItemsUnfiltered.Where(i => i.Id != Helper.Profile.Karyawan.Id && i.NamaKaryawan.ToLower().Contains(_searchText.ToLower())));
                SourceView = _ItemsFiltered;
            }
        }

        public Command LoadCommand { get; }
        public Command SearchCommand { get; }

        private async void LoadAction(object obj)
        {
            try
            {
                if (IsBusy)
                    return;

                var data = await Karyawan.GetItemsAsync();
                if(data!=null)
                {
                    _ItemsUnfiltered.Clear();
                    foreach (var item in data)
                    {
                        _ItemsUnfiltered.Add(item);
                    }

                    SourceView = new ObservableCollection<Karyawan>(_ItemsUnfiltered.Where(x=>x.Id != Helper.Profile.Karyawan.Id));
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Helper.ErrorMessage(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }


    }
}