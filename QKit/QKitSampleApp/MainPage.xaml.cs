using QKitSampleApp.InputAwarePanelSample;
using QKitSampleApp.JumpListSample;
using QKitSampleApp.ListPlaceholderSample;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace QKitSampleApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void InputAwarePanelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(InputAwarePanelSamplePage));
        }

        private void AlphaJumpListButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ContactsPage));
        }

        private void GenericJumpListButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MoviesPage));
        }

        private void ChatSampleButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ChatPage));
        }

        private void FormsSampleButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FormsPage));
        }

        private void SearchSampleButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SearchPage));
        }
    }
}
