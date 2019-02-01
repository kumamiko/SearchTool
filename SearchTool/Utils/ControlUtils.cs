using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SearchTool.Utils
{
    public class ScrollViewerBehavior
    {
        public static bool GetAutoScrollToTop(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoScrollToTopProperty);
        }

        public static void SetAutoScrollToTop(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollToTopProperty, value);
        }

        public static readonly DependencyProperty AutoScrollToTopProperty =
            DependencyProperty.RegisterAttached("AutoScrollToTop", typeof(bool), typeof(ScrollViewerBehavior), new PropertyMetadata(false, (o, e) =>
                {
                    var scrollViewer = o as ScrollViewer;
                    if (scrollViewer == null)
                    {
                        return;
                    }
                    if ((bool)e.NewValue)
                    {
                        scrollViewer.ScrollToTop();
                        SetAutoScrollToTop(o, false);
                    }
                }));
    }
}
