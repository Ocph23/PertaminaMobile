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
    public partial class AddDetailPelanggaranView : ContentPage
    {
        public AddDetailPelanggaranView()
        {
            InitializeComponent();
            BindingContext = new AddDetailPelanggaranViewModel();
        }

        private void closetap(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }

    public class AddDetailPelanggaranViewModel : BaseViewModel
    {

        public ObservableCollection<LevelGroup> Items { get; set; } = new ObservableCollection<LevelGroup>();
        public AddDetailPelanggaranViewModel()
        {
            LoadCommand = new Command(LoadAction);
            LaporkanCommand = new Command(LaporkanAction);
            CancelCommand = new Command(CancelAction);
            LoadCommand.Execute(null);
        }


        private void CancelAction(object obj)
        {
            MessagingCenter.Send(new List<DetailLevel>(), "DetailPelanggaran");
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void LaporkanAction(object obj)
        {
            var list = new List<DetailLevel>();
            foreach (var item in Items)
            {
                foreach (var data in item)
                {
                    if (data.Selected)
                        list.Add(data);
                }
            }



            if (list.Count <= 0)
            {
                Helper.ErrorMessage("Anda Belum Memilih Jenis Pelanggaran");
                return;
            }

           await  Application.Current.MainPage.Navigation.PopModalAsync();
            MessagingCenter.Send(list, "DetailPelanggaran");

        }

        public Command LoadCommand { get; }
        public Command LaporkanCommand { get; }
        public Command CancelCommand { get; }

        private async void LoadAction(object obj)
        {
            try
            {
                var datas = await LevelPelanggaran.GetItemsAsync();
                if (datas != null)
                {
                    Items.Clear();

                    foreach (var item in datas)
                    {
                        Items.Add(new LevelGroup(item.Nama, item.Datas));
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorMessage(ex.Message);
            }
        }
    }



    public class LevelGroup: ObservableCollection<DetailLevel>
    {

        public string Name { get; private set; }

        public LevelGroup(string name)
            : base()
        {
            Name = name;
        }

        public LevelGroup(string name, IEnumerable<DetailLevel> source)
            : base(source)
        {
            Name = name;
        }
    }


}