using System;

namespace Faster.Ioc.Zero.Benchmark.Standard
{
    public interface ISingleton
    {
        void DoSomething();
    }

    public class SingletonOne : ISingleton
    {
        public SingletonOne()
        {

        }

        public void DoSomething()
        {
            Console.WriteLine("Hello");
        }
    }


    public class SingletonTwo : ISingleton
    {
        public SingletonTwo()
        {

        }
        public void DoSomething()
        {
            Console.WriteLine("Hello");
        }
    }

    public class SingletonThree : ISingleton
    {
        public SingletonThree()
        {
        }
        public void DoSomething()
        {
            Console.WriteLine("Hello");
        }
    }
}
