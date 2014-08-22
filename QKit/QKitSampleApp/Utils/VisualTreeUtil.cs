using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace QKitSampleApp.Utils
{
    internal class VisualTreeUtil
    {
        internal static TResult FindVisualChild<TResult>(DependencyObject obj)
               where TResult : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is TResult)
                    return (TResult)child;
                else
                {
                    TResult childOfChild = FindVisualChild<TResult>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        internal static TResult FindVisualParent<TResult>(DependencyObject obj)
            where TResult : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            if (parent != null)
            {
                if (parent is TResult)
                    return (TResult)parent;
                else
                    return FindVisualParent<TResult>(parent);
            }
            return null;
        }
    }
}
