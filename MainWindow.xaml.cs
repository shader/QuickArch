using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
namespace QuickArch
{
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ContextMenu_Opened(object sender, System.Windows.RoutedEventArgs e)
        {
            ContextMenu menu = sender as ContextMenu;
            menu.DataContext = DataContext;
        }

        private void archTree_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(e.OriginalSource is TextBlock))
            {
                QuickArch.ViewModel.SystemViewModel item = archTree.SelectedItem as QuickArch.ViewModel.SystemViewModel;
                if (item != null)
                {
                    archTree.Focus();
                    item.IsSelected = false;
                    QuickArch.ViewModel.MainWindowViewModel mwVM = this.DataContext as QuickArch.ViewModel.MainWindowViewModel;
                    mwVM.SelectedComponentVM = null;
                }
            }
            else
            {
                QuickArch.ViewModel.SystemViewModel item = archTree.SelectedValue as QuickArch.ViewModel.SystemViewModel;
                if (item != null)
                {
                    item.IsSelected = true;
                    archTree.Focus();
                }
            }
        }

        private void TreeViewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item != null)
            {
                item.Focus();
                e.Handled = true;
            }
        }

    }
}
