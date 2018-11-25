namespace xzj
{
    partial class FormSignIn
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBoxPwd = new System.Windows.Forms.TextBox();
            this.textBoxAcount = new System.Windows.Forms.TextBox();
            this.btnConfigDB = new System.Windows.Forms.Button();
            this.btnTime = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSignIn = new System.Windows.Forms.Button();
            this.btnCloseForm = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxPwd
            // 
            this.textBoxPwd.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxPwd.Font = new System.Drawing.Font("宋体", 15F);
            this.textBoxPwd.Location = new System.Drawing.Point(488, 258);
            this.textBoxPwd.Margin = new System.Windows.Forms.Padding(1);
            this.textBoxPwd.Name = "textBoxPwd";
            this.textBoxPwd.PasswordChar = '*';
            this.textBoxPwd.Size = new System.Drawing.Size(231, 23);
            this.textBoxPwd.TabIndex = 5;
            this.textBoxPwd.UseSystemPasswordChar = true;
            this.textBoxPwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxPwd_KeyDown);
            // 
            // textBoxAcount
            // 
            this.textBoxAcount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxAcount.Font = new System.Drawing.Font("宋体", 15F);
            this.textBoxAcount.Location = new System.Drawing.Point(488, 196);
            this.textBoxAcount.Margin = new System.Windows.Forms.Padding(1);
            this.textBoxAcount.Name = "textBoxAcount";
            this.textBoxAcount.Size = new System.Drawing.Size(231, 23);
            this.textBoxAcount.TabIndex = 5;
            this.textBoxAcount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxPwd_KeyDown);
            // 
            // btnConfigDB
            // 
            this.btnConfigDB.BackColor = System.Drawing.Color.Transparent;
            this.btnConfigDB.FlatAppearance.BorderSize = 0;
            this.btnConfigDB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfigDB.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline);
            this.btnConfigDB.ForeColor = System.Drawing.Color.White;
            this.btnConfigDB.Location = new System.Drawing.Point(434, 478);
            this.btnConfigDB.Name = "btnConfigDB";
            this.btnConfigDB.Size = new System.Drawing.Size(129, 32);
            this.btnConfigDB.TabIndex = 6;
            this.btnConfigDB.Text = "配置数据库";
            this.btnConfigDB.UseVisualStyleBackColor = false;
            this.btnConfigDB.Click += new System.EventHandler(this.btnConfigDB_Click);
            // 
            // btnTime
            // 
            this.btnTime.BackColor = System.Drawing.Color.Transparent;
            this.btnTime.FlatAppearance.BorderSize = 0;
            this.btnTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTime.Font = new System.Drawing.Font("宋体", 9F);
            this.btnTime.ForeColor = System.Drawing.Color.Transparent;
            this.btnTime.Location = new System.Drawing.Point(560, 478);
            this.btnTime.Name = "btnTime";
            this.btnTime.Size = new System.Drawing.Size(228, 32);
            this.btnTime.TabIndex = 11;
            this.btnTime.Text = "_";
            this.btnTime.UseVisualStyleBackColor = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::xzj.Properties.Resources.signInBtnClose1;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(625, 328);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(103, 27);
            this.btnClose.TabIndex = 3;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSignIn
            // 
            this.btnSignIn.BackgroundImage = global::xzj.Properties.Resources.signInBtn1;
            this.btnSignIn.FlatAppearance.BorderSize = 0;
            this.btnSignIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSignIn.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSignIn.Location = new System.Drawing.Point(455, 326);
            this.btnSignIn.Name = "btnSignIn";
            this.btnSignIn.Size = new System.Drawing.Size(108, 29);
            this.btnSignIn.TabIndex = 2;
            this.btnSignIn.UseVisualStyleBackColor = true;
            this.btnSignIn.Click += new System.EventHandler(this.btnSignIn_Click);
            // 
            // btnCloseForm
            // 
            this.btnCloseForm.BackgroundImage = global::xzj.Properties.Resources.signInCloseForm;
            this.btnCloseForm.FlatAppearance.BorderSize = 0;
            this.btnCloseForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseForm.Location = new System.Drawing.Point(775, 0);
            this.btnCloseForm.Name = "btnCloseForm";
            this.btnCloseForm.Size = new System.Drawing.Size(25, 25);
            this.btnCloseForm.TabIndex = 1;
            this.btnCloseForm.UseVisualStyleBackColor = true;
            this.btnCloseForm.Click += new System.EventHandler(this.btnCloseForm_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::xzj.Properties.Resources.login;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 525);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // FormSignIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 525);
            this.Controls.Add(this.btnTime);
            this.Controls.Add(this.btnConfigDB);
            this.Controls.Add(this.textBoxAcount);
            this.Controls.Add(this.textBoxPwd);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSignIn);
            this.Controls.Add(this.btnCloseForm);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSignIn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "signIn";
            this.Load += new System.EventHandler(this.FormSignIn_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCloseForm;
        private System.Windows.Forms.Button btnSignIn;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox textBoxPwd;
        private System.Windows.Forms.TextBox textBoxAcount;
        private System.Windows.Forms.Button btnConfigDB;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnTime;
        private System.Windows.Forms.Timer timer1;


    }
}

