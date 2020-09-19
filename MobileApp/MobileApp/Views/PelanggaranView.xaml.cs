using MobileApp.Models;
using MobileApp.Models.Datas;
using MobileApp.ViewModels;
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
    public partial class PelanggaranView : ContentPage
    {
        private PelanggaranViewModel vm;


        public PelanggaranView()
        {
            InitializeComponent();
            BindingContext = vm = new PelanggaranViewModel();
        }

        private async void onSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Task.Delay(200);
            var senderItem = sender as ListView;
            if(senderItem.SelectedItem != null)
            {
               await Navigation.PushModalAsync(new PelaggaranDetailView(senderItem.SelectedItem));
                senderItem.SelectedItem = null;
            }
        }

     
        private  void close_tab(object sender, EventArgs e)
        {
          Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }


    public class PelanggaranViewModel : BaseViewModel
    {

        public ObservableCollection<Pelanggaran> SourceView { get; set; } = new ObservableCollection<Pelanggaran>();

        private object selected;

        public object Selected
        {
            get { return selected; }
            set { SetProperty(ref selected, value); }
        }

        public Command LoadCommand { get; }

        public PelanggaranViewModel()
        {
            LoadCommand = new Command(Load);
            LoadCommand.Execute(null);
        }

        private async void Load()
        {
            try
            {
                var source = await Pelanggarans.GetItemsAsync();
                if (source != null)
                {
                    SourceView.Clear();
                    foreach (var item in source)
                    {
                        SourceView.Add(item);
                    }
                }

                IsBusy = false;
            }
            catch (Exception ex)
            {
                Helper.ErrorMessage(ex.Message);
            }
        }
    }
}