using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace xzj
{
    class DBManager
    {
        private static DBManager instance = null;
        private static MySqlCommand cmd;
        private static MySqlConnection conn = null;
        //private static string sqlAddress = "";

        private DBManager()
        {
            conn = new MySqlConnection(UtilConfig.SQL_ADDRESS);
            conn.Open();
           
        }

        public static Boolean isConnectionWorks() { 
            MySqlConnection testConnection = new MySqlConnection(UtilConfig.SQL_ADDRESS);

            try
            {
                testConnection.Open();
                return true;
            }
            catch (Exception err) {
                Console.WriteLine(err.Message);
                return false;
            }
        }


        public static DBManager getInstance()
        {
            if (null == instance)
            {
                instance = new DBManager();
            }
            
            return instance;
            
        }

        public MySqlConnection getMySqlConnection()
        {
            return conn;
        }

        public MySqlCommand getMySqlCommand(string sql)
        {
            cmd = new MySqlCommand(sql, conn);
            //设置Command对象的类型  
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        //新增
        public int add(string sql)
        {
            int flag = 0;
           
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

        // 编辑update t_patient set p_name='郭靖',p_sex='男',p_age='150',p_tel='18450068965',p_health_type='市医保',p_address='贵州-贵阳-中山',p_dialyse_hospital='贵阳医学院'p_dialyse_hospital_contact='黄鹤',p_dialyse_hospital_tel='18450036796' where p_ID='522126186825365632'
        public int edit(string sql)
        {
            int flag = 0;
           
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

        //查询select id,r_date,r_ss_address,r_ss_type,r_ss_method,r_cc_method,r_zd_docotor,r_zs,r_qxhs,r_ss_record,r_is_sszz from t_patient where r_patient_ID='522126186825365632' order by id DESC limit 1
        public DataTable find(string sql)
        {
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

        // 删除
        public int delete(string sql)
        {
            int flag = 0;
          
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
    }
}
