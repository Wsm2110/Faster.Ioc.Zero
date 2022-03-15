using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace Faster.Ioc.Zero.Benchmark
{
    [HardwareCounters(
        HardwareCounter.CacheMisses,
        HardwareCounter.BranchInstructions,
        HardwareCounter.TotalCycles, HardwareCounter.BranchMispredictions)]

    [BenchmarkDotNet.Diagnostics.Windows.Configs.InliningDiagnoser(true, true)]
    public class Benchmark
    {
        [Params(500000)]
        public int N { get; set; }

        [Benchmark]
        public void Transient()
        {
            for (int i = 0; i < N; ++i)
            {
                var transientOne = Container.TransientOne;
                var transientTwo = Container.TransientTwo;
                var transientThree = Container.TransientThree;
            }
        }

        [Benchmark]
        public void Singletons()
        {
            for (int i = 0; i < N; ++i)
            {
                var singletonOne = Container.SingletonOne;
                var singletonTwo = Container.SingletonTwo;
                var singletonThree = Container.SingletonThree;
            }
        }

        [Benchmark]
        public void Combined()
        {
            for (int i = 0; i < N; ++i)
            {
                var combinedOne = Container.CombinedOne;
                var combinedTwo = Container.CombinedTwo;
                var combinedThree = Container.CombinedThree;
            }
        }


        [Benchmark]
        public void Complex()
        {
            for (int i = 0; i < N; ++i)
            {
                var complexOne = Container.ComplexOne;
                var complexTwo = Container.ComplexTwo;
                var complexThree = Container.ComplexThree;
            }
        }

        [Benchmark]
        public void Enumerable()
        {
            for (int i = 0; i < N; ++i)
            {
                var complexOne = Container.GetAllIComplex();
                var complexTwo = Container.GetAllISingleton();
                var complexThree = Container.GetAllICombined();
            }
        }

    }
}