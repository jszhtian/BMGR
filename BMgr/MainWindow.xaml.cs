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
                CoverBox.Text = "";
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
                for (int i = bufdata.Count - 1; i >= 0; i--)
                {
                    if (bufdata[i].id == rowid)
                        bufdata.Remove(bufdata[i]);
                }
                UpdateViewSource();
                if(dbclass.DeleteRecord(rowid)) MessageBox.Show("Operation Successfully Completed", "Info");
                
            }
        }

        private void RecUpd_Click(object sender, RoutedEventArgs e)
        {
            DataStruct eventdata = RecList.SelectedItem as DataStruct;
            if (eventdata != null && eventdata is DataStruct)
            {

                Int64 rowid = eventdata.id;
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
                if(dbclass.UpdateRecord(rowid, NameBox.Text, AuthBox.Text, TagString, PageBox.Text, PathBox.Text)) MessageBox.Show("Operation Successfully Completed", "Info");
                for (int i = 0; i < bufdata.Count; i++)
                {
                    if (bufdata[i].id == rowid)
                    {
                        bufdata[i].author = AuthBox.Text;
                        bufdata[i].name = NameBox.Text;
                        bufdata[i].page = int.Parse(PageBox.Text);
                        bufdata[i].path = PathBox.Text;
                        bufdata[i].tagstring = TagString;
                    }
                        
                }
                UpdateViewSource();
            }
        }

        private void pagebox_inputfilter(object sender, TextCompositionEventArgs e)
        {
            foreach (var ch in e.Text)
            {
                if (!(Char.IsDigit(ch)) )
                    {
                    e.Handled = true;
                    break;
                    }
            }
        }

        private void RecFind_Click(object sender, RoutedEventArgs e)
        {
            FindWindow wnd = new FindWindow();
            wnd.Owner= Application.Current.MainWindow;
            wnd.WindowStartupLocation= WindowStartupLocation.CenterOwner;
            wnd.ShowDialog();
            FindWindow.SearchMode result = FindWindow.result;
            if (result== FindWindow.SearchMode.cancelFind) MessageBox.Show("CANCEL Search", "Info");
            if (result == FindWindow.SearchMode.byAuthor)
            {
                if (!string.IsNullOrWhiteSpace(AuthBox.Text))
                {
                    DBClass dbclass = new DBClass();
                    bufdata = dbclass.QueryRecordByAuthor(AuthBox.Text);
                    RecList.ItemsSource = bufdata;
                }
                else
                {
                    MessageBox.Show("search condition is empty", "Info");
                }
                
            }
            if (result == FindWindow.SearchMode.byName)
            {
                if (!string.IsNullOrWhiteSpace(NameBox.Text))
                {
                    DBClass dbclass = new DBClass();
                    bufdata = dbclass.QueryRecordByName(NameBox.Text);
                    RecList.ItemsSource = bufdata;
                }
                else
                {
                    MessageBox.Show("search condition is empty", "Info");
                }
                
            }
            if (result == FindWindow.SearchMode.byTags)
            {
                DBClass dbclass = new DBClass();
                bufdata = dbclass.QueryRecord(true);
                if (TagList.Items.Count > 0)
                {
                    List<string> Tags = new List<string>();
                    foreach (var item in TagList.Items)
                    {
                        Tags.Add(item.ToString());
                    }
                    string[] TagsArray = new string[Tags.Count];
                    Tags.CopyTo(TagsArray);
                    for (int i = bufdata.Count-1; i >=0; i--)
                    {
                        if (!TagsClass.IsContainAllTags(TagsClass.GetTagListFromString(bufdata[i].tagstring), TagsArray)) bufdata.Remove(bufdata[i]);
                    }
                    RecList.ItemsSource = bufdata;
                }
                else
                {
                    MessageBox.Show("search condition is empty", "Info");
                }
            }
        }

        private void Load_Meta_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PathBox.Text))
            {
                if (PathBox.Text[PathBox.Text.Length-1] != '\\') PathBox.Text += "\\";
                string xmlPath = System.IO.Path.GetDirectoryName(PathBox.Text) + "\\meta.xml";
                if (!File.Exists(xmlPath))
                {
                    MessageBox.Show("No meta file found", "Info");
                    return;
                }
                string xmlString = File.ReadAllText(xmlPath, Encoding.Unicode);
                try
                {
                    MetaClass GetDeserializedObj = MetaClass.XmlDeserialize<MetaClass>(xmlString);
                    AuthBox.Text = GetDeserializedObj.author;
                    CoverBox.Text = GetDeserializedObj.cover;
                    NameBox.Text = GetDeserializedObj.name;
                    PageBox.Text = GetDeserializedObj.page.ToString();
                    string[] tags = TagsClass.GetTagListFromString(GetDeserializedObj.tagstring);
                    TagList.Items.Clear();
                    foreach (string val in tags)
                    {
                        addTagFunc(val);
                    }
                    MessageBox.Show("Operation Successfully Completed", "Info");
                    if (!string.IsNullOrWhiteSpace(CoverBox.Text))
                    {
                        string imgpath = System.IO.Path.GetDirectoryName(PathBox.Text) + "\\" + CoverBox.Text;
                        CoverWindow cover = new CoverWindow(imgpath);
                        cover.Owner = Application.Current.MainWindow;
                        cover.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        cover.Show();
                    }
                    
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error:" + ex.ToString(), "Error Info");
                }

            }
            else
            {
                MessageBox.Show("Path is empty", "Info");
            }
        }

        private void Save_Meta_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PathBox.Text))
            {
                if(PathBox.Text[PathBox.Text.Length-1]!= '\\') PathBox.Text+="\\";
                string xmlPath = System.IO.Path.GetDirectoryName(PathBox.Text) + "\\meta.xml";
                MetaClass meta = new MetaClass();
                meta.author = AuthBox.Text;
                meta.cover = CoverBox.Text;
                meta.name = NameBox.Text;
                meta.page = int.Parse(PageBox.Text);
                List<string> Tags = new List<string>();
                foreach (var item in TagList.Items)
                {
                    Tags.Add(item.ToString());
                }
                string[] TagsArray = new string[Tags.Count];
                Tags.CopyTo(TagsArray);
                meta.tagstring = TagsClass.GetTagStringFromList(TagsArray);
                var xmlString=MetaClass.XmlSerialize(meta);
                if (File.Exists(xmlPath))
                {
                    File.Delete(xmlPath);
                }
                File.WriteAllText(xmlPath, xmlString,Encoding.Unicode);
                MessageBox.Show("Operation Successfully Completed", "Info");
            }
            else
            {
                MessageBox.Show("Path is empty", "Info");
            }
        }
    }

}
