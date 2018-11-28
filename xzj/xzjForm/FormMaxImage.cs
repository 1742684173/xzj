using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xzj.xzjForm
{
    public partial class FormMaxImage : Form
    {
        public FormMaxImage(Image image)
        {
            InitializeComponent();
            this.pictureBox1.Image = image;
            this.Height = this.pictureBox1.Image.Height + 50;
            this.Width = this.pictureBox1.Image.Width + 20;
            // this.pictureBox1.Layou

        }
    }
}
