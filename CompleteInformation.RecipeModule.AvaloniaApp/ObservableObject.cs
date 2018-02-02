// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root
// of https://github.com/wieslawsoltes/Core2D for full license information.
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CompleteInformation.RecipeModule
{
    /// <summary>
    /// Observable object base class.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets is dirty flag.
        /// </summary>
        internal bool IsDirty { get; set; }

        /// <summary>
        /// Set the <see cref="IsDirty"/> flag value.
        /// </summary>
        /// <param name="value">The new value of <see cref="IsDirty"/> flag.</param>
        public void MarkAsDirty(bool value) => IsDirty = value;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify observers about property changes.
        /// </summary>
        /// <param name="propertyName">The property name that changed.</param>
        public void Notify([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Update property backing field and notify observers about property change.
        /// </summary>
        /// <typeparam name="T">The type of field.</typeparam>
        /// <param name="field">The field to update.</param>
        /// <param name="value">The new field value.</param>
        /// <param name="propertyName">The property name that changed.</param>
        /// <returns>True if backing field value changed.</returns>
        public bool Update<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                IsDirty = true;
                Notify(propertyName);
                return true;
            }
            return false;
        }
    }
}
