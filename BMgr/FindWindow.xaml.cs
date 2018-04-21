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
    /// FindWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FindWindow : Window
    {
        public enum SearchMode{cancelFind,byName,byAuthor,byTags};
        public static SearchMode result= SearchMode.cancelFind;
        public FindWindow()
        {
            InitializeComponent();
        }

        private void Cancel_button_Click(object sender, RoutedEventArgs e)
        {
            result = SearchMode.cancelFind;
            this.Close();
        }

        private void Run_button_Click(object sender, RoutedEventArgs e)
        {
            if (byauthor.IsChecked==true) result = SearchMode.byAuthor;
            if (byname.IsChecked == true) result = SearchMode.byName;
            if (bytag.IsChecked == true) result = SearchMode.byTags;
            this.Close();
        }
    }
}
