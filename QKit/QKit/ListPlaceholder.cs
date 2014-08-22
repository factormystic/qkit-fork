using System.ComponentModel;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace QKit
{
    /// <summary>
    /// Defines constants that specify which placeholder to show in a ListPlaceholder.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public enum ListPlaceholderMode
    {
        /// <summary>
        /// Automatically show LoadingPlaceholderContent if ListTarget's ItemsSource is null
        /// or EmptyPlaceholderContent when ItemsSource isn't null but has no items.
        /// </summary>
        Auto,
        /// <summary>
        /// Show LoadingPlaceholderContent whenever ListTarget has no items.
        /// </summary>
        Loading,
        /// <summary>
        /// Show EmptyPlaceholderContent whenever ListTarget has no items.
        /// </summary>
        Empty
    }

    /// <summary>
    /// Represents a control that will show a placeholder message when a target ListViewBase control is empty.
    /// </summary>
    public sealed class ListPlaceholder : Control
    {
        #region DependencyProperties
        /// <summary>
        /// Identifies the PlaceholderContent dependency property.
        /// </summary>
        public static readonly DependencyProperty PlaceholderContentProperty =
            DependencyProperty.Register("PlaceholderContent", typeof(object),
            typeof(ListPlaceholder), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the PlaceholderMode dependency property.
        /// </summary>
        public static readonly DependencyProperty PlaceholderModeProperty =
            DependencyProperty.Register("PlaceholderMode", typeof(ListPlaceholderMode),
            typeof(ListPlaceholder), new PropertyMetadata(ListPlaceholderMode.Auto, OnPlaceholderChanged));

        /// <summary>
        /// Identifies the PlaceholderTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty PlaceholderTemplateProperty =
            DependencyProperty.Register("PlaceholderTemplate", typeof(DataTemplate),
            typeof(ListPlaceholder), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the PlaceholderVisibility dependency property.
        /// </summary>
        public static readonly DependencyProperty PlaceholderVisibilityProperty =
            DependencyProperty.Register("PlaceholderVisibility", typeof(Visibility),
            typeof(ListPlaceholder), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Identifies the LoadingPlaceholderContent dependency property.
        /// </summary>
        public static readonly DependencyProperty LoadingPlaceholderContentProperty =
            DependencyProperty.Register("LoadingPlaceholderContent", typeof(object),
            typeof(ListPlaceholder), new PropertyMetadata(null, OnPlaceholderChanged));

        /// <summary>
        /// Identifies the EmptyPlaceholderContent dependency property.
        /// </summary>
        public static readonly DependencyProperty EmptyPlaceholderContentProperty =
            DependencyProperty.Register("EmptyPlaceholderContent", typeof(object),
            typeof(ListPlaceholder), new PropertyMetadata(null, OnPlaceholderChanged));

        /// <summary>
        /// Identifies the LoadingPlaceholderTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty LoadingPlaceholderTemplateProperty =
            DependencyProperty.Register("LoadingPlaceholderTemplate", typeof(DataTemplate),
            typeof(ListPlaceholder), new PropertyMetadata(null, OnPlaceholderChanged));

        /// <summary>
        /// Identifies the EmptyPlaceholderTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty EmptyPlaceholderTemplateProperty =
            DependencyProperty.Register("EmptyPlaceholderTemplate", typeof(DataTemplate),
            typeof(ListPlaceholder), new PropertyMetadata(null, OnPlaceholderChanged));

        /// <summary>
        /// Identifies the ListTargetProperty dependency property.
        /// </summary>
        public static readonly DependencyProperty ListTargetProperty =
            DependencyProperty.Register("ListTarget", typeof(ListViewBase),
            typeof(ListPlaceholder), new PropertyMetadata(null, OnListTargetChanged));
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the ListPlaceholder class.
        /// </summary>
        public ListPlaceholder()
        {
            this.DefaultStyleKey = typeof(ListPlaceholder);
        }
        #endregion

        #region Properties
        private bool IsUpdating { get; set; }

        /// <summary>
        /// Gets the current placeholder content that will be shown.
        /// This value will change depending on PlaceholderMode's value.
        /// </summary>
        public object PlaceholderContent
        {
            get { return GetValue(PlaceholderContentProperty); }
            private set { SetValue(PlaceholderContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the desired placeholder content to show.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always)]
        public ListPlaceholderMode PlaceholderMode
        {
            get { return (ListPlaceholderMode)GetValue(PlaceholderModeProperty); }
            set { SetValue(PlaceholderModeProperty, value); }
        }

        /// <summary>
        /// Gets the current placeholder DataTemplate to present the current placeholder content.
        /// </summary>
        public DataTemplate PlaceholderTemplate
        {
            get { return (DataTemplate)GetValue(PlaceholderTemplateProperty); }
            private set { SetValue(PlaceholderTemplateProperty, value); }
        }

        /// <summary>
        /// Gets the visibility of the placeholder content component.
        /// </summary>
        public Visibility PlaceholderVisibility
        {
            get { return (Visibility)GetValue(PlaceholderVisibilityProperty); }
            private set { SetValue(PlaceholderVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content for when the list is empty and loading.
        /// </summary>
        public object LoadingPlaceholderContent
        {
            get { return GetValue(LoadingPlaceholderContentProperty); }
            set { SetValue(LoadingPlaceholderContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content for when the list is empty and not loading.
        /// </summary>
        public object EmptyPlaceholderContent
        {
            get { return GetValue(EmptyPlaceholderContentProperty); }
            set { SetValue(EmptyPlaceholderContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate to present the placeholder when list is empty and loading.
        /// </summary>
        public DataTemplate LoadingPlaceholderTemplate
        {
            get { return (DataTemplate)GetValue(LoadingPlaceholderTemplateProperty); }
            set { SetValue(LoadingPlaceholderTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate to present the placeholder when list is empty and not loading.
        /// </summary>
        public DataTemplate EmptyPlaceholderTemplate
        {
            get { return (DataTemplate)GetValue(EmptyPlaceholderTemplateProperty); }
            set { SetValue(EmptyPlaceholderTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ListViewBase control that will cause this control to show when empty.
        /// </summary>
        public ListViewBase ListTarget
        {
            get { return (ListViewBase)GetValue(ListTargetProperty); }
            set { SetValue(ListTargetProperty, value); }
        }
        #endregion

        #region Methods
        private void UpdatePlaceholderState()
        {
            IsUpdating = true;
            if (ListTarget == null)
            {
                PlaceholderVisibility = Visibility.Collapsed;
                return;
            }

            if (PlaceholderMode != ListPlaceholderMode.Auto && (ListTarget.ItemsSource == null || ListTarget.Items.Count == 0))
            {
                if (PlaceholderMode == ListPlaceholderMode.Loading)
                {
                    PresentLoadingPlaceholder();
                    IsUpdating = false;
                    return;
                }
                else
                {
                    PresentEmptyPlaceholder();
                    IsUpdating = false;
                    return;
                }
            }
            else if (ListTarget.ItemsSource == null)
            {
                PresentLoadingPlaceholder();
                IsUpdating = false;
                return;
            }
            else if (ListTarget.Items.Count == 0)
            {
                PresentEmptyPlaceholder();
                IsUpdating = false;
                return;
            }

            PlaceholderVisibility = Visibility.Collapsed;
        }

        private void PresentLoadingPlaceholder()
        {
            PlaceholderContent = LoadingPlaceholderContent;
            PlaceholderTemplate = LoadingPlaceholderTemplate;
            PlaceholderVisibility = Visibility.Visible;
        }

        private void PresentEmptyPlaceholder()
        {
            PlaceholderContent = EmptyPlaceholderContent;
            PlaceholderTemplate = EmptyPlaceholderTemplate;
            PlaceholderVisibility = Visibility.Visible;
        }

        private void RegisterEvents(ListViewBase lvb)
        {
            lvb.Items.VectorChanged += Items_VectorChanged;
        }

        private void UnregisterEvents(ListViewBase lvb)
        {
            lvb.Items.VectorChanged -= Items_VectorChanged;
        }
        #endregion

        #region EventHandlers
        private static void OnPlaceholderChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var placeholder = dependencyObject as ListPlaceholder;
            if (placeholder != null && !placeholder.IsUpdating)
                placeholder.UpdatePlaceholderState();
        }

        private static void OnListTargetChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var placeholder = dependencyObject as ListPlaceholder;

            if (placeholder != null)
            {
                placeholder.UpdatePlaceholderState();

                var oldLvb = e.OldValue as ListViewBase;
                var newLvb = e.NewValue as ListViewBase;

                if (oldLvb != null)
                    placeholder.UnregisterEvents(oldLvb);
                if (newLvb != null)
                    placeholder.RegisterEvents(newLvb);
            }
        }

        private void Items_VectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs @event)
        {
            UpdatePlaceholderState();
        }
        #endregion
    }
}
