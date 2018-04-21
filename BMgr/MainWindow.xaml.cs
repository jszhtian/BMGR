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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.IO;

namespace BMgr
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void DBInit_Click(object sender, RoutedEventArgs e)
        {
            DBClass dbo = new DBClass();
            if (dbo.InitDB() == true)
            {
                MessageBox.Show("Operation Successfully Completed", "Info");
            }
        }

        private void DBDel_Click(object sender, RoutedEventArgs e)
        {
            var result=MessageBox.Show("Do you want to delete the DataBase?", "Warning", MessageBoxButton.YesNo);
            DBClass dbo = new DBClass();
            dbo.ForceClose();
            if (result == MessageBoxResult.Yes)
            {
                string fileName = "rec.db";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
            
        }

        private void AddTag_Click(object sender, RoutedEventArgs e)
        {
            addTagFunc(TagBox.Text);
        }

        private void DelTag_Click(object sender, RoutedEventArgs e)
        {
            if (TagList.SelectedItems.Count > 0)
            {
                List<string> RemoveList = new List<string>();
                foreach (var SelItem in TagList.SelectedItems)
                {
                    RemoveList.Add(SelItem.ToString());
                }
                foreach (var DelTarget in RemoveList)
                {
                    if(TagList.Items.Contains(DelTarget))TagList.Items.Remove(DelTarget);
                }
            }
        }

        private void RecNew_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameBox.Text) && !string.IsNullOrWhiteSpace(PageBox.Text))
            {
                List<string> Tags = new List<string>();
                if (TagList.Items.Count > 0)
                {
                    
                    foreach (var item in TagList.Items)
                    {
                        Tags.Add(item.ToString());
                    }
                }
                string[] TagsArray = new string[Tags.Count];
                Tags.CopyTo(TagsArray);
                string TagString = TagsClass.GetTagStringFromList(TagsArray);
                DBClass dbclass = new DBClass();
                if(dbclass.InsertRecord(NameBox.Text, AuthBox.Text, TagString, PageBox.Text, PathBox.Text)==true) MessageBox.Show("Operation Successfully Completed", "Info");
                bufdata = dbclass.QueryRecord();
                RecList.ItemsSource = bufdata;
                UpdateViewSource();
                
            }
            else
            {
                MessageBox.Show("A record must have name and page number to storage", "Info");
            }
        }

        private void addTagFunc(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                if (!TagList.Items.Contains(text)) TagList.Items.Add(text);
            }
        }
        List<DataStruct> bufdata;
        private void RecRef_Click(object sender, RoutedEventArgs e)
        {
            DBClass dbclass = new DBClass();
            bufdata = dbclass.QueryRecord();
            RecList.ItemsSource = bufdata;
        }

        private void RecList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataStruct eventdata = RecList.SelectedItem as DataStruct;
            if (eventdata != null && eventdata is DataStruct)
            {
                NameBox.Text = eventdata.name;
                AuthBox.Text = eventdata.author;
                PageBox.Text = eventdata.page.ToString();
                PathBox.Text = eventdata.path;
                string[] tags = TagsClass.GetTagListFromString(eventdata.tagstring);
                TagList.Items.Clear();
                foreach (string val in tags)
                {
                    addTagFunc(val);
                }
            }
        }
        private void UpdateViewSource()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(bufdata);
            view.Refresh();
        }
        private void RecDel_Click(object sender, RoutedEventArgs e)
        {
            DataStruct eventdata = RecList.SelectedItem as DataStruct;
            if (eventdata != null && eventdata is DataStruct)
            {
                Int64 rowid = eventdata.id;
                DBClass dbclass = new DBClass();
                for (int i = 0; i < bufdata.Count; i++)
                {
                    if (bufdata[i].id == rowid)
                        bufdata.Remove(bufdata[i]);
                }
                UpdateViewSource();
                if(dbclass.DeleteRecord(rowid)) MessageBox.Show("Operation Successfully Completed", "Info");
                
            }
        }
    }

}
