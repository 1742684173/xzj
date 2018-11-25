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
    public partial class FormAddSSZZ : Form
    {
        public FormAddSSZZ()
        {
            InitializeComponent();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ValueSender.isAdd = true;
            ValueSender.creator = this.labSSZZ_Creator.Text;
            ValueSender.createDate = dtpSSZZ_createDate.Value;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormAddSSZZ_Load(object sender, EventArgs e)
        {
            //获取用户名
            string account = DBSQLite.selectValue(UtilConfig.ACCOUNT_KEY);
            DataTable dataTable = DBManager.getInstance().find(string.Format("select e_name from t_emp where e_account='{0}'", account));
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    this.labSSZZ_Creator.Text = row["e_name"].ToString();
                }
            }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
