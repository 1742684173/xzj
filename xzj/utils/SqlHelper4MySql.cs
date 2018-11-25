using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace xzj.utils
{
    class SqlHelper4MySql
    {
        // static string conStr = ConfigurationManager.ConnectionStrings["sqlCon"].ConnectionString;
        private static string conStr = "server=localhost;port=3306;database=xzj;user=root;password=123456;SslMode = none;";

        public static string getConStr()
        {
            return SqlHelper4MySql.conStr;
        }
        public static void setConStr(String conStr)
        {
            SqlHelper4MySql.conStr = conStr;
        }

        public static MySqlConnection getConnection() {
            return new MySqlConnection(conStr);
        }

        // 由外界管理Connection（可能要做事务的情况）
        public static int ExecuteNonQuery(MySqlConnection con, String sql, params MySqlParameter[] ps)
        {
            using (MySqlCommand cmd = new MySqlCommand(sql, con))
            {
                if (null != ps)
                {
                    cmd.Parameters.AddRange(ps);
                }
                return cmd.ExecuteNonQuery();
            }
        }
        // 获得执行的影响条数
        public static int ExecuteNonQuery(String sql, params MySqlParameter[] ps)
        {
            using (MySqlConnection con = new MySqlConnection(conStr))
            {
                con.Open();
                return ExecuteNonQuery(con, sql, ps);
            }
        }


        public static object ExecuteScalar(MySqlConnection con, String sql, params MySqlParameter[] ps)
        {
            using (MySqlCommand cmd = new MySqlCommand(sql, con))
            {
                if (null != ps) {
                    cmd.Parameters.AddRange(ps);
                }
                return cmd.ExecuteScalar();
            }
        }

        // 只获取单行单列，一般sum，count之类的结果获取
        public static object ExecuteScalar(String sql, params MySqlParameter[] ps)
        {
            using (MySqlConnection con = new MySqlConnection(conStr))
            {
                con.Open();
                return ExecuteScalar(con, sql, ps);
            }
        }


        public static MySqlDataReader ExecuteReader(MySqlConnection con, String sql, params MySqlParameter[] ps)
        {
            MySqlCommand cmd = new MySqlCommand(sql, con);
            try
            {
                if (null != ps)
                {
                    cmd.Parameters.AddRange(ps);
                }
                return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // 获取一个reader，然后可以遍历这个reader，得到查询出来的每一行
        public static MySqlDataReader ExecuteReader(String sql, params MySqlParameter[] ps)
        {
            MySqlConnection con = new MySqlConnection(conStr);
            try
            {
                con.Open();
                return ExecuteReader(con, sql, ps);
            }
            catch (Exception ex)
            {
                // System.Windows.Forms.MessageBox.Show("保存失败!");
                con.Dispose();
                throw ex;
            }
          
        }

        public static DataTable getDataTable(string sql, params MySqlParameter[] ps)
        {
            MySqlConnection con = new MySqlConnection(conStr);
            try
            {
                con.Open();
                
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                adapter.SelectCommand.CommandType = CommandType.Text;
                if (null != ps) {
                    adapter.SelectCommand.Parameters.AddRange(ps);
                }
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                // System.Windows.Forms.MessageBox.Show("保存失败!");
                con.Dispose();
                throw ex;
            }
        }

        /*
        public static MySqlDateSet GetMySqlDateSet(String sql, params MySqlParameter[] ps)
        {
            MySqlDateSet ds = new MySqlDateSet();
            using (SqlDataAdapter sda = new SqlDataAdapter(sql, conStr))
            {
                sda.SelectCommand.Parameters.AddRange(ps);
                sda.Fill(ds);
            }
            return ds;
        }*/

    }
}
