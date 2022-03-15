using System;

namespace Faster.Ioc.Zero.Benchmark.Complex
{
    public interface ISubObjectThree
    {
    }


    public class SubObjectThree : ISubObjectThree
    {
      
        public SubObjectThree(IThirdService thirdService)
        {
            if (thirdService == null)
            {
                throw new ArgumentNullException(nameof(thirdService));
            }
        }

        protected SubObjectThree()
        {
        }
    }
}
