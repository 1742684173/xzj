using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;

namespace xzj
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Process[] tProcess = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (tProcess.Length > 1)
            {
                Application.Exit();
            }
            else
            {
                try
                {
                    DBSQLite.CreateDB();
                    DBSQLite.CreateTable();
                    UtilConfig.SQL_ADDRESS = DBSQLite.selectValue(UtilConfig.SQL_ADDRESS_KEY);
                    string account = DBSQLite.selectValue(UtilConfig.ACCOUNT_KEY);
                    string pwd = DBSQLite.selectValue(UtilConfig.PWD_KEY);

                    // 原先有保存连接，判断原先的数据库连接是否有用
                    Boolean isConnectionWorks = DBManager.isConnectionWorks();
                    if (isConnectionWorks)
                    {
                        bool flag = DBEmp.getInstance().isLogin(account, pwd);
                        if (flag)
                        {
                            Application.Run(new FormMain());
                        }
                        else
                        {
                            Application.Run(new FormSignIn());
                        }
                    }
                    else
                    {
                        Application.Run(new FormSignIn());
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }

                
            }
        }

      
    }
}
