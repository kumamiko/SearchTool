using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SearchTool.Views
{
    /// <summary>
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();

            borderTitle.MouseMove += (_, e) => { if (e.LeftButton == MouseButtonState.Pressed) DragMove(); };
            btnCloseWindow.Click += (_, __) => this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            try
            {
                Process.Start(button.Tag.ToString());
            }
            catch (Exception)
            {

            }
        }
    }
}
