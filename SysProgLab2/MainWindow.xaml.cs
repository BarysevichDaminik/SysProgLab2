using System.Linq;
using System.Windows;

namespace SysProgLab2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e) => Process();
        void Process()
        {
            WpfPlot1.Plot.Clear();
            WpfPlot2.Plot.Clear();

            if (int.TryParse(N_a.Text, out int _N_a) && _N_a > 0 &&
                int.TryParse(N_threads.Text, out int _N_threads) && _N_threads > 0 &&
                int.TryParse(K_.Text, out int _K) && _K > 0 &&
                int.TryParse(Delta_threads.Text, out int _Delta_threads) && _Delta_threads > 0 &&
                int.TryParse(Delta_K.Text, out int _Delta_K) && _Delta_K > 0)
            {
                Func func = new(_N_a, _K, _N_threads);
                List<double> dataX1_Plot1 = new();
                List<double> dataY1_Plot1 = new();
                List<double> dataX2_Plot1 = new();
                List<double> dataY2_Plot1 = new();

                List<double> dataX1_Plot2 = new();
                List<double> dataY1_Plot2 = new();
                List<double> dataX2_Plot2 = new();
                List<double> dataY2_Plot2 = new();

                for (int i = 0; i < 10; i++)
                {
                    func = new Func(_N_a, _K, _N_threads);
                    dataX1_Plot1.Add(_N_threads);
                    dataX2_Plot1.Add(_N_threads);
                    dataX1_Plot2.Add(_K);
                    dataX2_Plot2.Add(_K);
                    (double[], long) result1 = func.count();
                    (double[], long) result2 = func.countParallelTasks();
                    dataY1_Plot1.Add(result1.Item2);
                    dataY2_Plot1.Add(result2.Item2);
                    dataY1_Plot2.Add(result1.Item2);
                    dataY2_Plot2.Add(result2.Item2);
                    _N_threads += _Delta_threads;
                    _K += _Delta_K;
                }

                WpfPlot1.Plot.Add.Scatter(dataX1_Plot1.ToArray(), dataY1_Plot1.ToArray());
                WpfPlot1.Plot.Add.Scatter(dataX2_Plot1.ToArray(), dataY2_Plot1.ToArray());
                WpfPlot2.Plot.Add.Scatter(dataX1_Plot2.ToArray(), dataY1_Plot2.ToArray());
                WpfPlot2.Plot.Add.Scatter(dataX2_Plot2.ToArray(), dataY2_Plot2.ToArray());

                WpfPlot1.Refresh();
                WpfPlot2.Refresh();

            }
            else
            {
                MessageBox.Show("Все значения должны быть положительными и больше нуля.");
            }
        }
    }
}