﻿namespace Merchello.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Abstract implementation of IDisposable.
    /// </summary>
    /// <remarks>
    /// Can also be used as a pattern for when inheriting is not possible.
    /// 
    /// See also: https://msdn.microsoft.com/en-us/library/b1yfkh5e%28v=vs.110%29.aspx
    /// See also: https://lostechies.com/chrispatterson/2012/11/29/idisposable-done-right/
    /// 
    /// Note: if an object's ctor throws, it will never be disposed, and so if that ctor
    /// has allocated disposable objects, it should take care of disposing them.
    /// </remarks>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public abstract class DisposableObject : IDisposable
    {
        /// <summary>
        /// The locker.
        /// </summary>
        private readonly object _locko = new object();

        /// <summary>
        /// Designates the whether or not the object has been desposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Finalizes an instance of the <see cref="DisposableObject"/> class. 
        /// </summary>
        ~DisposableObject()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets a value indicating whether disposed.
        /// </summary>
        /// <remarks>
        /// for internal tests only (not thread safe)
        /// </remarks>
        internal bool Disposed
        {
            get { return _disposed; }
        }
        
        /// <summary>
        /// Implements IDisposable.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the resources.
        /// </summary>
        protected abstract void DisposeResources();

        /// <summary>
        /// Disposes any unmanaged resource.
        /// </summary>
        protected virtual void DisposeUnmanagedResources()
        {
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        private void Dispose(bool disposing)
        {
            lock (_locko)
            {
                if (_disposed) return;
                _disposed = true;
            }

            DisposeUnmanagedResources();

            if (disposing)
                DisposeResources();
        }
    }
}