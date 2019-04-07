using System;
using System.Drawing;

namespace ProgLib.Windows.Minimal
{
    public interface IColorEditor
    {
        #region Events

        /// <summary>
        /// Occurs when the <see cref="Color"/> property is changed.
        /// </summary>
        event EventHandler ColorChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the component color.
        /// </summary>
        /// <value>The component color.</value>
        Color Color { get; set; }

        #endregion
    }
}
