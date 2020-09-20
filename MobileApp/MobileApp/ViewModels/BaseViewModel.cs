using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using MobileApp.Models;
using MobileApp.Services;
using MobileApp.Models.Datas;

namespace MobileApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<Periode> Periodes => DependencyService.Get<IDataStore<Periode>>();
        public IDataStore<Absen> Absens => DependencyService.Get<IDataStore<Absen>>();
        public IDataStore<Level> LevelPelanggaran => DependencyService.Get<IDataStore<Level>>();
        public IDataStore<Pelanggaran> Pelanggarans => DependencyService.Get<IDataStore<Pelanggaran>>();
        public IKaraywanDataStore<Karyawan> Karyawan=> DependencyService.Get<IKaraywanDataStore<Karyawan>>();
        public IAuthInternalService AuthService => DependencyService.Get<IAuthInternalService>();
        public IDataStore<Notification> Notifications=> DependencyService.Get<IDataStore<Notification>>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
