using System;

namespace Faster.Ioc.Zero.Benchmark.Complex
{
    public interface ISubObjectTwo
    {
    }
    
    public class SubObjectTwo : ISubObjectTwo
    {
       
        public SubObjectTwo(ISecondService secondService)
        {
            if (secondService == null)
            {
                throw new ArgumentNullException(nameof(secondService));
            }
        }

        protected SubObjectTwo()
        {
        }
    }
}
