using MobileApp.Models;
using MobileApp.Models.Datas;
using MobileApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationView : ContentPage
    {
        public NotificationView()
        {
            InitializeComponent();
            BindingContext = new NotificationViewModel();
        }

        private void closetap(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }



    public class NotificationViewModel : MobileApp.ViewModels.BaseViewModel, INotification
    {
        public ObservableCollection<Notification> SourceView { get; set; } = new ObservableCollection<Notification>();
        public NotificationViewModel()
        {
            LoadCommand = new Command(Loadaction);
            LoadCommand.Execute(null);

            MessagingCenter.Subscribe<INotification, Notification>(this, "notification", async (sender, result) =>
            {
               await Task.Delay(1000);
                LoadCommand.Execute(null);
            });

        }

      
        public Command LoadCommand { get; }

        private async void Loadaction(object obj)
        {
            try
            {
                if (IsBusy)
                    return;

                var datas = await Notifications.GetItemsAsync();

                if (datas != null)
                {
                    SourceView.Clear();
                    foreach (var item in datas.OrderByDescending(x=>x.Created))
                    {
                        SourceView.Add(item);
                    }
                }
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