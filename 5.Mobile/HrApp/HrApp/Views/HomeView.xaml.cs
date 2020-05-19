//using Android.OS;
//using Android;
using Microcharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using HrApp.Services.Interfaces;
using HrApp.ViewModels;
using System.Globalization;

namespace HrApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentPage
    {
        private HomeViewModel _container;
        public HomeView()
        {
            InitializeComponent();
            _container = BindingContext as HomeViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var entries = new[]
            {
                new Microcharts.Entry(20)
                {
                    Label = "January",
                    ValueLabel = "20",
                    Color = SKColor.Parse("#266489")
                },
                new Microcharts.Entry(15)
                {
                Label = "February",
                ValueLabel = "15",
                Color = SKColor.Parse("#68B9C0")
                },
                new Microcharts.Entry(33)
                {
                Label = "March",
                ValueLabel = "33",
                Color = SKColor.Parse("#90D585")
                },
                new Microcharts.Entry(12)
                {
                Label = "April",
                ValueLabel = "12",
                Color = SKColor.Parse("#90B585")
                },
                new Microcharts.Entry(40)
                {
                Label = "May",
                ValueLabel = "40",
                Color = SKColor.Parse("#91D585")
                }
            };

            var chart4 = new PointChart() { Entries = entries, LabelTextSize = 35 };

            var chart5 = new RadialGaugeChart() { Entries = entries, LabelTextSize = 35 };
            var chart6 = new RadarChart() { Entries = entries, LabelTextSize = 35 };

            this.chart4.Chart = chart4;
            this.chart5.Chart = chart5;
            this.chart6.Chart = chart6;

            ProcessStatusChartBuild();
            ProcessSucceededChartBuild();
            MonthlyHiringsChartBuild();
        }

        public void ProcessStatusChartBuild()
        {
            var res = _container.GetProcesses();

            var chartEntries = new[]
            {
                new Microcharts.Entry(res.Count(r => r.Status == Domain.Model.Enum.ProcessStatus.InProgress))
                {
                    Label = "In Progress",
                    ValueLabel = res.Count(r => r.Status == Domain.Model.Enum.ProcessStatus.InProgress).ToString(),
                    Color = SKColor.Parse("#0080ff")
                },
                new Microcharts.Entry(res.Count(r => r.Status == Domain.Model.Enum.ProcessStatus.NA))
                {
                    Label = "Not Started",
                    ValueLabel = res.Count(r => r.Status == Domain.Model.Enum.ProcessStatus.NA).ToString(),
                    Color = SKColor.Parse("#D3D3D3")
                },
                new Microcharts.Entry(res.Count(r => r.Status == Domain.Model.Enum.ProcessStatus.Rejected))
                {
                    Label = "Rejected",
                    ValueLabel = res.Count(r => r.Status == Domain.Model.Enum.ProcessStatus.Rejected).ToString(),
                    Color = SKColor.Parse("#FF0000")
                },
                new Microcharts.Entry(res.Count(r => r.Status == Domain.Model.Enum.ProcessStatus.Hired))
                {
                    Label = "Hired",
                    ValueLabel = res.Count(r => r.Status == Domain.Model.Enum.ProcessStatus.Hired).ToString(),
                    Color = SKColor.Parse("#90D585")
                },
                new Microcharts.Entry(res.Count(r => r.Status == Domain.Model.Enum.ProcessStatus.Declined))
                {
                    Label = "Declined",
                    ValueLabel = res.Count(r => r.Status == Domain.Model.Enum.ProcessStatus.Declined).ToString(),
                    Color = SKColor.Parse("#ff6666")
                },
                new Microcharts.Entry(res.Count(r => r.Status == Domain.Model.Enum.ProcessStatus.Accepted))
                {
                    Label = "Offer Accepted",
                    ValueLabel = res.Count(r => r.Status == Domain.Model.Enum.ProcessStatus.Accepted).ToString(),
                    Color = SKColor.Parse("#FFA500")
                }
            };

            this.chart1.Chart = new BarChart() { Entries = chartEntries, LabelTextSize = 35 }; ;
        }

        public void ProcessSucceededChartBuild()
        {
            var res = _container.GetProcesses();
            var sp = res.Count(r => r.Status == Domain.Model.Enum.ProcessStatus.Hired || r.Status == Domain.Model.Enum.ProcessStatus.Accepted);
            var fp = res.Count(r => r.Status == Domain.Model.Enum.ProcessStatus.Rejected || r.Status == Domain.Model.Enum.ProcessStatus.Declined);

            var chartEntries = new[]
            {
                new Microcharts.Entry(sp)
                {
                    Label = "Successful processes",
                    ValueLabel = sp.ToString(),
                    Color = SKColor.Parse("#266489")
                },
                new Microcharts.Entry(fp)
                {
                    Label = "Failed processes",
                    ValueLabel = fp.ToString(),
                    Color = SKColor.Parse("#ff6666")
                }
            };

            this.chart2.Chart = new DonutChart() { Entries = chartEntries, LabelTextSize = 35 };
        }

        public void MonthlyHiringsChartBuild()
        {
            var length = 5;
            var res = _container.GetProcesses();//.Where(r => r.Status == Domain.Model.Enum.ProcessStatus.Hired);
            var chartEntries = new List<Microcharts.Entry>();

            for (int i = length; i >= 0; i--)
            {
                var proc = res.Count(r => r.HireDate.Year == DateTime.UtcNow.AddMonths(-i).Year && r.HireDate.Month == DateTime.UtcNow.AddMonths(-i).Month);
                var entry = new Microcharts.Entry(proc)
                {
                    Label = DateTime.UtcNow.AddMonths(-i).ToString("MMMM", CultureInfo.CreateSpecificCulture("en")),
                    ValueLabel = proc.ToString(),
                    Color = SKColor.Parse("#ff6666")
                };
                chartEntries.Add(entry);
            }

            var chart3 = new LineChart() { Entries = chartEntries.ToArray(), LabelTextSize = 35 };
            this.chart3.Chart = chart3;
        }
    }
}