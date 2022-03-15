using System;

namespace Faster.Ioc.Zero.Benchmark.Generics
{
  
    public class ImportGeneric<T>
    {
        protected static int counter;
        public ImportGeneric(IGenericInterface<T> importGenericInterface)
        {
            if (importGenericInterface == null)
            {
                throw new ArgumentNullException(nameof(importGenericInterface));
            }

            System.Threading.Interlocked.Increment(ref counter);
        }

        protected ImportGeneric()
        {
        }

        public static int Instances
        {
            get { return counter; }
            set { counter = value; }
        }
    }
}
