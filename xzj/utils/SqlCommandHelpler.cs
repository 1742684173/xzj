using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xzj.utils
{
    class SqlCommandHelpler
    {

        // 共用的
        public static string COMMON_SELECT_LAST_INSERT_ID = "SELECT LAST_INSERT_ID(); ";

        // 手术记录页面
        // t_patient表
        public static string T_PATIENT_SELECT_TEL_BY_ID = "SELECT p_tel FROM t_patient WHERE p_id = @p_id;";
        public static string T_PATIENT_SELECT_COUNT_BY_PATIENT_ID = "SELECT COUNT(*) FROM  t_patient t WHERE t.p_ID = @p_id";
        public static string T_PATIENT_INSERT = "INSERT INTO t_patient(" + 
            "p_name, " + 
            "p_sex, " +
            "p_age, " + 
            "p_tel, " + 
            "p_dialyse_hospital_tel, " +

            "p_health_type, " + 
            "p_address, " + 
            "p_dialyse_hospital, " + 
            "p_dialyse_hospital_contact, " + 
            "p_ID " + 
            ") VALUES( " +
            "@p_name, " + 
            "@p_sex, " + 
            "@p_age, " + 
            "@p_tel, " + 
            "@p_dialyse_hospital_tel, " +

            "@p_health_type, " + 
            "@p_address, " + 
            "@p_dialyse_hospital, " + 
            "@p_dialyse_hospital_contact, " + 
            "@p_ID) ";
        public static string T_PATIENT_UPDATE_BY_PATIENT_ID = "UPDATE t_patient t " +
            "SET t.p_name = @p_name, " +
            "t.p_sex = @p_sex, " +
            "t.p_age = @p_age, " +
            "t.p_tel = @p_tel, " +
            "t.p_health_type = @p_health_type, " +
            "t.p_address = @p_address, " +
            "t.p_dialyse_hospital = @p_dialyse_hospital, " +
            "t.p_dialyse_hospital_contact = @p_dialyse_hospital_contact, " +
            "t.p_dialyse_hospital_tel = @p_dialyse_hospital_tel " +
            "WHERE t.p_ID=@p_ID;";
			

        // t_record表 s i u d
        public static string T_RECORD_SELECT_PART_BY_ID = "SELECT r_date, r_ss_address, r_ss_type, r_ss_method FROM t_record WHERE id = @id";
        public static string T_RECORD_SELECT_TRACK_TIME_BY_ID = "SELECT r_sszz, r_zz1, r_zz2, r_zz3, r_zz4 FROM t_record WHERE id = @id";
        public static string T_RECORD_INSERT = "INSERT INTO t_record( " +
            "r_patient_ID, " +
            "r_date, " +
            "r_ss_address, " +
            "r_ss_type, " +
            "r_ss_method, " +

            "r_cc_method, " +
            "r_sszzqx, " + 
            "r_sszz, " + 
            "r_zz1, " +
            "r_zz2, " +

            "r_zz3, " +
            "r_zz4, " +
            "r_ssbw, " +
            "r_zd_docotor, " +
            "r_zs, " +

            "r_qxhs, " +
            "r_ss_record, " +
            "r_is_sszz " +
            ") VALUES( " +
            "@p_ID, " +
            "@r_date, " +
            "@r_ss_address, " +
            "@r_ss_type, " +
            "@r_ss_method, " +

            "@r_cc_method, " +
            "@r_sszzqx, " + 
            "@r_sszz, " + 
            "@r_zz1, " +
            "@r_zz2, " +

            "@r_zz3, " +
            "@r_zz4, " +
            "@r_ssbw, " +
            "@r_zd_docotor, " +
            "@r_zs, " +

            "@r_qxhs, " +
            "@r_ss_record, " +
            "@r_is_sszz);";

            

        // t_picture 图片表
        public static string T_PICTURE_SELECT_BY_RID = "SELECT p_content FROM t_picture WHERE p_r_id = @p_r_id ORDER BY p_order;";
        public static string T_PICTURE_INSERT = "INSERT INTO t_picture( " +
            "p_r_id, " +
            "p_path, " +
            "p_desc, " +
            "p_content, " +
            "p_order " +
            ") VALUES( " +
            "@p_r_id, " +
            "@p_path, " +
            "@p_desc, " +
            "@p_content, " +
            "@p_order)";

        // t_track 手术跟踪
        // 查询某个手术的已经跟踪的手术追踪
        public static string T_TRACK_SELECT_BY_RECORD_ID = "SELECT id, t_sszzrq, t_sszzcjr FROM t_track WHERE t_record_id = @t_record_id AND t_sfsszz = '是'";
        public static string T_TRACK_SELECT_BY_RECORD_ID_2 = "SELECT t_record_id, t_patient_id, t_zzrq FROM t_track WHERE id = @id;";
        public static string T_TRACK_SELECT_BY_RECORD_ID_3 = "SELECT t_sfsszzcg, t_sssbyy FROM t_track WHERE id = @id;";
        public static string T_TRACK_SELECT_BY_RECORD_ID_4 = "select t_ssct, t_ywxlbct, t_ywxm, t_ywbfz, t_ywxbjmqz," +
            "t_ywccbwphgmqk, t_nwzwdlqk, t_ccbwpfqk, t_grkzfs, t_zwcmzcjtzqk " +
            "from t_track WHERE id = @id;";
        public static string T_TRACK_DELETE_BY_RECORD_ID = "delete from t_track where id = @id;";
        public static string T_TRACK_INSERT_PART = "INSERT INTO t_track( " +
            "t_record_id, " +
            "t_patient_ID, " +
            "t_zzrq, " +
            "t_sfsszz, " +
            "t_sfsszzcg " +
            ") VALUES( " +
            "@t_record_id, " +
            "@t_patient_ID, " +
            "@t_zzrq, " +
            "@t_sfsszz, " +
            "@t_sfsszzcg) ";
        public static string I_TRACK_UPDATE = "update t_track set " +
            "t_sszzrq = @t_t_sszzrq, " +
            "t_sszzcjr = @t_sszzcjr, " +
            "t_sfsszz = @t_sfsszz, " +
            "t_sfsszzcg = @t_sfsszzcg, " +
            "t_sssbyy = @t_sssbyy, " +
            "t_ssct = @t_ssct, " +
            "t_ywxlbct = @t_ywxlbct, " +
            "t_ywxm = @t_ywxm, " +
            "t_ywbfz = @t_ywbfz, " +
            "t_ywxbjmqz = @t_ywxbjmqz, " +
            "t_ywccbwphgmqk = @t_ywccbwphgmqk, " +
            "t_nwzwdlqk = @t_nwzwdlqk, " +
            "t_ccbwpfqk = @t_ccbwpfqk, " +
            "t_grkzfs = @t_grkzfs, " +
            "t_zwcmzcjtzqk = @t_zwcmzcjtzqk " +
            "where id = @t_id ";




        // 手术追踪页面
        // 本月需要手术追踪的患者的姓名和身份证
        public static string T_TRACK_PATIENT_FIND_NAME_AND_ID_BY_ZZRQ = "SELECT t.id t_id, p_name, p_id, t_record_id FROM t_track t, t_patient p " +
            "WHERE (t.t_zzrq BETWEEN @startTime AND @endTime) " + 
            "AND (t.t_sfsszz = '否') " +
            "AND (p.p_id  = t.t_patient_ID) ";
        public static string T_TRACK_PATIENT_COUNT_NAME_AND_ID_BY_ZZRQ = "SELECT COUNT(*) FROM t_track t, t_patient p " +
            "WHERE (t.t_zzrq BETWEEN @startTime AND @endTime) " +
            "AND (t.t_sfsszz = '否') " +
            "AND (p.p_id  = t.t_patient_ID) ";
        // 根据姓名模糊查询患者 + 日期 的姓名和身份证
        // public static string T_PATIENT_SELECT_NAME_AND_ID_BY_NAME = "SELECT p_name, p_id FROM t_patient WHERE p_name LIKE @p_name;";
        public static string T_TRACK_PATIENT_FIND_NAME_AND_ID_BY_ZZRQ_AND_NAME = "SELECT t.id t_id, p_name, p_id, t_record_id FROM t_track t, t_patient p " +
            "WHERE (t.t_zzrq BETWEEN @startTime AND @endTime) " +
            "AND (t.t_sfsszz = '否') " +
            "AND (p.p_id  = t.t_patient_ID) " +
            "AND (p.p_name LIKE @p_name)";
        public static string T_TRACK_PATIENT_COUNT_NAME_AND_ID_BY_ZZRQ_AND_NAME = "SELECT COUNT(*) FROM t_track t, t_patient p " +
            "WHERE (t.t_zzrq BETWEEN @startTime AND @endTime) " +
            "AND (t.t_sfsszz = '否') " +
            "AND (p.p_id  = t.t_patient_ID) " +
            "AND (p.p_name LIKE @p_name)";


        // 手术追踪查询，根据 患者姓名，是否手术追踪，追踪日期范围
        public static string T_PATIENT_TRACK_RECORD_SELECT_BY_PNAME_FFZZCG_ZZRQ = "SELECT p_name 姓名, p_sex 性别, p_age 年龄, p_health_type 医保类型, DATE_FORMAT(r_date, '%Y-%m-%d') 手术日期, r_ss_method 手术方式, " +
            "r_ss_type 手术类型,  p_dialyse_hospital 常析透医院, p_tel 联系电话, t_sfsszz 是否手术追踪, t_sfsszzcg 是否手术追踪成功 FROM " +
            "t_patient p JOIN t_track t ON t.t_patient_ID = p.p_ID " +
            "JOIN t_record r on r.id = t.t_record_id " +
            "WHERE p_name LIKE @p_name AND t_sfsszz = @t_sfsszz AND t_zzrq BETWEEN @startTime AND @endTime ";

        public static string T_PATIENT_TRACK_RECORD_SELECT_BY_PNAME_ZZRQ = "SELECT p_name 姓名, p_sex 性别, p_age 年龄, p_health_type 医保类型, DATE_FORMAT(r_date, '%Y-%m-%d') 手术日期, r_ss_method 手术方式, " +
           "r_ss_type 手术类型,  p_dialyse_hospital 常析透医院, p_tel 联系电话, t_sfsszz 是否手术追踪, t_sfsszzcg 是否手术追踪成功 FROM " +
           "t_patient p JOIN t_track t ON t.t_patient_ID = p.p_ID " +
           "JOIN t_record r on r.id = t.t_record_id " +
           "WHERE p_name LIKE @p_name AND t_zzrq BETWEEN @startTime AND @endTime ";


        // 手术协议表
        public static string T_CONTROL_SELECT_BY_NAME = "SELECT id, c_order_id, c_name, c_desc FROM t_control WHERE c_name LIKE @c_name ORDER BY c_order_id";
        public static string T_CONTROL_SELECT_CONTENT_BY_ID = "SELECT c_content FROM t_control WHERE id = @id;";
        public static string T_CONTROL_UPDATE_BY_ID = "UPDATE t_control SET c_order_id = @c_order_id, c_name = @c_name, c_desc = @c_desc WHERE id = @id";
        public static string T_CONTROL_UPDATE_CONTENT_BY_ID = "UPDATE t_control SET c_content = @c_content WHERE id = @id;";
        // public static string T_CONTROL_SELECT_CONTENT_BY_ID = "select c_content fr"
        public static string T_CONTROL_INSERT = "INSERT INTO t_control(c_order_id, c_name, c_desc) VALUES (@c_order_id, @c_name, @c_desc);";
        public static string T_CONTROL_DELETE = "DELETE FROM t_control WHERE id = @id";

    }
}
