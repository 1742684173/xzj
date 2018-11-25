using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace xzj
{
    class DBDictionary
    {
        private static DBDictionary instance = null;
        private static MySqlCommand cmd;
        private static MySqlConnection conn = null;

        private DBDictionary()
        {
            conn = new MySqlConnection(UtilConfig.SQL_ADDRESS);
            conn.Open();
        }

        public static DBDictionary getInstance()
        {
            if (null == instance)
            {
                instance = new DBDictionary();
            }
            
            return instance;
        }

        /**
    d_order string,##排列序号
	d_parent_id string,##父名称
	d_name varchar(50),##字典名称
	d_desc varchar(500),#描述
   * */
        // 添加字典
        public int addDictionary(string d_order, int d_parent_id, string d_name, string d_desc)
        {
            int flag = 0;
            string sql = string.Format("insert into t_dictionary(d_order,d_parent_id,d_name,d_desc)"
                + "values('{0}','{1}','{2}','{3}')", d_order, d_parent_id, d_name, d_desc);

            try
            {
                MySqlCommand comm = new MySqlCommand(sql, conn);
                flag = comm.ExecuteNonQuery();
                return flag;

            }
            catch (Exception err)
            {
                Console.Write(err);
            }
            //conn.Close();
            return flag;
        }

        // 编辑字典
        public int editDictionary(int id,string d_order, int d_parent_id, string d_name, string d_desc)
        {
            int flag = 0;//"update t_emp set e_name='{1}',e_sex='{2}',e_birth='{3}',e_tel='{4}',e_email='{5}',e_address='{6}',e_pwd='{7}' where  e_account='{0}'"
            string sql = string.Format("update t_dictionary set d_order='{0}',d_name='{1}',d_desc='{2}' where  id={3}"
               , d_order, d_name, d_desc, id);

            try
            {
                MySqlCommand comm = new MySqlCommand(sql, conn);
                flag = comm.ExecuteNonQuery();
                return flag;

            }
            catch (Exception err)
            {
                Console.Write(err);
            }
            //conn.Close();
            return flag;
        }

        // 删除
        public int deleteDictionary(int id)
        {
            int flag = 0;
            string sql = string.Format("delete from t_dictionary where id = {0}", id);

            try
            {
                MySqlCommand comm = new MySqlCommand(sql, conn);
                flag = comm.ExecuteNonQuery();
                return flag;

            }
            catch (Exception err)
            {
                Console.Write(err);
            }
            //conn.Close();
            return flag;
        }

        //查询数据       getDictionarysById
        public DataTable getDictionarysById(int id)
        {
            string sql = string.Format("select id,d_order,d_parent_id,d_name,d_desc from t_dictionary where id={0}", id);

            Console.Write("getDictionarysById--->" + sql);
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
            catch (Exception err)
            {
                Console.Write(err);
            }

            return null;
        }

        //查询数据通过父id
        public DataTable getDictionarysByParentId(int parentId)
        {
            string sql = string.Format("select id,d_order,d_parent_id,d_name,d_desc from t_dictionary where d_parent_id={0} order by d_order,d_name", parentId);


            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
            catch (Exception err)
            {
                Console.Write(err);
            }

            return null;
        }

        //查询数据通过父id和名称
        public DataTable getDictionarysByParentIdAndName(int parentId,string d_name)
        {
            string sql = string.Format("select id,d_order,d_parent_id,d_name,d_desc from t_dictionary where d_parent_id={0} and d_name='{1}'", parentId, d_name);


            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
            catch (Exception err)
            {
                Console.Write(err);
            }

            return null;
        }

    }
}
