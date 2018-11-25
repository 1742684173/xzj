using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Printing;
using System.IO;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using xzj.utils;
using xzj.xzjForm;
using System.Drawing.Drawing2D;


namespace xzj
{
    public partial class FormMain : Form
    {
        private DataTable dataTable;
        private static int dictionary_parent_id = 1;
        static int listViewDictionaryFlag = 0;//1代表在列表上，0代表不在列表上
        int nowX = 0;
        int nowY = 0;
        Boolean mouseDown = false;
       
        public FormMain()
        {
            InitializeComponent();
            this.ImeMode = System.Windows.Forms.ImeMode.OnHalf;

            //this.WindowState = FormWindowState.Maximized;
            this.flowLayoutPanel4.BackColor = ColorTranslator.FromHtml("#0000cd");

            this.btnSSLR.BackColor = ColorTranslator.FromHtml("#3399ff");
            this.btnZDGL.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSJCX.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnCJFX.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnKSGL.BackColor = ColorTranslator.FromHtml("#0000cd");

            this.panelSSLR.Visible = true;
            this.panelZDGL.Visible = false;
            this.panelSJCX.Visible = false;
            this.panelCJFX.Visible = false;
            this.panelKSGL.Visible = false;

            // 最大化大小
            this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            //显示当前用户
            // this.labelAccountShow.BackColor = ColorTranslator.FromHtml("#0078d7");
            // 背景透明
            this.flowLayoutPanel5.Parent = this.pictureBox1;
            this.labelAccountShow.Parent = this.flowLayoutPanel5;
            this.label54.Parent = this.flowLayoutPanel5;
            //
            this.flowLayoutForm.Parent = this.pictureBox1;

            //获取用户名
            string account = DBSQLite.selectValue(UtilConfig.ACCOUNT_KEY);
            dataTable = DBManager.getInstance().find(string.Format("select e_name from t_emp where e_account='{0}'", account));
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    this.labelAccountShow.Text = "用户名【" + row["e_name"].ToString() + "】";
                    this.labelAccountShow.ForeColor = ColorTranslator.FromHtml("#fff");
                }
            }

            // 既然能够进到主界面，就说明登录成功了
            // 那么就可以设置SqlHelper4MySql要用的sql字符串
            SqlHelper4MySql.setConStr(UtilConfig.SQL_ADDRESS);
            Console.WriteLine("连接是：" + SqlHelper4MySql.getConStr());


            // 隐藏要展示的追踪时间
            hideAllTraceTime();

            //初始化手术录入
            initSSLR();
        }

        //关闭窗口
        private void picClose_Click(object sender, EventArgs e)
        {
            //this.DialogResult = DialogResult.OK;
            // this.Hide();
            // this.WindowState = FormWindowState.Minimized;
            Application.Exit();
        }

        //最大化 正常化
        private void picMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;

            }
            else
            {
                
                this.WindowState = FormWindowState.Maximized;
                
            }
            

            this.flowLayoutForm.Location = new Point(this.Width - 102, 0);
        }

        //最小
        private void picMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //显示手术录入
        private void btnSSLR_Click(object sender, EventArgs e)
        {
            this.btnSSLR.BackColor = ColorTranslator.FromHtml("#3399ff");
            this.btnSSZZ.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnZDGL.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSJCX.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnCJFX.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSSXGXY.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnKSGL.BackColor = ColorTranslator.FromHtml("#0000cd");

            this.panelSSLR.Visible = true;
            this.panelSSZZ.Visible = false;
            this.panelZDGL.Visible = false;
            this.panelSJCX.Visible = false;
            this.panelCJFX.Visible = false;
            this.panelSSXGXY.Visible = false;
            this.panelKSGL.Visible = false;


            initSSLR();
        }

        //字典combobox
        private void combobox(ComboBox cb,DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                ArrayList al = new ArrayList();
                foreach (DataRow row in dt.Rows)
                {
                    string text = row["d_name"].ToString();
                    int value = Convert.ToInt32(row["id"].ToString());
                    al.Add(new UtilTextAndValue(text, value));
                    //al.Add(text);
                }
                cb.DataSource = al;
                cb.DisplayMember = "DisplayText";
                cb.ValueMember = "RealValue";
                cb.SelectedIndex = -1;
                dt = null;
            }
        }


        private const string EVERY_THREE_MONTH = "每三个月";
        private const string EVERY_SIX_MONTH = "每六个月";
        //初始化手术录入
        private void initSSLR()
        {
            //this.labelSSLR_SSJL_ID.Text = "1111";//手术记录与手术跟踪的id
            this.tbSSLR_NAME.Text = "";//患者姓名
            //this.cbSSLR_SEX.Text = "";//性别
            this.tbSSLR_AGE.Text = "";//年龄
            this.tbSSLR_TEL.Text = "";//手机号
            this.tbSSLR_ID.Text = "";//身份证号
            this.tbSSLR_DETAIL_ADDRESS.Text = "";//详细地址
            this.tbSSLR_CTXYY.Text = "";//常透析医院
            this.tbSSLR_CTXYYLXR.Text = "";//常透析医院联系人
            this.tbSSLR_CTXYYLXRDH.Text = "";//常透析医院联系人电话
            this.rtbSSLR_SSJL.Text = "";//手术记录
            this.tbSSLR_ZDYS.Text = "";//主刀医生
            this.tbSSLR_ZS.Text = "";//助手
            this.tbSSLR_QXFS.Text = "";//器械护士

            this.cbSSLR_SEX.SelectedIndex = -1;
            this.cbSSLR_YBLX.SelectedIndex = -1;
           
            this.cbSSLR_PROVINCE.SelectedIndex = -1;
            this.cbSSLR_CITY.SelectedIndex = -1;
            this.cbSSLR_COUNTY.SelectedIndex = -1;

            this.cbSSLR_SSDD.SelectedIndex = -1;
            this.cbSSLR_SSLX.SelectedIndex = -1;
            this.cbSSLR_SSFS.SelectedIndex = -1;
            this.cbSSLR_CCFS.SelectedIndex = -1;

            this.cbSSLR_SSZZQX.SelectedIndex = -1;

            this.cbSSLR_SSBW.SelectedIndex = -1;
            hideAllTraceTime();
            //性别
            //this.cbSSLR_SEX.SelectedIndex = 0;

            //查询省
            DataTable dt = DBManager.getInstance().find("SELECT id,p_name as d_name FROM t_province");
            combobox(this.cbSSLR_PROVINCE, dt);//

            //查询市
            dt = DBManager.getInstance().find(string.Format("SELECT  id,c_name as d_name FROM t_city where c_province_id = {0}", this.cbSSLR_PROVINCE.SelectedValue));
            combobox(this.cbSSLR_CITY, dt);//

            //查询县
            dt = DBManager.getInstance().find(string.Format("SELECT  id,a_name as d_name FROM t_area where a_city_id = {0}", this.cbSSLR_CITY.SelectedValue));
            combobox(this.cbSSLR_COUNTY, dt);//

            //查询医保类型字典
            dt = DBDictionary.getInstance().getDictionarysByParentId(1);
            combobox(this.cbSSLR_YBLX, dt);//

            //查询手术地点字典
            dt = DBDictionary.getInstance().getDictionarysByParentId(5);
            combobox(this.cbSSLR_SSDD, dt);

            //查询手术类型字典
            dt = DBDictionary.getInstance().getDictionarysByParentId(4);
            combobox(this.cbSSLR_SSLX, dt);

            //查询手术方式字典
            dt = DBDictionary.getInstance().getDictionarysByParentId(6);
            combobox(this.cbSSLR_SSFS, dt);

            //查询穿刺方式字典
            dt = DBDictionary.getInstance().getDictionarysByParentId(7);
            combobox(this.cbSSLR_CCFS, dt);

            //查询手术追踪期限字典
            // dt = DBDictionary.getInstance().getDictionarysByParentId(8);
            // combobox(this.cbSSLR_SSZZQX, dt);
            this.cbSSLR_SSZZQX.Items.Clear();
            this.cbSSLR_SSZZQX.Items.Add(EVERY_THREE_MONTH);
            this.cbSSLR_SSZZQX.Items.Add(EVERY_SIX_MONTH);


            //查询手术部位字典
            dt = DBDictionary.getInstance().getDictionarysByParentId(9);
            combobox(this.cbSSLR_SSBW, dt);

            //手术日期
            this.dpSSLR_SSRQ.Value = Convert.ToDateTime(UtilTools.getDayAndTimeMM());

            /**
            //查询手术追踪-感染控制方式字典
            dt = DBDictionary.getInstance().getDictionarysByParentId(11);
            combobox(this.cbSSLR_SSZZ_GRKZFS, dt);

            //查询手术追踪-内痿自我锻炼情况字典
            dt = DBDictionary.getInstance().getDictionarysByParentId(9);
            combobox(this.cbSSLR_SSZZ_NWZWDLQK, dt);

            //查询手术追踪-穿刺部位皮肤情况字典
            dt = DBDictionary.getInstance().getDictionarysByParentId(10);
            combobox(this.cbSSLR_SSZZ_CCBWPFQK, dt);

           
            //是否畅通
            this.cbSSLR_SSZZ_SFTC.SelectedIndex = 0;
            //有无血流不畅通
            this.cbSSLR_SSZZ_YWXLBCT.SelectedIndex = 0;
            //有胸闷
            this.cbSSLR_SSZZ_YWXM.SelectedIndex = 0;
            //有无过敏情况
            this.cbSSLR_SSZZ_YWCCBWPFGMQK.SelectedIndex = 0;
            //有无并发症
            this.cbSSLR_SSZZ_YWBFZ.SelectedIndex = 0;
            //有无胸壁静脉曲张
            this.cbSSLR_SSZZ_YWXBJMQZ.SelectedIndex = 0;
            //是否复诊
            //this.cbSSLR_SSZZ_SSFZ.SelectedIndex = 0;
           **/
        }

        //字典管理
        private void btnZDGL_Click(object sender, EventArgs e)
        {
            this.btnSSLR.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSSZZ.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnZDGL.BackColor = ColorTranslator.FromHtml("#3399ff");
            this.btnSJCX.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnCJFX.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSSXGXY.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnKSGL.BackColor = ColorTranslator.FromHtml("#0000cd");

            this.panelSSLR.Visible = false;
            this.panelSSZZ.Visible = false;
            this.panelZDGL.Visible = true;
            this.panelSJCX.Visible = false;
            this.panelCJFX.Visible = false;
            this.panelSSXGXY.Visible = false;
            this.panelKSGL.Visible = false;

            this.setListViewDictionary("医保类型字典", 1);
        }

        //数据查询
        private void btnSJCX_Click(object sender, EventArgs e)
        {
            this.btnSSLR.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSSZZ.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnZDGL.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSJCX.BackColor = ColorTranslator.FromHtml("#3399ff");
            this.btnCJFX.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSSXGXY.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnKSGL.BackColor = ColorTranslator.FromHtml("#0000cd");

            this.panelSSLR.Visible = false;
            this.panelSSZZ.Visible = false;
            this.panelZDGL.Visible = false;
            this.panelSJCX.Visible = true;
            this.panelCJFX.Visible = false;
            this.panelSSXGXY.Visible = false;
            this.panelKSGL.Visible = false;

            initSJCX();
        }

        //统计分析
        private void btnCJFX_Click(object sender, EventArgs e)
        {
            this.btnSSLR.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSSZZ.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnZDGL.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSJCX.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnCJFX.BackColor = ColorTranslator.FromHtml("#3399ff");
            this.btnSSXGXY.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnKSGL.BackColor = ColorTranslator.FromHtml("#0000cd");
            //this.panelCJFX.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.panelSSLR.Visible = false;
            this.panelSSZZ.Visible = false;
            this.panelZDGL.Visible = false;
            this.panelSJCX.Visible = false;
            this.panelCJFX.Visible = true;
            this.panelSSXGXY.Visible = false;
            this.panelKSGL.Visible = false;

            this.dtpTJFX_GZLGL_KSSJ.Value = Convert.ToDateTime(UtilTools.getDayAndTimeMM());
            initTjfx();
        }




        // --------------科室管理

        // 科室管理
        private void btnKSGL_Click(object sender, EventArgs e)
        {
            this.btnSSLR.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSSZZ.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnZDGL.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSJCX.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnCJFX.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSSXGXY.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnKSGL.BackColor = ColorTranslator.FromHtml("#3399ff");

            this.panelSSLR.Visible = false;
            this.panelSSZZ.Visible = false;
            this.panelZDGL.Visible = false;
            this.panelSJCX.Visible = false;
            this.panelCJFX.Visible = false;
            this.panelSSXGXY.Visible = false;
            this.panelKSGL.Visible = true;
            

            //查询科室信息
            this.btnRoomInfoManager.PerformClick();
            // setKSGL_room();
        }

        //查询科室信息
        private void setKSGL_room()
        {
            dataTable = DBManager.getInstance().find("select id,r_hospital_name,r_address,r_province, r_rank, r_postcodes,  r_room_name, r_room_tel," +
                "r_room_fax,r_responsible,r_responsible_title, r_responsible_tel, r_responsible_phone,  r_responsible_email," +
                " r_dialyse_center_area, r_dialyse_unit_area, r_start_date from t_room where id = 0");
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                this.tbRoomHispitorName.Text = row["r_hospital_name"].ToString();//医院名称
                this.tbRoomHispitorAddress.Text = row["r_address"].ToString();//医院地址
                this.cbRoomProvince.Text = row["r_province"].ToString();//省
                this.cbRoomRank.Text = row["r_rank"].ToString();//等级
                this.tbRoomPostcode.Text = row["r_postcodes"].ToString();//邮政编码
                this.tbRoomName.Text = row["r_room_name"].ToString();//科室名称
                this.tbRoomTel.Text = row["r_room_tel"].ToString();//科室电话
                this.tbRoomFax.Text = row["r_room_fax"].ToString();//科室传真
                this.tbRoomFZR.Text = row["r_responsible"].ToString();//科室负责人
                this.tbRoomFZRZC.Text = row["r_responsible_title"].ToString();//科室负责人职称
                this.tbRoomFZRDH.Text = row["r_responsible_tel"].ToString();//科室负责人电话
                this.tbRoomFZRSJ.Text = row["r_responsible_phone"].ToString();//科室负责人手机
                this.tbRoomFZRYX.Text = row["r_responsible_email"].ToString();//科室负责人邮箱
                this.tbRoomTXZXMJ.Text = row["r_dialyse_center_area"].ToString();//透析中心面积
                this.tbRoomTXDYMJ.Text = row["r_dialyse_unit_area"].ToString();//透析单元面积
                this.dtpRoomKSRQ.Text = row["r_start_date"].ToString();//开始日期
            }
            else
            {
                MessageBox.Show("当前科室信息为空，请添加科室信息");
            }

        }




        //切换科室信息管理按钮
        private void btnRoomInfoManager_Click(object sender, EventArgs e)
        {
            this.btnRoomInfoManager.ForeColor = ColorTranslator.FromHtml("#3399ff");
            this.btnRoomInfoManager.BackColor = Color.White;

            this.btnRoomEmpManager.ForeColor = Color.White;
            this.btnRoomEmpManager.BackColor = ColorTranslator.FromHtml("#3399ff");

            this.panelRoomInfoManager.Visible = true;
            this.panelRoomEmpManager.Visible = false;

            //查询科室信息
            setKSGL_room();
        }

        //切换科室职员管理按钮
        private void btnRoomEmpManager_Click(object sender, EventArgs e)
        {
            this.tbFindEmpName.Text = "";
            this.tbFindEmpAccount.Text = "";

            this.btnRoomEmpManager.ForeColor = ColorTranslator.FromHtml("#3399ff");
            this.btnRoomEmpManager.BackColor = Color.White;

            this.btnRoomInfoManager.ForeColor = Color.White;
            this.btnRoomInfoManager.BackColor = ColorTranslator.FromHtml("#3399ff");

            this.panelRoomInfoManager.Visible = false;
            this.panelRoomEmpManager.Visible = true;

            this.setListViewEmp(null, null);//重新绑定

        }

        //填充人员信息表
        private void setListViewEmp(String account, string name)
        {

            string sql = string.Format("select id,e_account as '账号',e_name as '姓名',e_sex as '性别',e_birth as '生日',e_tel as '电话',e_email as '邮箱',e_address as '地址' " +
                "from t_emp where e_account like '%{0}%' and e_name like '%{1}%'", account, name);
            DataTable dt = DBManager.getInstance().find(sql);

            this.dgvKSGL_EMP.DataSource = null;//清空
            this.dgvKSGL_EMP.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//选中整行

            this.dgvKSGL_EMP.DataSource = dt;//设置数据
            this.dgvKSGL_EMP.Columns[0].Visible = false;//隐藏第一列

            // Header以外所有的单元格的背景色
            this.dgvKSGL_EMP.RowsDefaultCellStyle.BackColor = Color.White;

            ////列Header的背景色 CellBorderStyle->Single   EnableHeaderVisualStyles->false
            this.dgvKSGL_EMP.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#4f81DD");
            this.dgvKSGL_EMP.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            this.dgvKSGL_EMP.DefaultCellStyle.SelectionBackColor = Color.Gray;//设置选中的颜色
            
        }


        //通过帐户删除人员信息
        private void btnDeleteEmp_Click(object sender, EventArgs e)
        {
            //if (this.listViewEmp.SelectedItems.Count == 0)
            //{
            //    MessageBox.Show("请选择要删除的科室人员");
            //    return;
            //}
            //string account = this.listViewEmp.SelectedItems[0].Text;
            //string name = this.listViewEmp.SelectedItems[0].SubItems[1].Text;
            //DialogResult dr = MessageBox.Show("删除【" + name + "(" + account + ")】用户?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (dr == DialogResult.Yes)
            //{
            //    DBEmp.getInstance().deleteEmpByAccount(account);
            //    this.setListViewEmp(null, null);//重新绑定
            //}

        }

        //打开新增科室职员
        private void btnGoAddEmp_Click(object sender, EventArgs e)
        {
            FormAddEmp.ACCOUNT = null;
            FormAddEmp form = new FormAddEmp();
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
            {
                this.setListViewEmp(null, null);//重新绑定
            }
        }

        //编辑人员信息
        private void btnGoEditEmp_Click(object sender, EventArgs e)
        {
            try
            {
                int index = dgvKSGL_EMP.CurrentRow.Index; //获取选中行的行号
                string account = dgvKSGL_EMP.Rows[index].Cells[1].Value.ToString();
                if (string.IsNullOrEmpty(account))
                {
                    MessageBox.Show("请选择要编辑的科室人员");
                    return;
                }
                //string account = this.listViewEmp.SelectedItems[0].Text;
                FormAddEmp.ACCOUNT = account;
                FormAddEmp form = new FormAddEmp();
                form.ShowDialog();
                if (form.DialogResult == DialogResult.OK)
                {
                    this.setListViewEmp(null, null);//重新绑定
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("请选择要编辑的科室人员");
            }

        }

        //通过名称和用户名查询用户信息
        private void btnFindEmpByAccountAndName_Click(object sender, EventArgs e)
        {
            this.setListViewEmp(this.tbFindEmpAccount.Text, this.tbFindEmpName.Text);//重新绑定
        }

        //查询医保类型字典
        private void btnDictionaryYBLX_Click(object sender, EventArgs e)
        {
            this.listViewDictionarySSZD.Visible = false;
            this.listViewDictionaryQKZD.Visible = false;
            this.setListViewDictionary("医保类型字典", 1);
        }

        //查询手术字典
        private void btnDictionarySSZD_Click(object sender, EventArgs e)
        {
            this.listViewDictionarySSZD.Visible = !this.listViewDictionarySSZD.Visible;
            this.listViewDictionaryQKZD.Visible = false;

            if (this.listViewDictionarySSZD.Visible)
            {
                //设置listview表头颜色
                this.listViewDictionarySSZD.Items.Clear();
                this.listViewDictionarySSZD.FullRowSelect = true;

                this.listViewDictionarySSZD.BackColor = ColorTranslator.FromHtml("#41aaeb");
                ImageList imgList = new ImageList();
                imgList.ImageSize = new Size(30, 23);
                this.listViewDictionarySSZD.SmallImageList = imgList;

                DataTable dt = DBDictionary.getInstance().getDictionarysByParentId(2);
                foreach (DataRow row in dt.Rows)
                {
                    // 不展示手术追踪期限字典
                    if (row["id"].ToString().Equals("8")) {
                        continue;
                    }
                    ListViewItem item = new ListViewItem();
                    item.SubItems.Clear();
                    item.Text = row["d_name"].ToString();
                    item.ForeColor = Color.White;
                    item.SubItems.Add(row["id"].ToString());
                    //item.SubItems.Add(row["d_name"].ToString());
                    //记住view改成detail
                    // if (ro)
                    this.listViewDictionarySSZD.Items.Add(item);
                    this.listViewDictionarySSZD.Columns[0].Width = 300;
                    //隐藏第二列
                    //this.listViewDictionarySSZD.Columns[1].Width = 0;
                    //this.listViewDictionarySSZD.Items.Add(row["d_name"].ToString());
                }
                this.listViewDictionarySSZD.EndUpdate();
            }
        }

        //查询情况字典
        private void btnDictionaryQKZD_Click(object sender, EventArgs e)
        {
           
            this.listViewDictionarySSZD.Visible = false;
            this.listViewDictionaryQKZD.Visible = !this.listViewDictionaryQKZD.Visible;


            if (this.listViewDictionaryQKZD.Visible)
            {
                //设置listview表头颜色
                this.listViewDictionaryQKZD.Items.Clear();
                this.listViewDictionaryQKZD.FullRowSelect = true;
                this.listViewDictionarySSZD.FullRowSelect = true;

                this.listViewDictionaryQKZD.BackColor = ColorTranslator.FromHtml("#41aaeb");
                ImageList imgList = new ImageList();
                imgList.ImageSize = new Size(1, 23);
                this.listViewDictionaryQKZD.SmallImageList = imgList;

                DataTable dt = DBDictionary.getInstance().getDictionarysByParentId(3);
                foreach (DataRow row in dt.Rows)
                {
                    ListViewItem item = new ListViewItem();
                    item.SubItems.Clear();
                    item.Text = row["d_name"].ToString();
                    item.ForeColor = Color.White;
                    item.SubItems.Add(row["id"].ToString());
                    //item.SubItems.Add(row["d_name"].ToString());
                    //记住view改成detail
                    this.listViewDictionaryQKZD.Items.Add(item);
                    this.listViewDictionaryQKZD.Columns[0].Width = 2000;
                    //this.listViewDictionarySSZD.Items.Add(row["d_name"].ToString());
                }
                this.listViewDictionaryQKZD.EndUpdate();
            }
        }

        //科室增加
        private void btnSaveRoom_Click_1(object sender, EventArgs e)
        {
            string hispitorName = this.tbRoomHispitorName.Text;//医院名称
            string hispitorAddress = this.tbRoomHispitorAddress.Text;//医院地址
            string province = this.cbRoomProvince.Text;//省
            string rank = this.cbRoomRank.Text;//等级
            string postcode = this.tbRoomPostcode.Text;//邮政编码
            string roomName = this.tbRoomName.Text;//科室名称
            string roomTel = this.tbRoomTel.Text;//科室电话
            string roomFax = this.tbRoomFax.Text;//科室传真
            string roomFZR = this.tbRoomFZR.Text;//科室负责人
            string roomFZRZC = this.tbRoomFZRZC.Text;//科室负责人职称
            string roomFZRDH = this.tbRoomFZRDH.Text;//科室负责人电话
            string roomFZRSJ = this.tbRoomFZRSJ.Text;//科室负责人手机
            string roomFZRYX = this.tbRoomFZRYX.Text;//科室负责人邮箱
            string roomTXZXMJ = this.tbRoomTXZXMJ.Text;//透析中心面积
            string roomTXDYMJ = this.tbRoomTXDYMJ.Text;//透析单元面积
            string roomKSRQ = this.dtpRoomKSRQ.Text;//开始日期
            // hispitorName,hispitorAddress,province,rank,postcode,roomName,roomTel,roomFax,roomFZR,roomFZRZC,roomFZRDH,roomFZRSJ,roomFZRYX,roomTXZXMJ,roomTXDYMJ,roomKSRQ

            if (string.IsNullOrEmpty(hispitorName)) { MessageBox.Show("【医院名称】不能为空"); return; }
            //if (string.IsNullOrEmpty(hispitorAddress)) { MessageBox.Show("【医院地址】不能为空"); return; }
            //if (string.IsNullOrEmpty(province)) { MessageBox.Show("【省(直辖市)】不能为空"); return; }
            //if (string.IsNullOrEmpty(rank)) { MessageBox.Show("【等级】不能为空"); return; }
            //if (string.IsNullOrEmpty(postcode)) { MessageBox.Show("【邮政编码】不能为空"); return; }
            //if (postcode.Length != 6) { MessageBox.Show("【邮政编码】 格式有误"); return; }
            if (string.IsNullOrEmpty(roomName)) { MessageBox.Show("【科室名称】不能为空"); return; }
            //if (string.IsNullOrEmpty(roomTel)) { MessageBox.Show("【科室电话】不能为空"); return; }
            //if (roomTel.Length > 11) { MessageBox.Show("【科室电话】 格式有误"); return; }
            //if (string.IsNullOrEmpty(roomFax)) { MessageBox.Show("【科室传真】不能为空"); return; }
            //if (string.IsNullOrEmpty(roomFZR)) { MessageBox.Show("【科室负责人】不能为空"); return; }
            //if (string.IsNullOrEmpty(roomFZRZC)) { MessageBox.Show("【科室负责人职称】不能为空"); return; }
            //if (string.IsNullOrEmpty(roomFZRDH)) { MessageBox.Show("【科室负责人电话】不能为空"); return; }
            //if (roomFZRDH.Length > 11) { MessageBox.Show("【科室负责人电话】 格式有误"); return; }
            //if (string.IsNullOrEmpty(roomFZRSJ)) { MessageBox.Show("【科室负责人手机】不能为空"); return; }
            //if (roomFZRSJ.Length != 11) { MessageBox.Show("【科室负责人手机】 格式有误"); return; }
            //if (string.IsNullOrEmpty(roomFZRYX)) { MessageBox.Show("【科室负责人邮箱】不能为空"); return; }
            //if (!UtilTools.isEmail(roomFZRYX)) { MessageBox.Show("【科室负责人邮箱】 格式有误"); return; }
            //if (string.IsNullOrEmpty(roomTXZXMJ)) { MessageBox.Show("【透析中心面积】 不能为空"); return; }
            //if (string.IsNullOrEmpty(roomTXDYMJ)) { MessageBox.Show("【透析单元面积】 不能为空"); return; }
            //if (string.IsNullOrEmpty(roomKSRQ)) { MessageBox.Show("【开始日期】 不能为空"); return; }


            int flag = 0;
            string sql = "";
            dataTable = DBManager.getInstance().find("select * from t_room where id = 0");
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                //hispitorName, hispitorAddress, province, rank, postcode, roomName, roomTel, roomFax, roomFZR, roomFZRZC, roomFZRDH, roomFZRSJ, roomFZRYX, roomTXZXMJ, roomTXDYMJ, roomKSRQ
                sql = string.Format("update t_room set r_hospital_name='{0}',r_address='{1}',r_province='{2}', r_rank='{3}', r_postcodes='{4}', r_room_name='{5}', r_room_tel='{6}'," +
                "r_room_fax='{7}',r_responsible='{8}',r_responsible_title='{9}', r_responsible_tel='{10}', r_responsible_phone='{11}',  r_responsible_email='{12}'," +
                " r_dialyse_center_area='{13}', r_dialyse_unit_area='{14}', r_start_date='{15}' where id=0",
                 hispitorName, hispitorAddress, province, rank, postcode, roomName, roomTel, roomFax, roomFZR, roomFZRZC, roomFZRDH,
                 roomFZRSJ, roomFZRYX, roomTXZXMJ, roomTXDYMJ, roomKSRQ);
                flag = DBManager.getInstance().edit(sql);
            }
            else
            {
                sql = string.Format("insert into t_room(id,r_hospital_name,r_address,r_province, r_rank, r_postcodes,  r_room_name, r_room_tel," +
               "r_room_fax,r_responsible,r_responsible_title, r_responsible_tel, r_responsible_phone,  r_responsible_email," +
               " r_dialyse_center_area, r_dialyse_unit_area, r_start_date) values(0,'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}'," +
               "'{12}','{13}','{14}','{15}')", hispitorName, hispitorAddress, province, rank, postcode, roomName, roomTel, roomFax, roomFZR, roomFZRZC, roomFZRDH,
               roomFZRSJ, roomFZRYX, roomTXZXMJ, roomTXDYMJ, roomKSRQ);
                flag = DBManager.getInstance().add(sql);
            }
            if (flag > 0)
            {
                MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("保存失败");
            }
        }

        //只能输入数字
        private void number_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))//如果不是输入数字就不让输入
            {
                e.Handled = true;
            }
        }

        //鼠标离开 隐藏手术字典列表
        private void listViewDictionarySSZD_MouseLeave(object sender, EventArgs e)
        {
            this.listViewDictionarySSZD.Visible = false;
        }

        //鼠标离开 隐藏情况字典列表
        private void listViewDictionaryQKZD_MouseLeave(object sender, EventArgs e)
        {
            this.listViewDictionaryQKZD.Visible = false;
        }

        //选中手术字典列表
        private void listViewDictionarySSZD_SelectedIndexChanged(object sender, EventArgs e)
        {

            foreach (int i in this.listViewDictionarySSZD.SelectedIndices)
            {
                this.listViewDictionarySSZD.Items[i].Selected = true;
                string dName = this.listViewDictionarySSZD.SelectedItems[0].Text;
                int id = Convert.ToInt32(this.listViewDictionarySSZD.SelectedItems[0].SubItems[1].Text);
                //MessageBox.Show(this.listViewDictionarySSZD.SelectedItems[0].SubItems[1].Text);
                this.setListViewDictionary(dName, id);
            }
            this.listViewDictionarySSZD.Visible = false;
        }

        //选中情况字典列表
        private void listViewDictionaryQKZD_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (int i in this.listViewDictionaryQKZD.SelectedIndices)
            {
                this.listViewDictionaryQKZD.Items[i].Selected = true;
                string dName = this.listViewDictionaryQKZD.SelectedItems[0].Text;
                int id = Convert.ToInt32(this.listViewDictionaryQKZD.SelectedItems[0].SubItems[1].Text);
                this.setListViewDictionary(dName, id);
            }
            this.listViewDictionaryQKZD.Visible = false;
        }

        /// <summary>
        /// 使DataGridView的列自适应宽度
        /// </summary>
        /// <param name="dgViewFiles"></param>
        private void AutoSizeColumn(DataGridView dgViewFiles)
        {
            int width = 0;
            //使列自使用宽度
            //对于DataGridView的每一个列都调整
            for (int i = 0; i < dgViewFiles.Columns.Count; i++)
            {
                //将每一列都调整为自动适应模式
                dgViewFiles.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
                //记录整个DataGridView的宽度
                width += dgViewFiles.Columns[i].Width;
            }
            //判断调整后的宽度与原来设定的宽度的关系，如果是调整后的宽度大于原来设定的宽度，
            //则将DataGridView的列自动调整模式设置为显示的列即可，
            //如果是小于原来设定的宽度，将模式改为填充。
            if (width > dgViewFiles.Size.Width)
            {
                dgViewFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }
            else
            {
                dgViewFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            //冻结某列 从左开始 0，1，2
            dgViewFiles.Columns[1].Frozen = true;
        }

        //设置显示字典
        private void setListViewDictionary(string dName, int parentId)
        {
            this.dgvDictionary.DataSource = null;//清空
            this.dgvDictionary.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//选中整行

            dictionary_parent_id = parentId;
            this.labelDictionaryShow.Text = dName;//显示选中的字典名称

            string sql = string.Format("select id,d_parent_id,d_order,d_name,d_desc from t_dictionary where d_parent_id={0} order by d_order,d_name", parentId);
            DataTable dt = DBManager.getInstance().find(sql);
           
            this.dgvDictionary.DataSource = dt;

            this.dgvDictionary.Columns[0].Visible = false;
            this.dgvDictionary.Columns[1].Visible = false;

            this.dgvDictionary.Columns[0].FillWeight = 1;
            this.dgvDictionary.Columns[1].FillWeight = 1;
            this.dgvDictionary.Columns[2].FillWeight = 10;
            this.dgvDictionary.Columns[3].FillWeight = 20;
            this.dgvDictionary.Columns[4].FillWeight = 40;

            this.dgvDictionary.Columns[0].HeaderText = "id";
            //this.dgvDictionary.Columns[0].HeaderText.Bold = true;
            this.dgvDictionary.Columns[1].HeaderText = "pId";
            this.dgvDictionary.Columns[2].HeaderText = "排列序号";
            this.dgvDictionary.Columns[3].HeaderText = dName;
            this.dgvDictionary.Columns[4].HeaderText = "描述";

            // Header以外所有的单元格的背景色
            this.dgvDictionary.RowsDefaultCellStyle.BackColor = Color.White;

            ////列Header的背景色 CellBorderStyle->Single   EnableHeaderVisualStyles->false
            this.dgvDictionary.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#4f81DD");
            this.dgvDictionary.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvDictionary.ColumnHeadersDefaultCellStyle.Font = new Font("宋体",12f, FontStyle.Bold) ;

            //设置选中的颜色
            this.dgvDictionary.DefaultCellStyle.SelectionBackColor = Color.Gray;

        }

        //前往添加字典界面
        private void btnGoAddDictionaryy_Click(object sender, EventArgs e)
        {
            FormAddDictionary formAddDictionary = new FormAddDictionary();
            formAddDictionary.setDesc(-1, dictionary_parent_id, "添加", this.labelDictionaryShow.Text);
            formAddDictionary.ShowDialog();
            if (formAddDictionary.DialogResult == DialogResult.OK)
            {
                this.setListViewDictionary(this.labelDictionaryShow.Text, dictionary_parent_id);//重新绑定
            }
        }

        //修改字典
        private void btnGoEditDictionary_Click(object sender, EventArgs e)
        {
            try
            {
                int index = dgvDictionary.CurrentRow.Index; //获取选中行的行号
                int id = Convert.ToInt32(dgvDictionary.Rows[index].Cells[0].Value.ToString());
               
                if (id == 0)
                {
                    MessageBox.Show("请选择要修改的字典");
                    return;
                }

                FormAddDictionary formAddDictionary = new FormAddDictionary();
                formAddDictionary.setDesc(id, dictionary_parent_id, "修改", this.labelDictionaryShow.Text);
                formAddDictionary.ShowDialog();
                if (formAddDictionary.DialogResult == DialogResult.OK)
                {
                    this.setListViewDictionary(this.labelDictionaryShow.Text, dictionary_parent_id);//重新绑定
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("请选择要修改的字典");
            }
            
        }

        //删除字典
        private void btnDeleteDictionary_Click(object sender, EventArgs e)
        {
            try
            {
                int index = dgvDictionary.CurrentRow.Index; //获取选中行的行号
                int id = Convert.ToInt32(dgvDictionary.Rows[index].Cells[0].Value.ToString());
                string name = dgvDictionary.Rows[index].Cells[3].Value.ToString();

                if (id == 0)
                {
                    MessageBox.Show("请选择要修改的字典");
                    return;
                }

                DialogResult dr = MessageBox.Show("删除【" + name + "】?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    DBDictionary.getInstance().deleteDictionary(id);
                    this.setListViewDictionary(this.labelDictionaryShow.Text, dictionary_parent_id);//重新绑定
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("请选择要删除的字典");
            }
            
        }




        private void btnSaveSSJL2_Click(object sender, EventArgs e)
        {

            string name = this.tbSSLR_NAME.Text;//患者姓名
            string sex = this.cbSSLR_SEX.Text;//性别
            string age = this.tbSSLR_AGE.Text;//年龄
            string tel = this.tbSSLR_TEL.Text;//手机号
            string ID = this.tbSSLR_ID.Text;//身份证号
            string yblx = this.cbSSLR_YBLX.Text;//医保类型
            string province = this.cbSSLR_PROVINCE.Text;//省
            string city = this.cbSSLR_CITY.Text;//市
            string county = this.cbSSLR_COUNTY.Text;//县
            string deatilAddress = this.tbSSLR_DETAIL_ADDRESS.Text;//详细地址
            string ctxyy = this.tbSSLR_CTXYY.Text;//常透析医院
            string ctxyylxr = this.tbSSLR_CTXYYLXR.Text;//常透析医院联系人
            string ctxyylxrdh = this.tbSSLR_CTXYYLXRDH.Text;//常透析医院联系人电话
            DateTime ssrq = this.dpSSLR_SSRQ.Value;//手术日期
            string ssdd = this.cbSSLR_SSDD.Text;// SelectedValue.ToString();//手术地点
            string sslx = this.cbSSLR_SSLX.Text;// SelectedValue.ToString();//手术类型
            string ssfs = this.cbSSLR_SSFS.Text;//SelectedValue.ToString();//手术方式
            string ccfs = this.cbSSLR_CCFS.Text;//SelectedValue.ToString();//穿刺方式
            DateTime sszz = this.dtSSLR_SSZZ.Value; // 手术追踪日期
            string sszzqx = this.cbSSLR_SSZZQX.Text;//SelectedValue.ToString();//手术追踪期限--每三个月/每六个月
            DateTime zz1 = this.dtTraceTime1.Value;
            DateTime zz2 = this.dtTraceTime2.Value;
            DateTime zz3 = this.dtTraceTime3.Value;
            DateTime zz4 = this.dtTraceTime4.Value;
            string ssbw = this.cbSSLR_SSBW.Text; // 手术部位
            string ssjl = this.rtbSSLR_SSJL.Rtf;//手术记录
            string zdys = this.tbSSLR_ZDYS.Text;//主刀医生
            string zs = this.tbSSLR_ZS.Text;//助手
            string qxfs = this.tbSSLR_QXFS.Text;//器械护士

            // int imgNumber = this.imgLstTTLB.Images.Count; // 上传图片数量
           
            

            string address = province + "-" + city + "-" + county + "-" + deatilAddress + "";
           
            if (string.IsNullOrEmpty(name)) { MessageBox.Show("【姓名】不能为空"); return; }
            if (string.IsNullOrEmpty(sex)) { MessageBox.Show("【性别】不能为空"); return; }
            if (string.IsNullOrEmpty(tel)) { MessageBox.Show("【电话】不能为空"); return; }
            if (tel.Length != 11) { MessageBox.Show("【电话】格式错误"); return; }
            if (string.IsNullOrEmpty(ID)) { MessageBox.Show("【身份证号】不能为空"); return; }
            if (ID.Length != 18) { MessageBox.Show("【身份证号】格式错误"); return; }
            if (string.IsNullOrEmpty(ctxyy)) { MessageBox.Show("【常透析医院】不能为空"); return; }
            if (string.IsNullOrEmpty(ctxyylxr)) { MessageBox.Show("【常透析医院联系人】不能为空"); return; }
            if (string.IsNullOrEmpty(ctxyylxrdh)) { MessageBox.Show("【常透析医院联系人电话】不能为空"); return; }
            if (ctxyylxrdh.Length != 11) { MessageBox.Show("【常透析医院联系人电话】格式错误"); return; }
            if (string.IsNullOrEmpty(sszzqx)) { MessageBox.Show("请选择【手术追踪期限】"); return; }
            if (string.IsNullOrEmpty(ssjl)) { MessageBox.Show("【手术记录】不能为空"); return; }
            if (string.IsNullOrEmpty(zdys)) { MessageBox.Show("【主刀医生】不能为空"); return; }
            if (string.IsNullOrEmpty(zs)) { MessageBox.Show("【助手】不能为空"); return; }
            if (string.IsNullOrEmpty(qxfs)) { MessageBox.Show("【器械护士】不能为空"); return; }
            
            // if (0 == imgNumber) { MessageBox.Show("【请上传手术图片】"); }
            if (null == picSSTP.Image)
            {
                MessageBox.Show("【请上传手术图片】");
            }

            // Console.WriteLine("--> OK 进入保存");

            using (MySqlConnection con = SqlHelper4MySql.getConnection()) {
                try
                {
                    con.Open();
                    MySqlTransaction tx = con.BeginTransaction();

                    // 1，保存患者 ，如果已经存在该患者就更新，如果不存在就插入

                    // 1.1查询该患者是否已经存在
                    MySqlParameter[] ps = new MySqlParameter[] {
                        new MySqlParameter("@p_id", ID)
                    };
                    // Console.WriteLine("-->" + SqlHelper4MySql.ExecuteScalar(con, SqlCommandHelpler.T_PATIENT_SELECT_COUNT_BY_PATIENT_ID, ps));
                    Boolean patientExists = Convert.ToInt32(SqlHelper4MySql.ExecuteScalar(con, SqlCommandHelpler.T_PATIENT_SELECT_COUNT_BY_PATIENT_ID, ps)) > 0 ? true : false;

                    // @p_name, @p_sex, @p_age, @p_tel, @p_dialyse_hospital_tel, 
                    // @p_health_type, @p_address,@p_dialyse_hospital, @p_dialyse_hospital_contact, @p_ID
                    ps = new MySqlParameter[] {
                        new MySqlParameter("@p_name", name),
                        new MySqlParameter("@p_sex", sex),
                        new MySqlParameter("@p_age", age),
                        new MySqlParameter("@p_tel", tel),
                        new MySqlParameter("@p_dialyse_hospital_tel", ctxyylxrdh),

                        new MySqlParameter("@p_health_type", yblx),
                        new MySqlParameter("@p_address", address),
                        new MySqlParameter("@p_dialyse_hospital", ctxyy),
                        new MySqlParameter("@p_dialyse_hospital_contact", ctxyylxr),
                        new MySqlParameter("@p_ID", ID),
                    };
                    if (patientExists)
                    {
                        // 1.2.1存在就更新
                        SqlHelper4MySql.ExecuteNonQuery(con, SqlCommandHelpler.T_PATIENT_UPDATE_BY_PATIENT_ID, ps);
                    }
                    else
                    {
                        // 1.2.2不存在就插入
                        SqlHelper4MySql.ExecuteNonQuery(con, SqlCommandHelpler.T_PATIENT_INSERT, ps);
                    }

                    // 2保存手术记录
                    ps = new MySqlParameter[] {
                        new MySqlParameter("@p_ID", ID),
                        new MySqlParameter("@r_date", ssrq),
                        new MySqlParameter("@r_ss_address", ssdd),
                        new MySqlParameter("@r_ss_type", sslx),
                        new MySqlParameter("@r_ss_method", ssfs),

                        new MySqlParameter("@r_cc_method", ccfs),
                        new MySqlParameter("@r_sszzqx", sszz),
                        new MySqlParameter("@r_sszz", sszzqx),
                        new MySqlParameter("@r_zz1", zz1),
                        new MySqlParameter("@r_zz2", zz2),

                        new MySqlParameter("@r_zz3", zz3),
                        new MySqlParameter("@r_zz4", zz4),
                        new MySqlParameter("@r_ssbw", ssbw),
                        new MySqlParameter("@r_zd_docotor", zdys),
                        new MySqlParameter("@r_zs", zs),

                        new MySqlParameter("@r_qxhs", qxfs),
                        new MySqlParameter("@r_ss_record", ssjl),
                        new MySqlParameter("@r_is_sszz", "否"),
                    };
                    SqlHelper4MySql.ExecuteNonQuery(con, SqlCommandHelpler.T_RECORD_INSERT, ps);
                    // 得到手术记录的ID
                    int recordId = Convert.ToInt32(SqlHelper4MySql.ExecuteScalar(con, SqlCommandHelpler.COMMON_SELECT_LAST_INSERT_ID, null));
                    // Console.WriteLine("手术记录的ID是:" + recordId);
                    

                    // 3保存图片
                    /*for (int i = 0; i < imgNumber; i++)
                    {

                        // 
                        Image img = this.imgLstTTLB.Images[i];
                        ps = new MySqlParameter[] {
                            new MySqlParameter("@p_r_id", recordId),
                            new MySqlParameter("@p_path", ""),
                            new MySqlParameter("@p_desc", ""),
                            new MySqlParameter("@p_content", ImageHelpler.imageToByte(img)),
                            new MySqlParameter("@p_order", i),
                        };
                        SqlHelper4MySql.ExecuteNonQuery(con, SqlCommandHelpler.T_PICTURE_INSERT, ps);
                    }*/
                    if (null != picSSTP.Image)
                    {
                        // Image img =  // this.imgLstTTLB.Images[i];
                        ps = new MySqlParameter[] {
                            new MySqlParameter("@p_r_id", recordId),
                            new MySqlParameter("@p_path", ""),
                            new MySqlParameter("@p_desc", ""),
                            new MySqlParameter("@p_content", ImageHelpler.imageToByte(picSSTP.Image)),
                            new MySqlParameter("@p_order", 1),
                        };
                        SqlHelper4MySql.ExecuteNonQuery(con, SqlCommandHelpler.T_PICTURE_INSERT, ps);
                    }
                    if (null != pictureBox10.Image)
                    {
                        // Image img = this.imgLstTTLB.Images[i];
                        ps = new MySqlParameter[] {
                            new MySqlParameter("@p_r_id", recordId),
                            new MySqlParameter("@p_path", ""),
                            new MySqlParameter("@p_desc", ""),
                            new MySqlParameter("@p_content", ImageHelpler.imageToByte(pictureBox10.Image)),
                            new MySqlParameter("@p_order", 2),
                        };
                        SqlHelper4MySql.ExecuteNonQuery(con, SqlCommandHelpler.T_PICTURE_INSERT, ps);
                    }

                    // 4保存手术追踪。根据每三个月/每六个月创建4/2个对应的手术跟踪
                    // 第一次追踪
                    ps = new MySqlParameter[] {
                        new MySqlParameter("@t_record_id", recordId),
                        new MySqlParameter("@t_zzrq", zz1),
                        new MySqlParameter("@t_patient_ID", ID),
                        new MySqlParameter("@t_sfsszz", "否"),
                        new MySqlParameter("@t_sfsszzcg", "否")
                    };
                    SqlHelper4MySql.ExecuteNonQuery(con, SqlCommandHelpler.T_TRACK_INSERT_PART, ps);
                    // 第二次追踪
                    ps[1] = new MySqlParameter("@t_zzrq", zz2);
                    SqlHelper4MySql.ExecuteNonQuery(con, SqlCommandHelpler.T_TRACK_INSERT_PART, ps);

                    if (sszzqx.Equals("每三个月"))
                    {
                        // 第三次追踪
                        ps[1] = new MySqlParameter("@t_zzrq", zz3);
                        SqlHelper4MySql.ExecuteNonQuery(con, SqlCommandHelpler.T_TRACK_INSERT_PART, ps);
                        // 第四次追踪
                        ps[1] = new MySqlParameter("@t_zzrq", zz4);
                        SqlHelper4MySql.ExecuteNonQuery(con, SqlCommandHelpler.T_TRACK_INSERT_PART, ps);
                    }

                    tx.Commit();

                    MessageBox.Show("保存成功");
                    this.tbSSLR_NAME.Text = "";//患者姓名
                    this.cbSSLR_SEX.Text = "";//性别
                    this.tbSSLR_AGE.Text = "";//年龄
                    this.tbSSLR_TEL.Text = "";//手机号
                    this.tbSSLR_ID.Text = "";//身份证号
                    this.tbSSLR_DETAIL_ADDRESS.Text = "";//详细地址
                    this.tbSSLR_CTXYY.Text = "";//常透析医院
                    this.tbSSLR_CTXYYLXR.Text = "";//常透析医院联系人
                    this.tbSSLR_CTXYYLXRDH.Text = "";//常透析医院联系人电话
                    this.rtbSSLR_SSJL.Text = "";//手术记录
                    this.tbSSLR_ZDYS.Text = "";//主刀医生
                    this.tbSSLR_ZS.Text = "";//助手
                    this.tbSSLR_QXFS.Text = "";//器械护士
                    this.cbSSLR_SEX.SelectedIndex = -1;
                    this.cbSSLR_YBLX.SelectedIndex = -1;

                    this.cbSSLR_PROVINCE.SelectedIndex = -1;
                    this.cbSSLR_CITY.SelectedIndex = -1;
                    this.cbSSLR_COUNTY.SelectedIndex = -1;

                    this.cbSSLR_SSDD.SelectedIndex = -1;
                    this.cbSSLR_SSLX.SelectedIndex = -1;
                    this.cbSSLR_SSFS.SelectedIndex = -1;
                    this.cbSSLR_CCFS.SelectedIndex = -1;

                    this.cbSSLR_SSZZQX.SelectedIndex = -1;

                    this.cbSSLR_SSBW.SelectedIndex = -1;
                    removeAllSSTP();
                    hideAllTraceTime();
                }
                catch (Exception ex) {
                    MessageBox.Show("保存失败：" + ex.Message);
                }
            }

        }

        //保存手术记录
        private void btnSaveSSJL_Click(object sender, EventArgs e)
        {
            string name = this.tbSSLR_NAME.Text;//患者姓名
            string sex = this.cbSSLR_SEX.Text;//性别
            string age = this.tbSSLR_AGE.Text;//年龄
            string tel = this.tbSSLR_TEL.Text;//手机号
            string ID = this.tbSSLR_ID.Text;//身份证号
            string yblx = this.cbSSLR_YBLX.Text;//医保类型
            string province = this.cbSSLR_PROVINCE.Text;//省
            string city = this.cbSSLR_CITY.Text;//市
            string county = this.cbSSLR_COUNTY.Text;//县
            string deatilAddress = this.tbSSLR_DETAIL_ADDRESS.Text;//详细地址
            string ctxyy = this.tbSSLR_CTXYY.Text;//常透析医院
            string ctxyylxr = this.tbSSLR_CTXYYLXR.Text;//常透析医院联系人
            string ctxyylxrdh = this.tbSSLR_CTXYYLXRDH.Text;//常透析医院联系人电话
            DateTime ssrq = this.dpSSLR_SSRQ.Value;//手术日期
            string ssdd = this.cbSSLR_SSDD.Text;// SelectedValue.ToString();//手术地点
            string sslx = this.cbSSLR_SSLX.Text;// SelectedValue.ToString();//手术类型
            string ssfs = this.cbSSLR_SSFS.Text;//SelectedValue.ToString();//手术方式
            string ccfs = this.cbSSLR_CCFS.Text;//SelectedValue.ToString();//穿刺方式
            string sszzqx = this.cbSSLR_SSZZQX.Text;//SelectedValue.ToString();//手术追踪期限
            string ssjl = this.rtbSSLR_SSJL.Rtf;//手术记录
            string zdys = this.tbSSLR_ZDYS.Text;//主刀医生
            string zs = this.tbSSLR_ZS.Text;//助手
            string qxfs = this.tbSSLR_QXFS.Text;//器械护士

            string address = province + "-" + city + "-" + county + "-" + deatilAddress+"";

            if (string.IsNullOrEmpty(name)) { MessageBox.Show("【姓名】不能为空"); return; }
            if (string.IsNullOrEmpty(sex)) { MessageBox.Show("【性别】不能为空"); return; }
            if (string.IsNullOrEmpty(tel)) { MessageBox.Show("【电话】不能为空"); return; }
            if (tel.Length != 11) { MessageBox.Show("【电话】格式错误"); return; }
            if (string.IsNullOrEmpty(ID)) { MessageBox.Show("【身份证号】不能为空"); return; }
            if (ID.Length != 18) { MessageBox.Show("【身份证号】格式错误"); return; }
            if (string.IsNullOrEmpty(ctxyy)) { MessageBox.Show("【常透析医院】不能为空"); return; }
            if (string.IsNullOrEmpty(ctxyylxr)) { MessageBox.Show("【常透析医院联系人】不能为空"); return; }
            if (string.IsNullOrEmpty(ctxyylxrdh)) { MessageBox.Show("【常透析医院联系人电话】不能为空"); return; }
            if (ctxyylxrdh.Length != 11) { MessageBox.Show("【常透析医院联系人电话】格式错误"); return; }
            if (string.IsNullOrEmpty(ssjl)) { MessageBox.Show("【手术记录】不能为空"); return; }
            if (string.IsNullOrEmpty(zdys)) { MessageBox.Show("【主刀医生】不能为空"); return; }
            if (string.IsNullOrEmpty(zs)) { MessageBox.Show("【助手】不能为空"); return; }
            if (string.IsNullOrEmpty(qxfs)) { MessageBox.Show("【器械护士】不能为空"); return; }




            int flag = -1;
            
            string sql = "sslr_proc";   //存储过程名称myproc  

            MySqlCommand cmd = DBManager.getInstance().getMySqlCommand(sql);
            //设置Command对象的类型  
            cmd.CommandType = CommandType.StoredProcedure;

            //p_name,p_sex,p_age,p_tel,p_ID,p_health_type,p_address,p_dialyse_hospital,p_dialyse_hospital_contact,p_dialyse_hospital_tel
            //name, sex, age, tel, ID, yblx, address, ctxyy, ctxyylxr, ctxyylxrdh
            cmd.Parameters.AddWithValue("@i_p_name", name);
            cmd.Parameters.AddWithValue("@i_p_sex", sex);
            cmd.Parameters.AddWithValue("@i_p_age", age);
            cmd.Parameters.AddWithValue("@i_p_tel", tel);
            cmd.Parameters.AddWithValue("@i_p_ID", ID);
            cmd.Parameters.AddWithValue("@i_p_health_type", yblx);
            cmd.Parameters.AddWithValue("@i_p_address", address);
            cmd.Parameters.AddWithValue("@i_p_dialyse_hospital", ctxyy);
            cmd.Parameters.AddWithValue("@i_p_dialyse_hospital_contact", ctxyylxr);
            cmd.Parameters.AddWithValue("@i_p_dialyse_hospital_tel", ctxyylxrdh);//sszzqx

            //r_patient_ID,r_date,r_ss_address,r_ss_type,r_ss_method,r_cc_method,r_zd_docotor,r_zs,r_qxhs,r_ss_record,r_is_sszz
            //ID, ssrq, ssdd, sslx, ssfs, ccfs, zdys,zs, qxfs, ssjl
            cmd.Parameters.AddWithValue("@i_r_date", ssrq);
            cmd.Parameters.AddWithValue("@i_r_ss_address", ssdd);
            cmd.Parameters.AddWithValue("@i_r_ss_type", sslx);
            cmd.Parameters.AddWithValue("@i_r_ss_method", ssfs);
            cmd.Parameters.AddWithValue("@i_r_cc_method", ccfs);
            cmd.Parameters.AddWithValue("@i_r_zd_docotor", zdys);
            cmd.Parameters.AddWithValue("@i_r_zs", zs);
            cmd.Parameters.AddWithValue("@i_r_qxhs", qxfs);
            cmd.Parameters.AddWithValue("@i_r_ss_record", ssjl);
            cmd.Parameters.AddWithValue("@i_r_is_sszz", "否");
            cmd.Parameters.Add("@error_code", SqlDbType.Int).Direction = ParameterDirection.Output;

            //执行  
            cmd.ExecuteNonQuery();

            flag = Convert.ToInt32(cmd.Parameters["@error_code"].Value.ToString());
            if (flag >= 0)
            {
                MessageBox.Show("保存成功");
                this.tbSSLR_NAME.Text = "";//患者姓名
                this.cbSSLR_SEX.Text = "";//性别
                this.tbSSLR_AGE.Text = "";//年龄
                this.tbSSLR_TEL.Text = "";//手机号
                this.tbSSLR_ID.Text = "";//身份证号
                this.tbSSLR_DETAIL_ADDRESS.Text = "";//详细地址
                this.tbSSLR_CTXYY.Text = "";//常透析医院
                this.tbSSLR_CTXYYLXR.Text = "";//常透析医院联系人
                this.tbSSLR_CTXYYLXRDH.Text = "";//常透析医院联系人电话
                this.rtbSSLR_SSJL.Text = "";//手术记录
                this.tbSSLR_ZDYS.Text = "";//主刀医生
                this.tbSSLR_ZS.Text = "";//助手
                this.tbSSLR_QXFS.Text = "";//器械护士

                this.cbSSLR_SEX.SelectedIndex = -1;
                this.cbSSLR_YBLX.SelectedIndex = -1;

                this.cbSSLR_PROVINCE.SelectedIndex = -1;
                this.cbSSLR_CITY.SelectedIndex = -1;
                this.cbSSLR_COUNTY.SelectedIndex = -1;

                this.cbSSLR_SSDD.SelectedIndex = -1;
                this.cbSSLR_SSLX.SelectedIndex = -1;
                this.cbSSLR_SSFS.SelectedIndex = -1;
                this.cbSSLR_CCFS.SelectedIndex = -1;

                this.cbSSLR_SSZZQX.SelectedIndex = -1;

                this.cbSSLR_SSBW.SelectedIndex = -1;
            }
            else
            {
                DBManager.getInstance().edit(sql);
                MessageBox.Show("保存失败");
            }
            
        }

        //监听身份证输入
        private void tbSSLR_ID_TextChanged(object sender, EventArgs e)
        {
            if (this.tbSSLR_ID.Text.Length == 18)
            {
                //int flag = 0;
                for (int i = 0; i < 17; i++)
                {

                    if (!UtilTools.IsNumber(this.tbSSLR_ID.Text.ToString().Substring(i, 1)) && UtilTools.getAgeByID(this.tbSSLR_ID.Text) <=0)
                    {
                        MessageBox.Show("身份证输入有误");
                        return;
                    }
                    // flag++;
                }
                this.tbSSLR_AGE.Text = UtilTools.getAgeByID(this.tbSSLR_ID.Text) + "";
            }
        }

        //取消手术记录
        private void btnRecordClear_Click(object sender, EventArgs e)
        {
            initSSLR();
        }

        //手术记录名字输入后 回车键查询
        private void tbSSLR_NAME_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(this.tbSSLR_NAME.Text))
                {
                    MessageBox.Show("请输入名称");
                    return;
                }

                //r_patient_ID,r_date,r_ss_address,r_ss_type,r_ss_method,r_cc_method,r_zd_docotor,r_zs,r_qxhs,r_ss_record,r_is_sszz
                string sql = string.Format("select p_name,p_sex,p_age,p_tel,p_ID,p_health_type,p_address,p_dialyse_hospital,p_dialyse_hospital_contact," +
                    "p_dialyse_hospital_tel from t_patient where p_name='{0}'", this.tbSSLR_NAME.Text);
                dataTable = DBManager.getInstance().find(sql);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    if (dataTable.Rows.Count > 1)
                    {
                        MessageBox.Show("该名称有多个，请再输入身份证");
                    }
                    else
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            this.tbSSLR_NAME.Text = row["p_name"].ToString();//患者姓名
                            this.cbSSLR_SEX.Text = row["p_sex"].ToString();//性别
                            this.tbSSLR_AGE.Text = row["p_age"].ToString();//年龄
                            this.tbSSLR_TEL.Text = row["p_tel"].ToString();//手机号
                            this.tbSSLR_ID.Text = row["p_ID"].ToString();//身份证号
                            this.cbSSLR_YBLX.Text = row["p_health_type"].ToString();//医保类型
                            this.cbSSLR_PROVINCE.Text = row["p_address"].ToString().Split('-')[0];//省
                            this.cbSSLR_CITY.Text = row["p_address"].ToString().Split('-')[1];//市
                            this.cbSSLR_COUNTY.Text = row["p_address"].ToString().Split('-')[2];//县
                            this.tbSSLR_DETAIL_ADDRESS.Text = row["p_address"].ToString().Split('-')[3];//详细地址
                            this.tbSSLR_CTXYY.Text = row["p_dialyse_hospital"].ToString();//常透析医院
                            this.tbSSLR_CTXYYLXR.Text = row["p_dialyse_hospital_contact"].ToString();//常透析医院联系人
                            this.tbSSLR_CTXYYLXRDH.Text = row["p_dialyse_hospital_tel"].ToString();//常透析医院联系人电话
                        }

                        //查询该患者的最近一次的手术记录
                        sql = string.Format("select id,r_date,r_ss_address,r_ss_type,r_ss_method,r_cc_method,r_zd_docotor,r_zs,r_qxhs,r_ss_record,r_is_sszz" +
                        " from t_record where r_patient_ID='{0}' order by id DESC limit 1", this.tbSSLR_ID.Text);
                        dataTable = DBManager.getInstance().find(sql);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            foreach (DataRow row in dataTable.Rows)
                            {
                                this.labelSSLR_SSJL_ID.Text = row["id"].ToString();//手术记录id
                                this.dpSSLR_SSRQ.Value = Convert.ToDateTime(row["r_date"].ToString());//手术日期
                                this.cbSSLR_SSDD.Text = row["r_ss_address"].ToString();//手术地点
                                this.cbSSLR_SSLX.Text = row["r_ss_type"].ToString();//手术类型
                                this.cbSSLR_SSFS.Text = row["r_ss_method"].ToString();//手术方式
                                this.cbSSLR_CCFS.Text = row["r_cc_method"].ToString();//穿刺方式
                                this.rtbSSLR_SSJL.Text = row["r_ss_record"].ToString();//手术记录
                                this.tbSSLR_ZDYS.Text = row["r_zd_docotor"].ToString();//主刀医生
                                this.tbSSLR_ZS.Text = row["r_zs"].ToString();//助手
                                this.tbSSLR_QXFS.Text = row["r_qxhs"].ToString();//器械护士
                            }
                        }
                        
                    }
                }
                else
                {
                    MessageBox.Show("该名称不存在");
                }

            }
        }

        //手术记录身份证输入后 回车键查询
        private void tbSSLR_ID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(this.tbSSLR_ID.Text))
                {
                    MessageBox.Show("请输入身份证");
                    return;
                }

                string sql = string.Format("select p_name,p_sex,p_age,p_tel,p_ID,p_health_type,p_address,p_dialyse_hospital,p_dialyse_hospital_contact," +
                    "p_dialyse_hospital_tel from t_patient where p_ID='{0}'", this.tbSSLR_ID.Text);
                dataTable = DBManager.getInstance().find(sql);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        this.tbSSLR_NAME.Text = row["p_name"].ToString();//患者姓名
                        this.cbSSLR_SEX.Text = row["p_sex"].ToString();//性别
                        this.tbSSLR_AGE.Text = row["p_age"].ToString();//年龄
                        this.tbSSLR_TEL.Text = row["p_tel"].ToString();//手机号
                        this.tbSSLR_ID.Text = row["p_ID"].ToString();//身份证号
                        this.cbSSLR_YBLX.Text = row["p_health_type"].ToString();//医保类型
                        this.cbSSLR_PROVINCE.Text = row["p_address"].ToString().Split('-')[0];//省
                        this.cbSSLR_CITY.Text = row["p_address"].ToString().Split('-')[1];//市
                        this.cbSSLR_COUNTY.Text = row["p_address"].ToString().Split('-')[2];//县
                        this.tbSSLR_DETAIL_ADDRESS.Text = row["p_address"].ToString().Split('-')[3];//详细地址
                        this.tbSSLR_CTXYY.Text = row["p_dialyse_hospital"].ToString();//常透析医院
                        this.tbSSLR_CTXYYLXR.Text = row["p_dialyse_hospital_contact"].ToString();//常透析医院联系人
                        this.tbSSLR_CTXYYLXRDH.Text = row["p_dialyse_hospital_tel"].ToString();//常透析医院联系人电话
                    }

                    //查询该患者的最近一次的手术记录
                    sql = string.Format("select id,r_date,r_ss_address,r_ss_type,r_ss_method,r_cc_method,r_zd_docotor,r_zs,r_qxhs,r_ss_record,r_is_sszz" +
                    " from t_record where r_patient_ID='{0}' order by id DESC limit 1", this.tbSSLR_ID.Text);
                    dataTable = DBManager.getInstance().find(sql);
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            this.labelSSLR_SSJL_ID.Text = row["id"].ToString();//手术记录id
                            this.dpSSLR_SSRQ.Value = Convert.ToDateTime(row["r_date"].ToString());//手术日期
                            this.cbSSLR_SSDD.Text = row["r_ss_address"].ToString();//手术地点
                            this.cbSSLR_SSLX.Text = row["r_ss_type"].ToString();//手术类型
                            this.cbSSLR_SSFS.Text = row["r_ss_method"].ToString();//手术方式
                            this.cbSSLR_CCFS.Text = row["r_cc_method"].ToString();//穿刺方式
                            this.rtbSSLR_SSJL.Text = row["r_ss_record"].ToString();//手术记录
                            this.tbSSLR_ZDYS.Text = row["r_zd_docotor"].ToString();//主刀医生
                            this.tbSSLR_ZS.Text = row["r_zs"].ToString();//助手
                            this.tbSSLR_QXFS.Text = row["r_qxhs"].ToString();//器械护士
                        }
                    }
                   
                }
                else
                {
                    MessageBox.Show("该身份证不存在");
                }
            }

        }

       //输入数字包括小数点
        private void decimal_KeyPress(object sender, KeyPressEventArgs e)
        {
            //判断按键是不是要输入的类型。
            TextBox textBox1 = (TextBox)sender;

            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 46)

                e.Handled = true;


            //小数点的处理。

            if ((int)e.KeyChar == 46)                           //小数点
            {

                if (textBox1.Text.Length <= 0)

                    e.Handled = true;   //小数点不能在第一位

                else
                {

                    float f;

                    float oldf;

                    bool b1 = false, b2 = false;

                    b1 = float.TryParse(textBox1.Text, out oldf);

                    b2 = float.TryParse(textBox1.Text + e.KeyChar.ToString(), out f);

                    if (b2 == false)
                    {

                        if (b1 == true)

                            e.Handled = true;

                        else

                            e.Handled = false;

                    }

                }

            }
        }

        //新增手术追踪
        private void btnSaveSSLR_SSZZ_Click(object sender, EventArgs e)
        {
            int recordId = -1;//手术记录id
            try
            {
                recordId = Convert.ToInt32(this.labelSSLR_SSJL_ID.Text);
            }
            catch (Exception err)
            {
                MessageBox.Show("请查询手术记录单，再进行跟踪");
                return;
            }

            string sfct = this.cbSSLR_SSZZ_SFTC.Text; //是否畅通
            string ywxlbct = this.cbSSLR_SSZZ_YWXLBCT.Text; //有无血液不畅通
            string ywxm = this.cbSSLR_SSZZ_YWXM.Text; //有无胸闷
            string ywccbwpfgmqk = this.cbSSLR_SSZZ_YWCCBWPFGMQK.Text; //有无穿刺部位皮肤过敏情况：
            string ywbfz = this.cbSSLR_SSZZ_YWBFZ.Text; //有无并发症
            string ywxbjmqz = this.cbSSLR_SSZZ_YWXBJMQZ.Text; //有无胸壁静脉曲张
            string grkzfs = this.cbSSLR_SSZZ_GRKZFS.Text;//SelectedValue.ToString(); //感染控制方式
            string nwzwdlqk = this.cbSSLR_SSZZ_NWZWDLQK.Text;//SelectedValue.ToString(); //内痿自我锻炼情况
            string ccbwpfqk = this.cbSSLR_SSZZ_CCBWPFQK.Text;//SelectedValue.ToString(); //穿刺部位皮肤情况
            string zwcmjtzqk = this.tbSSLR_SSZZ_ZWQMJTZQK.Text; //自我触摸及听诊情况

            if (string.IsNullOrEmpty(sfct)) { MessageBox.Show("【是否畅通】不能为空"); return; }
            if (string.IsNullOrEmpty(ywxlbct)) { MessageBox.Show("【有无血液不畅通】不能为空"); return; }
            if (string.IsNullOrEmpty(ywxm)) { MessageBox.Show("【有无胸闷】不能为空"); return; }
            if (string.IsNullOrEmpty(ywccbwpfgmqk)) { MessageBox.Show("【有无穿刺部位皮肤过敏情况】不能为空"); return; }
            if (string.IsNullOrEmpty(ywbfz)) { MessageBox.Show("【有无并发症】不能为空"); return; }
            if (string.IsNullOrEmpty(ywxbjmqz)) { MessageBox.Show("【有无胸壁静脉曲张】不能为空"); return; }
            if (string.IsNullOrEmpty(grkzfs)) { MessageBox.Show("【感染控制方式】不能为空"); return; }
            if (string.IsNullOrEmpty(nwzwdlqk)) { MessageBox.Show("【内痿自我锻炼情况】不能为空"); return; }
            if (string.IsNullOrEmpty(ccbwpfqk)) { MessageBox.Show("【穿刺部位皮肤情况】不能为空"); return; }
            if (string.IsNullOrEmpty(zwcmjtzqk)) { MessageBox.Show("【自我触摸及听诊情况】不能为空"); return; }

            //判断手术记录是否存在
            string sql = string.Format("select * from t_record where id={0}", recordId);
            dataTable = DBManager.getInstance().find(sql);
            if (dataTable == null && dataTable.Rows.Count == 0)
            {
                MessageBox.Show("手术记录为空，不能进行手术跟踪");
                return;
            }

            //判断手术记录的手术追踪是否存在
            sql = string.Format("select * from t_track where t_record_id={0}", recordId);
            dataTable = DBManager.getInstance().find(sql);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DialogResult dr = MessageBox.Show("该手术已追踪，是否继续追踪?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr != DialogResult.Yes)
                {
                    return;
                }
            }

            //添加手术追踪
            sql = string.Format("insert into t_track(t_record_id,t_sszz_deadline,t_sfrq,t_ccfs,t_ssct,t_ywxlbct,t_ywxm,t_ywccbwphgmqk,t_ywbfz,t_ywxbjmqz," +
                "t_grkzfs,t_nwzwdlqk,t_ccbwpfqk,t_sffz,t_jmyfw,t_sjqbxsjmy,t_xjqbxsjmy,t_xll,t_ypzxsj,t_zwcmzcjtzqk,t_sfys)"+
                " values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}')",
                   recordId,sfct, ywxlbct, ywxm, ywccbwpfgmqk, ywbfz, ywxbjmqz, grkzfs, nwzwdlqk, ccbwpfqk, zwcmjtzqk);
            int flag = DBManager.getInstance().add(sql);
            if (flag > 0)
            {
                //修改手术记录为已追踪
                sql = string.Format("update t_record set r_is_sszz='是' where id={0}", recordId);
                flag = DBManager.getInstance().edit(sql);
                if (flag > 0)
                {
                    MessageBox.Show("保存成功");
                }
                else
                {
                    MessageBox.Show("保存失败");
                }
                //MessageBox.Show("保存成功");
            }
            else
            {
                MessageBox.Show("保存失败");
            }
        }

        //数据查询-->手术记录单
        private void btnSJCX_SSJLD_Click(object sender, EventArgs e)
        {
            initSJCX();
        }

        //数据查询-->手术追踪单
        private void btnSJCX_SSZZCX_Click(object sender, EventArgs e)
        {
            this.dtpSJCX_SSZZ_KSRQ.Value = Convert.ToDateTime(UtilTools.getDayAndTimeMM());
            this.tbSJCX_SSZZ_NAME.Text = "";//SelectedIndex
            this.cbSJCX_SSZZ_CXTJ.SelectedIndex = -1;

            // Header以外所有的单元格的背景色
            this.dgvSJCX_SSZZ.RowsDefaultCellStyle.BackColor = Color.White;

            ////列Header的背景色 CellBorderStyle->Single   EnableHeaderVisualStyles->false
            this.dgvSJCX_SSZZ.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#4f81DD");
            this.dgvSJCX_SSZZ.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvSJCX_SSZZ.ColumnHeadersDefaultCellStyle.Font = new Font("宋体", 12f, FontStyle.Bold);

            //设置选中的颜色
            this.dgvSJCX_SSZZ.DefaultCellStyle.SelectionBackColor = Color.Gray;


            this.btnSJCX_SSZZCX.ForeColor = ColorTranslator.FromHtml("#3399ff");
            this.btnSJCX_SSZZCX.BackColor = Color.White;

            this.btnSJCX_SSJLD.ForeColor = Color.White;
            this.btnSJCX_SSJLD.BackColor = ColorTranslator.FromHtml("#3399ff");

            this.panelSJCX_SSZZD.Visible = true;
            this.panelSJCX_SSJLD.Visible = false;

            string sql = "select 姓名,性别,年龄,医保类型,穿刺方式,手术类型,常透析医院,联系电话,是否手术追踪" +
                        "t.t_sszz_deadline,t.t_sfrq,t.t_ccfs,t.t_ssct,t.t_ywxlbct,t.t_ywxm, " +
                        "t.t_ywccbwphgmqk,t.t_ywbfz,t.t_ywxbjmqz,t.t_grkzfs,t.t_nwzwdlqk,t.t_ccbwpfqk, " +
                        "t.t_sffz,t.t_jmyfw,t.t_sjqbxsjmy,t.t_xjqbxsjmy,t.t_xll,t.t_ypzxsj,t.t_zwcmzcjtzqk, " +
                        "t.t_sfys " +
                    "from t_track t";


            // btnSJCX_SSZZ_FIND_Click(null, null);
            btn_SJCX_SSZZCX_2_Click(null, null);
        }





        //初始化数据查询界面
        private void initSJCX()
        {
            this.tbSJCX_NAME.Text = "";
            this.cbSJCX_XSSSZZ.Checked = false;

            // Header以外所有的单元格的背景色
            this.dgvSJCX_RECORDS.RowsDefaultCellStyle.BackColor = Color.White;

            ////列Header的背景色 CellBorderStyle->Single   EnableHeaderVisualStyles->false
            this.dgvSJCX_RECORDS.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#4f81DD");
            this.dgvSJCX_RECORDS.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvSJCX_RECORDS.ColumnHeadersDefaultCellStyle.Font = new Font("宋体", 12f, FontStyle.Bold);

            //设置选中的颜色
            this.dgvSJCX_RECORDS.DefaultCellStyle.SelectionBackColor = Color.Gray;


            this.btnSJCX_SSJLD.ForeColor = ColorTranslator.FromHtml("#3399ff");
            this.btnSJCX_SSJLD.BackColor = Color.White;

            this.btnSJCX_SSZZCX.ForeColor = Color.White;
            this.btnSJCX_SSZZCX.BackColor = ColorTranslator.FromHtml("#3399ff");

            this.dtpSJCX_KSSJ.Value = Convert.ToDateTime(UtilTools.getDayAndTimeMM());
            this.dtpSJCX_JSSJ.Value = Convert.ToDateTime(UtilTools.getDayAndTimeMM());

            this.panelSJCX_SSJLD.Visible = true;
            this.panelSJCX_SSZZD.Visible = false;

            this.gbSJCX_SSJLD_SSZZ.Visible = false;

            string sql = string.Format("select r.id 'id',p.p_name '姓名',date_format(r.r_date,'%Y年%m月%d日') '手术日期' from t_patient p,t_record r where p.p_ID = r.r_patient_ID");
            selectRecords(sql);

            sql = string.Format("select r_hospital_name,r_room_name from t_room");
            dataTable = DBManager.getInstance().find(sql);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    this.labelSSJLD_TITLE.Text = row["r_hospital_name"].ToString() + row["r_room_name"].ToString();
                }
            }
            // this.labelSSJLD_TITLE.TextAlign = ContentAlignment.TopCenter;
            this.labelSSJLD_TITLE.Location = new Point((this.panelSJCX_SSJL_Head.Width - this.labelSSJLD_TITLE.Width) / 2, this.labelSSJLD_TITLE.Location.Y);
            // this.labelSSJLD_TITLE.
            initRecord();
            
        }

        //初始化手术记录单
        private void initRecord()
        {
            this.cbSJCX_XSSSZZ.Checked = false;

            this.all_name.Text = "";
            this.all_sex.Text = "";
            this.all_age.Text = "";
            this.all_tel.Text = "";
            this.all_ID.Text = "";
            this.all_yblx.Text = "";
            this.all_czdz.Text = "";
            this.all_ctxyy.Text = "";
            this.all_ctxyylxr.Text = "";
            this.all_ctxyylxrdh.Text = "";
            this.all_ssrq.Text = "";
            this.all_ssdd.Text = "";
            this.all_sslx.Text = "";
            this.all_ssfs.Text = "";
            this.all_ccfs.Text = "";
            //this.all_ccfs_1.Text = row["r_cc_method"].ToString();
            this.all_zdys.Text = "";
            this.all_zs.Text = "";
            this.all_qxhs.Text = "";
            this.all_ssjl.Text = "";
            //this.all_name.Text = row["r_is_sszz"].ToString();
            this.all_sszzqx.Text = "";
            this.all_sfrq.Text = "";
            this.all_ccfs_1.Text = "";
            this.all_ssct.Text = "";
            this.all_ywxlbct.Text = "";
            this.all_ywxm.Text = "";
            this.all_ywccbwpfgmqk.Text = "";
            this.all_ywbfz.Text = "";
            this.all_ywxbjmqz.Text = "";
            this.all_grkzfs.Text = "";
            this.all_nwzwdlqk.Text = "";
            this.all_ccbwpfqk.Text = "";
            this.all_sffz.Text = "";
            this.all_grkzfs.Text = "";
            this.all_jmyfw.Text = "";
            this.all_sjqbxsjmy.Text = "";
            this.all_xjqbxsjmy.Text = "";
            this.all_xll.Text = "";
            this.all_ypzxsj.Text = "";
            this.all_zwcmzcjtzqk.Text = "";
            this.all_sfys.Text = "";

            this.labSJCX_TraceTime.Text = "";
            this.labSJCX_TraceValue1.Text = "";
            this.labSJCX_TraceValue2.Text = "";
            this.labSJCX_TraceValue3.Text = "";
            this.labSJCX_TraceValue4.Text = "";
                    
        }

        //设置手术记录列表
        private void selectRecords(string sql)
        {
           
            this.dgvSJCX_RECORDS.DataSource = null;//清空
            this.dgvSJCX_RECORDS.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//选中整行

            DataTable dt = DBManager.getInstance().find(sql);
            this.dgvSJCX_RECORDS.DataSource = dt;

            //第一列隐藏
            this.dgvSJCX_RECORDS.Columns[0].Visible = false;
            // Header以外所有的单元格的背景色
            dgvDictionary.RowsDefaultCellStyle.BackColor = Color.White;

            ////列Header的背景色 CellBorderStyle->Single   EnableHeaderVisualStyles->false
            dgvDictionary.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#4f81DD");
            dgvDictionary.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgvDictionary.DefaultCellStyle.SelectionBackColor = Color.Gray;//设置选中的颜色
        }

        //搜索手术记录单
        private void btnSJCX_FIND_RECORDS_Click(object sender, EventArgs e)
        {
            this.cbSJCX_XSSSZZ.Checked = false;
            string name = this.tbSJCX_NAME.Text;//姓名

            string kssj = this.dtpSJCX_KSSJ.Value.ToString("yyyy-MM") + "-01";//开始时间
            string jssj = this.dtpSJCX_JSSJ.Value.ToString("yyyy-MM") + "-31";//结束时间

            string sqlStr = "select r.id '身份证',p.p_name '姓名',date_format(r.r_date,'%Y年%m月%d日') '手术日期' from t_patient p,t_record r where p.p_ID = r.r_patient_ID " +
                "and r.r_date > '{0}' and r.r_date <= '{1}' ";
            if (!string.IsNullOrEmpty(name))
            {
                sqlStr += "and p.p_name like '%{2}%' ";
            }

            //sqlStr += " GROUP BY p.p_ID";
            string sql = string.Format(sqlStr,kssj,jssj,name);
            selectRecords(sql);
        }


        private void changePage(int showPage) {
            if (0 > showPage || showPage >= ctrs.Count)
            {
                return;
            }
            lab_currentPage.Text = (showPage + 1).ToString();
            for (int i = 0; i < ctrs.Count; i++) 
            {
                if (i == showPage) 
                {
                    ctrs[i].Show();
                } 
                else 
                {
                    ctrs[i].Hide();
                }
            }

        }

        int totalPage = 0;
        int currentPage = 0;
        List<Control> ctrs = new List<Control>();
        //选中手术记录-》弹出手术记录及患者信息
        private void dgvSJCX_RECORDS_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            initRecord();
            int index = this.dgvSJCX_RECORDS.CurrentRow.Index;
            string id = this.dgvSJCX_RECORDS.Rows[index].Cells[0].Value.ToString();
            
            string sql = string.Format(
                "select "+
	                "p.p_name,p.p_sex,p.p_age,p.p_tel,p.p_ID,p.p_health_type,p.p_address, "+
                    "p.p_dialyse_hospital,p.p_dialyse_hospital_contact,p.p_dialyse_hospital_tel, " +
                    "r.r_date,r.r_ss_address,r.r_ss_type,r.r_ss_method,r.r_cc_method,r.r_zd_docotor, " +
                    "r.r_zs,r_qxhs,r.r_ss_record,r.r_is_sszz, r.r_sszz, r.r_zz1, " +
                    "r.r_zz2, r.r_zz3, r.r_zz4, r.r_ssbw " +
                "from t_patient p,t_record r " +
                "where  " +
                    "p.p_ID = r.r_patient_ID " +
                    "and r.id={0}", id
            );
            dataTable = DBManager.getInstance().find(sql);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach(DataRow row in dataTable.Rows)
                {
                    this.all_name.Text = row["p_name"].ToString();
                    this.all_sex.Text = row["p_sex"].ToString();
                    this.all_age.Text = row["p_age"].ToString();
                    this.all_tel.Text = row["p_tel"].ToString();
                    this.all_ID.Text = row["p_ID"].ToString();
                    this.all_yblx.Text = row["p_health_type"].ToString();
                    this.all_czdz.Text = row["p_address"].ToString();
                    this.all_ctxyy.Text = row["p_dialyse_hospital"].ToString();
                    this.all_ctxyylxr.Text = row["p_dialyse_hospital_contact"].ToString();
                    this.all_ctxyylxrdh.Text = row["p_dialyse_hospital_tel"].ToString();
                    this.all_ssrq.Text = Convert.ToDateTime(row["r_date"]).ToString("yyyy年MM月dd日");
                    this.all_ssdd.Text = row["r_ss_address"].ToString();
                    this.all_sslx.Text = row["r_ss_type"].ToString();
                    this.all_ssfs.Text = row["r_ss_method"].ToString();
                    this.all_ccfs.Text = row["r_cc_method"].ToString();
                    //this.all_ccfs_1.Text = row["r_cc_method"].ToString();
                    this.all_zdys.Text = row["r_zd_docotor"].ToString();
                    this.all_zs.Text = row["r_zs"].ToString();
                    this.all_qxhs.Text = row["r_qxhs"].ToString();
                    // this.all_ssjl.Rtf = row["r_ss_record"].ToString();


                    for (int i = 2; i < ctrs.Count; i++) {
                        ctrs[i].Dispose();
                        // ctrs[i].
                    }

                    ctrs = new List<Control>();
                    currentPage = 0;

                    ctrs.Add(panelSSCX_SSJLD); // 第一页
                    ctrs.Add(panelSSJLD2); // 第二页
                    // 后续的分页
                    List<String> records = RichTextBoxHelper.splitRichTextBoxToRtfs(896, 626, row["r_ss_record"].ToString());
                    this.all_ssjl.Rtf = records[0];
                    // MessageBox.Show("可以分成:" + records.Count);

                    for (int i = 1; i < records.Count; i++) {
                        Panel p = new Panel();
                        p.Location = new Point(138, 44);
                        p.Size = new Size(630, 952);
                        p.BorderStyle = BorderStyle.FixedSingle;
                        // p.BackColor = Color.Red;
                        RichTextBox rtb = new RichTextBox();
                        rtb.Dock = DockStyle.Fill;
                        rtb.BorderStyle = BorderStyle.None;
                        rtb.Size = all_ssjl.Size;
                        rtb.Rtf = records[i];
                        rtb.Name = "rtb";
                        
                        p.Controls.Add(rtb);

                        panel51.Controls.Add(p);
                        ctrs.Add(p);
                    }
                    changePage(0);
                    lab_totalPage.Text = ctrs.Count.ToString();

                    currentIndex = 1;


                    // this.richTextBoxPrintCtrl1.Rtf = row["r_ss_record"].ToString();
                    // this.all_ssjl.
                    // all_ssjl.
                    // this.all_ssjl.Text = "test";

                    this.labSJCX_TraceTime.Text = row["r_sszz"].ToString();
                    this.labSJCX_TraceValue1.Text = Convert.ToDateTime(row["r_zz1"]).ToString("yyyy年MM月");
                    this.labSJCX_TraceValue2.Text = Convert.ToDateTime(row["r_zz2"]).ToString("yyyy年MM月");
                    if (this.labSJCX_TraceTime.Text.Equals("每三个月"))
                    {
                        this.labSJCX_Trace3.Show();
                        this.labSJCX_TraceValue3.Show();
                        this.labSJCX_TraceValue4.Show();
                        this.labSJCX_Trace4.Show();
                        this.labSJCX_TraceValue3.Text = Convert.ToDateTime(row["r_zz3"]).ToString("yyyy年MM月");
                        this.labSJCX_TraceValue4.Text = Convert.ToDateTime(row["r_zz4"]).ToString("yyyy年MM月");
                    }
                    else {
                        this.labSJCX_Trace3.Hide();
                        this.labSJCX_TraceValue3.Hide();
                        this.labSJCX_TraceValue4.Hide();
                        this.labSJCX_Trace4.Hide();
                    }
                    this.labSJCX_SSBW.Text = row["r_ssbw"].ToString();
                    
                    // this.all_ssjl.Cont
                }
            }

            // 载入图片
            // grpbxImgs.Controls.Clear(); // 先清空所有的图片
            // panelSJCX_SSJL_SSST.Height = 260; // 用来放图片的panel的高度还原成260，即初始化的高度
            pictureBox8.Image = null;
            pictureBox9.Image = null;
            MySqlParameter[] ps = new MySqlParameter[] {
                new MySqlParameter("@p_r_id", id)
            };
            using (MySqlDataReader reader = SqlHelper4MySql.ExecuteReader(SqlCommandHelpler.T_PICTURE_SELECT_BY_RID, ps))
            {
                /*
                
                int startX = 20;
                int startY = 20;
                Point p = new Point(startX, startY);*/
                int i = 0;
                while (reader.Read()) {

                    long length = reader.GetBytes(0, 0, null, 0, int.MaxValue);
                    byte[] bytes = new byte[length];
                    reader.GetBytes(0, 0, bytes, 0, bytes.Length);

                    if (i == 0)
                    {
                        i++;
                        pictureBox8.Image = ImageHelpler.byteToImage(bytes);
                    }
                    else {
                        pictureBox9.Image = ImageHelpler.byteToImage(bytes);
                    }
                    
                        // pictureBox8
                    /*
                    Console.WriteLine("第" + i++ + "张图片");
                    if (i < 3) {
                        PictureBox picbx = new PictureBox();
                        picbx.Size = new Size(ValueSender.imageWidth, ValueSender.imageHeight);
                        long length = reader.GetBytes(0, 0, null, 0, int.MaxValue);
                        byte[] bytes = new byte[length];
                        reader.GetBytes(0, 0, bytes, 0, bytes.Length);
                        picbx.Image = ImageHelpler.byteToImage(bytes);

                        // int poc = y++ * ValueSender.imageHeight + picPositionY;
                        picbx.Location = p;
                        if ((p.Y + picbx.Height) > grpbxImgs.Height)
                        {
                            panelSJCX_SSJL_SSST.Height = (p.Y + picbx.Height) + startY;
                        }

                        p = picLocation(p);
                        grpbxImgs.Controls.Add(picbx);
                    }
                    */
                    // all_ssjl.draw
                }
            }  

        }

        private Point picLocation(Point p) { 
            //if ()
            // 宽626
            // 高969
            if (p.X > 300)
            {
                // 那么横排只能放一张图片，那么4张图就只能如下排版
                // 图片1
                // 图片2
                // 图片3
                // 图片4
                p.X = 20;
                p.Y = ValueSender.imageHeight + 20 + p.Y;
                
            }
            else
            {
                // 那么横排能放2张图
                // 图片1 图片2
                // 图片3 图片4
                p.X = 310;
               
            }
            return p;


        }



        //是否显示手术追踪
        private void cbSJCX_XSSSZZ_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb =  (CheckBox)sender;
            this.gbSJCX_SSJLD_SSZZ.Visible = false;

            if (this.dgvSJCX_RECORDS.CurrentRow == null)
            {
                MessageBox.Show("你还没有选择一条记录，不能显示手术追踪");
                return;
            }
            
            
            if (cb.Checked)
            {
                int index = this.dgvSJCX_RECORDS.CurrentRow.Index;
                string id = this.dgvSJCX_RECORDS.Rows[index].Cells[0].Value.ToString();

                string sql = string.Format(
                    "select " +
                        "t.t_sszz_deadline,t.t_sfrq,t.t_ccfs,t.t_ssct,t.t_ywxlbct,t.t_ywxm, " +
                        "t.t_ywccbwphgmqk,t.t_ywbfz,t.t_ywxbjmqz,t.t_grkzfs,t.t_nwzwdlqk,t.t_ccbwpfqk, " +
                        "t.t_sffz,t.t_jmyfw,t.t_sjqbxsjmy,t.t_xjqbxsjmy,t.t_xll,t.t_ypzxsj,t.t_zwcmzcjtzqk, " +
                        "t.t_sfys " +
                    "from t_track t " +
                    "where t_record_id={0}", id
                );
                dataTable = DBManager.getInstance().find(sql);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        this.all_sszzqx.Text = row["t_sszz_deadline"].ToString();
                        this.all_sfrq.Text = row["t_sfrq"].ToString();
                        this.all_ccfs_1.Text = row["t_ccfs"].ToString();
                        this.all_ssct.Text = row["t_ssct"].ToString();
                        this.all_ywxlbct.Text = row["t_ywxlbct"].ToString();
                        this.all_ywxm.Text = row["t_ywxm"].ToString();
                        this.all_ywccbwpfgmqk.Text = row["t_ywccbwphgmqk"].ToString();
                        this.all_ywbfz.Text = row["t_ywbfz"].ToString();
                        this.all_ywxbjmqz.Text = row["t_ywxbjmqz"].ToString();
                        this.all_grkzfs.Text = row["t_grkzfs"].ToString();
                        this.all_nwzwdlqk.Text = row["t_nwzwdlqk"].ToString();
                        this.all_ccbwpfqk.Text = row["t_ccbwpfqk"].ToString();
                        this.all_sffz.Text = row["t_sffz"].ToString();
                        this.all_grkzfs.Text = row["t_grkzfs"].ToString();
                        this.all_jmyfw.Text = row["t_jmyfw"].ToString();
                        this.all_sjqbxsjmy.Text = row["t_sjqbxsjmy"].ToString();
                        this.all_xjqbxsjmy.Text = row["t_xjqbxsjmy"].ToString();
                        this.all_xll.Text = row["t_xll"].ToString();
                        this.all_ypzxsj.Text = row["t_ypzxsj"].ToString();
                        this.all_zwcmzcjtzqk.Text = row["t_zwcmzcjtzqk"].ToString();
                        this.all_sfys.Text = row["t_sfys"].ToString();

                    }
                    this.gbSJCX_SSJLD_SSZZ.Visible = true;
                }
                else
                {
                    MessageBox.Show("该记录没有手术追踪");
                }
            }
        }


        
        DataTable dataTableForSSZZ;
        // 查询手术追踪2
        private void btn_SJCX_SSZZCX_2_Click(object sender, EventArgs e)
        {

            this.dgvSJCX_SSZZ.DataSource = null;

            string name = this.tbSJCX_SSZZ_NAME.Text;//姓名
            string sszz = this.cbSJCX_SSZZ_CXTJ.Text;//追踪状态 是 否  全部
            sszz = string.IsNullOrEmpty(sszz) ? "全部" : sszz;

            DateTime date = this.dtpSJCX_SSZZ_KSRQ.Value;
            // 根据日期查询
            DateTime startTime = new DateTime(date.Year, date.Month, 1, 0, 0, 0);
            DateTime endTime;
            if (12 == date.Month)
            {
                endTime = new DateTime(date.Year + 1, 1, 1, 0, 0, 0);
            }
            else
            {
                endTime = new DateTime(date.Year, date.Month + 1, 1, 0, 0, 0);
            }

            // DataTable dataTable;
            if (sszz.Equals("全部"))
            {
                MySqlParameter[] ps = new MySqlParameter[] {
                    new MySqlParameter("@p_name", "%" + name + "%"),
                    new MySqlParameter("@startTime", startTime),
                    new MySqlParameter("@endTime", endTime)
                };
                dataTableForSSZZ = SqlHelper4MySql.getDataTable(SqlCommandHelpler.T_PATIENT_TRACK_RECORD_SELECT_BY_PNAME_ZZRQ, ps);
            }
            else
            {
                MySqlParameter[] ps = new MySqlParameter[] {
                    new MySqlParameter("@p_name", "%" + name + "%"),
                    new MySqlParameter("@t_sfsszz", sszz),
                    new MySqlParameter("@startTime", startTime),
                    new MySqlParameter("@endTime", endTime)
                };
                dataTableForSSZZ = SqlHelper4MySql.getDataTable(SqlCommandHelpler.T_PATIENT_TRACK_RECORD_SELECT_BY_PNAME_FFZZCG_ZZRQ, ps);
            }
            if (dataTableForSSZZ == null || dataTableForSSZZ.Rows.Count == 0)
            {
                MessageBox.Show("当前数据为空");
            }
            else
            {
                this.dgvSJCX_SSZZ.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//选中整行
                this.dgvSJCX_SSZZ.DataSource = null;//清空
                this.dgvSJCX_SSZZ.DataSource = dataTableForSSZZ;

                // Header以外所有的单元格的背景色
                this.dgvSJCX_SSZZ.RowsDefaultCellStyle.BackColor = Color.White;

                ////列Header的背景色 CellBorderStyle->Single   EnableHeaderVisualStyles->false
                this.dgvSJCX_SSZZ.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#4f81DD");
                this.dgvSJCX_SSZZ.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                this.dgvSJCX_SSZZ.DefaultCellStyle.SelectionBackColor = Color.Gray;//设置选中的颜色
            }
        }

        //查询手术追踪
        private void btnSJCX_SSZZ_FIND_Click(object sender, EventArgs e)
        {
            // getDataTable


            
            this.dgvSJCX_SSZZ.DataSource = null;

            string name = this.tbSJCX_SSZZ_NAME.Text;//姓名
            
            string kssj = this.dtpSJCX_SSZZ_KSRQ.Value.ToString("yyyy-MM") + "-01";//开始时间
            string jssj = this.dtpSJCX_SSZZ_JSRQ.Value.ToString("yyyy-MM") + "-31";//结束时间
            string sszz = this.cbSJCX_SSZZ_CXTJ.Text;//追踪状态 未追踪 已追踪  全部

            string sql = "select p.p_name 姓名,p.p_sex 性别,p.p_age 年龄,p.p_health_type 医保类型,r.r_cc_method 穿刺方式,r.r_ss_type 手术类型,"+
                "p.p_dialyse_hospital 常透析医院,p.p_tel 联系电话,r.r_is_sszz 是否手术跟踪" +
                " from t_patient p left join t_record r on r.r_patient_ID=p.p_ID where r_date>'{0}' and r_date<'{1}' ";
            if (!string.IsNullOrEmpty(name))
            {
                sql += "and p.p_name like '%"+name+"%' ";
            }

            if (!string.IsNullOrEmpty(sszz))
            {
                if (sszz.Equals("已追踪"))
                {
                    sql += "and r.r_is_sszz = '是' ";
                }
                else if (sszz.Equals("未追踪"))
                {
                    sql += "and r.r_is_sszz = '否' ";
                }
            }

            dataTable = DBManager.getInstance().find(string.Format(sql, kssj, jssj));

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                MessageBox.Show("当前数据为空");
            }
            else
            {
                this.dgvSJCX_SSZZ.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//选中整行
                this.dgvSJCX_SSZZ.DataSource = null;//清空
                this.dgvSJCX_SSZZ.DataSource = dataTable;

                // Header以外所有的单元格的背景色
                this.dgvSJCX_SSZZ.RowsDefaultCellStyle.BackColor = Color.White;

                ////列Header的背景色 CellBorderStyle->Single   EnableHeaderVisualStyles->false
                this.dgvSJCX_SSZZ.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#4f81DD");
                this.dgvSJCX_SSZZ.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                this.dgvSJCX_SSZZ.DefaultCellStyle.SelectionBackColor = Color.Gray;//设置选中的颜色

            }
        }

        static int TJ_FLAG = 0;//0表示是基本信息统计，1表示是手术统计
        //基本信息统计
        private void btnTJFX_JBXXTJ_Click(object sender, EventArgs e)
        {
            initTjfx();
        }

        //初始化信息统计
        private void initTjfx()
        {
            TJ_FLAG = 0;

            this.btnTJFX_JBXXTJ.ForeColor = ColorTranslator.FromHtml("#3399ff");
            this.btnTJFX_JBXXTJ.BackColor = Color.White;

            this.btnTJFX_SSTJ.ForeColor = Color.White;
            this.btnTJFX_SSTJ.BackColor = ColorTranslator.FromHtml("#3399ff");

            this.btnTJFX_GZLTJ.ForeColor = Color.White;
            this.btnTJFX_GZLTJ.BackColor = ColorTranslator.FromHtml("#3399ff");

            this.panelTJFX_PIE.Visible = true;
            this.panelTJFX_GZLTJ.Visible = false;

            this.dtpTJCX_KSSJ.Value = Convert.ToDateTime(UtilTools.getDayAndTimeMM());
            this.dtpTJCX_JSSJ.Value = Convert.ToDateTime(UtilTools.getDayAndTimeMM());

            JBXXTJ_PIE();
        }

        //基本信息统计饼图
        private void JBXXTJ_PIE()
        {
            tjSex(null,null);
            tjAge(null, null);
            tjZWDL(null,null);
            tjCCBWPFTJ(null,null);
        }

        //统计性别
        private void tjSex(string kssj,string jssj)
        {
            this.chartPie_1.Series[0].Points.Clear();
            List<int> data = new List<int>();
            if(string.IsNullOrEmpty(kssj) &&　string.IsNullOrEmpty(jssj))
            {
                string sql = 
                    "select p.p_sex,count(*) as total "+
                    "from t_patient p,t_record r "+
                    "where p.p_ID=r.r_patient_ID  group by p.p_sex";
                dataTable = DBManager.getInstance().find(sql);
            }else{
                string sql = string.Format(
                     "select p.p_sex,count(*) as total "+
                    "from t_patient p,t_record r "+
                    "where p.p_ID=r.r_patient_ID and r.r_date > '{0}' and r.r_date < '{1}' "+
                    "group by p.p_sex", kssj, jssj);
                dataTable = DBManager.getInstance().find(sql);
            }

            if(dataTable != null && dataTable.Rows.Count > 0)
            {
                List<int> arrays = new List<int>();
                List<string> sexs = new List<string>();
                foreach(DataRow row in dataTable.Rows)
                {
                    int y = Convert.ToInt32(row["total"].ToString());
                    string x = row["p_sex"].ToString();
                   // string legel = row["p_sex"].ToString() + "[" + y + "](" + row["percent"].ToString() + ")";
                    arrays.Add(y);
                    sexs.Add(x);
                }

                this.chartPie_1.Series[0]["PieLabelStyle"] = "Outside";//将文字移到外侧
                this.chartPie_1.Series[0]["PieLineColor"] = "Black";//绘制黑色的连线。
                this.chartPie_1.Series[0].Points.DataBindXY(sexs, arrays);
                //this.chartPie_1.Series[0].Legend =
            }
           
            this.labelTJFX_PIE_1.Text = "男女比例";
        }

        //统计年龄
        private void tjAge(string kssj, string jssj)
        {
            this.chartPie_2.Series[0].Points.Clear();
            List<int> data = new List<int>();
            if (string.IsNullOrEmpty(kssj) && string.IsNullOrEmpty(jssj))
            {
                string sql =
                    "select p_age,count(*) as total,"+
	                    " case"+
		                    " when p_age between 1 and 18 then '[<=18]'"+
		                    " when p_age between 10 and 30 then '[18-30]'"+
		                    " when p_age between 30 and 40 then '[30-40]'"+
		                    " when p_age between 40 and 50 then '[40-50]'"+
		                    " when p_age between 50 and 60 then '[50-60]'"+
		                    " when p_age between 60 and 70 then '[60-70]'"+
		                    " when p_age between 70 and 80 then '[70-80]'"+
		                    " when p_age > 80 then '[>80]'"+
	                    " end as age_temp"+
                    " from t_patient p,t_record r"+
                    " where"+
                    " p.p_ID = r.r_patient_ID"+
                    " group by age_temp";
                dataTable = DBManager.getInstance().find(sql);
            }
            else
            {
                string sql =
                    "select p_age,count(*) as total,"+
                        " case" +
                            " when p_age between 1 and 18 then '[<=18]'" +
                            " when p_age between 10 and 30 then '[18-30]'" +
                            " when p_age between 30 and 40 then '[30-40]'" +
                            " when p_age between 40 and 50 then '[40-50]'" +
                            " when p_age between 50 and 60 then '[50-60]'" +
                            " when p_age between 60 and 70 then '[60-70]'" +
                            " when p_age between 70 and 80 then '[70-80]'" +
                            " when p_age > 80 then '[>80]'" +
                        " end as age_temp" +
                    " from t_patient p,t_record r" +
                    " where" +
                    " p.p_ID = r.r_patient_ID" +
                    " and r.r_date > '{0}' and r.r_date < '{1}'"+
                    " group by age_temp";
                sql = string.Format(sql, kssj, jssj);
                dataTable = DBManager.getInstance().find(sql);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                List<int> arrays = new List<int>();
                List<string> ages = new List<string>();
                foreach (DataRow row in dataTable.Rows)
                {
                    int y = Convert.ToInt32(row["total"].ToString());
                    string x = row["age_temp"].ToString();
                    //string legel = row["p_age"].ToString() + "[" + y + "](" + row["percent"].ToString() + ")";
                    arrays.Add(Convert.ToInt32(row["total"].ToString()));
                    ages.Add(x);
                }

                this.chartPie_2.Series[0]["PieLabelStyle"] = "Outside";//将文字移到外侧
                this.chartPie_2.Series[0]["PieLineColor"] = "Black";//绘制黑色的连线。
                this.chartPie_2.Series[0].Points.DataBindXY(ages, arrays);
            }

            this.labelTJFX_PIE_2.Text = "年龄比例";
        }

        //统计自我锻炼情况统计
        private void tjZWDL(string kssj, string jssj)
        {
            this.chartPie_3.Series[0].Points.Clear();
            List<int> data = new List<int>();
            if (string.IsNullOrEmpty(kssj) && string.IsNullOrEmpty(jssj))
            {
                string sql =
                    "select t.t_nwzwdlqk,count(*) as total "+
                    "from t_track t,t_record r  "+
                    "where t.t_record_id = r.id and t.t_sfsszzcg='是' " +
                    "group by t.t_nwzwdlqk";
                dataTable = DBManager.getInstance().find(sql);
            }
            else
            {
                string sql = string.Format(
                     "select t.t_nwzwdlqk,count(*) as total "+
                    "from t_track t,t_record r  "+
                    "where t.t_record_id = r.id and t.t_sfsszzcg='是' " +
                    "and r.r_date > '{0}' and r.r_date < '{1}' "+
                    "group by t.t_nwzwdlqk", kssj, jssj);
                dataTable = DBManager.getInstance().find(sql);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                List<int> arrays = new List<int>();
                List<string> values = new List<string>();
                foreach (DataRow row in dataTable.Rows)
                {
                    int y = Convert.ToInt32(row["total"].ToString());
                    string x = row["t_nwzwdlqk"].ToString();
                    //string legel = row["t_nwzwdlqk"].ToString() + "[" + y + "](" + row["percent"].ToString() + "%)";
                    arrays.Add(y);
                    values.Add(x);
                }

                this.chartPie_3.Series[0]["PieLabelStyle"] = "Outside";//将文字移到外侧
                this.chartPie_3.Series[0]["PieLineColor"] = "Black";//绘制黑色的连线。
                this.chartPie_3.Series[0].Points.DataBindXY(values, arrays);
                
            }

            this.labelTJFX_PIE_3.Text = "自我锻炼情况统计";
        }

        //穿刺部位皮肤情况统计
        private void tjCCBWPFTJ(string kssj, string jssj)
        {
            this.chartPie_4.Series[0].Points.Clear();
            List<int> data = new List<int>();
            if (string.IsNullOrEmpty(kssj) && string.IsNullOrEmpty(jssj))
            {
                string sql =
                    "select t.t_ccbwpfqk,count(*) as total " +
                    "from t_track t,t_record r  "+
                    "where t.t_record_id = r.id  and t.t_sfsszzcg='是' " +
                    "group by t.t_ccbwpfqk";
                dataTable = DBManager.getInstance().find(sql);
            }
            else
            {
                string sql = string.Format(
                     "select t.t_ccbwpfqk,count(*) as total " +
                    "from t_track t,t_record r  "+
                    "where t.t_record_id = r.id "+
                    "and r.r_date > '{0}' and r.r_date < '{1}'  and t.t_sfsszzcg='是' " +
                    "group by t.t_ccbwpfqk", kssj, jssj);
                dataTable = DBManager.getInstance().find(sql);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                List<int> arrays = new List<int>();
                List<string> values = new List<string>();
                foreach (DataRow row in dataTable.Rows)
                {
                    int y = Convert.ToInt32(row["total"].ToString());
                    string x = row["t_ccbwpfqk"].ToString();
                    //string legel = row["t_nwzwdlqk"].ToString() + "[" + y + "](" + row["percent"].ToString() + "%)";
                    arrays.Add(y);
                    values.Add(x);
                }

                this.chartPie_4.Series[0]["PieLabelStyle"] = "Outside";//将文字移到外侧
                this.chartPie_4.Series[0]["PieLineColor"] = "Black";//绘制黑色的连线。
                this.chartPie_4.Series[0].Points.DataBindXY(values, arrays);
                
            }

            this.labelTJFX_PIE_4.Text = "穿刺部位皮肤情况统计";
        }

        //根据时间查询统计
        private void btnCJCX_time_Click(object sender, EventArgs e)
        {
            string kssj = this.dtpTJCX_KSSJ.Value.ToString("yyyy-MM")+"-01";
            string jssj = this.dtpTJCX_JSSJ.Value.ToString("yyyy-MM")+"-31";

            //基本信息查询
            if (TJ_FLAG == 0)
            {
                //MessageBox.Show("基本信息查询");
                tjSex(kssj, jssj);
                tjAge(kssj, jssj);
                tjZWDL(kssj, jssj);
                tjCCBWPFTJ(kssj, jssj);
            }
            else
            {
                //TJ_FLAG = 1;
                ccfstj(kssj, jssj);
                sslxtj(kssj, jssj);
                ssfstj(kssj, jssj);
                ctxyytj(kssj, jssj);
            }
            
        }
        
        //手术统计
        private void btnTJFX_SSTJ_Click(object sender, EventArgs e)
        {
            this.btnTJFX_SSTJ.ForeColor = ColorTranslator.FromHtml("#3399ff");
            this.btnTJFX_SSTJ.BackColor = Color.White;

            this.btnTJFX_JBXXTJ.ForeColor = Color.White;
            this.btnTJFX_JBXXTJ.BackColor = ColorTranslator.FromHtml("#3399ff");

            this.btnTJFX_GZLTJ.ForeColor = Color.White;
            this.btnTJFX_GZLTJ.BackColor = ColorTranslator.FromHtml("#3399ff");

            this.panelTJFX_PIE.Visible = true;
            this.panelTJFX_GZLTJ.Visible = false;

            this.dtpTJCX_KSSJ.Value = Convert.ToDateTime(UtilTools.getDayAndTimeMM());
            this.dtpTJCX_JSSJ.Value = Convert.ToDateTime(UtilTools.getDayAndTimeMM());

            initTjss();
        }

        //工作量统计
        private void btnTJFX_GZLTJ_Click(object sender, EventArgs e)
        {
            // Header以外所有的单元格的背景色
            this.dgvTJCX_GZL.RowsDefaultCellStyle.BackColor = Color.White;

            ////列Header的背景色 CellBorderStyle->Single   EnableHeaderVisualStyles->false
            this.dgvTJCX_GZL.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#4f81DD");
            this.dgvTJCX_GZL.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvTJCX_GZL.ColumnHeadersDefaultCellStyle.Font = new Font("宋体", 12f, FontStyle.Bold);

            //设置选中的颜色
            this.dgvTJCX_GZL.DefaultCellStyle.SelectionBackColor = Color.Gray;

            this.btnTJFX_GZLTJ.ForeColor = ColorTranslator.FromHtml("#3399ff");
            this.btnTJFX_GZLTJ.BackColor = Color.White;

            this.btnTJFX_JBXXTJ.ForeColor = Color.White;
            this.btnTJFX_JBXXTJ.BackColor = ColorTranslator.FromHtml("#3399ff");

            this.btnTJFX_SSTJ.ForeColor = Color.White;
            this.btnTJFX_SSTJ.BackColor = ColorTranslator.FromHtml("#3399ff");

            this.panelTJFX_PIE.Visible = false;
            this.panelTJFX_GZLTJ.Visible = true;

            this.dtpTJFX_GZLGL_KSSJ.Value = Convert.ToDateTime(UtilTools.getDayAndTimeMM());
            this.dtpTJFX_GZLGL_JSSJ.Value = Convert.ToDateTime(UtilTools.getDayAndTimeMM());


            //工作量统计
           // initGJLTJ();
        }

        //手术统计
        private void initTjss()
        {
            TJ_FLAG = 1;
            ccfstj(null, null);
            sslxtj(null, null);
            ssfstj(null, null);
            ctxyytj(null, null);
        }

        //穿刺方式统计
        private void ccfstj(string kssj, string jssj)
        {
            this.chartPie_1.Series[0].Points.Clear();
            List<int> data = new List<int>();
            if (string.IsNullOrEmpty(kssj) && string.IsNullOrEmpty(jssj))
            {
                /*
                string sql =
                    "select t.t_ccfs,count(*) as total " +
                    "from t_track t,t_record r  " +
                    "where t.t_record_id = r.id " +
                    "group by t.t_ccfs";*/
                string sql =
                    "SELECT r_cc_method, COUNT(*) AS total FROM t_record GROUP BY r_cc_method";
                dataTable = DBManager.getInstance().find(sql);
            }
            else
            {
                string sql = string.Format(
                    "SELECT r_cc_method, COUNT(*) AS total FROM t_record " +
                    "WHERE r_date > '{0}' and r_date < '{1}' " +
                    "GROUP BY r_cc_method ", kssj, jssj);
                // string sql = string.Format(
                     // "select t.t_ccfs,count(*) as total " +
                    // "from t_track t,t_record r  " +
                    // "where t.t_record_id = r.id " +
                    // "and r.r_date > '{0}' and r.r_date < '{1}' " +
                    // "group by t.t_ccfs", kssj, jssj);
                dataTable = DBManager.getInstance().find(sql);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                List<int> arrays = new List<int>();
                List<string> values = new List<string>();
                foreach (DataRow row in dataTable.Rows)
                {
                    int y = Convert.ToInt32(row["total"].ToString());
                    string x = row["r_cc_method"].ToString();
                    //string legel = row["t_nwzwdlqk"].ToString() + "[" + y + "](" + row["percent"].ToString() + "%)";
                    arrays.Add(y);
                    values.Add(x);
                }

                this.chartPie_1.Series[0]["PieLabelStyle"] = "Outside";//将文字移到外侧
                this.chartPie_1.Series[0]["PieLineColor"] = "Black";//绘制黑色的连线。
                this.chartPie_1.Series[0].Points.DataBindXY(values, arrays);

            }

            this.labelTJFX_PIE_1.Text = "穿刺方式统计";
        }

        //手术类型统计
        private void sslxtj(string kssj, string jssj)
        {
            this.chartPie_2.Series[0].Points.Clear();
            List<int> data = new List<int>();
            if (string.IsNullOrEmpty(kssj) && string.IsNullOrEmpty(jssj))
            {
                string sql =
                    "select r.r_ss_type,count(*) as total " +
                    "from t_track t,t_record r  " +
                    "where t.t_record_id = r.id " +
                    "group by r.r_ss_type";
                dataTable = DBManager.getInstance().find(sql);
            }
            else
            {
                string sql = string.Format(
                     "select r.r_ss_type,count(*) as total " +
                    "from t_track t,t_record r  " +
                    "where t.t_record_id = r.id " +
                    "and r.r_date > '{0}' and r.r_date < '{1}' " +
                    "group by r.r_ss_type", kssj, jssj);
                dataTable = DBManager.getInstance().find(sql);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                List<int> arrays = new List<int>();
                List<string> values = new List<string>();
                foreach (DataRow row in dataTable.Rows)
                {
                    int y = Convert.ToInt32(row["total"].ToString());
                    string x = row["r_ss_type"].ToString();
                    //string legel = row["t_nwzwdlqk"].ToString() + "[" + y + "](" + row["percent"].ToString() + "%)";
                    arrays.Add(y);
                    values.Add(x);
                }

                this.chartPie_2.Series[0]["PieLabelStyle"] = "Outside";//将文字移到外侧
                this.chartPie_2.Series[0]["PieLineColor"] = "Black";//绘制黑色的连线。
                this.chartPie_2.Series[0].Points.DataBindXY(values, arrays);

            }

            this.labelTJFX_PIE_2.Text = "手术类型统计";
        }

        //手术方式统计
        private void ssfstj(string kssj, string jssj)
        {
            this.chartPie_3.Series[0].Points.Clear();
            List<int> data = new List<int>();
            if (string.IsNullOrEmpty(kssj) && string.IsNullOrEmpty(jssj))
            {
                string sql =
                    "select r.r_ss_method,count(*) as total " +
                    "from t_track t,t_record r  " +
                    "where t.t_record_id = r.id " +
                    "group by r.r_ss_method";
                dataTable = DBManager.getInstance().find(sql);
            }
            else
            {
                string sql = string.Format(
                     "select r.r_ss_method,count(*) as total " +
                    "from t_track t,t_record r  " +
                    "where t.t_record_id = r.id " +
                    "and r.r_date > '{0}' and r.r_date < '{1}' " +
                    "group by r.r_ss_method", kssj, jssj);
                dataTable = DBManager.getInstance().find(sql);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                List<int> arrays = new List<int>();
                List<string> values = new List<string>();
                foreach (DataRow row in dataTable.Rows)
                {
                    int y = Convert.ToInt32(row["total"].ToString());
                    string x = row["r_ss_method"].ToString();
                    //string legel = row["t_nwzwdlqk"].ToString() + "[" + y + "](" + row["percent"].ToString() + "%)";
                    arrays.Add(y);
                    values.Add(x);
                }

                this.chartPie_3.Series[0]["PieLabelStyle"] = "Outside";//将文字移到外侧
                this.chartPie_3.Series[0]["PieLineColor"] = "Black";//绘制黑色的连线。
                this.chartPie_3.Series[0].Points.DataBindXY(values, arrays);

            }

            this.labelTJFX_PIE_3.Text = "手术方式统计";
        }

        //常透析医院统计
        private void ctxyytj(string kssj, string jssj)
        {
            this.chartPie_4.Series[0].Points.Clear();
            List<int> data = new List<int>();
            if (string.IsNullOrEmpty(kssj) && string.IsNullOrEmpty(jssj))
            {
                string sql =
                    "select p.p_dialyse_hospital,count(*) as total " +
                     "from t_patient p,t_record r  " +
                    "where p.p_ID = r.r_patient_ID " +
                    "group by  p.p_dialyse_hospital";
                dataTable = DBManager.getInstance().find(sql);
            }
            else
            {
                string sql = string.Format(
                     "select p.p_dialyse_hospital,count(*) as total " +
                    "from t_patient p,t_record r  " +
                    "where p.p_ID = r.r_patient_ID " +
                    "and r.r_date > '{0}' and r.r_date < '{1}' " +
                    "group by  p.p_dialyse_hospital", kssj, jssj);
                dataTable = DBManager.getInstance().find(sql);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                List<int> arrays = new List<int>();
                List<string> values = new List<string>();
                foreach (DataRow row in dataTable.Rows)
                {
                    int y = Convert.ToInt32(row["total"].ToString());
                    string x = row["p_dialyse_hospital"].ToString();
                    //string legel = row["t_nwzwdlqk"].ToString() + "[" + y + "](" + row["percent"].ToString() + "%)";
                    arrays.Add(y);
                    values.Add(x);
                }

                this.chartPie_4.Series[0]["PieLabelStyle"] = "Outside";//将文字移到外侧
                this.chartPie_4.Series[0]["PieLineColor"] = "Black";//绘制黑色的连线。
                this.chartPie_4.Series[0].Points.DataBindXY(values, arrays);

            }

            this.labelTJFX_PIE_4.Text = "常透析医院统计";
        }

        //工作量统计
        private void btnTJFX_GZL_Click(object sender, EventArgs e)
        {
            initTJFX_XXTJ();
        }

        private DataTable dataTableForTJFX_XXTJ;
        //详细统计
        private void initTJFX_XXTJ()
        {
            string kssj = this.dtpTJFX_GZLGL_KSSJ.Value.ToString("yyyy-MM") + "-01";
            string jssj = this.dtpTJFX_GZLGL_JSSJ.Value.ToString("yyyy-MM") + "-31";

            DataTable dt1 = null, dt2 = null, dt3 = null;
            //详细统计
            if (this.tbTJCX_XXTJ.Checked)
            {
             
                string sql = string.Format("select " +
                           " r.r_zd_docotor as '主刀医生'," +
                           " r.r_zs as '助手'," +
                           " r.r_qxhs as '器械护士'," +
                           " p.p_name as '患者姓名'," +
                          "  p.p_sex as '患者性别'," +
                           " r.r_ss_type as '手术类型'," +
                          "  r.r_ss_method as '手术方式'," +
                          "  r.r_cc_method as '穿刺方式'" +
                          "  from " +
                          "  t_record r left join t_patient p on r.r_patient_ID = p.p_ID" +
                          "  where r.r_date > '{0}' and  r.r_date < '{1}'", kssj, jssj);

                dt1 = DBManager.getInstance().find(sql);
            }

            //统计模式
            if (this.tbTJCX_TJMS.Checked)
            {
                dt1 = this.findZDYS_TJ(kssj, jssj);
                dt2 = this.findZS_TJ(kssj, jssj);
                dt3 = this.findQXHS_TJ(kssj, jssj);
                dt1.Merge(dt2, false, MissingSchemaAction.AddWithKey);
                dt1.Merge(dt3, false, MissingSchemaAction.AddWithKey);
            }
           

            this.dgvTJCX_GZL.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//选中整行
            this.dgvTJCX_GZL.DataSource = null;//清空
            this.dgvTJCX_GZL.DataSource = dt1;
            dataTableForTJFX_XXTJ = dt1;

            // Header以外所有的单元格的背景色
            this.dgvTJCX_GZL.RowsDefaultCellStyle.BackColor = Color.White;

            ////列Header的背景色 CellBorderStyle->Single   EnableHeaderVisualStyles->false
            this.dgvTJCX_GZL.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#4f81DD");
            this.dgvTJCX_GZL.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            this.dgvTJCX_GZL.DefaultCellStyle.SelectionBackColor = Color.Gray;//设置选中的颜色
        }

        //统计模式-》查询主刀医生
        private DataTable findZDYS_TJ(string kssj, string jssj)
        {
            string sql = string.Format("select " +
                       " r.r_zd_docotor as '医护人员姓名'," +
                       " '主刀医生' as '工作性质'," +
                       " count(*) as '手术台次' " +
                      "  from " +
                      "  t_record r left join t_patient p on r.r_patient_ID = p.p_ID" +
                      "  where r.r_date > '{0}' and  r.r_date < '{1}'" +
                      " group by r.r_zd_docotor", kssj, jssj);

            dataTable = DBManager.getInstance().find(sql);
            return dataTable;
        }

        //统计模式-》查询助手
        private DataTable findZS_TJ(string kssj, string jssj)
        {
            string sql = string.Format("select " +
                       " r.r_zs as '医护人员姓名'," +
                       " '助手' as '工作性质'," +
                       " count(*) as '手术台次' " +
                      "  from " +
                      "  t_record r left join t_patient p on r.r_patient_ID = p.p_ID" +
                      "  where r.r_date > '{0}' and  r.r_date < '{1}'" +
                      " group by r.r_zs", kssj, jssj);


            dataTable = DBManager.getInstance().find(sql);
            return dataTable;
        }

        //统计模式-》器械护士
        private DataTable findQXHS_TJ(string kssj, string jssj)
        {
            string sql = string.Format("select " +
                       " r.r_qxhs as '医护人员姓名'," +
                       " '器械护士' as '工作性质'," +
                        " count(*) as '手术台次' " +
                      "  from " +
                      "  t_record r left join t_patient p on r.r_patient_ID = p.p_ID" +
                      "  where r.r_date > '{0}' and  r.r_date < '{1}'"+
                      " group by r.r_qxhs", kssj, jssj);

            dataTable = DBManager.getInstance().find(sql);
            return dataTable;
        }

        //打印手术记录单
        private void btnPrintSSCX_SSJL_Click(object sender, EventArgs e)
        {
            this.printDialogSSCX_SSJL.Document = this.printDucumentSSCX_SSJL;
            if (this.printDialogSSCX_SSJL.ShowDialog() == DialogResult.OK)
            {
                this.printDucumentSSCX_SSJL.Print();
            }
        }


        struct MY_PRINT_PAGE {
            public Point points;
            public Bitmap bitmaps;
        }
        // private MY_PRINT_PAGE[] printPages;
        private List<List<Bitmap>> printPages = new List<List<Bitmap>>();

        int currentPageIndex = 0;
        // 打印前
        private void printDucumentSSCX_SSJL_BeginPrint(object sender, PrintEventArgs e)
        {
            printPages = new List<List<Bitmap>>();
            currentPageIndex = 0;
            Console.WriteLine("BeginPrint");

            int pageHeight = 969;
            int top = 0;
            List<Bitmap> page = new List<Bitmap>();

            // 高可用的：969
            // 宽可用的：627


            // 第一页

            // 头部 高度70   共70
            Bitmap head = new Bitmap(this.panelSJCX_SSJL_Head.Width, this.panelSJCX_SSJL_Head.Height);

            this.panelSJCX_SSJL_Head.DrawToBitmap(head, new Rectangle(0, 0, head.Width, head.Height));
            // head.
            // this.panelSJCX_SSJL_Head.DrawToBitmap(

            // e.Graphics.DrawImage(head, left, top, head.Width, head.Height);
            // top += head.Height;
            top += head.Height;
            page.Add(head);
            Console.WriteLine("head之后当前top值:" + top);

            // 基本信息 高度260  共330
            Bitmap baseInfo = new Bitmap(this.panelSJCX_SSJL_BaseInfo.Width, this.panelSJCX_SSJL_BaseInfo.Height);
            this.panelSJCX_SSJL_BaseInfo.DrawToBitmap(baseInfo, new Rectangle(0, 0, baseInfo.Width, baseInfo.Height));
            // e.Graphics.DrawImage(baseInfo, left, top, baseInfo.Width, baseInfo.Height);
            // top += baseInfo.Height;
            top += baseInfo.Height;
            page.Add(baseInfo);
            Console.WriteLine("baseInfo之后当前top值:" + top);

            // 手术部位头部 高度40 共380
            /*
            Bitmap SSBW_NAME = new Bitmap(this.panelSJCX_SSZZCX_SSBW_NAME.Width, this.panelSJCX_SSZZCX_SSBW_NAME.Height);
            this.panelSJCX_SSZZCX_SSBW_NAME.DrawToBitmap(SSBW_NAME, new Rectangle(0, 0, SSBW_NAME.Width, SSBW_NAME.Height));
            // e.Graphics.DrawImage(baseInfo, left, top, baseInfo.Width, baseInfo.Height);
            // top += baseInfo.Height;
            top += SSBW_NAME.Height;
            page.Add(SSBW_NAME);
            Console.WriteLine("SSBW_NAME之后当前top值:" + top);
            */

            // 手术部位图片 高度290   共670
            Bitmap SSJL_SSST = new Bitmap(this.panelSJCX_SSJL_SSST.Width, this.panelSJCX_SSJL_SSST.Height);
            this.panelSJCX_SSJL_SSST.DrawToBitmap(SSJL_SSST, new Rectangle(0, 0, SSJL_SSST.Width, SSJL_SSST.Height));
            // e.Graphics.DrawImage(baseInfo, left, top, baseInfo.Width, baseInfo.Height);
            // top += baseInfo.Height;
            top += SSJL_SSST.Height;
            page.Add(SSJL_SSST);
            Console.WriteLine("SSJL_SSST之后当前top值:" + top);

            // 主刀医生等信息  高30
            Bitmap zdys = new Bitmap(this.panelSJCX_ZDYS.Width, this.panelSJCX_ZDYS.Height);
            this.panelSJCX_ZDYS.DrawToBitmap(zdys, new Rectangle(0, 0, zdys.Width, zdys.Height));
            // e.Graphics.DrawImage(baseInfo, left, top, baseInfo.Width, baseInfo.Height);
            // top += baseInfo.Height;
            // top += zdys.Height;
            page.Add(zdys);
            printPages.Add(page);


            // 第二页
            page = new List<Bitmap>();

            // 手术记录名称 高度30 共700
            Bitmap SSJL_NAME = new Bitmap(this.panelSJCX_SSJL_NAME.Width, this.panelSJCX_SSJL_NAME.Height);
            this.panelSJCX_SSJL_NAME.DrawToBitmap(SSJL_NAME, new Rectangle(0, 0, SSJL_NAME.Width, SSJL_NAME.Height));
            // e.Graphics.DrawImage(baseInfo, left, top, baseInfo.Width, baseInfo.Height);
            // top += baseInfo.Height;
            top += SSJL_NAME.Height;
            page.Add(SSJL_NAME);
            Console.WriteLine("SSJL_NAME之后当前top值:" + top);
            Bitmap ssjl = CutImage.RtbToBitmap(this.all_ssjl);
            page.Add(ssjl);
            page.Add(zdys);
            printPages.Add(page);

            // 后续的页
            for (int i = 2; i < ctrs.Count; i++) 
            {
                page = new List<Bitmap>();
                object o = ctrs[i].Controls.GetEnumerator().Current;
                Control[] ctrs2 = ctrs[i].Controls.Find("rtb", false);
                RichTextBox rtb = (RichTextBox)ctrs2[0];
                Bitmap rtbPic = CutImage.RtbToBitmap(rtb);
                page.Add(rtbPic);
                page.Add(zdys);
                printPages.Add(page);
            }

            /*

            // cutImage
            // 手术记录  超过359的情况，和没有超过359的情况
            
            Bitmap ssjl = CutImage.RtbToBitmap(this.all_ssjl);
            // e.Graphics.DrawImage(ssjl, left, top, ssjl.Width, ssjl.Height);
            // 969
            if (ssjl.Height > 939)
            {
                page = new List<Bitmap>();
                // 需要分页处理
                Bitmap cut1 = ImageHelpler.cutImage(ssjl, 0, 0, ssjl.Width, 939);
                page.Add(cut1);
                top += cut1.Height;
                page.Add(zdys);
                printPages.Add(page);

                int nextY = cut1.Height;
                int leftHeiht = ssjl.Height - cut1.Height;
                int blockLength = 930;
                int pageNumber = (leftHeiht / blockLength) + 1;
                for (int i = 0; i < pageNumber; i++) {
                    page = new List<Bitmap>();
                    if (leftHeiht > blockLength)
                    {
                        Bitmap bitmapCut = ImageHelpler.cutImage(ssjl, 0, nextY, ssjl.Width, blockLength);
                        nextY += blockLength;
                        leftHeiht -= blockLength;
                        page.Add(bitmapCut);
                        page.Add(zdys);
                        printPages.Add(page);
                    }
                    else {
                        Bitmap bitmapCut = ImageHelpler.cutImage(ssjl, 0, nextY, ssjl.Width, leftHeiht);
                        page.Add(bitmapCut);
                    }
                    
                }
            }
            else {
                page = new List<Bitmap>();
                
                // 不需要分页处理
                top += ssjl.Height;
                page.Add(ssjl);
            }

            page.Add(zdys);
            printPages.Add(page);
            */
        }

        
        private void printDucumentSSCX_SSJL_PrintPage(object sender, PrintPageEventArgs e)
        {
            Console.WriteLine("PrintPage, currentPageIndex:" + currentPageIndex);
            //打印内容 为 局部的 this.panel1
            int left = e.MarginBounds.Left;
            int top = e.MarginBounds.Top;
            int right = e.MarginBounds.Right;
            int bottom = e.MarginBounds.Bottom;
            // Console.Write("全部范围：left" + e.PageBounds.Left + ", top" + e.PageBounds.Top + ",Right:" + e.PageBounds.Right + ",Bottom" + e.PageBounds.Bottom);
            // Console.WriteLine("A4值范围: left:" + left + ", top:" + top + ", right:" + right + ",bottom" + bottom);

            // 设置图片插入高清模式
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // e.Graphics.draw
           

            List<Bitmap> page = printPages[currentPageIndex++];
            for (int i = 0; i < page.Count; i++) {
                Console.WriteLine("第" + (i + 1) + "张图片");
                Bitmap b = page[i];
                e.Graphics.DrawImage(b, left + 10, top, b.Width, b.Height);
                // e.Graphics.DrawImageUnscaledAndClipped(b, new Rectangle(left + 10, top, b.Width, b.Height));
                top += b.Height;
            }
            if (1 == currentPageIndex)
            {
                // 横线
                e.Graphics.DrawLine(new Pen(Color.Black), new Point(100, 155), new Point(727, 155));
                // 竖线
                e.Graphics.DrawLine(new Pen(Color.Black), new Point(155, 155), new Point(155, 1029));
                // 横线
                e.Graphics.DrawLine(new Pen(Color.Black), new Point(100, 430), new Point(727, 430));
            }
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(100, 100), new Point(727, 100));
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(100, 1029), new Point(727, 1029));

            e.Graphics.DrawLine(new Pen(Color.Black), new Point(100, 100), new Point(100, 1029));
            e.Graphics.DrawLine(new Pen(Color.Black), new Point(727, 100), new Point(727, 1029));
            if ((printPages.Count) > currentPageIndex)
            {
                e.HasMorePages = true;
            }
            else {
                e.HasMorePages = false;
            }
           

            
            // e.Graphics.DrawImage(_NewBitmap, left, top, _NewBitmap.Width, _NewBitmap.Height);

            
            // Bitmap ssjl = new Bitmap(this.all_ssjl.Width, this.all_ssjl.Height);
            // this.all_ssjl.DrawToBitmap(ssjl, new Rectangle(0, 0, ssjl.Width, ssjl.Height));
            // string[] lines = all_ssjl.Lines;
            // all_ssjl.Lines[0].
            // for (int j = 0; j < lines.Length; j++) {
                // e.Graphics.DrawString(lines[j], Font.Bold, new Brush(Brush, 100, 500);
            // }
            
            // e.Graphics.DrawImage(ssjl, left, top, ssjl.Width, ssjl.Height);
            // top += baseInfo.Height;

            // checkPrint = richTextBoxPrintCtrl1.Print(checkPrint, richTextBoxPrintCtrl1.TextLength, e);
            /*
            if (checkPrint < richTextBoxPrintCtrl1.TextLength)
                e.HasMorePages = true;
            else
                e.HasMorePages = false;
            */
            

            /*Bitmap _NewBitmap = new Bitmap(this.panelSSCX_SSJLD.Width, this.panelSSCX_SSJLD.Height);//
            // 把panel画上去
            this.panelSSCX_SSJLD.DrawToBitmap(_NewBitmap, new Rectangle(0, 0, _NewBitmap.Width, _NewBitmap.Height));*/


            
            
        }

        //打印手术追踪查询
        private void printDocumentSJCX_SSZZ_PrintPage(object sender, PrintPageEventArgs e)
        {
            //打印内容 为 局部的 this.panel1
            Bitmap _NewBitmap = new Bitmap(this.panelSJCX_SSZZ.Width, this.panelSJCX_SSZZ.Height);
            this.panelSJCX_SSZZ.DrawToBitmap(_NewBitmap, new Rectangle(0, 0, _NewBitmap.Width, _NewBitmap.Height));
            e.Graphics.DrawImage(_NewBitmap, 0, 0, _NewBitmap.Width, _NewBitmap.Height);
        }

        

        //手术追踪打印
        private void btnSSCX_SSZZDY_Click(object sender, EventArgs e)
        {
            
            this.printDialogSSCX_SSJL.Document = this.printDucumentSSCX_SSJL;
            if (this.printDialogSSCX_SSJL.ShowDialog() == DialogResult.OK)
            {
                this.printDocumentSJCX_SSZZ.Print();
            }
        }
        // 手术追踪导出pdf
        private void btnSJCX_SSZZ_Out_Click(object sender, EventArgs e)
        {
            // dataTableForSSZZ
            if (null == dataTableForSSZZ || 0 == dataTableForSSZZ.Rows.Count) {
                MessageBox.Show("请先查询数据!");
                return;
            }
            string strFileName = "";
            SaveFileDialog savFile = new SaveFileDialog();
            savFile.Filter = "PDF文件|.pdf";
            savFile.ShowDialog();
            if (savFile.FileName != "")
            {
                strFileName = savFile.FileName;
            }
            else
            {
                return;
            }

            // Console.WriteLine("fn:" + strFileName);
            PDFHelpler.ToPdf(dataTableForSSZZ, strFileName);
            // string sql = "SELECT p_name 姓名, p_sex 性别 FROM t_patient";
            // MySqlDataAdapter ad = new MySqlDataAdapter();
            // DataTable dt = SqlHelper4MySql.getDataTable(sql, null);
            // this.dataGridView1.DataSource = dt;
            
        }



    
        //设置字体加粗
        private void menuSSLR_CT_Click(object sender, EventArgs e)
        {
            // string fontType = this.menuSSLR_ZZLX.Text;
            // int fontSize = Convert.ToInt32(this.menuSSLR_ZZDX.Text);
            /*if (string.IsNullOrEmpty(fontType))
            {
                fontType = "宋体";
            }*/
            /*
            if (this.rtbSSLR_SSJL.SelectionFont.Bold)
            {
                this.rtbSSLR_SSJL.SelectionFont = new Font(this.rtbSSLR_SSJL.SelectionFont.FontFamily, this.rtbSSLR_SSJL.SelectionFont.Size, FontStyle.Regular);
            }
            else {
                this.rtbSSLR_SSJL.SelectionFont = new Font(this.rtbSSLR_SSJL.SelectionFont.FontFamily, this.rtbSSLR_SSJL.SelectionFont.Size, FontStyle.Bold);
                // this.rtbSSLR_SSJL.SelectionFont = new Font(fontType, fontSize, FontStyle.Regular);
            }
            */
            FontHelpler.changeFontTo(this.rtbSSLR_SSJL, "bold");
        }

        //设置字体 斜体
        private void menuSSLR_XT_Click(object sender, EventArgs e)
        {
            /*string fontType = this.menuSSLR_ZZLX.Text;
            // int fontSize = Convert.ToInt32(this.menuSSLR_ZZDX.Text);
            int fontSize = 12;
            if (string.IsNullOrEmpty(fontType))
            {
                fontType = "宋体";
            }*/

            // this.rtbSSLR_SSJL.SelectionFont = new Font(fontType, fontSize, FontStyle.Italic);
            FontHelpler.changeFontTo(this.rtbSSLR_SSJL, "italic");
            /*
            if (this.rtbSSLR_SSJL.SelectionFont.Italic)
            {
                this.rtbSSLR_SSJL.SelectionFont = new Font(this.rtbSSLR_SSJL.SelectionFont.FontFamily, this.rtbSSLR_SSJL.SelectionFont.Size, FontStyle.Regular);
            }
            else
            {
                this.rtbSSLR_SSJL.SelectionFont = new Font(this.rtbSSLR_SSJL.SelectionFont.FontFamily, this.rtbSSLR_SSJL.SelectionFont.Size, FontStyle.Italic);
                // this.rtbSSLR_SSJL.SelectionFont = new Font(fontType, fontSize, FontStyle.Regular);
            }*/
        }


        

        //设置字体下划线
        private void menuSSLR_XHX_Click(object sender, EventArgs e)
        {
            /*
            string fontType = this.menuSSLR_ZZLX.Text;
            // int fontSize = Convert.ToInt32(this.menuSSLR_ZZDX.Text);
            int fontSize = 12;
            if (string.IsNullOrEmpty(fontType))
            {
                fontType = "宋体";
            }

            this.rtbSSLR_SSJL.SelectionFont = new Font(fontType, fontSize, FontStyle.Underline);
            */
            /*
            if (this.rtbSSLR_SSJL.SelectionFont.Underline)
            {
                this.rtbSSLR_SSJL.SelectionFont = new Font(this.rtbSSLR_SSJL.SelectionFont.FontFamily, this.rtbSSLR_SSJL.SelectionFont.Size, FontStyle.Regular);
            }
            else
            {

                this.rtbSSLR_SSJL.SelectionFont = new Font(this.rtbSSLR_SSJL.SelectionFont.FontFamily, 
                    this.rtbSSLR_SSJL.SelectionFont.Size,
                    FontStyle.Underline);
                // this.rtbSSLR_SSJL.SelectionFont = new Font(fontType, fontSize, FontStyle.Regular);
            }*/
            FontHelpler.changeFontTo(this.rtbSSLR_SSJL, "underline");
        }

        //文本左对齐
        private void menuSSLR_ZDQ_Click(object sender, EventArgs e)
        {
            this.rtbSSLR_SSJL.SelectionAlignment = HorizontalAlignment.Left;
        }

        //文本右对齐
        private void menuSSLR_YDQ_Click(object sender, EventArgs e)
        {
            this.rtbSSLR_SSJL.SelectionAlignment = HorizontalAlignment.Right;
        }

        //文本剧中
        private void menuSSLR_JZ_Click(object sender, EventArgs e)
        {
            this.rtbSSLR_SSJL.SelectionAlignment = HorizontalAlignment.Center;
        }

        //设置字体类型 粗体 宋体
        private void menuSSLR_setFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fontType = this.menuSSLR_ZZLX.Text;
            // int fontSize = Convert.ToInt32(this.menuSSLR_ZZDX.Text);
            int fontSize = 12;
            if (string.IsNullOrEmpty(fontType))
            {
                fontType = "宋体";
            }

            this.rtbSSLR_SSJL.SelectionFont = new Font(fontType, fontSize, FontStyle.Bold);
        }

        //监听窗体的变化
        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                //this.Hide();
                //this.notifyIcon1.Visible = true;
                this.notifyIcon1.Text = "析之助手术登记管理系统";//鼠标放在托盘时显示的文字
                this.notifyIcon1.BalloonTipText = "析之助手术登记管理系统";//气泡显示的文字
                this.notifyIcon1.ShowBalloonTip(1000);//托盘气泡的显现时间
                
            }
        }

        //双击托盘，显示应用
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        //让datagridview自动 编号
        private void dgvTJCX_GZL_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            autoAddId(sender,e);
        }

        //工作量统计让datagridview自动 编号
        public void autoAddId(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView dt = (DataGridView)sender;
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dgvTJCX_GZL.RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics,
                  (e.RowIndex + 1).ToString(),
                   dt.RowHeadersDefaultCellStyle.Font,
                   rectangle,
                   dt.RowHeadersDefaultCellStyle.ForeColor,
                   TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        //选择省 -》改变市与县
        private void cbSSLR_PROVINCE_SelectedIndexChanged(object sender, EventArgs e)
        {
            //查询市
            dataTable = DBManager.getInstance().find(string.Format("SELECT  id,c_name as d_name FROM t_city where c_province_id = {0}", this.cbSSLR_PROVINCE.SelectedValue));
            combobox(this.cbSSLR_CITY, dataTable);//

            //查询县
            dataTable = DBManager.getInstance().find(string.Format("SELECT  id,a_name as d_name FROM t_area where a_city_id = {0}", this.cbSSLR_CITY.SelectedValue));
            combobox(this.cbSSLR_COUNTY, dataTable);//

        }

        //查询县
        private void cbSSLR_CITY_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //查询县
            dataTable = DBManager.getInstance().find(string.Format("SELECT  id,a_name as d_name FROM t_area where a_city_id = {0}", this.cbSSLR_CITY.SelectedValue));
            combobox(this.cbSSLR_COUNTY, dataTable);//
        }

        //托盘item事件
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            for (int i = 0; i < contextMenuStripIcon.Items.Count; i++)
            {

                if (contextMenuStripIcon.Items[i].Selected)
                {

                    if (contextMenuStripIcon.Items[i].Text.Trim() == "退出登录")
                    {
                        this.Hide();
                        new System.Threading.Thread(() =>
                        {
                            Application.Run(new FormSignIn());
                        }).Start(); ;
                        
                    }

                    else if (contextMenuStripIcon.Items[i].Text.Trim() == "退出")
                    {
                        Application.Exit(); 
                    }

                }

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.labelNowDate.Text = UtilTools.getDateAndWeek();
        }

        //切换到手术追踪
        private void btnSSZZ_Click(object sender, EventArgs e)
        {
            this.btnSSLR.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSSZZ.BackColor = ColorTranslator.FromHtml("#3399ff");
            this.btnZDGL.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSJCX.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnCJFX.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSSXGXY.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnKSGL.BackColor = ColorTranslator.FromHtml("#0000cd");

            this.panelSSLR.Visible = false;
            this.panelSSZZ.Visible = true;
            this.panelZDGL.Visible = false;
            this.panelSJCX.Visible = false;
            this.panelCJFX.Visible = false;
            this.panelSSXGXY.Visible = false;
            this.panelKSGL.Visible = false;

            this.lstbxPatientNames.Items.Clear();
            this.labSSZZ_SSRQ.Text = "";
            this.labSSZZ_SSDD.Text = "";
            this.labSSZZ_SSLX.Text = "";
            this.labSSZZ_SSFS.Text = "";
            this.labSSZZ_LXFS.Text = "";


            this.dtpTraceTime.Value = Convert.ToDateTime(UtilTools.getDayAndTimeMM());

            // 查询内痿自我锻炼
            DataTable dt = DBDictionary.getInstance().getDictionarysByParentId(10);
            combobox(this.cbSSLR_SSZZ_NWZWDLQK, dt);//

            this.txtbxPatientName.Text = "";

            // 查询穿刺部位皮肤情况
            dt = DBDictionary.getInstance().getDictionarysByParentId(11);
            combobox(this.cbSSLR_SSZZ_CCBWPFQK, dt);

            // 查询感染控制方式
            dt = DBDictionary.getInstance().getDictionarysByParentId(12);
            combobox(this.cbSSLR_SSZZ_GRKZFS, dt);


            initSSZZState();

        }

        

        //切换到手术相关协议
        private void btnSSXGXY_Click(object sender, EventArgs e)
        {
            this.btnSSXGXY_XYMC.Text = "";

            this.btnSSLR.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSSZZ.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnZDGL.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSJCX.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnCJFX.BackColor = ColorTranslator.FromHtml("#0000cd");
            this.btnSSXGXY.BackColor = ColorTranslator.FromHtml("#3399ff");
            this.btnKSGL.BackColor = ColorTranslator.FromHtml("#0000cd");

            this.panelSSLR.Visible = false;
            this.panelSSZZ.Visible = false;
            this.panelZDGL.Visible = false;
            this.panelSJCX.Visible = false;
            this.panelCJFX.Visible = false;
            this.panelSSXGXY.Visible = true;
            this.panelKSGL.Visible = false;


            // 加载手术相关协议
            loadControl("");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            loadControl(this.btnSSXGXY_XYMC.Text);
        }

        private void loadControl(string ctrName) {
            this.dgvSSXGXY_Control.DataSource = null;//清空
            this.dgvSSXGXY_Control.Columns.Clear();

            this.dgvSSXGXY_Control.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//选中整行


            MySqlParameter[] ps = new MySqlParameter[]{
                new MySqlParameter("@c_name", "%" + ctrName + "%")
            };
            DataTable dt = SqlHelper4MySql.getDataTable(SqlCommandHelpler.T_CONTROL_SELECT_BY_NAME, ps);
            this.dgvSSXGXY_Control.DataSource = dt;

            DataGridViewButtonColumn dgvbc = new DataGridViewButtonColumn();
            dgvbc.DefaultCellStyle.NullValue = "内容";
            
            this.dgvSSXGXY_Control.Columns.Add(dgvbc);
         

            this.dgvSSXGXY_Control.Columns[0].Visible = false;

            this.dgvSSXGXY_Control.Columns[0].FillWeight = 1;
            this.dgvSSXGXY_Control.Columns[1].FillWeight = 10;
            this.dgvSSXGXY_Control.Columns[2].FillWeight = 40;
            this.dgvSSXGXY_Control.Columns[3].FillWeight = 40;
            this.dgvSSXGXY_Control.Columns[4].FillWeight = 10;

            this.dgvSSXGXY_Control.Columns[0].HeaderText = "id";
            this.dgvSSXGXY_Control.Columns[1].HeaderText = "排列顺序";
            this.dgvSSXGXY_Control.Columns[2].HeaderText = "协议名称";
            this.dgvSSXGXY_Control.Columns[3].HeaderText = "协议描述";
            this.dgvSSXGXY_Control.Columns[4].HeaderText = "内容";

            // Header以外所有的单元格的背景色
            this.dgvSSXGXY_Control.RowsDefaultCellStyle.BackColor = Color.White;
            ////列Header的背景色 CellBorderStyle->Single   EnableHeaderVisualStyles->false
            this.dgvSSXGXY_Control.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#4f81DD");
            this.dgvSSXGXY_Control.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvSSXGXY_Control.ColumnHeadersDefaultCellStyle.Font = new Font("宋体", 12f, FontStyle.Bold);
            this.dgvSSXGXY_Control.DefaultCellStyle.SelectionBackColor = Color.Gray;//设置选中的颜色

            
            //using (MySqlDataReader reader = SqlHelper4MySql.ExecuteReader(SqlCommandHelpler.T_CONTROL_SELECT_BY_NAME, ps))
            //{
            //    while (reader.Read()) {
            //        int index = this.dgvSSXGXY_Control.Rows.Add();
            //        // SELECT c_order_id, c_name, c_desc
            //        this.dgvSSXGXY_Control.Rows[index].Cells[0].Value = reader.GetString("id");
            //        this.dgvSSXGXY_Control.Rows[index].Cells[1].Value = reader.GetString("c_order_id");
            //        this.dgvSSXGXY_Control.Rows[index].Cells[2].Value = reader.GetString("c_name");
            //        this.dgvSSXGXY_Control.Rows[index].Cells[3].Value = reader.GetString("c_desc");
            //        // this.dgvSSXGXY_Control.Rows[index].Cells[3].Value = "内容";
            //        // DataGridViewButtonColumn
            //    }
            //}

        }




        //去添加手术追踪界面
        private void btnGoAddSSZZ_Click(object sender, EventArgs e)
        {
            // if (ValueSender.isAdd) {
                // MessageBox.Show("您已经为本次追踪记录添加创建时间和创建人，无法再添加!");
            // }

            ValueSender.isAdd = false;
            FormAddSSZZ form = new FormAddSSZZ();
            form.ShowDialog();
            // Console.WriteLine("here");
            if (ValueSender.isAdd) { 
                int rowNumber = this.dgvSSZZ.Rows.Add();
                this.dgvSSZZ.Rows[rowNumber].Cells[0].Value = ValueSender.createDate.ToShortDateString();
                this.dgvSSZZ.Rows[rowNumber].Cells[1].Value = ValueSender.creator;
            }
            
        }
        private void initSSZZState() {
            this.btnGoAddSSZZ.Enabled = false;
            this.btnSSZZ_SB.Enabled = false;
            this.btnSaveSSLR_SSZZ.Enabled = false;
            this.dgvSSZZ.Rows.Clear();
            ValueSender.isAdd = false;

            this.cbxSSZZ_SFCG.SelectedIndex = -1;
            this.rtbSSZZ_FailReason.Text = "";
            this.tbSSLR_SSZZ_ZWQMJTZQK.Text = "";
            this.cbSSLR_SSZZ_SFTC.SelectedIndex = -1;
            this.cbSSLR_SSZZ_YWXLBCT.SelectedIndex = -1;
            this.cbSSLR_SSZZ_YWXM.SelectedIndex = -1;
            this.cbSSLR_SSZZ_YWBFZ.SelectedIndex = -1;
            this.cbSSLR_SSZZ_YWXBJMQZ.SelectedIndex = -1;
            this.cbSSLR_SSZZ_YWCCBWPFGMQK.SelectedIndex = -1;
            this.cbSSLR_SSZZ_CCBWPFQK.SelectedIndex = -1;
            this.cbSSLR_SSZZ_NWZWDLQK.SelectedIndex = -1;
            this.cbSSLR_SSZZ_CCBWPFQK.SelectedIndex = -1;
            this.cbSSLR_SSZZ_NWZWDLQK.SelectedIndex = -1;
            this.cbSSLR_SSZZ_GRKZFS.SelectedIndex = -1;


        }
        private void enableSSZZState() {
            this.btnGoAddSSZZ.Enabled = true;
            this.btnSSZZ_SB.Enabled = true;
            this.btnSaveSSLR_SSZZ.Enabled = true;
            // this.dgvSSZZ.Rows.Clear();
        }



        //手术记录添加图片
        private void btnSSJLa_addPic_Click(object sender, EventArgs e)
        {
            //打开的文件选择对话框上的标题
            this.openFileDialogSSLR.Title = "请选择图片";
            //设置文件类型
            this.openFileDialogSSLR.Filter = "PNG图片|*.png|JPG图片|*.jpg|BMP图片|*.bmp|Gif图片|*.gif";
            //设置默认文件类型显示顺序
            this.openFileDialogSSLR.FilterIndex = 1;
            //保存对话框是否记忆上次打开的目录
            this.openFileDialogSSLR.RestoreDirectory = true;
            //设置是否允许多选
            this.openFileDialogSSLR.Multiselect = true;
            //按下确定选择的按钮
            if (this.openFileDialogSSLR.ShowDialog() == DialogResult.OK)
            {

                int imgLength = this.openFileDialogSSLR.FileNames.Length;
                if (null != pictureBox10.Image) {
                    MessageBox.Show("无法再添加图片，请先删除所有图片!");
                    return;
                }
                if (imgLength > 2)
                {
                    MessageBox.Show("图片最多两张!");
                    return;
                }

                if (1 == imgLength) {
                    String imgPath = this.openFileDialogSSLR.FileNames[0];
                    Image img = Image.FromFile(imgPath);
                    if (null == picSSTP.Image)
                    {
                        picSSTP.Image = img;
                    } else {
                        pictureBox10.Image = img;    
                    }
                }

                if (2 == imgLength) {
                    if (null != picSSTP.Image)
                    {
                        MessageBox.Show("只能再添加一张图片!");
                        return;
                    }
                    else 
                    {
                        String imgPath = this.openFileDialogSSLR.FileNames[0];
                        Image img = Image.FromFile(imgPath);
                        picSSTP.Image = img;
                        imgPath = this.openFileDialogSSLR.FileNames[1];
                        img = Image.FromFile(imgPath);
                        pictureBox10.Image = img;    
                    }  
                }
                

                /*
                picSSTP.Image = null;
                pictureBox10.Image = null;
                for (int i = 0; i < imgLength; i++)
                {
                    String imgPath = this.openFileDialogSSLR.FileNames[i];
                    Image img = Image.FromFile(imgPath);
                    if (i == 0)
                    {
                        picSSTP.Image = img;
                    }
                    else {
                        pictureBox10.Image = img;
                    }

                }*/
                
            }
        }

        private void panel122_MouseHover(object sender, EventArgs e)
        {
            this.listViewDictionarySSZD.Visible = false;
            this.listViewDictionaryQKZD.Visible = false;
        }

        //去增加协议界面
        private void btnSSXGXY_add_control_Click(object sender, EventArgs e)
        {
            FormAddControl formAddControl = new FormAddControl();
            //formAddControl.setDesc(-1, dictionary_parent_id, "添加字典", this.labelDictionaryShow.Text);
            formAddControl.ShowDialog();
            
            if (formAddControl.DialogResult == DialogResult.OK)
            {
                //this.setListViewDictionary(this.labelDictionaryShow.Text, dictionary_parent_id);//重新绑定
                button7_Click(null, null);
            }
        }
        


        private void hideAllTraceTime() {
            // 3个月 12/3 4次
            // 6个月 12/6 2次
            labTraceTime1.Hide();
            labTraceTime2.Hide();
            labTraceTime3.Hide();
            labTraceTime4.Hide();
            labTraceTime5.Hide();
            labTraceTime6.Hide();
            labTraceTime7.Hide();
            labTraceTime8.Hide();
            labTraceTime9.Hide();
            labTraceTime10.Hide();
            labTraceTime11.Hide();
            labTraceTime12.Hide();
            dtTraceTime1.Hide();
            dtTraceTime2.Hide();
            dtTraceTime3.Hide();
            dtTraceTime4.Hide();
            dtTraceTime5.Hide();
            dtTraceTime6.Hide();
            dtTraceTime7.Hide();
            dtTraceTime8.Hide();
            dtTraceTime9.Hide();
            dtTraceTime10.Hide();
            dtTraceTime11.Hide();
            dtTraceTime12.Hide();
        }

        // 下拉选择手术追踪 值改变
        private void cbSSLR_SSZZQX_SelectedValueChanged(object sender, EventArgs e)
        {
            if (showTraceTime) {
                hideAllTraceTime();

                DateTime dateTime = this.dtSSLR_SSZZ.Value;
                // MessageBox.Show(EVERY_THREE_MONTH + "  " + this.cbSSLR_SSZZQX.Text.Equals(EVERY_THREE_MONTH) + " " + this.cbSSLR_SSZZQX.Text);
                if (this.cbSSLR_SSZZQX.Text.Equals(EVERY_THREE_MONTH))
                {
                    const int monthInteval = 3;
                    labTraceTime1.Show();
                    dtTraceTime1.Show();
                    dtTraceTime1.Value = dateTime.AddMonths(monthInteval * 1);
                    labTraceTime2.Show();
                    dtTraceTime2.Show();
                    dtTraceTime2.Value = dateTime.AddMonths(monthInteval * 2);
                    labTraceTime3.Show();
                    dtTraceTime3.Show();
                    dtTraceTime3.Value = dateTime.AddMonths(monthInteval * 3);
                    labTraceTime4.Show();
                    dtTraceTime4.Show();
                    dtTraceTime4.Value = dateTime.AddMonths(monthInteval * 4);
                }
                else
                {
                    // EVERY_SIX_MONTH
                    const int monthInteval = 6;
                    labTraceTime1.Show();
                    dtTraceTime1.Show();
                    dtTraceTime1.Value = dateTime.AddMonths(monthInteval * 1);
                    labTraceTime2.Show();
                    dtTraceTime2.Show();
                    dtTraceTime2.Value = dateTime.AddMonths(monthInteval * 2);
                }

                /* if (5 <= traceTimes)
                {
                    labTraceTime5.Show();
                    dtTraceTime5.Show();
                    dtTraceTime5.Value = dateTime.AddMonths(monthInteval * 5);
                }
                if (6 <= traceTimes)
                {
                    labTraceTime6.Show();
                    dtTraceTime6.Show();
                    dtTraceTime6.Value = dateTime.AddMonths(monthInteval * 6);
                }
                if (7 <= traceTimes)
                {
                    labTraceTime7.Show();
                    dtTraceTime7.Show();
                    dtTraceTime7.Value = dateTime.AddMonths(monthInteval * 7);
                }
                if (8 <= traceTimes)
                {
                    labTraceTime8.Show();
                    dtTraceTime8.Show();
                    dtTraceTime8.Value = dateTime.AddMonths(monthInteval * 8);
                }
                if (9 <= traceTimes)
                {
                    labTraceTime9.Show();
                    dtTraceTime9.Show();
                    dtTraceTime9.Value = dateTime.AddMonths(monthInteval * 9);
                }
                if (10 <= traceTimes)
                {
                    labTraceTime10.Show();
                    dtTraceTime10.Show();
                    dtTraceTime10.Value = dateTime.AddMonths(monthInteval * 10);
                }
                if (11 <= traceTimes)
                {
                    labTraceTime11.Show();
                    dtTraceTime11.Show();
                    dtTraceTime11.Value = dateTime.AddMonths(monthInteval * 11);
                }
                if (12 == traceTimes) {
                    labTraceTime12.Show();
                    dtTraceTime12.Show();
                    dtTraceTime12.Value = dateTime.AddMonths(monthInteval * 12);
                }*/
            }
           
        }
        private Boolean showTraceTime = false;
        // 下拉选择手术追踪 点击事件
        private void cbSSLR_SSZZQX_Click(object sender, EventArgs e)
        {
            showTraceTime = true;
        }

        // 选择图片
        private void lstboxTTLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.lstboxTTLB.SelectedIndex;
            // Console.WriteLine("当前选择图片下标：" + index);
            if (-1 != index)
            {
                picSSTP.Image = imgLstTTLB.Images[index];
            }
        }

        private void btnRemoveSSTP_Click(object sender, EventArgs e)
        {
            int index = this.lstboxTTLB.SelectedIndex;
            if (-1 == index)
            {
                if (0 == this.imgLstTTLB.Images.Count)
                {
                    MessageBox.Show("当前没有可移除的图片!");
                }
                else
                {
                    MessageBox.Show("请选择要移除的图片!");
                }
            }
            else
            {
                this.lstboxTTLB.Items.RemoveAt(index);
                this.imgLstTTLB.Images.RemoveAt(index);
                this.picSSTP.CreateGraphics().Clear(Color.White);
            }
        }

        private void removeAllSSTP() {
            this.lstboxTTLB.Items.Clear();
            this.imgLstTTLB.Images.Clear();
            this.picSSTP.CreateGraphics().Clear(Color.White);
        }
        private void btnRemoveAllSSTP_Click(object sender, EventArgs e)
        {
            // removeAllSSTP();
            pictureBox10.Image = null;
            picSSTP.Image = null;
        }

        private void cbSSLR_CITY_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void btnSSZZ_SB_Click(object sender, EventArgs e)
        {
            if (!ValueSender.isAdd) { MessageBox.Show("请添加本次追踪创建人及创建日期!"); return; }
            if (-1 == currentIndex) { MessageBox.Show("请选择患者!"); return; }

            DateTime createDate = ValueSender.createDate;
            string creator = ValueSender.creator;
            string sssbyy = this.rtbSSZZ_FailReason.Text;
            if (string.IsNullOrEmpty(sssbyy)) { MessageBox.Show("请输入【手术失败原因】"); return; }

            int tId = trackID[currentIndex];
            MySqlParameter[] ps = new MySqlParameter[] {
                new MySqlParameter("@t_t_sszzrq", createDate), // 创建日期
                new MySqlParameter("@t_sszzcjr", creator), // 创建人
                new MySqlParameter("@t_sfsszz", "是"), // 是否手术跟踪
                new MySqlParameter("@t_sfsszzcg", "否"), // 是否手术跟踪成功
                new MySqlParameter("@t_sssbyy", sssbyy), // 手术失败原因

                new MySqlParameter("@t_ssct", null), // 是否畅通
                new MySqlParameter("@t_ywxlbct", null),
                new MySqlParameter("@t_ywxm", null),
                new MySqlParameter("@t_ywbfz", null),
                new MySqlParameter("@t_ywxbjmqz", null),

                new MySqlParameter("@t_ywccbwphgmqk", null),
                new MySqlParameter("@t_nwzwdlqk", null),
                new MySqlParameter("@t_ccbwpfqk", null),
                new MySqlParameter("@t_grkzfs", null),
                new MySqlParameter("@t_zwcmzcjtzqk", null),

                new MySqlParameter("@t_id", tId) // 这条追踪的id
            };
            try
            {
                MySqlConnection con = SqlHelper4MySql.getConnection();
                con.Open();
                MySqlTransaction tx = con.BeginTransaction();
                // 1,更新追踪信息
                SqlHelper4MySql.ExecuteNonQuery(con, SqlCommandHelpler.I_TRACK_UPDATE, ps);
                // 2,保存追踪失败信息之后往记录表中插入一条新追踪信息
                // 这里要考虑一下，下个月有这个人的追踪记录，则不要插入追踪信息
                // 如：一个人   1,4,7,10四个月需要手术追踪
                // 如果1月失败，2月失败，3月失败，那么3月失败的时候，4月我就不要创建手术追踪了，
                // 因为4月已经存在手术跟踪了。  根据手术记录表里面的 zz1,zz2,zz3,zz4来判断。

               

                // 2.1先从t_track表中读出当前该追踪信息对应的： 手术记录id,患者id，追踪日期。
                // 并将他们作为新追踪信息的参数放入 psTrack 中
                MySqlParameter[] psTrack = new MySqlParameter[1]; // 假装初始化
                DateTime nextTime = new DateTime();
                ps = new MySqlParameter[] {
                     new MySqlParameter("@id", tId) // 这条追踪的id
                };
                MySqlDataReader reader = SqlHelper4MySql.ExecuteReader(SqlCommandHelpler.T_TRACK_SELECT_BY_RECORD_ID_2, ps);
                while (reader.Read())
                {
                    //  t_record_id, t_patient_id, t_zzrq 
                    DateTime date = reader.GetDateTime("t_zzrq");
                    if (12 == date.Month)
                    {
                        nextTime = new DateTime(date.Year + 1, 1, date.Day, 0, 0, 0);
                    }
                    else
                    {
                        nextTime = new DateTime(date.Year, date.Month + 1, date.Day, 0, 0, 0);
                    }
                    psTrack = new MySqlParameter[] {
                        new MySqlParameter("@t_record_id", reader.GetInt32("t_record_id")),
                        new MySqlParameter("@t_patient_ID", reader.GetString("t_patient_id")),
                        new MySqlParameter("@t_zzrq", nextTime),
                        new MySqlParameter("@t_sfsszz", "否"),
                        new MySqlParameter("@t_sfsszzcg", "否")
                    };
                }


                // 2.2从手术记录表读取 该手术的四个追踪日期，根据四个追踪日期判断是否应该添加这条追踪记录
                int recordId = recordID[currentIndex];
                ps = new MySqlParameter[] {
                    new MySqlParameter("@id", recordId) // 手术记录id
                };
                reader = SqlHelper4MySql.ExecuteReader(SqlCommandHelpler.T_RECORD_SELECT_TRACK_TIME_BY_ID, ps);
                Boolean canAdd = true;
                while (reader.Read())
                {
                    String sszz = reader.GetString("r_sszz");
                    DateTime zz1 = reader.GetDateTime("r_zz1");
                    DateTime zz2 = reader.GetDateTime("r_zz2");
                    DateTime zz3 = reader.GetDateTime("r_zz3");
                    DateTime zz4 = reader.GetDateTime("r_zz4");
                    int y = nextTime.Year;
                    int m = nextTime.Month;

                    if (sszz.Equals("每三个月"))
                    {
                        if (zz1.Year == y && zz1.Month == m)
                        {
                            canAdd = false;
                        }
                        else if (zz2.Year == y && zz2.Month == m)
                        {
                            canAdd = false;
                        }
                        else if (zz3.Year == y && zz3.Month == m)
                        {
                            canAdd = false;
                        }
                        else if (zz4.Year == y && zz4.Month == m)
                        {
                            canAdd = false;
                        }

                    }
                    else
                    {
                        // 每六个月
                        if (zz1.Year == y && zz1.Month == m)
                        {
                            canAdd = false;
                        }
                        else if (zz2.Year == y && zz2.Month == m)
                        {
                            canAdd = false;
                        }
                    }
                }
                if (canAdd)
                {
                    // Console.WriteLine("应该添加");
                    SqlHelper4MySql.ExecuteNonQuery(con, SqlCommandHelpler.T_TRACK_INSERT_PART, psTrack);
                } else {
                    // Console.WriteLine("不添加");
                }

                tx.Commit();
                con.Close();

                MessageBox.Show("保存成功!");
                // 清空界面上的东西
                this.lstbxPatientNames.Items.RemoveAt(currentIndex);
                currentIndex = -1;
                this.tbSSLR_SSZZ_ZWQMJTZQK.Text = "";
                this.rtbSSZZ_FailReason.Text = "";
                initSSZZState();

            }
            catch (Exception err)
            {
                MessageBox.Show("保存失败:" + err.Message);
            }


        }


        private void btnSaveSSLR_SSZZ_Click_1(object sender, EventArgs e)
        {

            if (!ValueSender.isAdd) { MessageBox.Show("请添加本次追踪创建人及创建日期!"); return; }
            if (-1 == currentIndex) { MessageBox.Show("请选择患者!"); return;  }


            // Datethis.dgvSSZZ.Rows[this.dgvSSZZ.Rows.Count - 1].Cells[0];
            DateTime createDate = ValueSender.createDate;
            string creator = ValueSender.creator;
            string sfct = this.cbSSLR_SSZZ_SFTC.Text; // 是否畅通
            string ywxlbct = this.cbSSLR_SSZZ_YWXLBCT.Text;// 有无血流不畅通
            string ywxm = this.cbSSLR_SSZZ_YWXM.Text; // 有无胸闷
            string ywbfz = this.cbSSLR_SSZZ_YWBFZ.Text; // 有无并发症
            string ywxbjmqz = this.cbSSLR_SSZZ_YWXBJMQZ.Text; // 有无胸壁静脉曲张
            string ywccbwpfgmqk = this.cbSSLR_SSZZ_YWCCBWPFGMQK.Text; // 有无穿刺部位皮肤过敏情况
            string nwzwdlqk = this.cbSSLR_SSZZ_NWZWDLQK.Text; // 内痿自我锻炼情况
            string ccbwpfqk = this.cbSSLR_SSZZ_CCBWPFQK.Text; // 穿刺部位皮肤情况
            string grkzfs = this.cbSSLR_SSZZ_GRKZFS.Text; // 感染控制方式
            string zwcmjtzqk = this.tbSSLR_SSZZ_ZWQMJTZQK.Text; // 自我触摸及听诊情况

            if (string.IsNullOrEmpty(sfct)) { MessageBox.Show("请选择【是否畅通】"); return; }
            if (string.IsNullOrEmpty(ywxlbct)) { MessageBox.Show("请选择【有无血流不畅通】"); return; }
            if (string.IsNullOrEmpty(ywxm)) { MessageBox.Show("请选择【有无胸闷】"); return; }
            if (string.IsNullOrEmpty(ywbfz)) { MessageBox.Show("请选择【有无并发症】"); return; }
            if (string.IsNullOrEmpty(ywxbjmqz)) { MessageBox.Show("请选择【有无胸壁静脉曲张】"); return; }
            if (string.IsNullOrEmpty(ywccbwpfgmqk)) { MessageBox.Show("请选择【有无穿刺皮肤过敏情况】"); return; }
            if (string.IsNullOrEmpty(nwzwdlqk)) { MessageBox.Show("请选择【内痿自我锻炼情况】"); return; }
            if (string.IsNullOrEmpty(ccbwpfqk)) { MessageBox.Show("请选择【穿刺部位皮肤情况】"); return; }
            if (string.IsNullOrEmpty(grkzfs)) { MessageBox.Show("请选择【感染控制方式】"); return; }
            if (string.IsNullOrEmpty(zwcmjtzqk)) { MessageBox.Show("请输入【自我触摸及听诊情况】"); return; }


            MySqlParameter[] ps = new MySqlParameter[] {
                new MySqlParameter("@t_t_sszzrq", createDate), // 创建日期
                new MySqlParameter("@t_sszzcjr", creator), // 创建人
                new MySqlParameter("@t_sfsszz", "是"), // 是否手术跟踪
                new MySqlParameter("@t_sfsszzcg", "是"), // 是否手术跟踪成功
                new MySqlParameter("@t_sssbyy", null), // 手术失败原因

                new MySqlParameter("@t_ssct", sfct), // 是否畅通
                new MySqlParameter("@t_ywxlbct", ywxlbct),
                new MySqlParameter("@t_ywxm", ywxm),
                new MySqlParameter("@t_ywbfz", ywbfz),
                new MySqlParameter("@t_ywxbjmqz", ywxbjmqz),

                new MySqlParameter("@t_ywccbwphgmqk", ywccbwpfgmqk),
                new MySqlParameter("@t_nwzwdlqk", nwzwdlqk),
                new MySqlParameter("@t_ccbwpfqk", ccbwpfqk),
                new MySqlParameter("@t_grkzfs", grkzfs),
                new MySqlParameter("@t_zwcmzcjtzqk", zwcmjtzqk),

                new MySqlParameter("@t_id", trackID[currentIndex]) // 这条追踪的id
            };
            try
            {
                SqlHelper4MySql.ExecuteNonQuery(SqlCommandHelpler.I_TRACK_UPDATE, ps);
                MessageBox.Show("保存成功!");
                // 清空界面上的东西
                // 测试测试
                this.lstbxPatientNames.Items.RemoveAt(currentIndex);
                currentIndex = -1;
                this.tbSSLR_SSZZ_ZWQMJTZQK.Text = "";
                // this.dgvSSZZ.Rows.Clear();
                initSSZZState();

            }
            catch (Exception err)
            {
                MessageBox.Show("保存失败:" + err.Message);
                throw;
            }
            

        }

        private string[] patientNames;
        private string[] patientID;
        private int[] recordID;
        private int[] trackID;
        private int currentIndex = -1;
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string patientName = this.txtbxPatientName.Text;
            DateTime date = this.dtpTraceTime.Value;
            DateTime startTime = new DateTime(date.Year, date.Month, 1, 0, 0, 0);
            DateTime endTime;
            if (12 == date.Month)
            {
                endTime = new DateTime(date.Year + 1, 1, 1, 23, 59, 59);
            }
            else
            {
                endTime = new DateTime(date.Year, date.Month + 1, 1, 23, 59, 59);
                
            }
            endTime = endTime.AddDays(-1);
            // Console.WriteLine("startTime" + startTime);
            // Console.WriteLine("endTime" + endTime);
                
            if (String.IsNullOrEmpty(patientName)) {
                // 查询方式一：根据日期查询
                MySqlParameter[] ps = new MySqlParameter[] {
                    new MySqlParameter("@startTime", startTime),
                    new MySqlParameter("@endTime", endTime)
                };
                int count = Convert.ToInt32(SqlHelper4MySql.ExecuteScalar(SqlCommandHelpler.T_TRACK_PATIENT_COUNT_NAME_AND_ID_BY_ZZRQ, ps));
                if (0 == count) {
                    MessageBox.Show("没有符合条件的患者!");
                    return;
                }
                patientNames = new string[count];
                patientID = new string[count];
                recordID = new int[count];
                trackID = new int[count];
                int i = 0;
                using (MySqlDataReader reader = SqlHelper4MySql.ExecuteReader(SqlCommandHelpler.T_TRACK_PATIENT_FIND_NAME_AND_ID_BY_ZZRQ, ps))
                {
                    while (reader.Read())
                    {
                        // Console.WriteLine(">>>" + reader.GetString("p_name") + ">>>" + reader.GetString("p_id"));
                        patientNames[i] = reader.GetString("p_name");
                        patientID[i] = reader.GetString("p_id");
                        recordID[i] = reader.GetInt32("t_record_id");
                        trackID[i++] = reader.GetInt32("t_id");
                    }
                }

            } else {
                // 根据患者姓名模糊查询 + 日期查询
                MySqlParameter[] ps = new MySqlParameter[] {
                    new MySqlParameter("@startTime", startTime),
                    new MySqlParameter("@endTime", endTime),
                    new MySqlParameter("@p_name", "%" + patientName + "%")
                };
                int count = Convert.ToInt32(SqlHelper4MySql.ExecuteScalar(SqlCommandHelpler.T_TRACK_PATIENT_COUNT_NAME_AND_ID_BY_ZZRQ_AND_NAME, ps));
                if (0 == count)
                {
                    MessageBox.Show("没有符合条件的患者!");
                    return;
                }
                patientNames = new string[count];
                patientID = new string[count];
                recordID = new int[count];
                trackID = new int[count];
                int i = 0;
                using (MySqlDataReader reader = SqlHelper4MySql.ExecuteReader(SqlCommandHelpler.T_TRACK_PATIENT_FIND_NAME_AND_ID_BY_ZZRQ_AND_NAME, ps))
                {
                    while (reader.Read()) {
                        // Console.WriteLine(">>>" + reader.GetString("p_name") + ">>>" + reader.GetString("p_id"));
                        patientNames[i] = reader.GetString("p_name");
                        patientID[i] = reader.GetString("p_id");
                        recordID[i] = reader.GetInt32("t_record_id");
                        // recordID[i] = reader.GetInt32("t_record_id");
                        trackID[i++] = reader.GetInt32("t_id");
                    }
                }
            }
            this.lstbxPatientNames.Items.Clear();
            this.lstbxPatientNames.Items.AddRange(patientNames);

        }

        private void lstbxPatientNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.lstbxPatientNames.SelectedIndex;
            currentIndex = index;

            initSSZZState();
            if (-1 != index) {
                enableSSZZState();

                
                int rId = recordID[index];
                string pID = patientID[index];


                // 载入手术信息
                MySqlParameter[] ps = new MySqlParameter[] {
                    new MySqlParameter("@id", rId)
                };
                using (MySqlDataReader reader = SqlHelper4MySql.ExecuteReader(SqlCommandHelpler.T_RECORD_SELECT_PART_BY_ID, ps))
	            {
                    while (reader.Read())
                    {
                        this.labSSZZ_SSRQ.Text = reader.GetDateTime("r_date").ToLongDateString();
                        this.labSSZZ_SSDD.Text = reader.GetString("r_ss_address");
                        this.labSSZZ_SSLX.Text = reader.GetString("r_ss_type");
                        this.labSSZZ_SSFS.Text = reader.GetString("r_ss_method");

                    }
	            }

                // 载入患者信息
                ps = new MySqlParameter[] {
                    new MySqlParameter("@p_id", pID)
                };
                using (MySqlDataReader reader = SqlHelper4MySql.ExecuteReader(SqlCommandHelpler.T_PATIENT_SELECT_TEL_BY_ID, ps))
                {
                    while (reader.Read())
                    {
                        this.labSSZZ_LXFS.Text = reader.GetString("p_tel");
                        // Console.WriteLine(reader.GetString("p_tel"));
                    }
                }

                // 载入该患者本次手术所有已追踪的创建人和创建日期
                ps = new MySqlParameter[] {
                    new MySqlParameter("@t_record_id", rId)
                };
                using (MySqlDataReader reader = SqlHelper4MySql.ExecuteReader(SqlCommandHelpler.T_TRACK_SELECT_BY_RECORD_ID, ps))
                {
                    while (reader.Read())
                    { 
                       //  this.labSSZZ_LXFS.Text = reader.GetString("t_sszzrq");
                        // Console.WriteLine(reader.GetString("t_sszzcjr"));
                        int rowNumber = this.dgvSSZZ.Rows.Add();
                        this.dgvSSZZ.Rows[rowNumber].Cells[0].Value = reader.GetDateTime("t_sszzrq").ToShortDateString();
                        this.dgvSSZZ.Rows[rowNumber].Cells[1].Value = reader.GetString("t_sszzcjr");
                    }
                }
                

            }
        }

        private void panel122_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbxSSZZ_SFCG_SelectedValueChanged(object sender, EventArgs e)
        {
            string value = Convert.ToString(this.cbxSSZZ_SFCG.Text);
            if (string.IsNullOrEmpty(value)) {
                this.grupbxSSZZ_ZZSB.Hide();
                this.grupbxSSZZ_ZZCG.Hide();
            }
            else if (value.Equals("是"))
            {
                this.grupbxSSZZ_ZZSB.Hide();
                this.grupbxSSZZ_ZZCG.Show();
            }
            else {
                this.grupbxSSZZ_ZZSB.Show();
                this.grupbxSSZZ_ZZCG.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // dlgPrintPreview.Document = 
            printPreviewDialogSJCX_SSJL.Document = this.printDucumentSSCX_SSJL;
            printPreviewDialogSJCX_SSJL.ShowDialog();

            // Graphics g = pictureBox3.CreateGraphics();
            // g.DrawLine(new Pen(Color.Black), new Point(0, 0), new Point(727, 100));
            // g.DrawLine(new Pen(Color.Black), new Point(0, 0), new Point(727, 1069));

        }

        private void all_ccfs_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void all_ssjl_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            panelSJCX_SSJL.Height = e.NewRectangle.Height + 50; 
            // all_ssjl.Height = e.NewRectangle.Height + 50;
            // groupBox2.Height = e.NewRectangle.Height + 50;
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void btnTJFX_GZLTJ_Export_Click(object sender, EventArgs e)
        {
            // dataTableForTJFX_XXTJ
            if (null == dataTableForTJFX_XXTJ || 0 == dataTableForTJFX_XXTJ.Rows.Count)
            {
                MessageBox.Show("请先查询数据!");
                return;
            }
            string strFileName = "";
            SaveFileDialog savFile = new SaveFileDialog();
            savFile.Filter = "PDF文件|.pdf";
            savFile.ShowDialog();
            if (savFile.FileName != "")
            {
                strFileName = savFile.FileName;
            }
            else
            {
                return;
            }

            Console.WriteLine("fn:" + strFileName);
            PDFHelpler.ToPdf(dataTableForTJFX_XXTJ, strFileName);
        }

        private void btnTJFX_SSTJ_OUT_Click(object sender, EventArgs e)
        {

            //Bitmap nvbl1 = new Bitmap(this.panelSJTJ_SSTJ_1.Width, this.panelSJTJ_SSTJ_1.Height);
            //this.panelSJTJ_SSTJ_1.DrawToBitmap(nvbl1, new Rectangle(0, 0, nvbl1.Width, nvbl1.Height));

            //Bitmap nvbl2 = new Bitmap(this.panelSJTJ_SSTJ_2.Width, this.panelSJTJ_SSTJ_2.Height);
            //this.panelSJTJ_SSTJ_2.DrawToBitmap(nvbl2, new Rectangle(0, 0, nvbl2.Width, nvbl2.Height));

            //Bitmap nvbl3 = new Bitmap(this.panelSJTJ_SSTJ_3.Width, this.panelSJTJ_SSTJ_3.Height);
            //this.panelSJTJ_SSTJ_3.DrawToBitmap(nvbl3, new Rectangle(0, 0, nvbl3.Width, nvbl3.Height));

            //Bitmap nvbl4 = new Bitmap(this.panelSJTJ_SSTJ_4.Width, this.panelSJTJ_SSTJ_4.Height);
            //this.panelSJTJ_SSTJ_4.DrawToBitmap(nvbl4, new Rectangle(0, 0, nvbl4.Width, nvbl4.Height));

            Bitmap nvbl1 = new Bitmap(this.panelTotal.Width, this.panelTotal.Height);
            this.panelTotal.DrawToBitmap(nvbl1, new Rectangle(0, 0, nvbl1.Width, nvbl1.Height));

            Bitmap[] bms = new Bitmap[1];
            bms[0] = nvbl1;
            //bms[1] = nvbl2;
            //bms[2] = nvbl3;
            //bms[3] = nvbl4;

            string strFileName = "";
            SaveFileDialog savFile = new SaveFileDialog();
            savFile.Filter = "PDF文件|.pdf";
            savFile.ShowDialog();
            if (savFile.FileName != "")
            {
                strFileName = savFile.FileName;
            }
            else
            {
                return;
            }

            Console.WriteLine("fn:" + strFileName);
            PDFHelpler.imageToPDF(bms, strFileName);
            // nvbl4.get

            // e.Graphics.DrawImage(baseInfo, left, top, baseInfo.Width, baseInfo.Height);
            // top += baseInfo.Height;
            // top += baseInfo.Height;
            // page.Add(baseInfo);
            // Console.WriteLine("baseInfo之后当前top值:" + top);


            
        }

        private void dgvSSXGXY_Control_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // e.ColumnIndex
            // e.RowIndex
            int r = e.RowIndex;
            int c = e.ColumnIndex;
            Console.WriteLine("row:" + r + " col:" + c);
            // Console.WriteLine(this.dgvSSXGXY_Control.Rows[e.RowIndex].Cells[e.ColumnIndex].);
            if (c ==4 && r >= 0)
            {
                // row:0 col:4
                // row:1 col:4
                // row:2 col:4
                int id = Convert.ToInt32(this.dgvSSXGXY_Control.Rows[r].Cells[0].Value);
                FormControlContent f = new FormControlContent(id);
                f.ShowDialog();

            }
        }

        private void btnSSXGXY_XGXY_Click(object sender, EventArgs e)
        {
            DataGridViewRow dgvr = this.dgvSSXGXY_Control.CurrentRow;
            if (null == dgvr)
            {
                MessageBox.Show("请先选择要修改的行");
            }
            else {
                // MessageBox.Show("选中了");
                int id = Convert.ToInt32(dgvr.Cells[0].Value);
                string orderId = Convert.ToString(dgvr.Cells[1].Value);
                string name = Convert.ToString(dgvr.Cells[2].Value);
                string desc = Convert.ToString(dgvr.Cells[3].Value);

                FormModifyControl form = new FormModifyControl(id, orderId, name, desc);
                form.ShowDialog();
                if (form.DialogResult == DialogResult.OK)
                {
                    button7_Click(null, null);
                }
             
            }
        }

        private void btnSSXGXY_SCXY_Click(object sender, EventArgs e)
        {
            DataGridViewRow dgvr = this.dgvSSXGXY_Control.CurrentRow;
            if (null == dgvr)
            {
                MessageBox.Show("请先选择要删除的行");
            }
            else
            {
                DialogResult dr = MessageBox.Show("确定删除【" + dgvr.Cells[1].Value + "】", "确定删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes) {
                    MySqlParameter[] ps = new MySqlParameter[] {
                        new MySqlParameter("@id", dgvr.Cells[0].Value)
                    };
                    SqlHelper4MySql.ExecuteNonQuery(SqlCommandHelpler.T_CONTROL_DELETE, ps);
                    // 重新加载当前表格
                    button7_Click(null, null);
                }

            }
        }

        private void printPreviewDialogSJCX_SSJL_MouseDown(object sender, MouseEventArgs e)
        {
            // MessageBox.Show("hi");
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
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

        private void label54_Click(object sender, EventArgs e)
        {

           
            this.Close();
            new System.Threading.Thread(() =>
            {
                Application.Run(new FormSignIn());
            }).Start();

            //FormSignIn f = new FormSignIn();
            //f.Show();
        }

        private void dpSSLR_SSRQ_ValueChanged(object sender, EventArgs e)
        {
            dtSSLR_SSZZ.Value = dpSSLR_SSRQ.Value;
        }

        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(this.BackColor);   
        }

        private void panelSJCX_SSJL_Head_Paint(object sender, PaintEventArgs e)
        {

        }

        private void grpbxImgs_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(this.BackColor);   
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (currentPage == 0)
            {
                return;
            }
            else {
                // currentPage++;
                changePage(--currentPage);
            }
            // panelSSCX_SSJLD.Show();
            // panelSSJLD2.Hide();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            // panelSSCX_SSJLD.Hide();
            // panelSSJLD2.Show();
            if (ctrs.Count == 0) {
                MessageBox.Show("请先选择患者!");
            }
            if (currentPage > (ctrs.Count - 1))
            {
                return;
            }
            else
            {
                changePage(++currentPage);
            }
        }

      
        private void contextMenuStripRichSet_Click(object sender, EventArgs e)
        {
            RichTextBox richTextBox = (RichTextBox)(sender as ContextMenuStrip).SourceControl;
            for (int i = 0; i < contextMenuStripRichSet.Items.Count; i++)
            {

                if (contextMenuStripRichSet.Items[i].Selected)
                {

                    if (contextMenuStripRichSet.Items[i].Text.Trim() == "复制")
                    {
                        richTextBox.Copy();
                    }

                    else if (contextMenuStripRichSet.Items[i].Text.Trim() == "剪切")
                    {
                        richTextBox.Cut();
                    }
                    else if (contextMenuStripRichSet.Items[i].Text.Trim() == "粘贴")
                    {
                        richTextBox.Paste(); 
                    }
                    else if (contextMenuStripRichSet.Items[i].Text.Trim() == "全选")
                    {
                        richTextBox.SelectAll();
                    }

                }

            }
        }

      



       

        

        

       

       

       

       

        

    }

       
}