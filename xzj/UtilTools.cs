using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace xzj
{
    class UtilTools
    {
        private static string[] weeks = new string[]{"Monday","Tuesday","Wednesday","Thursday","Friday","Saturday","Sunday"};
        private static string[] weeks_ch = new string[] { "一", "二", "三", "四", "五", "六", "日" };

        //根据身份证获取年龄
        public static int getAgeByID(string ID)
        {
            int nowY = DateTime.Now.Year;
            int birthY = Convert.ToInt32(ID.Substring(6, 4));

            return nowY - birthY;
        }

        //获取当前周几
        public static String getWeek()
        {
            //DayOfWeek dayOfWeek = new DayOfWeek();
            return DateTime.Now.DayOfWeek.ToString();
        }

        //获取当前时间
        public static String getTime()
        {
            return DateTime.Now.ToString("hh:mm");
        }

        //获取当前日期
        public static String getDate()
        {
            return DateTime.Now.ToString("yyyy:MM:dd");
        }

        /// <summary>
        /// 判断字符串是否是数字
        /// </summary>
        public static bool IsNumber(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            const string pattern = "^[0-9]*$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(s);
        }

        //获取当前时间
        public static String getDayAndTime()
        {
            return DateTime.Now.ToString("yyyy:MM:dd hh:mm:ss");
        }

        //获取当前时间
        public static String getDayAndTimeMM()
        {
            return DateTime.Now.ToString("yyyy-MM-dd hh:mm");
        }

        //获取当前日期及周几
        public static String getDateAndWeek()
        {
            string result = "当前时间：";
            int y = DateTime.Now.Year;
            int m = DateTime.Now.Month;
            int d = DateTime.Now.Day;
            

            result += y + "年"+m+"月"+d+"日";

            for (int i = 0; i < weeks.Length; i++)
            {
                if (DateTime.Now.DayOfWeek.ToString().Equals(weeks[i]))
                {
                    result += " 周" + weeks_ch[i];
                }
            }

            return result;
        }
        
       //邮箱格式判断
        public static bool isEmail(String email)
        {
            Regex r = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");
            return r.IsMatch(email);
        }

        //保存值
        public static bool saveValueByKey(string key, string value)
        {
            bool b = false;
            try
            {
                deleteValueByKey(key);
                // 创建文件
                FileStream fs = new FileStream("mydate", FileMode.OpenOrCreate, FileAccess.ReadWrite); //可以指定盘符，也可以指定任意文件名，还可以为word等文件
                StreamWriter sw = new StreamWriter(fs); // 创建写入流
                string s = key+"|"+value;
                sw.WriteLine(s); // 写入Hello World
                sw.Close(); //关闭文件
                b = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("------>" + e.Message);
            }
            finally
            {
                
            }
            return b;
        }

        //获取值
        public static string getValueByKey(string key)
        {
            string result = "";
            try
            {
                // 创建文件
                FileStream fs = new FileStream("mydate", FileMode.OpenOrCreate, FileAccess.ReadWrite); //可以指定盘符，也可以指定任意文件名，还可以为word等文件
                //StreamWriter sw = new StreamWriter(fs); // 创建写入流
                StreamReader sr = new StreamReader(fs);
                List<string> arrayList = new List<string>();
                while (sr.ReadLine() != null)
                {
                    arrayList.Add(sr.ReadLine());
                }
                sr.Close(); //关闭文件
                foreach (string str in arrayList)
                {
                    string[] arrays = str.Split('|');
                    if (arrays[0].Equals(key))
                    {
                        result = arrays[1];
                        break;
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("------>" + e.Message);
            }
            finally
            {

            }
            return result;
        }

        //删除值
        public static bool deleteValueByKey(string key)
        {
            bool result = false;
            try
            {
                // 创建文件
                FileStream fs = new FileStream("./mydate", FileMode.OpenOrCreate, FileAccess.ReadWrite); //可以指定盘符，也可以指定任意文件名，还可以为word等文件
                StreamWriter sw = new StreamWriter(fs); // 创建写入流
                StreamReader sr = new StreamReader(fs);
                List<string> arrayList = new List<string>();
                while (sr.ReadLine() != null)
                {
                    string str = sr.ReadLine();
                    string[] arrays = str.Split('|');
                    if (!arrays[0].Equals(key))
                    {
                        arrayList.Add(sr.ReadLine());
                    }
                }
                sr.Close(); //关闭文件
                foreach (string str in arrayList)
                {
                    sw.WriteLine(fs); 
                }
                sw.Close(); //关闭文件
                result = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("------>" + e.Message);
            }
            finally
            {

            }
            return result;
        }
    }
}
