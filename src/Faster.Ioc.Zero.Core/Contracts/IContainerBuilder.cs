namespace Faster.Ioc.Zero.Core
{
    public interface IContainerBuilder
    {

        /// <summary>
        /// Bootstraps the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        void Bootstrap(Builder builder);

    }
}
