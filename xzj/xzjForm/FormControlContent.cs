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
    public partial class FormControlContent : Form
    {
        private int controlId;
        public FormControlContent(int id)
        {
            InitializeComponent();
            controlId = id;
            MySqlParameter[] ps = new MySqlParameter[] {
                new MySqlParameter("@id", id)
            };
            Object content = SqlHelper4MySql.ExecuteScalar(SqlCommandHelpler.T_CONTROL_SELECT_CONTENT_BY_ID, ps);
            try
            {
                if (null != content)
                {
                    this.rtbSSLR_SSJL.Rtf = (string)content;
                }
            }
            catch (Exception)
            {
                
                // throw;
            }
        }

        private void menuSSLR_CT_Click(object sender, EventArgs e)
        {
            FontHelpler.changeFontTo(this.rtbSSLR_SSJL, "bold");
        }

        private void menuSSLR_XT_Click(object sender, EventArgs e)
        {
            FontHelpler.changeFontTo(this.rtbSSLR_SSJL, "italic");
        }

        private void menuSSLR_XHX_Click(object sender, EventArgs e)
        {
            FontHelpler.changeFontTo(this.rtbSSLR_SSJL, "underline");
        }

        private void menuSSLR_ZDQ_Click(object sender, EventArgs e)
        {
            //rtbSSLR_SSJL
            this.rtbSSLR_SSJL.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void menuSSLR_YDQ_Click(object sender, EventArgs e)
        {
            this.rtbSSLR_SSJL.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void menuSSLR_JZ_Click(object sender, EventArgs e)
        {
            this.rtbSSLR_SSJL.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void btnSaveSSJL2_Click(object sender, EventArgs e)
        {
            MySqlParameter[] ps = new MySqlParameter[] {
                new MySqlParameter("@c_content", this.rtbSSLR_SSJL.Rtf),
                new MySqlParameter("@id", this.controlId)
            };
            SqlHelper4MySql.ExecuteNonQuery(SqlCommandHelpler.T_CONTROL_UPDATE_CONTENT_BY_ID, ps);
            MessageBox.Show("保存成功!");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.printDialog1.Document = this.printDocument1;
            // this.printDialog1.ShowDialog();
            if (this.printDialog1.ShowDialog() == DialogResult.OK)
            {
                this.printDocument1.Print();
            }
        }

        int currentPageIndex = 0;
        private List<Bitmap> maps = new List<Bitmap>();
        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            currentPageIndex = 0;
            maps = new List<Bitmap>();
            Bitmap bm = CutImage.RtbToBitmap(this.rtbSSLR_SSJL);
            int pageHeight = 969;
            if (bm.Height > pageHeight)
            {
                int imgNumber = (bm.Height / pageHeight) + 1;

                int nextY = 0;
                int leftHeight = bm.Height;
                for (int i = 0; i < imgNumber; i++) {

                    if (leftHeight > pageHeight)
                    {

                        maps.Add(ImageHelpler.cutImage(bm, 0, nextY, bm.Width, pageHeight));
                        leftHeight -= pageHeight;
                        nextY += pageHeight;

                    }
                    else {
                        maps.Add(ImageHelpler.cutImage(bm, 0, nextY, bm.Width, leftHeight));
                    }
                    
                }
            }
            else {
                maps.Add(bm);
            }
        }


        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int left = e.MarginBounds.Left;
            int top = e.MarginBounds.Top;
            int right = e.MarginBounds.Right;
            int bottom = e.MarginBounds.Bottom;

            // for (int i = 0; i < maps.Count; i++) {
            e.Graphics.DrawImage(maps[currentPageIndex], left + 10, top, maps[currentPageIndex].Width, maps[currentPageIndex].Height);
            currentPageIndex++;
            if (maps.Count > currentPageIndex)
            {
                e.HasMorePages = true;
            }
            else {
                e.HasMorePages = false;
            }
            // e.Graphics.DrawLine(new Pen(Color.Red), new Point(100, 100), new Point(727, 100));
            // e.Graphics.DrawLine(new Pen(Color.Red), new Point(100, 1069), new Point(727, 1069));
            // e.Graphics.DrawLine(new Pen(Color.Red), new Point(100, 100), new Point(100, 1069));
            // e.Graphics.DrawLine(new Pen(Color.Red), new Point(727, 100), new Point(727, 1069));
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            this.printPreviewDialog1.Document = this.printDocument1;
            this.printPreviewDialog1.ShowDialog();
        }

        private void rtbSSLR_SSJL_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            this.panelRTB.Height = e.NewRectangle.Height + 50;
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void picMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
