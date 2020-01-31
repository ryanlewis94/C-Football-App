using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FootballApp.Controls
{
    /// <summary>
    /// Interaction logic for TeamControl.xaml
    /// </summary>
    public partial class TeamControl : UserControl
    {
        public TeamControl()
        {
            InitializeComponent();
            ((INotifyCollectionChanged)FixtureList.Items).CollectionChanged += ListView_CollectionChanged;
            ((INotifyCollectionChanged)MatchList.Items).CollectionChanged += ListView_CollectionChanged;
        }

        private void ListView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (VisualTreeHelper.GetChildrenCount(FixtureList) > 0)
            {
                Border border = (Border)VisualTreeHelper.GetChild(FixtureList, 0);
                ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                scrollViewer.ScrollToBottom();
            }
            if (VisualTreeHelper.GetChildrenCount(MatchList) > 0)
            {
                Border border = (Border)VisualTreeHelper.GetChild(MatchList, 0);
                ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                scrollViewer.ScrollToTop();
            }
        }
    }
}
