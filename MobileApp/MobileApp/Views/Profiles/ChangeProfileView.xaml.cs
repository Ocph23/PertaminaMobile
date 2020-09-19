using MobileApp.Models.Datas;
using MobileApp.ViewModels;
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
    public partial class ChangeProfileView : ContentPage
    {
        private ChangeProfileViewModel vm;

        public ChangeProfileView()
        {
            InitializeComponent();
            BindingContext = vm = new ChangeProfileViewModel();
        }

        private void closetap(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }

    public class ChangeProfileViewModel : BaseViewModel, IKaryawan
    {
        public ChangeProfileViewModel()
        {
            SaveCommand = new Command(SaveAction, saveValidate);
            karyawan = Helper.Profile.Karyawan;
            Id = karyawan.Id;
            Alamat= karyawan.Alamat;
            DataPhoto= karyawan.DataPhoto;
            Email= karyawan.Email;
            KodeKaryawan= karyawan.KodeKaryawan;
            Kontak= karyawan.Kontak;
            NamaKaryawan= karyawan.NamaKaryawan;
            Photo= karyawan.Photo;
            Status= karyawan.Status;
            UserId= karyawan.UserId;
        }

        private bool saveValidate(object arg)
        {
            if (string.IsNullOrEmpty(NamaKaryawan) || string.IsNullOrEmpty(Alamat) || string.IsNullOrEmpty(Kontak) || string.IsNullOrEmpty(Photo))
                return false;
            return true;
        }

        private async void SaveAction(object obj)
        {
            if (string.IsNullOrEmpty(NamaKaryawan) || string.IsNullOrEmpty(Kontak) || string.IsNullOrEmpty(Alamat))
            {
                Helper.ErrorMessage("Lengkapi Data Anda !");
                return;
            }

            if (!EmailValidate(Email))
            {
                Helper.ErrorMessage("Email anda tidak valid !");
                return;
            }

            try
            {
                IsBusy = true;

              var result= await Karyawan.UpdateItemAsync(new Models.Datas.Karyawan
                {
                    Alamat = Alamat,
                    Email = Email,
                    Id = karyawan.Id,
                    KodeKaryawan = karyawan.KodeKaryawan,
                    Kontak = Kontak,
                    NamaKaryawan = NamaKaryawan,
                    Photo = karyawan.Photo,
                    UserId = karyawan.UserId, 
                    Perusahaan=karyawan.Perusahaan
                });

                if (result)
                    Helper.InfoMessage("Data Berhasil Diubah !");
                else
                throw new SystemException("Data tidak berhasil diubah !");
            }
            catch (Exception ex)
            {
                Helper.ErrorMessage("Data Berhasil Diubah !");
            }finally
            {
                IsBusy = false;
            }





        }

        private int _id;
        private string _kode;
        private string _nama;
        private string _alamat;
        private string _kontak;
        private string _email;
        private string _userId;
        private string _photo;
        private bool _status;
        private byte[] _dataPhoto;

        public int Id { get { return _id; } set { SetProperty(ref _id, value); }} 
        public string KodeKaryawan { get { return _kode; } set { SetProperty(ref _kode, value); } }
        public string NamaKaryawan { get { return _nama; } set {SetProperty(ref _nama ,value); }}
        public string Alamat { get { return _alamat; } set {SetProperty(ref _alamat,value); }}
        public string Kontak { get { return _kontak; } set {SetProperty(ref _kontak,value); }}
        public string Email { get { return _email; } set {SetProperty(ref _email,value);}
        }

        private bool EmailValidate(string value)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(value);
                return addr.Address == value;
            }
            catch
            {
                return false;
            }

        }

        public string UserId { get { return _userId; } set {SetProperty(ref _userId,value); }}
        public string Photo { get { return _photo; } set {SetProperty(ref _photo,value); }}
        public bool Status { get { return _status; } set {SetProperty(ref _status,value); }}
        public byte[] DataPhoto { get { return _dataPhoto; } set {SetProperty(ref _dataPhoto ,value); }}

        public Command SaveCommand { get; }

        private Karyawan karyawan;
    }
}
