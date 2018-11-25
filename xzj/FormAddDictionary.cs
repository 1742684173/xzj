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
    public partial class FormAddDictionary : Form
    {
        private int dictionary_id;//1代表医保类型 2代表手术字典 3代表情况字典
        private int dictionary_parent_id;//-1就增加 大于0就编辑
        private string title;
        private string typeName;
        private static string name_flag = "";//标记名称的，如果修改过后和修改之前一样，就不用判断与其它名称是否一样
        
        public void setDesc(int id,int pid,string title,string typeName)
        {
            this.dictionary_id = id;
            this.dictionary_parent_id = pid;
            this.title = title;
            this.typeName = typeName;
        }

        public FormAddDictionary()
        {
            InitializeComponent();
            //设置输入法
            this.ImeMode = System.Windows.Forms.ImeMode.OnHalf;
        }

        private void FormAddDictionary_Load(object sender, EventArgs e)
        {

            // MessageBox.Show("hi" + this.dictionary_parent_id);
            //this.Text = this.title;
            this.labelTitle.Text = this.title+this.typeName;
            //修改
            if (this.dictionary_id >= 0)
            {
               
                DataTable dt = DBDictionary.getInstance().getDictionarysById(this.dictionary_id);

                foreach (DataRow row in dt.Rows)
                {
                    this.tbRank.Text = row["d_order"].ToString();
                    this.tbName.Text = row["d_name"].ToString();
                    this.tbDesc.Text = row["d_desc"].ToString();
                    name_flag = row["d_name"].ToString();
                    //this.dictionary_parent_id = 
                }
            }
            else
            {
                setRank();
                
            }
        }

        //自动编号
        private void setRank()
        {
            string sql = string.Format("select d_order from t_dictionary where d_parent_id = {0} order by d_order desc limit 1", this.dictionary_parent_id);
            DataTable dt = DBManager.getInstance().find(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow rows in dt.Rows)
                {
                    this.tbRank.Text = Convert.ToInt32(rows["d_order"].ToString()) + 1 + "";
                }

            }
            else
            {
                this.tbRank.Text = "1";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string rank = this.tbRank.Text;
            string name = this.tbName.Text;
            string desc = this.tbDesc.Text;

            if (string.IsNullOrEmpty(rank))
            {
                MessageBox.Show("序号不能为空");
                return;
            }

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("名称不能为空");
                return;
            }
            // 此处任务 parent_id=8的就是 手术追踪期限字典
            else if (8 == this.dictionary_parent_id)
            {
                try
                {
                    Convert.ToInt32(name);
                }
                catch (Exception)
                {
                    MessageBox.Show("请输入数字，1代表每一月，2代表每两月!");
                    return;
                }
            }
            /*
            if (string.IsNullOrEmpty(desc))
            {
                MessageBox.Show("描述不能为空");
                return;
            }*/

            //编辑
            if (this.dictionary_id >= 0)
            {
                DialogResult dr = MessageBox.Show("确定要编辑?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    if (!name_flag.Equals(name))
                    {
                        DataTable dt = DBDictionary.getInstance().getDictionarysByParentIdAndName(this.dictionary_parent_id, name);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            MessageBox.Show("该名称已有，请重新编辑");
                            return;
                        }
                    }

                    int flag = DBDictionary.getInstance().editDictionary(this.dictionary_id, rank, this.dictionary_parent_id, name, desc);
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
                DataTable dt = DBDictionary.getInstance().getDictionarysByParentIdAndName(this.dictionary_parent_id, name);
                if (dt == null || dt.Rows.Count == 0)
                {

                    int flag = DBDictionary.getInstance().addDictionary(rank, this.dictionary_parent_id, name, desc);
                    if (flag > 0)
                    {
                        DialogResult dr = MessageBox.Show("添加成功,是否继续添加?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            //清理
                            setRank();
                            this.tbName.Text = "";
                            //this.cbSex.Text;
                            this.tbDesc.Text = "";
                           
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
                    MessageBox.Show("该名称已添加");
                }
            }
        }

        private void tbRank_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))//如果不是输入数字就不让输入
            {
                e.Handled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();//关闭容器
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
