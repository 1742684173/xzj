using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace xzj
{
    class DBSQLite
    {
        private static string path = @"./mydate.sqlite";
        /// <summary>
        /// 数据库连接定义
        /// </summary>
        private SQLiteConnection dbConnection;

        /// <summary>
        /// SQL命令定义
        /// </summary>
        private SQLiteCommand dbCommand;

        /// <summary>
        /// 数据读取定义
        /// </summary>
        private SQLiteDataReader dataReader;

        //---创建数据库
        public static void CreateDB()
        {
            try
            {
                SQLiteConnection cn = new SQLiteConnection("data source=" + path);
                cn.Open();
                cn.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
           
        }

        //---删除数据库
        public static void DeleteDB()
        {
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
        }

        public static void CreateTable()
        {
            try
            {
                SQLiteConnection cn = new SQLiteConnection("data source=" + path);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS mydata(id INTEGER PRIMARY KEY AUTOINCREMENT, key varchar(20),value varchar(20))";
                    //cmd.CommandText = "CREATE TABLE IF NOT EXISTS t1(id varchar(4),score int)";
                    cmd.ExecuteNonQuery();
                }
                cn.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
        }

        public static void DeleteTable()
        {
            try
            {
                SQLiteConnection cn = new SQLiteConnection("data source=" + path);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = "DROP TABLE IF EXISTS mydata";
                    cmd.ExecuteNonQuery();
                }
                cn.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine("-->DeleteTable" + err);
            }
        }

        public static void clear()
        {
            try
            {
                SQLiteConnection cn = new SQLiteConnection("data source=" + path);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = "delete from mydata";
                    cmd.ExecuteNonQuery();
                }
                cn.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine("-->DeleteTable" + err);
            }
        }

        public static void clearByKey(string key)
        {
            try
            {
                SQLiteConnection cn = new SQLiteConnection("data source=" + path);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = string.Format("delete from mydata where key='{0}'", key);
                    cmd.ExecuteNonQuery();
                }
                cn.Close();
            }
            catch (Exception err)
            {
                Console.WriteLine("-->DeleteTable" + err);
            }
        }

        public static int insertValue(string key, string value)
        {
            int flag = 0;
            string sqlstr = string.Format("INSERT INTO mydata(key,value) VALUES('{0}','{1}')", key, value);
            try
            {
                SQLiteConnection cn = new SQLiteConnection("data source=" + path);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(sqlstr, cn);
                    //cmd.CommandText = sqlstr;
                    //cmd.Parameters.Add("key", System.Data.DbType.String).Value = key;
                    //cmd.Parameters.Add("value", System.Data.DbType.String).Value = value;
                    flag = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("-->insertValue" + err);
            }
            return flag;
        }

        public static int updateValue(string key, string value)
        {
            string myvalue = selectValue(key);
            if (string.IsNullOrEmpty(myvalue))
            {
                insertValue(key, value);
                return 0;
            }
            int flag = 0;
            // [bugfix lxy]
            string sqlstr = string.Format("update mydata set value='{0}' where key='{1}'", value, key);
            try
            {
                SQLiteConnection cn = new SQLiteConnection("data source=" + path);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(sqlstr, cn);
                    //cmd.CommandText = sqlstr;
                    //cmd.Parameters.Add("key", System.Data.DbType.String).Value = key;
                    //cmd.Parameters.Add("value", System.Data.DbType.String).Value = value;
                    flag = cmd.ExecuteNonQuery();
                    cn.Close();
                }
             }
             catch (Exception err)
             {
                 Console.WriteLine("-->updateValue" + err);
             }
            
            return flag;
        }

        public static string selectValue(string key)
        {
            string result = null;
            string sqlstr = string.Format("select value from mydata where key='{0}'", key);
            try
            {
                SQLiteConnection cn = new SQLiteConnection("data source=" + path);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = new SQLiteCommand(sqlstr,cn);
                    //cmd.CommandText = sqlstr;
                    //cmd.Parameters.Add("key", System.Data.DbType.String).Value = key;
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    while (sr.Read())
                    {
                        result = sr.GetString(0);
                    }
                    sr.Close();
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("-->selectValue" + err);
            }
             
            return result;
        }
    }
}
