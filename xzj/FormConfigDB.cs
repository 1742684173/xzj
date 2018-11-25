using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace xzj
{
    public partial class FormConfigDB : Form
    {
       
        public FormConfigDB()
        {
            InitializeComponent();
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            //this.Close();
            this.Close();
        }

        private void ConfigDBForm_Load(object sender, EventArgs e)
        {
            string dbName = DBSQLite.selectValue(UtilConfig.DB_NAME_KEY);
            string ipAdd = DBSQLite.selectValue(UtilConfig.DB_IP_KEY);
            string user = DBSQLite.selectValue(UtilConfig.DB_USER_KEY);
            string pwd = DBSQLite.selectValue(UtilConfig.DB_PWD_KEY);

            if (dbName != null)
            {
                this.tbDbName.Text = dbName;
            }

            if (dbName != null)
            {
                this.tbIP.Text = ipAdd;
            }

            if (dbName != null)
            {
                this.tbUserName.Text = user;
            }

            if (dbName != null)
            {
                this.tbPwd.Text = pwd;
            }

        }

        //保存配置
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.tbDbName.Text != null)
            {
                if (DBSQLite.selectValue(UtilConfig.DB_NAME_KEY) != null)
                {
                    DBSQLite.updateValue(UtilConfig.DB_NAME_KEY, this.tbDbName.Text);
                }
                else
                {
                    DBSQLite.insertValue(UtilConfig.DB_NAME_KEY, this.tbDbName.Text);
                }
               
            }

            if (this.tbIP.Text != null)
            {
                if (DBSQLite.selectValue(UtilConfig.DB_IP_KEY) != null)
                {
                    DBSQLite.updateValue(UtilConfig.DB_IP_KEY, this.tbIP.Text);
                }
                else
                {
                    DBSQLite.insertValue(UtilConfig.DB_IP_KEY, this.tbIP.Text);
                }
            }

            if (this.tbUserName.Text != null)
            {
                //UtilTools.setData(UtilConfig.DB_USER_KEY, this.tbUserName.Text);
                if (DBSQLite.selectValue(UtilConfig.DB_USER_KEY) != null)
                {
                    DBSQLite.updateValue(UtilConfig.DB_USER_KEY, this.tbUserName.Text);
                }
                else
                {
                    DBSQLite.insertValue(UtilConfig.DB_USER_KEY, this.tbUserName.Text);
                }
            }

            if (this.tbPwd.Text != null)
            {
               // UtilTools.setData(UtilConfig.DB_PWD_KEY, this.tbPwd.Text);
                if (DBSQLite.selectValue(UtilConfig.DB_PWD_KEY) != null)
                {
                    DBSQLite.updateValue(UtilConfig.DB_PWD_KEY, this.tbPwd.Text);
                }
                else
                {
                    DBSQLite.insertValue(UtilConfig.DB_PWD_KEY, this.tbPwd.Text);
                }
            }

            string dbName = DBSQLite.selectValue(UtilConfig.DB_NAME_KEY);
            string ipAdd = DBSQLite.selectValue(UtilConfig.DB_IP_KEY);
            string user = DBSQLite.selectValue(UtilConfig.DB_USER_KEY);
            string pwd = DBSQLite.selectValue(UtilConfig.DB_PWD_KEY);

            if (dbName == null || ipAdd == null || user == null || pwd == null)
            {
                MessageBox.Show("保存失败");
            }
            else
            {
                string sqlAddress = "server=" + ipAdd + ";port=3306;database=" + dbName + ";user=" + user + ";password=" + pwd + ";";
                //UtilTools.setData(UtilConfig.SQL_ADDRESS_KEY, sqlAddress);
                if (DBSQLite.selectValue(UtilConfig.SQL_ADDRESS_KEY) != null)
                {
                    DBSQLite.updateValue(UtilConfig.SQL_ADDRESS_KEY, sqlAddress);
                }
                else
                {
                    DBSQLite.insertValue(UtilConfig.SQL_ADDRESS_KEY, sqlAddress);
                }


                DialogResult dr = MessageBox.Show("保存成功，是否退出？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }

        }

        //测试连接
        private void btnTest_Click(object sender, EventArgs e)
        {
            string dbName = this.tbDbName.Text;
            string ipAdd = this.tbIP.Text;
            string user = this.tbUserName.Text;
            string pwd = this.tbPwd.Text;
            string sqlAddress = "server=" + ipAdd + ";port=3306;database=" + dbName + ";user=" + user + ";password=" + pwd + ";";
            try
            {
                 MySqlConnection conn = new MySqlConnection(sqlAddress);
                 conn.Open();
                 MessageBox.Show("连接成功");
            }
            catch (Exception err)
            {
                MessageBox.Show("连接失败，失败信息：" + err.Message); ;
            }
           
        }
       
    }
}
