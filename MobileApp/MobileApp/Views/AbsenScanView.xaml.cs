using MobileApp.Controls;
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
using ZXing.Net.Mobile.Forms;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AbsenScanView : ContentPage
    {
        private AbsenViewModel vm;

        public AbsenScanView()
        {
            InitializeComponent();
            scannerView.Options = new ZXing.Mobile.MobileBarcodeScanningOptions { DelayBetweenContinuousScans = 100, AutoRotate = true  };
            this.BindingContext=vm = new AbsenViewModel();
        }



        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.IsScanning = false;
            vm.IsAnalyzing= false;
            vm.SanningStatus = SanningStatus.None;
            vm.SetCommand();

        }
    }




    public class AbsenViewModel:BaseViewModel
    {
        public AbsenViewModel()
        {
            SanningStatus =  SanningStatus.None;
            IsScanning = false;
            IsAnalyzing = false;
            SetCommand();
        }

        public  Task SetCommand()
        {
            AbsenCommand = new Command(AbsenAction, AbsenValidate);
            LemburCommand = new Command(LemburAction, LemburValidate);
            ScanningCommand = new Command(ScanningAction, x => IsScanning);
          
            return Task.CompletedTask;
        }

        private void ScanningAction(object obj)
        {
            IsAnalyzing = false;
            IsScanning = false;
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
                            if(absen.AbsenType== Models.AbsenType.Kerja)
                            {
                                var result = await Absens.AddItemAsync(absen);
                            }
                            else
                            {
                                var page = new InputLembur(absen);
                                Helper.ShowLemburInput(page);
                            }
                            SanningStatus = SanningStatus.None;
                        }
                        else
                        {
                            throw new SystemException("QR Code Sudah Tidak Berlaku, Hubungi Petugas");
                        }
                    }
                    SetCommand();
                }
                catch (Exception ex)
                {
                 
                    SanningStatus = SanningStatus.None;
                    if(ex.GetType().Name== "JsonReaderException")
                    {
                        Helper.ErrorMessage("Maaf ini bukan QR Code Absen Pertamina - Jayapura");

                    }else
                        Helper.ErrorMessage(ex.Message);
                    SetCommand();
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
            IsAnalyzing = true;
            IsScanning = true;
            SetCommand();
        }

        private void LemburAction(object obj)
        {
            SanningStatus =  SanningStatus.Lembur;
            IsAnalyzing = true; 
            IsScanning = true;
            SetCommand();
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

        public bool IsAnalyzing
        {
            get { return isAnalyzing; }
            set { SetProperty(ref isAnalyzing, value); }
        }


        private SanningStatus isAbsen;
        private Command _absenCommand;
        private Command _lemburCommand;
        private Command _scanningCommand;
        private bool isAnalyzing;

        public SanningStatus SanningStatus
        {
            get { return isAbsen; }
            set { SetProperty(ref isAbsen , value); }
        }


        private bool showDescription;
        public bool IsShowDescription
        {
            get { return showDescription; }
            set { SetProperty(ref showDescription ,value); }
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