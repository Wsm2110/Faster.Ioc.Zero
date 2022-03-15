using System;

namespace Faster.Ioc.Zero.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class Builder
    {

        #region Registration Methods
        
        /// <summary>
        /// Registers an implementation type for the specified interface
        /// </summary>
        /// <typeparam name="TInterface">RegisteredType to register</typeparam>
        /// <typeparam name="TImplementation">Implementing type</typeparam>
        /// <returns>IRegisteredType object</returns>
        public void Register<TInterface, TImplementation>() where TImplementation : TInterface { }

        /// <summary>
        /// Registers the specified override.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="override">The override.</param>
        public void Register<TInterface, TImplementation>(Func<TInterface> @override) where TImplementation :new() { }

        /// <summary>
        /// Registers the specified override.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="override">The override.</param>
        /// <param name="lifetime">The lifetime.</param>
        public void Register<TInterface, TImplementation>(Func<TInterface> @override, Lifetime lifetime) where TImplementation : TInterface, new() { }

        /// <summary>
        /// Registers an implementation type for the specified interface
        /// </summary>
        /// <typeparam name="TInterface">RegisteredType to register</typeparam>
        /// <typeparam name="TImplementation">Implementing type</typeparam>
        /// <returns>IRegisteredType object</returns>
        public void Register<TInterface, TImplementation>(Lifetime lifetime) where TImplementation : TInterface { }

        #endregion

    }
}