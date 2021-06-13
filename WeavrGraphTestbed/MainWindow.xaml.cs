using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeavrGraphLibrary.DataStructures;

namespace WeavrGraphTestbed
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TimeSeries<double> series1;
        TimeSeries<double> series2;
        Thread backgroundDataProducer;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            series1 = new TimeSeries<double>();
            series2 = new TimeSeries<double>();

            TestGraph.AddData(series1);
            TestGraph.AddData(series2);

            backgroundDataProducer = new Thread(new ThreadStart(ProduceData));
            backgroundDataProducer.Start();
        }

        void ProduceData()
        {
            Random r = new Random();
            for (int i = 0; i < 30; i++)
            {
                series1.AddEntry(i * 60, r.NextDouble() * 300);
                series2.AddEntry(i * 60, r.NextDouble() * 300);
                Thread.Sleep(1000);
            }
        }
    }
}
