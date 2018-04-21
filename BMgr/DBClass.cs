using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace BMgr
{
    public class DataStruct
    {
        public Int64 id { get; set; }
        public string name { get; set; }
        public string author { get; set; }
        public string tagstring { get; set; }
        public Int64 page { get; set; }
        public string path { get; set; }
    }
    class DBClass
    {
        private static SQLiteConnection DBconn;
        public DBClass()
        {
            if (DBconn == null)
            {
                DBconn = new SQLiteConnection("Data Source=rec.db;Version=3;Compress=True;");
                DBconn.Open();
            }

        }
        public bool InitDB()
        {
            SQLiteCommand cmd;
            cmd = DBconn.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS Record(name text NOT NULL,author text,tag text,page INTEGER NOT NULL,path text);";
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error:" + ex.ToString(), "Error Info");
                return false;
            }
            return true;
        }
        public void ForceClose()
        {
            if(DBconn.State== ConnectionState.Open) DBconn.Close();
            DBconn = null;
        }
        public List<DataStruct> QueryRecord(bool LoadAll=false)
        {
            SQLiteCommand cmd;
            SQLiteDataReader reader;
            List<DataStruct> list = new List<DataStruct>();
            cmd = DBconn.CreateCommand();
            if (!LoadAll)
            {
                cmd.CommandText = "SELECT rowid,name,author,tag,page,path FROM Record LIMIT 500;";
            }
            else
            {
                cmd.CommandText = "SELECT rowid,name,author,tag,page,path FROM Record;";
            }
            
            try
            {
                reader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error:" + ex.ToString(), "Error Info");
                return null;
            }
            while (reader.Read())
            {
                DataStruct data = new DataStruct();
                data.name = (string)reader["name"];
                data.id = (Int64)reader["rowid"];
                data.author = (string)reader["author"];
                data.tagstring = (string)reader["tag"];
                data.path = (string)reader["path"];
                data.page = (Int64)reader["page"];
                list.Add(data);
            }
            return list;
        }
        public bool InsertRecord(string Name,string Author,string TagString,string page,string path)
        {
            int pagenum = 0;
            if(int.TryParse(page,out pagenum)!=true) return false;
            SQLiteCommand cmd;
            cmd = DBconn.CreateCommand();
            cmd.CommandText = "insert into Record(name,author,tag,page,path)VALUES(@Name,@Author,@Tag,@Page,@Path);";
            cmd.Parameters.Add("@Name", DbType.String);
            cmd.Parameters.Add("@Author", DbType.String);
            cmd.Parameters.Add("@Tag", DbType.String);
            cmd.Parameters.Add("@Page", DbType.Int64);
            cmd.Parameters.Add("@Path", DbType.String);
            cmd.Parameters[0].Value = Name;
            cmd.Parameters[1].Value = Author;
            cmd.Parameters[2].Value = TagString;
            cmd.Parameters[3].Value = pagenum;
            cmd.Parameters[4].Value = path;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error:" + ex.ToString(), "Error Info");
                return false;
            }
            return true;
        }
        public bool DeleteRecord(Int64 id)
        {
            SQLiteCommand cmd;
            cmd = DBconn.CreateCommand();
            cmd.CommandText = "DELETE FROM Record WHERE rowid=@id;";
            cmd.Parameters.Add("@id", DbType.Int64);
            cmd.Parameters[0].Value = id;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error:" + ex.ToString(), "Error Info");
                return false;
            }
            return true;
        }
        public bool UpdateRecord(Int64 id,string Name, string Author, string TagString, string page, string path)
        {
            int pagenum = 0;
            if (int.TryParse(page, out pagenum) != true) return false;
            SQLiteCommand cmd;
            cmd = DBconn.CreateCommand();
            cmd.CommandText = "update Record set name=@Name,author=@Author,tag=@Tag,page=@Page,path=@Path where rowid=@id;";
            cmd.Parameters.Add("@Name", DbType.String);
            cmd.Parameters.Add("@Author", DbType.String);
            cmd.Parameters.Add("@Tag", DbType.String);
            cmd.Parameters.Add("@Page", DbType.Int64);
            cmd.Parameters.Add("@Path", DbType.String);
            cmd.Parameters.Add("@id", DbType.Int64);
            cmd.Parameters[0].Value = Name;
            cmd.Parameters[1].Value = Author;
            cmd.Parameters[2].Value = TagString;
            cmd.Parameters[3].Value = pagenum;
            cmd.Parameters[4].Value = path;
            cmd.Parameters[5].Value = id;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error:" + ex.ToString(), "Error Info");
                return false;
            }
            return true;
        }
        public List<DataStruct> QueryRecordByName(string name)
        {
            SQLiteCommand cmd;
            SQLiteDataReader reader;
            List<DataStruct> list = new List<DataStruct>();
            cmd = DBconn.CreateCommand();
            cmd.CommandText = "SELECT rowid,name,author,tag,page,path FROM Record WHERE name LIKE @name;";
            cmd.Parameters.Add("@name", DbType.String);
            cmd.Parameters[0].Value = "%"+name+"%";
            try
            {
                reader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error:" + ex.ToString(), "Error Info");
                return null;
            }
            while (reader.Read())
            {
                DataStruct data = new DataStruct();
                data.name = (string)reader["name"];
                data.id = (Int64)reader["rowid"];
                data.author = (string)reader["author"];
                data.tagstring = (string)reader["tag"];
                data.path = (string)reader["path"];
                data.page = (Int64)reader["page"];
                list.Add(data);
            }
            return list;
        }
        public List<DataStruct> QueryRecordByAuthor(string author)
        {
            SQLiteCommand cmd;
            SQLiteDataReader reader;
            List<DataStruct> list = new List<DataStruct>();
            cmd = DBconn.CreateCommand();
            cmd.CommandText = "SELECT rowid,name,author,tag,page,path FROM Record WHERE author LIKE @author;";
            cmd.Parameters.Add("@author", DbType.String);
            cmd.Parameters[0].Value = "%"+author+"%";
            try
            {
                reader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error:" + ex.ToString(), "Error Info");
                return null;
            }
            while (reader.Read())
            {
                DataStruct data = new DataStruct();
                data.name = (string)reader["name"];
                data.id = (Int64)reader["rowid"];
                data.author = (string)reader["author"];
                data.tagstring = (string)reader["tag"];
                data.path = (string)reader["path"];
                data.page = (Int64)reader["page"];
                list.Add(data);
            }
            return list;
        }
    }
}
