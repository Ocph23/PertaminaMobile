using MobileApp.ViewModels;
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
            var result = obj as Result;
            Device.BeginInvokeOnMainThread(async () =>
            {
                // await DisplayAlert("Scanned result", result.Text, "OK");
                IsScanning = false;
                SanningStatus = SanningStatus.None;
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
        }

        private void LemburAction(object obj)
        {
            SanningStatus =  SanningStatus.Lembur;
            IsScanning = true;
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



}