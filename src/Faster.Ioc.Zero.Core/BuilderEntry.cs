using System;

using System.Runtime.InteropServices;

namespace Faster.Ioc.Zero.Core
{

    [System.Diagnostics.DebuggerDisplay("RegisteredType:{RegisteredType.Name}")]
    [StructLayout(LayoutKind.Sequential)]
    public class BuilderEntry
    {


        #region Properties

        /// <summary>
        /// Gets the lifetime(transient, or singelton)
        /// </summary>
        /// <value>
        /// The lifetime.
        /// </value>
        public Lifetime Lifetime { get; set; }

        /// <summary>
        /// Type used while registering a Type
        /// </summary>
        /// <value>
        /// The registeredType.
        /// </value>
        public Type RegisteredType { get; set; }

        /// <summary>
        /// Gets or sets the type of the return.
        /// </summary>
        /// <value>
        /// The type of the return.
        /// </value>
        public Type ReturnType { get; set; }

        /// <summary>
        /// Gets or sets the hashcode.
        /// </summary>
        /// <value>
        /// The hashcode.
        /// </value>
        public int Hashcode { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BuilderEntry" /> class.
        /// </summary>
        /// <param name="registeredType">Type of the registered.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <exception cref="ArgumentNullException">options</exception>
        public BuilderEntry(Type registeredType, Type returnType, Lifetime lifetime)
        {
            RegisteredType = registeredType;
            ReturnType = returnType;
            Lifetime = lifetime;
        }

        #endregion
    }
}
