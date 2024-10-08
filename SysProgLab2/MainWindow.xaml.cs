﻿using System.Linq;
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
                List<double> dataX2_Plot1 = new();
                List<double> dataY2_Plot1 = new();

                List<double> dataX2_Plot2 = new();
                List<double> dataY2_Plot2 = new();

                (double[], long) result1 = func.count();

                WpfPlot1.Plot.Add.Line(_N_threads, result1.Item2, _N_threads + 38, result1.Item2);
                WpfPlot2.Plot.Add.Line(_N_threads, _K, _N_threads + 38, _K);

                for (int i = 0; i < 20; i++)
                {
                    dataX2_Plot1.Add(func.N_threads);
                    dataX2_Plot2.Add(func.K);
                    (double[], long) result2 = func.countParallelThreads();
                    dataY2_Plot1.Add(result2.Item2);
                    dataY2_Plot2.Add(_K);
                    func.N_threads += _Delta_threads;
                    func.K += _Delta_K;
                }

                List<double> NormY2_Plot1 = Smooth(dataY2_Plot1, 2);

                WpfPlot1.Plot.Add.Scatter(dataX2_Plot1.ToArray(), NormY2_Plot1.ToArray());
                WpfPlot2.Plot.Add.Scatter(dataX2_Plot2.ToArray(), dataY2_Plot2.ToArray());

                WpfPlot1.Refresh();
                WpfPlot2.Refresh();

            }
            else
            {
                MessageBox.Show("Все значения должны быть положительными и больше нуля.");
            }
        }
        public static List<double> Smooth(List<double> data, int windowSize)
        {
            List<double> smoothedData = new List<double>();
            for (int i = 0; i < data.Count; i++)
            {
                double sum = 0;
                int count = 0;
                for (int j = i - windowSize; j <= i + windowSize; j++)
                {
                    if (j >= 0 && j < data.Count)
                    {
                        sum += data[j];
                        count++;
                    }
                }
                smoothedData.Add(sum / count);
            }
            return smoothedData;
        }
    }
}