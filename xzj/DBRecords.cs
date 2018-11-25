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
    class DBRecords
    {
        private static DBRecords instance = null;
        private static MySqlCommand cmd;
        private static MySqlConnection conn = null;
        //private static string sqlAddress = "";

        private DBRecords()
        {
            conn = new MySqlConnection(UtilConfig.SQL_ADDRESS);
            conn.Open();
        }

        public static DBRecords getInstance()
        {
            if (null == instance)
            {
                instance = new DBRecords();
            }
            
            return instance;
            
        }

        /**
         * id
         * t_name varchar(50),##姓名，
	t_sex varchar(2),##性别（选择男/女，
	t_age char(3),##年龄（）根据身份证自动生成，
	t_tel varchar(11),##手机号，（验证）
	t_ID varchar(50),##身份证号
	t_health_type varchar(50),##医保类型（下拉框：字典表-医保类型），
	t_address  varchar(100),##常住地址：省（下拉框），市（下拉框），县区（下拉框），
	t_dialyse_hospital varchar(50),##常析透医院，
	t_dialyse_hospital_contact varchar(50),##常析透医院联系人，
	t_dialyse_hospital_tel varchar(50),##常析透医院联系人电话（验证
	r_date varchar(20),##手术日期（自动生成当天时间，精确到分钟）--是否能修改时间？
	r_ss_address varchar(100),##手术地点（科室信息里面的医院名称）--科室信息里面的医院名称都是一样的？
	r_ss_type varchar(50),##手术类型（下拉框：字典表-手术类型）
	r_ss_method varchar(50),##手术方式（下拉框：字典表-手术方式）
	r_cc_method varchar(50),##穿刺方式（下拉框：字典表-穿刺方式）类型
	r_zd_docotor varchar(50),##主刀医生
	r_zs varchar(50),## 助手
	r_qxhs varchar(50),##器械护士
	r_ss_record varchar(2000),##手术记录（文本框，能够调整文本格式和字体）--格式和字体？格式是？字体是（宋体，黑体？）？
	
         * t_name,t_sex,t_age, t_tel, t_ID, t_health_type, t_address, t_dialyse_hospital,
           t_dialyse_hospital_contact, t_dialyse_hospital_tel, r_date, r_ss_address, r_ss_type, r_ss_method,
            r_cc_method, r_zd_docotor, r_zs, r_qxhs, r_ss_record
         **/
        // 添加手术记录
        public int addRecord(string t_name, string t_sex, string t_age, string t_tel, string t_ID, string t_health_type, string t_address, string t_dialyse_hospital,
            string t_dialyse_hospital_contact, string t_dialyse_hospital_tel, string r_date,string r_ss_address, string r_ss_type, string r_ss_method,
            string r_cc_method, string r_zd_docotor, string r_zs, string r_qxhs, string r_ss_record)
        {
            int flag = 0;
            string sql = string.Format("insert into t_record(t_name,t_sex,t_age, t_tel, t_ID, t_health_type, t_address, t_dialyse_hospital,"+
                "t_dialyse_hospital_contact, t_dialyse_hospital_tel, r_date, r_ss_address, r_ss_type, r_ss_method,r_cc_method, r_zd_docotor, r_zs, r_qxhs, r_ss_record)"+
                 "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}')", 
                 t_name,t_sex,t_age, t_tel, t_ID, t_health_type, t_address, t_dialyse_hospital,t_dialyse_hospital_contact, t_dialyse_hospital_tel,
                 r_date, r_ss_address, r_ss_type, r_ss_method, r_cc_method, r_zd_docotor, r_zs, r_qxhs, r_ss_record);

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
