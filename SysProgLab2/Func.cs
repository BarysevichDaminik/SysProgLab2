using System;
using System.Diagnostics;

internal sealed class Func
{
    double[] a;
    int K;
    int N_threads;
    public Func(int N_a, int K, int N_threads)
    {
        (a, this.K, this.N_threads) = (new double[N_a], K, N_threads);
        Random rand = new();
        for (int i = 0; i < N_a; i++)
        {
            a[i] = rand.NextDouble();
        }
    }
    public (double[], long) count()
    {
        double[] b = new double[a.Length];
        Stopwatch stopwatch = Stopwatch.StartNew();

        for (int i = 0; i < a.Length; i++)
        {
            for (int j = 0; j < K; j++)
            {
                b[i] += Math.Pow(a[i], 1.789);
            }
        }
        stopwatch.Stop();
        return (b, stopwatch.ElapsedTicks);
    }
    public (double[], long) countParallelTasks()
    {
        double[] b = new double[a.Length];
        Thread[] threads = new Thread[N_threads];
        int chunkSize = a.Length / N_threads;
        Stopwatch stopwatch = Stopwatch.StartNew();

        for (int t = 0; t < N_threads; t++)
        {
            int start = t * chunkSize;
            int end = (t == N_threads - 1) ? a.Length : start + chunkSize;
            threads[t] = new Thread(() =>
            {
                for (int i = start; i < end; i++)
                {
                    for (int j = 0; j < K; j++)
                    {
                        b[i] += Math.Pow(a[i], 1.789);
                    }
                }
            });
            threads[t].Start();
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }
        stopwatch.Stop();
        return (b, stopwatch.ElapsedTicks);
    }
}
