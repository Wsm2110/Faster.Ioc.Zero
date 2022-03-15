namespace Faster.Ioc.Zero.SourceGenerator
{
    public interface IContainer
    {      
        /// <summary>
        /// Bootstraps the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        void Bootstrap(Builder builder);

    }
}
