using QKit.Utils;
using System.ComponentModel;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace QKit
{
    /// <summary>
    /// Represents a control will augment a ListViewBase control to look and behave like a native AlphaJumpList control.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Always)]
    public sealed class AlphaJumpList : JumpListBase
    {
        #region Fields
        private const string PartAlphaPickerName = "part_AlphaPicker";
        private const double PickerNoMargin = -9.5; // Margin of 0 minus item template margin of 9.5
        private const double PickerShortMargin = 15;
        private const double PickerStandardMargin = 19;
        private const double PickerLongMargin = 45;
        private const double PickerExtraLongMargin = 66.5; // Margin of 76 minus item template margin of 9.5
        private GridView partAlphaPicker;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the AlphaJumpList class.
        /// </summary>
        public AlphaJumpList()
        {
            this.DefaultStyleKey = typeof(AlphaJumpList);

            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                DisplayInformation.GetForCurrentView().OrientationChanged += AlphaJumpList_OrientationChanged;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Finds the part_AlphaPicker GridView in the control template and adjusts its size for device orientation.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            partAlphaPicker = GetTemplateChild(PartAlphaPickerName) as GridView;
            UpdateAlphaPickerDimensions();
        }

        private void UpdateAlphaPickerDimensions()
        {
            if (partAlphaPicker != null)
            {
                var currentOrientation = DisplayOrientations.Portrait;
                var scrollViewer = VisualTreeUtil.FindVisualChild<ScrollViewer>(partAlphaPicker);

                if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                    currentOrientation = DisplayInformation.GetForCurrentView().CurrentOrientation;

                if (scrollViewer != null)
                {
                    switch (currentOrientation)
                    {
                        case DisplayOrientations.Landscape:
                        case DisplayOrientations.LandscapeFlipped:
                            scrollViewer.Height = 384;
                            scrollViewer.Width = 640;
                            if (partAlphaPicker.Items.Count > 28)
                                partAlphaPicker.Padding = new Thickness(PickerShortMargin, PickerStandardMargin, PickerNoMargin, PickerExtraLongMargin);
                            else
                                partAlphaPicker.Padding = new Thickness(PickerShortMargin, PickerStandardMargin, PickerNoMargin, 0);
                            break;
                        case DisplayOrientations.None:
                        case DisplayOrientations.Portrait:
                        case DisplayOrientations.PortraitFlipped:
                        default:
                            var aspectRatio = Window.Current.Bounds.Height / Window.Current.Bounds.Width;
                            scrollViewer.Height = 384 * aspectRatio;
                            scrollViewer.Width = 384;
                            if (partAlphaPicker.Items.Count > 28)
                                partAlphaPicker.Padding = new Thickness(PickerStandardMargin, PickerLongMargin, PickerNoMargin, PickerExtraLongMargin);
                            else
                                partAlphaPicker.Padding = new Thickness(PickerStandardMargin, PickerShortMargin, PickerNoMargin, -100);
                            break;
                    }
                }
                else
                    partAlphaPicker.Loaded += PartAlphaPicker_Loaded;
            }
        }
        #endregion

        #region Event Handlers
        private void AlphaJumpList_OrientationChanged(DisplayInformation sender, object args)
        {
            UpdateAlphaPickerDimensions();
        }

        private void PartAlphaPicker_Loaded(object sender, RoutedEventArgs e)
        {
            var gridView = sender as GridView;
            if (gridView != null)
            {
                gridView.Loaded -= PartAlphaPicker_Loaded;
                UpdateAlphaPickerDimensions();
            }
        }
        #endregion
    }
}
