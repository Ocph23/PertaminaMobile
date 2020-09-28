using Microcharts;
using MobileApp.ViewModels;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentPage
    {
        public HomeView()
        {
            InitializeComponent();
            this.BindingContext = new HomeViewModel();
        }

        protected override void OnAppearing()
        {
           
        }
    }

    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
            // _chartView = chartView;
            Chart= new DonutChart() { BackgroundColor=SKColors.Transparent };
            this.Photo = Helper.Account.PhotoUrl;
            this.ProfileName = Helper.Account.DisplayName;
            NotifCommand = new Command(NotifAction, x => true);
             Task.Run(()=>  LoadChart());
        }

      

        private void NotifAction(object obj)
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new NotificationView());
        }


        public async Task LoadChart()
        {
            try
            {
                var score = await Karyawan.GetDataScore(Helper.Profile.Karyawan.Id);
                if (score != null)
                {
                    var colorScore = SKColor.Parse("#FFC085");
                    if (score.Score > 100)
                        colorScore = SKColors.YellowGreen;
                    var entries = new List<ChartEntry>{
                                    new ChartEntry((float)score.Score)
                                    {
                                        Color = colorScore,

                                    },
                                    new ChartEntry((float)score.Potongan)
                                    {
                                        Color = SKColors.OrangeRed
                                    },

                                };

                   
                    this.Chart.Entries= entries;
                    ScoreView = score.Score;
                }
            }
            catch 
            {
            }
        }

        public string Photo { get;  set; }
        public string ProfileName { get; set; }
        public Command NotifCommand { get; }

        private double scoreView;

        public double ScoreView
        {
            get { return scoreView; }
            set { SetProperty(ref scoreView , value); }
        }


        private Chart chart;

        public Chart Chart
        {
            get { return chart; }
            set { SetProperty(ref chart , value); }
        }


    }
}