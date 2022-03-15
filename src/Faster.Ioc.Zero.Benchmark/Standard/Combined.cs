using System;

namespace Faster.Ioc.Zero.Benchmark.Standard
{
    public interface ICombined
    {
        void DoSomething();
    }
    
    public class CombinedOne : ICombined
    {
     public CombinedOne(ISingleton singletonOne, ITransient transientOne)
        {
            if (singletonOne == null)
            {
                throw new ArgumentNullException(nameof(singletonOne));
            }

            if (transientOne == null)
            {
                throw new ArgumentNullException(nameof(transientOne));
            }
        }
        public void DoSomething()
        {
            Console.WriteLine("Combined");
        }
    }


    public class CombinedTwo : ICombined
    {
        protected static int counter;

       
       
        public CombinedTwo(ISingleton singletonTwo, ITransient transientTwo)
        {
            if (singletonTwo == null)
            {
                throw new ArgumentNullException(nameof(singletonTwo));
            }

            if (transientTwo == null)
            {
                throw new ArgumentNullException(nameof(transientTwo));
            }

            
        }
        
        public void DoSomething()
        {
            Console.WriteLine("Combined");
        }
    }

  
    public class CombinedThree : ICombined
    {
       
      

        public CombinedThree(ISingleton singletonThree, ITransient transientThree)
        {
            if (singletonThree == null)
            {
                throw new ArgumentNullException(nameof(singletonThree));
            }

            if (transientThree == null)
            {
                throw new ArgumentNullException(nameof(transientThree));
            }

       
        }


        public void DoSomething()
        {
            Console.WriteLine("Combined");
        }
    }
}
