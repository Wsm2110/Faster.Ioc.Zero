using System;

namespace Faster.Ioc.Zero.Benchmark.Standard
{
    public interface ITransient
    {
        void DoSomething();
    }

    public class TransientOne : ITransient
    {

        public TransientOne()
        {
        }

        public void DoSomething()
        {
            Console.WriteLine("World");
        }
    }


    public class TransientTwo : ITransient
    {
        public TransientTwo()
        {
        }

        public void DoSomething()
        {
            Console.WriteLine("World");
        }
    }


    public class TransientThree : ITransient
    {

        public TransientThree()
        {
        }

        public void DoSomething()
        {
            Console.WriteLine("World");
        }
    }
}
