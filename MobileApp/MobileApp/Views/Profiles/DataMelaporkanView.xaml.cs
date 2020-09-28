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

namespace MobileApp.Views.Profiles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataMelaporkanView : ContentPage
    {
        public DataMelaporkanView()
        {
            InitializeComponent();
            BindingContext = new DataMelaporkanViewModel();
        }

        private async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Task.Delay(200);
            var senderItem = sender as ListView;
            if (senderItem.SelectedItem != null)
            {
                await Navigation.PushModalAsync(new PelaggaranDetailView(senderItem.SelectedItem));
                senderItem.SelectedItem = null;
            }
        }

        private void closetap(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }


    public class DataMelaporkanViewModel : BaseViewModel
    {
        public ObservableCollection<Pelanggaran> SourceView { get; set; } = new ObservableCollection<Pelanggaran>();

        private object selected;

        public object Selected
        {
            get { return selected; }
            set { SetProperty(ref selected, value); }
        }

        private bool notHaveResult;
        public bool NotHaveResult
        {
            get { return notHaveResult; }
            set { SetProperty(ref notHaveResult, value); }
        }


        public Command LoadCommand { get; }

        public DataMelaporkanViewModel()
        {
            LoadCommand = new Command(LoadAction);
            LoadCommand.Execute(null);
        }

        private async void LoadAction(object obj)
        {
            try
            {
                var source = await Pelanggarans.GetItemsmelaporkanAsync(true);
                if (source == null || source.Count() <= 0)
                    NotHaveResult = true;
                else
                    NotHaveResult = false;

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
            finally
            {
                IsBusy = false;
            }
        }

    }
}