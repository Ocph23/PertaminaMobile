using MobileApp.Models.Datas;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views.Profiles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddmelaporkanView : ContentPage
    {
        private AddmelaporkanViewModel vm;

        public AddmelaporkanView(object selectedItem)
        {
            InitializeComponent();
            var karyawan = (MobileApp.Models.Datas.Karyawan)selectedItem;
            BindingContext = vm = new AddmelaporkanViewModel(karyawan);
        }

        private void closetap(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void deleteImage(object sender, EventArgs e)
        {
            var selected = TheCarousel.CurrentItem as BuktiPelanggaran;
            vm.Files.Remove(selected);
            Device.BeginInvokeOnMainThread( async() =>
            {
               await Task.Delay(100);
                if (vm.Files.Count == 0)
                {
                    TheCarousel.CurrentItem = null;
                }
            });

        }
        private void showImage(object sender, EventArgs e)
        {
            var image = (Image)sender;
            var page = new ImageView(image.Source);
            Navigation.PushModalAsync(page);
        }
    }


    public class AddmelaporkanViewModel : ViewModels.BaseViewModel
    {
        public Pelanggaran Data { get; set; }
        public ObservableCollection<BuktiPelanggaran> Files { get; set; } = new ObservableCollection<BuktiPelanggaran>();
        public ObservableCollection<DetailLevel> Details { get; set; } = new ObservableCollection<DetailLevel>();

        public new Karyawan Karyawan { get; set; }
        public Command AddDetailPelanggaranCommand { get; }
        public Command CameraCommand { get; }
        public Command FolderCommand { get; }
        public Command CancelCommand { get; }
        public Command SaveCommand { get; }

        public AddmelaporkanViewModel(Karyawan karyawan)
        {
            Karyawan = karyawan;
           
            AddDetailPelanggaranCommand = new Command(AddDetailAction);
            CameraCommand = new Command(CameraAction);
            FolderCommand = new Command(FolderAction);

            CancelCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopModalAsync());
            SaveCommand = new Command(SaveActionAsync);
            Data = new Pelanggaran();
            Data.Files = new List<BuktiPelanggaran>();


            MessagingCenter.Subscribe<List<DetailLevel>>(this, "DetailPelanggaran", async (arg) => {
               await SetDetails(arg);
            });
        }

        private async void SaveActionAsync(object obj)
        {
            try
            {
                if (IsBusy) return;


                IsBusy = true;
                if (Details.Count <= 0 || Files.Count <= 0)
                {
                    Helper.ErrorMessage("Anda Belum Memilih Pelanggaran/Bukti");
                    return;
                }

                var pelangagran = new Pelanggaran()
                {
                    Files = Files,
                    Jenis = PelanggaranType.Pengaduan,
                    TanggalKejadian = DateTime.Now,
                    Tanggal = DateTime.Now,
                    PelaporId = Helper.Profile.Karyawan.Id,
                    TerlaporId = Karyawan.Id,
                    PerusahaanId = Karyawan.Perusahaan.Id,
                    Status =StatusPelanggaran.Baru
                };


                if (Helper.Profile.Roles.Contains("admin")|| Helper.Profile.Roles.Contains("manager"))
                    pelangagran.Jenis = PelanggaranType.Pelanggaran;

                pelangagran.ItemPelanggarans = new List<DetailPelanggaran>();
                foreach (var item in Details)
                {
                    pelangagran.ItemPelanggarans.Add(new DetailPelanggaran { DetailLevelId = item.Id, NilaiKaryawan = item.NilaiKaryawan, NilaiPerusahaan = item.NilaiPerusahaan, Penambahan = item.Penambahan });
                }

               var saved =await Pelanggarans.AddItemAsync(pelangagran);
                if (saved)
                {
                    Helper.InfoMessage("Anda Berhasil Melaporkan Pelanggaran");
                   await Application.Current.MainPage.Navigation.PopModalAsync();
                }
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

        private async Task SetDetails(List<DetailLevel> arg)
        {
            await Task.Delay(500);
            if (arg != null)
            {
                Details.Clear();
                foreach (var item in arg)
                {
                    Details.Add(item);
                }
            }
            IsBusy = false;
        }

        private async void AddDetailAction(object obj)
        {
           
           await Application.Current.MainPage.Navigation.PushModalAsync(new AddDetailPelanggaranView());

        }

        private async void FolderAction(object obj)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                Helper.ErrorMessage(":( Permission not granted to photos.");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });


            if (file == null)
                return;


            var type = file.GetType();
            var bukti = new BuktiPelanggaran() { FileType="Image"};


            var stream = file.GetStream();
           

            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                bukti.Data = ms.ToArray();
            }

            file.Dispose();

            if (Files.Count <= 0)
            {
                Files.Clear();
            }
            Files.Add(bukti);

        }

        private async void CameraAction(object obj)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                Helper.ErrorMessage(":( Permission not granted to photos.");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            var bukti = new BuktiPelanggaran() { FileType = "Image" };


            var stream = file.GetStream();


            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                bukti.Data = ms.ToArray();
            }

            file.Dispose();


            Files.Add(bukti);
        }
    }
}