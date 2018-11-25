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

namespace xzj
{
    public partial class FormAddControl : Form
    {
        public FormAddControl()
        {
            InitializeComponent();

        }

        //自动编号
        private void setRank()
        {
            string sql = string.Format("select c_order_id from t_control order by c_order_id desc limit 1");
            DataTable dt = DBManager.getInstance().find(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow rows in dt.Rows)
                {
                    this.tbRank.Text = Convert.ToInt32(rows["c_order_id"].ToString()) + 1 + "";
                }

            }
            else
            {
                this.tbRank.Text = "1";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string orderId = this.tbRank.Text;
            string name = this.tbName.Text;
            string desc = this.tbDesc.Text;

            if (string.IsNullOrEmpty(orderId))
            {
                MessageBox.Show("请输入排列顺序!");
                return;
            }
            else {
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
            if (string.IsNullOrEmpty(name)) {
                MessageBox.Show("请输入协议名称!");
                return;
            }

            MySqlParameter[] ps = new MySqlParameter[] {
                new MySqlParameter("@c_order_id",orderId),
                new MySqlParameter("@c_name", name),
                new MySqlParameter("@c_desc", desc)
            };
            SqlHelper4MySql.ExecuteNonQuery(SqlCommandHelpler.T_CONTROL_INSERT, ps);
            DialogResult dr = MessageBox.Show("保存成功，是否退出？", "信息提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

            this.tbRank.Text = "";
            this.tbName.Text = "";
            this.tbDesc.Text = "";
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void picClose_Click(object sender, EventArgs e)
        {
            //this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormAddControl_Load(object sender, EventArgs e)
        {
            setRank();
        }
    }
}
