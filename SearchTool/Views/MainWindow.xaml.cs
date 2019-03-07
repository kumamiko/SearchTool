using SearchTool.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace SearchTool.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            spTitle.MouseMove += (_, e) => { e.Handled = true; if (e.LeftButton == MouseButtonState.Pressed) DragMove(); };
            gridClose.MouseMove += (_, e) => { e.Handled = true; if (e.LeftButton == MouseButtonState.Pressed) DragMove(); };
            btnExit.Click += (_, __) => Application.Current.Shutdown();
            gridClose.MouseLeftButtonUp += (_, __) => 
            {
                Storyboard sb = this.FindResource("OpenAnimation") as Storyboard;
                sb.Begin(rootBorder);
            };
            btnClose.Click += (_, __) =>
            {
                Storyboard sb = this.FindResource("CloseAnimation") as Storyboard;
                sb.Begin(rootBorder);
            };

            txtSearch.DragEnter += (_, e) => e.Effects = DragDropEffects.Copy;
            txtSearch.PreviewDragOver += (s, e) => { txtSearch.Text = string.Empty; };

            rootBorder.PreviewDrop += (_, e) =>
            {
                e.Effects = DragDropEffects.Copy;
            };
            rootBorder.Drop += (_, e) =>
            {
                e.Handled = true;

                if (!e.Data.GetDataPresent(DataFormats.Text)) return;

                txtSearch.Text = e.Data.GetData(typeof(string)).ToString();

                //给 txtSearch 添加 Enter
                txtSearch.RaiseEvent(new KeyEventArgs(
                      Keyboard.PrimaryDevice,
                      PresentationSource.FromVisual(this),
                      0,
                      Key.Enter)
                { RoutedEvent = TextBox.KeyDownEvent });
            };

            MainWindowViewModel.ScrollToTop += () => ScrollToTop();
        }

        private void MenuTopmost_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = !this.Topmost;
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().Show();
        }

        public void ScrollToTop() => scrollviewerContent.ScrollToTop();
    }
}
