namespace Faster.Ioc.Zero.Benchmark.Generics
{
 
    public class GenericExport<T> : IGenericInterface<T>
    {
        public T Value { get; set; }
    }
}
