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

    }

    public class ChangeProfileViewModel : BaseViewModel, IKaryawan
    {
        public ChangeProfileViewModel()
        {
            SaveCommand = new Command(SaveAction, saveValidate);
            var karyawan = Helper.Profile.Karyawan;
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

        private void SaveAction(object obj)
        {
            
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
        public string Email { get { return _email; } set {SetProperty(ref _email,value); }}
        public string UserId { get { return _userId; } set {SetProperty(ref _userId,value); }}
        public string Photo { get { return _photo; } set {SetProperty(ref _photo,value); }}
        public bool Status { get { return _status; } set {SetProperty(ref _status,value); }}
        public byte[] DataPhoto { get { return _dataPhoto; } set {SetProperty(ref _dataPhoto ,value); }}

        public Command SaveCommand { get; }
    }
}
