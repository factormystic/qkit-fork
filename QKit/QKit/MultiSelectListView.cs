using QKit.Utils;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace QKit
{
    /// <summary>
    /// Represents the method that will handle a SelectionModeChanged event.
    /// </summary>
    /// <param name="sender">The object where the handler is attached.</param>
    /// <param name="e">Event data for the event.</param>
    public delegate void SelectionModeChangedEventHandler(object sender, RoutedEventArgs e);

    /// <summary>
    /// Represents a ListView control that enables edge selection and enhanced multiselection.
    /// </summary>
    public class MultiSelectListView : ListView
    {
        #region DependencyProperties
        /// <summary>
        /// Identifies the SelectionMode dependency property.
        /// </summary>
        public new static readonly DependencyProperty SelectionModeProperty =
            DependencyProperty.Register("SelectionMode", typeof(ListViewSelectionMode),
            typeof(MultiSelectListView), new PropertyMetadata(ListViewSelectionMode.None, OnSelectionModeChanged));
        #endregion

        #region Events
        /// <summary>
        /// Occurs when a MultiSelectListView's selection mode changes.
        /// </summary>
        public event SelectionModeChangedEventHandler SelectionModeChanged;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the MultiSelectListView class.
        /// </summary>
        public MultiSelectListView()
        {
            this.DefaultStyleKey = typeof(MultiSelectListView);
            this.Loaded += MultiSelectListView_Loaded;
            this.Unloaded += MultiSelectListView_Unloaded;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the selection behavior for a MultiSelectListView instance.
        /// </summary>
        public new ListViewSelectionMode SelectionMode
        {
            get { return (ListViewSelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }
        #endregion

        #region Methods
        private void OnSelectionModeChanged(RoutedEventArgs e)
        {
            SelectionModeChangedEventHandler handler = SelectionModeChanged;

            if (handler != null)
                handler(this, e);
        }

        private void SyncBaseSelectionMode()
        {
            base.SelectionMode = this.SelectionMode;
        }
        #endregion

        #region Event Handlers
        private static void OnSelectionModeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var multiSelectListView = dependencyObject as MultiSelectListView;
            if (multiSelectListView != null)
            {
                multiSelectListView.SelectionChanged -= multiSelectListView.MultiSelectionListView_SelectionChanged;

                // Avoiding weird bug by making sure if SelectionMode was none,
                // then nothing is selected first.
                if (e.OldValue.Equals(ListViewSelectionMode.None))
                    multiSelectListView.SelectedItem = null;

                multiSelectListView.SyncBaseSelectionMode();
                multiSelectListView.OnSelectionModeChanged(new RoutedEventArgs());
                multiSelectListView.SelectionChanged += multiSelectListView.MultiSelectionListView_SelectionChanged;
            }
        }

        private void MultiSelectListView_Loaded(object sender, RoutedEventArgs e)
        {
            var parentPage = VisualTreeUtil.FindVisualParent<Page>(this);
            if (parentPage != null && parentPage.Frame != null)
                parentPage.Frame.Navigating += Frame_Navigating;

            SelectionChanged += MultiSelectionListView_SelectionChanged;
        }

        private void MultiSelectListView_Unloaded(object sender, RoutedEventArgs e)
        {
            var parentPage = VisualTreeUtil.FindVisualParent<Page>(this);
            if (parentPage != null && parentPage.Frame != null)
                parentPage.Frame.Navigating -= Frame_Navigating;

            SelectionChanged -= MultiSelectionListView_SelectionChanged;
        }

        private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back &&
                SelectionMode == ListViewSelectionMode.Multiple)
            {
                e.Cancel = true;
                SelectionMode = ListViewSelectionMode.None;
            }
        }

        private void MultiSelectionListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedItems == null || SelectedItems.Count == 0)
                SelectionMode = ListViewSelectionMode.None;
        }
        #endregion
    }
}
