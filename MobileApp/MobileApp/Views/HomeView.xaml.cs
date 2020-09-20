using Microcharts;
using Microcharts.Forms;
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
            this.BindingContext = new HomeViewModel(chartView);
        }

        protected override void OnAppearing()
        {
            var vm = (HomeViewModel)this.BindingContext;
            Task.Run(() => vm.LoadChart());

            base.OnAppearing();
        }


        private void ChartView1_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);

            // set up drawing tools
            var paint = new SKPaint
            {
                IsAntialias = true,
                Color = new SKColor(0x2c, 0x3e, 0x50),
                StrokeCap = SKStrokeCap.Round
            };

            // create the Xamagon path
            var path = new SKPath();
            path.MoveTo(71.4311121f, 56f);
            path.CubicTo(68.6763107f, 56.0058575f, 65.9796704f, 57.5737917f, 64.5928855f, 59.965729f);
            path.LineTo(43.0238921f, 97.5342563f);
            path.CubicTo(41.6587026f, 99.9325978f, 41.6587026f, 103.067402f, 43.0238921f, 105.465744f);
            path.LineTo(64.5928855f, 143.034271f);
            path.CubicTo(65.9798162f, 145.426228f, 68.6763107f, 146.994582f, 71.4311121f, 147f);
            path.LineTo(114.568946f, 147f);
            path.CubicTo(117.323748f, 146.994143f, 120.020241f, 145.426228f, 121.407172f, 143.034271f);
            path.LineTo(142.976161f, 105.465744f);
            path.CubicTo(144.34135f, 103.067402f, 144.341209f, 99.9325978f, 142.976161f, 97.5342563f);
            path.LineTo(121.407172f, 59.965729f);
            path.CubicTo(120.020241f, 57.5737917f, 117.323748f, 56.0054182f, 114.568946f, 56f);
            path.LineTo(71.4311121f, 56f);
            path.Close();
            canvas.DrawPath(path, paint);
        }
    }

    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel(Microcharts.Forms.ChartView chartView)
        {
            _chartView = chartView;
            this.Photo = Helper.Account.PhotoUrl;
            this.ProfileName = Helper.Account.DisplayName;
            NotifCommand = new Command(NotifAction, x => true);
        }

        public async Task LoadChart()
        {
            try
            {
               var score = await Karyawan.GetDataScore(Helper.Profile.Karyawan.Id);
                if (score != null)
                {
                    var entries = new List<ChartEntry>{
                                    new ChartEntry((float)score.Score)
                                    {
                                        Color = SKColor.Parse("#55a3f7"),

                                    },
                                    new ChartEntry((float)score.Pengaduan)
                                    {
                                        Color = SKColors.GreenYellow
                                    },
                                    new ChartEntry((float)score.Potongan)
                                    {
                                        Color = SKColors.OrangeRed
                                    },

                                };

                    var chart = new DonutChart { Entries = entries, BackgroundColor = SKColor.Parse("#f1f1f1") };
                    _chartView.Chart = chart;
                    ScoreView = score.Score+score.Pengaduan;
                }
            }
            catch 
            {
            }
        }

        private void NotifAction(object obj)
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new NotificationView());
        }

        private ChartView _chartView;

        public Uri Photo { get;  set; }
        public string ProfileName { get; set; }
        public Command NotifCommand { get; }

        private double scoreView;

        public double ScoreView
        {
            get { return scoreView; }
            set { SetProperty(ref scoreView , value); }
        }

    }
}