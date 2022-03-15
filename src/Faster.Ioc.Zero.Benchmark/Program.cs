using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Faster.Ioc.Zero.Benchmark.Complex;
using Faster.Ioc.Zero.Benchmark.Standard;
using Faster.Ioc.Zero.Core;

namespace Faster.Ioc.Zero.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmark>();
        }
    }

    public class ContainerBuilder : IContainerBuilder
    {
        public void Bootstrap(Builder builder)
        {
            builder.Register<ITransient, TransientOne>();
            builder.Register<ITransient, TransientTwo>();
            builder.Register<ITransient, TransientThree>();

            builder.Register<ISingleton, SingletonOne>(Lifetime.Singleton);
            builder.Register<ISingleton, SingletonTwo>(Lifetime.Singleton);
            builder.Register<ISingleton, SingletonThree>(Lifetime.Singleton);

            builder.Register<ICombined, CombinedOne>();
            builder.Register<ICombined, CombinedTwo>();
            builder.Register<ICombined, CombinedThree>();

            builder.Register<IComplex, ComplexOne>();
            builder.Register<IComplex, ComplexTwo>();
            builder.Register<IComplex, ComplexThree>();

            builder.Register<IFirstService, FirstService>(Lifetime.Singleton);
            builder.Register<ISecondService, SecondService>(Lifetime.Singleton);
            builder.Register<IThirdService, ThirdService>(Lifetime.Singleton);

            builder.Register<ISubObjectOne, SubObjectOne>();
            builder.Register<ISubObjectTwo, SubObjectTwo>();
            builder.Register<ISubObjectThree, SubObjectThree>();
        }
    }
}
