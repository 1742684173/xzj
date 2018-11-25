using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace xzj
{
    class UtilConfig
    {
        public const string DB_NAME_KEY = "db_name_key";//数据库名称
        public const string DB_IP_KEY = "db_ip_key";//ip
        public const string DB_USER_KEY = "db_user_key";//数据库用户名
        public const string DB_PWD_KEY = "db_pwd_key";//数据库密码
        public static string SQL_ADDRESS_KEY = "sql_address_key";//SQL连接
        public static string SQL_ADDRESS = "";//"server=localhost;port=3306;database=xzj;user=root;password=123456;SslMode = none;";//数据
        //public static string sqlAddress = "server=localhost;port=3306;database=xzj;user=" + user + ";password=" + pwd + ";SslMode = none;";//数据
        public static string ACCOUNT_KEY = "account_key";//保存当前登录的帐号
        public static string PWD_KEY = "pwd_key";//保存当前登录的密码
        public static string ACCOUNT = "";//保存当前登录的帐号
        public static string IS_LOGIN = "is_login";//记录是否登录
         
        
    
    }
}
