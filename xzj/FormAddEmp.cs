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
    public partial class FormAddEmp : Form
    {
        public static string ACCOUNT = "";

        public FormAddEmp()
        {
            InitializeComponent();
            this.ImeMode = System.Windows.Forms.ImeMode.OnHalf;
        }

        private void FormAddEmp_Load(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(ACCOUNT))
            {
                this.labelTitle.Text = "编辑用户";
                this.tbAccount.Enabled = false;
                DataTable dt = DBEmp.getInstance().getEmps(ACCOUNT, null);

                foreach (DataRow row in dt.Rows)
                {
                    this.tbAccount.Text = row["e_account"].ToString();
                    this.tbName.Text = row["e_name"].ToString();
                    this.cbSex.Text = row["e_sex"].ToString();
                    this.dtpBirth.Text = row["e_birth"].ToString();
                    this.tbTel.Text = row["e_tel"].ToString();
                    this.tbEmail.Text = row["e_email"].ToString();
                    this.tbAddress.Text = row["e_address"].ToString();
                    this.tbPwd.Text = row["e_pwd"].ToString();
                    this.tbSurePwd.Text = row["e_pwd"].ToString();
                }
            }
            else
            {
                this.cbSex.SelectedIndex = 0;
            }
        }

        //添加用户
        private void btnSave_Click(object sender, EventArgs e)
        {
            //account,name,sex,birth,tel,email,address,pwd
            string account = this.tbAccount.Text;
            string name = this.tbName.Text;
            string sex = this.cbSex.Text;
            string birth = this.dtpBirth.Text;
            string tel = this.tbTel.Text;
            string email = this.tbEmail.Text;
            string address = this.tbAddress.Text;
            string pwd = this.tbPwd.Text;
            string surePwd = this.tbSurePwd.Text;

            if (string.IsNullOrEmpty(account))
            {
                MessageBox.Show("用户名不能为空");
                return;
            }

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("名字不能为空");
                return;
            }

            //if (string.IsNullOrEmpty(sex))
            //{
            //    MessageBox.Show("性别不能为空");
            //    return;
            //}

            //if (string.IsNullOrEmpty(tel))
            //{
            //    MessageBox.Show("电话不能为空");
            //    return;
            //}

            //if (tel.Length != 11)
            //{
            //    MessageBox.Show("电话不正确");
            //    return;
            //}

            //if (string.IsNullOrEmpty(email))
            //{
            //    MessageBox.Show("邮箱不能为空");
            //    return;
            //}

            //if (!UtilTools.isEmail(email))
            //{
            //    MessageBox.Show("邮箱格式不正确");
            //    return;
            //}

            //if (string.IsNullOrEmpty(address))
            //{
            //    MessageBox.Show("地址不能为空");
            //    return;
            //}

            if (string.IsNullOrEmpty(pwd))
            {
                MessageBox.Show("请输入密码");
                return;
            }

            if (pwd.Length < 1)
            {
                MessageBox.Show("密码至少大于1位");
                return;
            }

            if (string.IsNullOrEmpty(pwd))
            {
                MessageBox.Show("请确认密码");
                return;
            }

            if (pwd != surePwd)
            {
                MessageBox.Show("两次密码不一样");
                return;
            }
            
            //编辑
            if (!string.IsNullOrEmpty(ACCOUNT))
            {
                DialogResult dr = MessageBox.Show("确定要编辑?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    int flag = DBEmp.getInstance().editEmp(account, name, sex, birth, tel, email, address, pwd);
                    if (flag > 0)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("编辑失败");
                    }
                }
                else
                {
                    this.Close();//关闭容器
                }
                
            }
                //添加
            else
            {
                bool b = DBEmp.getInstance().isAccount(account);
                if (!b)
                {
                    int flag = DBEmp.getInstance().addEmp(account, name, sex, birth, tel, email, address, pwd);
                    if (flag > 0)
                    {
                        DialogResult dr = MessageBox.Show("添加成功,是否继续添加?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            //清理
                            this.tbAccount.Text = "";
                            this.tbName.Text = "";
                            //this.cbSex.Text;
                            this.dtpBirth.Text = "";
                            this.tbTel.Text = "";
                            this.tbEmail.Text = "";
                            this.tbAddress.Text = "";
                            this.tbPwd.Text = "";
                            this.tbSurePwd.Text = "";
                        }
                        else
                        {
                            this.DialogResult = DialogResult.OK;
                            this.Close();//关闭容器
                        }
                    }
                    else
                    {
                        MessageBox.Show("添加失败");
                    }
                }
                else
                {
                    MessageBox.Show("该帐号已添加");
                }
            }
            
            

        }

        //只输入数字
        private void tbTel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))//如果不是输入数字就不让输入
            {
                e.Handled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

       

    }
}
