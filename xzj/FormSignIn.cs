using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xzj
{
    public partial class FormSignIn : Form
    {
        private static string ACCOUNT = "";
        int nowX = 0;
        int nowY = 0;
        Boolean mouseDown = false;

        public FormSignIn()
        {
            InitializeComponent();

        }

        private void FormSignIn_Load(object sender, EventArgs e)
        {
            this.btnConfigDB.Parent = this.pictureBox1;
            this.btnTime.Parent = this.pictureBox1;
        }
       
        //登录
        private void btnSignIn_Click(object sender, EventArgs e)
        {
            string account = this.textBoxAcount.Text;
            string pwd = this.textBoxPwd.Text;

            string sqlAddress = DBSQLite.selectValue(UtilConfig.SQL_ADDRESS_KEY) + "";
            if (sqlAddress.Length == 0)
            {
                MessageBox.Show("你还没有配置数据库，需要先配置数据库，才能登录");
                return;
            }

            if (string.IsNullOrEmpty(account))
            {
                MessageBox.Show("账号不能为空");
                return;
            }

            if (string.IsNullOrEmpty(pwd))
            {
                MessageBox.Show("密码不能为空");
                return;
            }


            bool flag = DBEmp.getInstance().isAccount(account);

            if (flag)
            {
                flag = DBEmp.getInstance().isLogin(account, pwd);
                if (flag)
                {
                    //UtilConfig.ACCOUNT = account;
                    DBSQLite.updateValue(UtilConfig.ACCOUNT_KEY, account);
                    DBSQLite.updateValue(UtilConfig.PWD_KEY, pwd);
                    goMainForm();
                }
                else
                {
                    MessageBox.Show("用户名与密码不匹配");
                }
                ;
            }
            else
            {
                MessageBox.Show("账号不存在");
            }

            
        }

        //退出系统
        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            DBSQLite.clearByKey(UtilConfig.ACCOUNT_KEY);
            Application.Exit();
        }

        //退出系统
        private void btnClose_Click(object sender, EventArgs e)
        {
            DBSQLite.clearByKey(UtilConfig.ACCOUNT_KEY);
            Application.Exit();
        }

        //弹出数据库配置
        private void btnConfigDB_Click(object sender, EventArgs e)
        {
            FormConfigDB form = new FormConfigDB();
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
            {
                UtilConfig.SQL_ADDRESS = DBSQLite.selectValue(UtilConfig.SQL_ADDRESS_KEY);
            }
        }

        //获取实时的时间
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.btnTime.Text = UtilTools.getDayAndTime();
        }

        private void goMainForm()
        {
            FormMain mainForm = new FormMain();
            mainForm.Show();
            Hide();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            // Console.WriteLine("按下鼠标");
            nowX = e.X;
            nowY = e.Y;
            mouseDown = true;
            Console.WriteLine("x:" + this.Location.X + ", y " + this.Location.Y);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                int moveX = e.X - nowX;
                int moveY = e.Y - nowY;
                // Console.WriteLine("移动" + moveX + " " + moveY);
                this.Location = new Point(this.Location.X + moveX, this.Location.Y + moveY);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void textBoxPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btnSignIn_Click(null, null);
            }  
        }


      
    }
}
