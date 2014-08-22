using System.ComponentModel;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace QKit
{
    /// <summary>
    /// Provides the infrastructure for the GenericJumpList and AlphaJumpList classes.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ContentProperty(Name = "BaseList")]
    public class JumpListBase : Control
    {
        #region AttachedProperties
        private static readonly DependencyProperty ItemsSourceHandledProperty = DependencyProperty.RegisterAttached(
            "ItemsSourceHandled", typeof(bool), typeof(JumpListBase),
            new PropertyMetadata(false));

        private static readonly DependencyProperty CachedVisibilityProperty = DependencyProperty.RegisterAttached(
            "CachedVisibility", typeof(Visibility), typeof(JumpListBase),
            new PropertyMetadata(Visibility.Visible));

        private static bool GetItemsSourceHandled(DependencyObject element)
        {
            return (bool)element.GetValue(ItemsSourceHandledProperty);
        }

        private static void SetItemsSourceHandled(DependencyObject element, bool value)
        {
            element.SetValue(ItemsSourceHandledProperty, value);
        }

        private static Visibility GetCachedVisibility(DependencyObject element)
        {
            return (Visibility)element.GetValue(CachedVisibilityProperty);
        }

        private static void SetCachedVisibility(DependencyObject element, Visibility value)
        {
            element.SetValue(CachedVisibilityProperty, value);
        }
        #endregion

        #region DependencyProperties
        private static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object),
            typeof(JumpListBase), new PropertyMetadata(null, OnItemsSourceChanged));

        private static readonly DependencyProperty CollectionViewProperty =
            DependencyProperty.Register("CollectionView", typeof(ICollectionView),
            typeof(JumpListBase), new PropertyMetadata(null));

        /// <summary>
        /// Indentifies the CollectionGroups dependency property.
        /// </summary>
        public static readonly DependencyProperty CollectionGroupsProperty =
            DependencyProperty.Register("CollectionGroups", typeof(IObservableVector<object>),
            typeof(JumpListBase), new PropertyMetadata(null));

        /// <summary>
        /// Indentifies the JumpListGropuStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty JumpListGroupStyleProperty =
            DependencyProperty.Register("JumpListGroupStyle", typeof(GroupStyle),
            typeof(JumpListBase), new PropertyMetadata(null, OnJumpListGroupStyleChanged));

        /// <summary>
        /// Indentifies the BaseListProperty dependency property.
        /// </summary>
        public static readonly DependencyProperty BaseListProperty =
            DependencyProperty.Register("BaseList", typeof(ListViewBase),
            typeof(JumpListBase), new PropertyMetadata(null, OnBaseListChanged));
        #endregion

        #region Fields
        private const string PartSemanticZoomName = "part_SemanticZoom";
        private SemanticZoom partSemanticZoom;
        #endregion

        #region Constructors
        /// <summary>
        /// Provides base-class initialization behavior for classes that are derived from the JumpListBase class.
        /// </summary>
        protected JumpListBase()
        {
        }
        #endregion

        #region Properties
        private object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets the View of grouped items.
        /// </summary>
        public ICollectionView CollectionView
        {
            get { return (ICollectionView)GetValue(CollectionViewProperty); }
            private set { SetValue(CollectionViewProperty, value); }
        }

        /// <summary>
        /// Gets the group objects for each group of items.
        /// </summary>
        public IObservableVector<object> CollectionGroups
        {
            get { return (IObservableVector<object>)GetValue(CollectionGroupsProperty); }
            private set { SetValue(CollectionGroupsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the GroupStyle that will be used to display the grouped list.
        /// </summary>
        public GroupStyle JumpListGroupStyle
        {
            get { return (GroupStyle)GetValue(JumpListGroupStyleProperty); }
            set { SetValue(JumpListGroupStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ListViewBase control that will display the list of items.
        /// </summary>
        public ListViewBase BaseList
        {
            get { return (ListViewBase)GetValue(BaseListProperty); }
            set { SetValue(BaseListProperty, value); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Finds the part_SemanticZoom component of the control template and handles the hiding and 
        /// showing of the current page's AppBar for performance improvements.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ApplyBaseList(BaseList);

            if (partSemanticZoom != null)
                partSemanticZoom.ViewChangeStarted -= partSemanticZoom_ViewChangeStarted;

            partSemanticZoom = GetTemplateChild(PartSemanticZoomName) as SemanticZoom;

            if (partSemanticZoom != null)
                partSemanticZoom.ViewChangeStarted += partSemanticZoom_ViewChangeStarted;
        }

        /// <summary>
        /// Integrates a ListViewBase control for the main list display by applying properties and GroupStyles.
        /// </summary>
        /// <param name="newBaseList">ListViewBase control to integrate</param>
        private void ApplyBaseList(ListViewBase newBaseList)
        {
            if (newBaseList != null)
            {
                newBaseList.Padding = Padding;
                if (!newBaseList.GroupStyle.Contains(JumpListGroupStyle) && JumpListGroupStyle != null)
                    newBaseList.GroupStyle.Add(JumpListGroupStyle);
            }
        }

        /// <summary>
        /// Modifies the BaseList's ItemsSource to work with JumpList control.
        /// Call this method right after you assign or reassign BaseList's ItemsSource programatically.
        /// Do not call this method if you are binding the BaseList's ItemSource.
        /// </summary>
        public void ApplyItemsSource()
        {
            ApplyItemsSource(BaseList, true);
        }

        /// <summary>
        /// Sets a ListViewBase's ItemsSource to this ItemsSource.
        /// Then, change the ListViewBase's ItemsSource to the a ICollectionView
        /// of the original ItemsSource to display grouped items.
        /// </summary>
        /// <param name="baseList">ListViewBase control that contains the original ItemsSource</param>
        /// <param name="ignorePreviousHandling">Flag to ignore whether a ListViewBase has already been modified</param>
        private void ApplyItemsSource(ListViewBase baseList, bool ignorePreviousHandling = false)
        {
            if (baseList != null && (!GetItemsSourceHandled(baseList) || ignorePreviousHandling))
            {
                var itemsSourceBinding = baseList.GetBindingExpression(ListViewBase.ItemsSourceProperty);

                if (itemsSourceBinding != null)
                    SetBinding(ItemsSourceProperty, itemsSourceBinding.ParentBinding);
                else
                    ItemsSource = baseList.ItemsSource;

                baseList.ClearValue(JumpListBase.ItemsSourceProperty);

                Binding collectionViewBinding = new Binding();
                collectionViewBinding.Source = this;
                collectionViewBinding.Path = new PropertyPath("CollectionView");
                baseList.SetBinding(ListViewBase.ItemsSourceProperty, collectionViewBinding);

                SetItemsSourceHandled(baseList, true);
            }
        }

        /// <summary>
        /// Undo BaseList ItemsSource modification done for JumpLists.
        /// </summary>
        public void ReleaseItemsSource()
        {
            ReleaseItemsSource(BaseList, true);
        }

        /// <summary>
        /// Restores the ItemsSource of this JumpList control and the BaseList controls.
        /// </summary>
        /// <param name="baseList">ListViewBase previously modified</param>
        /// <param name="ignorePreviousHandling">Flag to ignore whether a ListViewBase has already been modified</param>
        private void ReleaseItemsSource(ListViewBase baseList, bool ignorePreviousHandling = false)
        {
            if (baseList != null && (GetItemsSourceHandled(baseList) || ignorePreviousHandling))
            {
                var itemsSourceBinding = GetBindingExpression(JumpListBase.ItemsSourceProperty);

                if (itemsSourceBinding != null)
                    baseList.SetBinding(ListView.ItemsSourceProperty, itemsSourceBinding.ParentBinding);
                else
                    baseList.ItemsSource = ItemsSource;

                ClearValue(JumpListBase.ItemsSourceProperty);
            }
        }
        #endregion

        #region EventHandlers
        private static void OnItemsSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var jumpList = dependencyObject as JumpListBase;
            if (e.NewValue == null)
            {
                jumpList.CollectionView = null;
                jumpList.CollectionGroups = null;
            }
            else
            {
                var collectionView = e.NewValue as ICollectionView;
                if (collectionView == null)
                {
                    var cvs = e.NewValue as CollectionViewSource;
                    if (cvs == null)
                        collectionView = new CollectionViewSource { Source = e.NewValue, IsSourceGrouped = true }.View;
                    else
                        collectionView = cvs.View;
                }

                jumpList.CollectionView = collectionView;
                jumpList.CollectionGroups = collectionView.CollectionGroups;
            }
        }

        private static void OnBaseListChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var jumpList = dependencyObject as JumpListBase;
            var oldBaseList = e.OldValue as ListViewBase;
            var newBaseList = e.NewValue as ListViewBase;

            if (oldBaseList != null)
            {
                jumpList.ReleaseItemsSource(oldBaseList);
                jumpList.BaseList.GroupStyle.Remove(jumpList.JumpListGroupStyle);
            }

            if (newBaseList != null)
            {
                jumpList.ApplyItemsSource(newBaseList);
                jumpList.ApplyBaseList(newBaseList);
            }
        }

        private static void OnJumpListGroupStyleChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var jumpList = dependencyObject as JumpListBase;
            var oldGroupStyle = e.OldValue as GroupStyle;
            var newGroupStyle = e.NewValue as GroupStyle;

            if (oldGroupStyle != null)
                jumpList.BaseList.GroupStyle.Remove(oldGroupStyle);

            if (newGroupStyle != null)
                jumpList.ApplyBaseList(jumpList.BaseList);
        }

        private void partSemanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            Page page = null;
            var frame = Window.Current.Content as Frame;
            if (frame != null)
                page = frame.Content as Page;

            if (page != null && page.BottomAppBar != null)
            {
                if (e.IsSourceZoomedInView)
                {
                    SetCachedVisibility(page.BottomAppBar, page.BottomAppBar.Visibility);
                    page.BottomAppBar.Visibility = Visibility.Collapsed;
                }
                else
                    page.BottomAppBar.Visibility = GetCachedVisibility(page.BottomAppBar);
            }
        }
        #endregion
    }
}
