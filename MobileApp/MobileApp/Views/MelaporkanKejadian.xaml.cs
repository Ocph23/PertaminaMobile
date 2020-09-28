using MobileApp.Models.Datas;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MelaporkanKejadian : ContentPage
    {
        private MelaporkanKejadianViewModel vm;

        public MelaporkanKejadian()
        {
            InitializeComponent();
            BindingContext = vm = new MelaporkanKejadianViewModel();
        }

        private void closetap(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void deleteImage(object sender, EventArgs e)
        {
            var selected = TheCarousel.CurrentItem as BuktiKejadian;
            vm.Model.Files.Remove(selected);

        }
        private void showImage(object sender, EventArgs e)
        {
            var image = (Image)sender;
            var page = new MobileApp.Views.Profiles.ImageView(image.Source);
            Navigation.PushModalAsync(page);
        }
    }


    public class MelaporkanKejadianViewModel : MobileApp.ViewModels.BaseViewModel
    {

        public Kejadian Model { get; set; } = new Kejadian() {  PelaporId= Helper.Profile.Karyawan.Id, Waktu=DateTime.Now};

        public Command AddDetailPelanggaranCommand { get; }
        public Command CameraCommand { get; }
        public Command FolderCommand { get; }
        public Command CancelCommand { get; }
        public Command SaveCommand { get; }

        public MelaporkanKejadianViewModel()
        {

            CameraCommand = new Command(CameraAction);
            FolderCommand = new Command(FolderAction);

            CancelCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopModalAsync());
            SaveCommand = new Command(SaveActionAsync);
        }

        private async void SaveActionAsync(object obj)
        {
            try
            {
                if (IsBusy) return;


                var saved = await Kejadians.AddItemAsync(Model);
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
            var bukti = new BuktiKejadian() { FileType = "Image" };


            var stream = file.GetStream();


            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                bukti.Data = ms.ToArray();
            }

            file.Dispose();

            if (Model.Files.Count <= 0)
            {
               Model.Files.Clear();
            }
            Model.Files.Add(bukti);

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

            var bukti = new BuktiKejadian() { FileType = "Image" };


            var stream = file.GetStream();


            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                bukti.Data = ms.ToArray();
            }

            file.Dispose();


           Model.Files.Add(bukti);
        }
    }
}