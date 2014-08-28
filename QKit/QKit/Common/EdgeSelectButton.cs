using QKit.Utils;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace QKit.Common
{
    /// <summary>
    /// Represents a left-edge button to enable a ListView's multiple selection mode.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class EdgeSelectButton : ButtonBase
    {

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the EdgeSelectButton class.
        /// </summary>
        public EdgeSelectButton()
        {
            this.DefaultStyleKey = typeof(EdgeSelectButton);
            this.Click += EdgeSelectButton_Click;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Called before the PointerPressed event occurs.
        /// </summary>
        /// <param name="e">Event data for event.</param>
        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            VisualStateManager.GoToState(this, "Pressed", true);
        }

        /// <summary>
        /// Called before the PointerReleased event occurs.
        /// </summary>
        /// <param name="e">Event data for event.</param>
        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
            VisualStateManager.GoToState(this, "Normal", true);
        }

        /// <summary>
        /// Called before the PointerCaptureLost event occurs.
        /// </summary>
        /// <param name="e">Event data for event.</param>
        protected override void OnPointerCaptureLost(PointerRoutedEventArgs e)
        {
            base.OnPointerCaptureLost(e);
            VisualStateManager.GoToState(this, "Normal", true);
        }
        #endregion

        #region Event Handlers
        private void EdgeSelectButton_Click(object sender, RoutedEventArgs e)
        {
            var parentListViewItem = VisualTreeUtil.FindVisualParent<ListViewItem>(this);
            if (parentListViewItem != null)
            {
                var parentListView = VisualTreeUtil.FindVisualParent<MultiSelectListView>(parentListViewItem);

                if (parentListView != null)
                {
                    parentListView.SelectionMode = ListViewSelectionMode.Multiple;
                    parentListView.SelectedItem = parentListView.ItemFromContainer(parentListViewItem);
                }
            }
        }
        #endregion
    }
}
