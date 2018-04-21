using System;
using System.Collections.Generic;
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

namespace BMgr
{
    /// <summary>
    /// CoverWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CoverWindow : Window
    {
        public CoverWindow(string CoverPath)
        {
            InitializeComponent();
            try
            {
                BitmapImage bimg = new BitmapImage();
                bimg.BeginInit();
                bimg.UriSource = new Uri(CoverPath);
                bimg.EndInit();
                Cover.Source = bimg;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error:" + ex.ToString(), "Error Info");
            }
            
        }
    }
}
