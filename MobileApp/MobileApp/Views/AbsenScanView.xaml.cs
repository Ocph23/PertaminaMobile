using MobileApp.Models.Datas;
using MobileApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AbsenScanView : ContentPage
    {
        public AbsenScanView()
        {
            InitializeComponent();
            this.BindingContext = new AbsenViewModel();
        }
    }




    public class AbsenViewModel:BaseViewModel
    {
        public AbsenViewModel()
        {
            IsScanning = false;
            SanningStatus =  SanningStatus.None;
            AbsenCommand = new Command(AbsenAction, AbsenValidate);
            LemburCommand = new Command(LemburAction, LemburValidate);
            ScanningCommand = new Command(ScanningAction,x=>IsScanning);

        }

        private void ScanningAction(object obj)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var data = obj as Result;
                    var qrData = JsonConvert.DeserializeObject<QRData>(data.Text.ToString());
                    if (qrData == null)
                        return;
                    if (qrData != null)
                    {
                        if (DateTime.Now >= qrData.Mulai && DateTime.Now <= qrData.Selesai)
                        {
                            var absen = new Absen { KaryawanId = Helper.Profile.Karyawan.Id, Masuk = DateTime.Now, Pulang = null };
                            absen.AbsenType = this.SanningStatus == SanningStatus.Absen ? Models.AbsenType.Kerja : Models.AbsenType.Lembur;
                            IsScanning = false;
                            var result = await Absens.AddItemAsync(absen);
                            SanningStatus = SanningStatus.None;
                        }
                        else
                        {
                            throw new SystemException("QR Code Sudah Tidak Berlaku, Hubungi Petugas");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Helper.ErrorMessage(ex.Message);
                }
            });
        }

        private bool LemburValidate(object arg)
        {
            return  SanningStatus!= SanningStatus.Lembur;
        }

        private bool AbsenValidate(object arg)
        {
            return  SanningStatus!= SanningStatus.Absen;
        }

        private void AbsenAction(object obj)
        {
            SanningStatus =  SanningStatus.Absen;
            IsScanning = true;
            AbsenCommand = new Command(AbsenAction, AbsenValidate);
            LemburCommand = new Command(LemburAction, LemburValidate);
        }

        private void LemburAction(object obj)
        {
            SanningStatus =  SanningStatus.Lembur;
            IsScanning = true;
            AbsenCommand = new Command(AbsenAction, AbsenValidate);
            LemburCommand = new Command(LemburAction, LemburValidate);
        }



        public Command AbsenCommand { 
            get {
                return _absenCommand;
            } set {
                SetProperty(ref _absenCommand, value);
            } 
        }
        public Command LemburCommand {
            get
            {
                return _lemburCommand;
            }
            set
            {
                SetProperty(ref _lemburCommand, value);
            }
        }
        public Command ScanningCommand
        {
            get
            {
                return _scanningCommand;
            }
            set
            {
                SetProperty(ref _scanningCommand, value);
            }
        }

        private bool isScaning;

        public bool IsScanning
        {
            get { return isScaning; }
            set { SetProperty(ref isScaning ,value); }
        }


        private SanningStatus isAbsen;
        private Command _absenCommand;
        private Command _lemburCommand;
        private Command _scanningCommand;

        public SanningStatus SanningStatus
        {
            get { return isAbsen; }
            set { SetProperty(ref isAbsen , value); }
        }


    }


    public enum SanningStatus
    {
        None, Absen, Lembur
    }



    public class QRData
    {
        public DateTime Mulai { get; set; }
        public DateTime Selesai { get; set; }
    }


}