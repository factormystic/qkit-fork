using QKit.Utils;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace QKit.Common
{
    /// <summary>
    /// Represents a left-edge button to enable a ListView's multiple selection mode.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class EdgeSelectButton : Button
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
