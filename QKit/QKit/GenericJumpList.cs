using System.ComponentModel;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace QKit
{
    /// <summary>
    /// Represents a control will augment a ListViewBase control to look and behave like a native JumpList control.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Always)]
    public sealed class GenericJumpList : JumpListBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the GenericJumpList class.
        /// </summary>
        public GenericJumpList()
        {
            this.DefaultStyleKey = typeof(GenericJumpList);
        }
        #endregion
    }
}
