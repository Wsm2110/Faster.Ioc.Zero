using System;

namespace Faster.Ioc.Zero.Benchmark.Complex
{
    public interface IComplex
    {
    }
    public class ComplexOne : IComplex
    {
        public ComplexOne(
            IFirstService firstService,
            ISecondService secondService,
            IThirdService thirdService,
            ISubObjectOne subObjectOne,
            ISubObjectTwo subObjectTwo,
            ISubObjectThree subObjectThree)
        {
            if (firstService == null)
            {
                throw new ArgumentNullException(nameof(firstService));
            }

            if (secondService == null)
            {
                throw new ArgumentNullException(nameof(secondService));
            }

            if (thirdService == null)
            {
                throw new ArgumentNullException(nameof(thirdService));
            }

            if (subObjectOne == null)
            {
                throw new ArgumentNullException(nameof(subObjectOne));
            }

            if (subObjectTwo == null)
            {
                throw new ArgumentNullException(nameof(subObjectTwo));
            }

            if (subObjectThree == null)
            {
                throw new ArgumentNullException(nameof(subObjectThree));
            }
        }
    }


    public class ComplexTwo : IComplex
    {
        public ComplexTwo(
              IFirstService firstService,
              ISecondService secondService,
              IThirdService thirdService,
              ISubObjectOne subObjectOne,
              ISubObjectTwo subObjectTwo,
              ISubObjectThree subObjectThree)
        {
            if (firstService == null)
            {
                throw new ArgumentNullException(nameof(firstService));
            }

            if (secondService == null)
            {
                throw new ArgumentNullException(nameof(secondService));
            }

            if (thirdService == null)
            {
                throw new ArgumentNullException(nameof(thirdService));
            }

            if (subObjectOne == null)
            {
                throw new ArgumentNullException(nameof(subObjectOne));
            }

            if (subObjectTwo == null)
            {
                throw new ArgumentNullException(nameof(subObjectTwo));
            }

            if (subObjectThree == null)
            {
                throw new ArgumentNullException(nameof(subObjectThree));
            }
        }
    }


    public class ComplexThree : IComplex
    {
        public ComplexThree(
            IFirstService firstService,
            ISecondService secondService,
            IThirdService thirdService,
            ISubObjectOne subObjectOne,
            ISubObjectTwo subObjectTwo,
            ISubObjectThree subObjectThree)
        {
            if (firstService == null)
            {
                throw new ArgumentNullException(nameof(firstService));
            }

            if (secondService == null)
            {
                throw new ArgumentNullException(nameof(secondService));
            }

            if (thirdService == null)
            {
                throw new ArgumentNullException(nameof(thirdService));
            }

            if (subObjectOne == null)
            {
                throw new ArgumentNullException(nameof(subObjectOne));
            }

            if (subObjectTwo == null)
            {
                throw new ArgumentNullException(nameof(subObjectTwo));
            }

            if (subObjectThree == null)
            {
                throw new ArgumentNullException(nameof(subObjectThree));
            }
        }
    }
}
