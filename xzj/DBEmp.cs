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
    class DBEmp
    {
        private static DBEmp instance = null;
        private static MySqlCommand cmd;
        private static MySqlConnection conn = null;
        //private static string sqlAddress = "";

        private DBEmp()
        {

            conn = new MySqlConnection(UtilConfig.SQL_ADDRESS);
            conn.Open();
            
        }


        public static DBEmp getInstance()
        {
            if (null == instance)
            {
                instance = new DBEmp();
            }
            
            return instance;
            
        }



        // 登录
        public bool isLogin(string account, string pwd)
        {
            bool result = false;
            string sql = string.Format("select id from t_emp where e_account='{0}' and e_pwd='{1}'", account,  pwd);
            try
            {
                MySqlCommand comm = new MySqlCommand(sql, conn);
                int flag = (int)comm.ExecuteScalar();
                if (flag > 0)
                {
                    result = true;
                }

            }
            catch (Exception err)
            {
                Console.Write(err);
            }
            //conn.Close();
            return result;
        }

        // 用户是否存在
        public bool isAccount(string account)
        {
            bool result = false;
            string sql = string.Format("select id from t_emp where e_account='{0}'", account);
            try
            {

                MySqlCommand comm = new MySqlCommand(sql, conn);
                int flag = (int)comm.ExecuteScalar();
                if (flag > 0)
                {
                    result = true;
                }
            }
            catch (Exception err)
            {
                Console.Write(err);
            }
            //conn.Close();
            return result;
        } 

        /**
     id int not null auto_increment,##唯一标识 自增 
 	e_account varchar(50),##用户名
	e_name varchar(50),##姓名
 	e_sex char(2),##性别
 	e_birth varchar(20),##出生日期
 	e_tel varchar(11),##手机号
 	e_email varchar(20),##邮箱
 	e_address varchar(100),##地址
 	e_pwd varchar(20),##密码
    * */
        // 添加用户
        public int addEmp(string account, string name, string sex, string birth, string tel, string email, string address, string pwd)
        {
            int flag = 0;
            string sql = string.Format("insert into t_emp(e_account,e_name,e_sex,e_birth,e_tel,e_email,e_address,e_pwd)"
                + "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", account, name, sex, birth, tel, email, address, pwd);
            
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

        // 添加用户
        public int editEmp(string account, string name, string sex, string birth, string tel, string email, string address, string pwd)
        {
            int flag = 0;//"update t_emp set e_name='{1}',e_sex='{2}',e_birth='{3}',e_tel='{4}',e_email='{5}',e_address='{6}',e_pwd='{7}' where  e_account='{0}'"
            string sql = string.Format("update t_emp set e_name='{1}',e_sex='{2}',e_birth='{3}',e_tel='{4}',e_email='{5}',e_address='{6}',e_pwd='{7}' where  e_account='{0}'"
               , account, name, sex, birth, tel, email, address, pwd);

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

        // 删除用户 by account
        public int deleteEmpByAccount(string account)
        {
            int flag = 0;
            string sql = string.Format("delete from t_emp where e_account = '{0}'", account);

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
    
        //查询数据
        public DataTable getEmps(string account,string name)
        {
            string sql = "";//string.Format("select id,e_account,e_name,e_sex,e_birth,e_tel,e_email,e_address from t_emp where e_account='{0}'", account)
            if (string.IsNullOrEmpty(account) && string.IsNullOrEmpty(name))
            {
                sql = string.Format("select id,e_account,e_name,e_sex,e_birth,e_tel,e_email,e_address,e_pwd from t_emp");
            }
            else if (!string.IsNullOrEmpty(account) && string.IsNullOrEmpty(name))
            {
                sql = string.Format("select id,e_account,e_name,e_sex,e_birth,e_tel,e_email,e_address,e_pwd from t_emp where e_account like '%{0}%'", account);
            }
            else if (string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(name))
            {
                sql = string.Format("select id,e_account,e_name,e_sex,e_birth,e_tel,e_email,e_address,e_pwd from t_emp where e_name like '%{0}%'", name);
            }
            else
            {
                sql = string.Format("select id,e_account,e_name,e_sex,e_birth,e_tel,e_email,e_address,e_pwd from t_emp where e_account like '%{0}%' and e_name like '%{1}%'", account, name);
            }

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
            //string conStr = "Server=localhost;user=root;pwd=admin;database=test";
            //using (MySqlConnection myCon = new MySqlConnection(conStr))
            //{
               
            //}
        }
    }
}
