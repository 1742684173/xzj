using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xzj.utils;

namespace xzj.xzjForm
{
    public partial class FormModifyControl : Form
    {
        private int id;
        public FormModifyControl(int id, string orderId, string name, string desc)
        {
            InitializeComponent();


            // MySqlParameter[] ps = new MySqlParameter[]{
                // new MySqlParameter(id)
            // };

            // SqlHelper4MySql.ExecuteReader()
            this.id = id;
            this.tbRank.Text = orderId;
            this.tbName.Text = name;
            this.tbDesc.Text = desc;


        }

        private void FormModifyControl_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string orderId = this.tbRank.Text;
            string name = this.tbName.Text;
            string desc = this.tbDesc.Text;

            // c_order_id = @c_order_id, c_name = @c_name, c_desc = @c_desc WHERE id = @id";
            if (string.IsNullOrEmpty(orderId))
            {
                MessageBox.Show("请输入排列顺序!");
                return;
            }
            else
            {
                try
                {
                    Convert.ToInt32(orderId);
                }
                catch (Exception)
                {
                    MessageBox.Show("排列顺序请输入数字!");
                    return;
                }
            }
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("请输入协议名称!");
                return;
            }

            MySqlParameter[] ps = new MySqlParameter[]{
                new MySqlParameter("@id", id),
                new MySqlParameter("@c_name", name),
                new MySqlParameter("@c_desc", desc),
                new MySqlParameter("@c_order_id", orderId)
            };
            SqlHelper4MySql.ExecuteNonQuery(SqlCommandHelpler.T_CONTROL_UPDATE_BY_ID, ps);
            DialogResult dr = MessageBox.Show("修改成功，是否退出？", "信息提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
