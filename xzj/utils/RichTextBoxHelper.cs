using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xzj.utils
{
    class RichTextBoxHelper
    {
        static RichTextBox splitRtb;
        private static void splitRtb_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            splitRtb.Height = e.NewRectangle.Height + 10;
        }

        public static System.Collections.Generic.List<String> splitRichTextBoxToRtfs(int MaxRichTextBoxHeight, int richTextBoxWidth, string rtf)
        {

            RichTextBox srcRtb = new RichTextBox();
            srcRtb.Rtf = rtf;

            splitRtb = new RichTextBox();
            splitRtb.Width = richTextBoxWidth;

            splitRtb.ContentsResized += new System.Windows.Forms.ContentsResizedEventHandler(splitRtb_ContentsResized);

            System.Collections.Generic.List<String> list = new List<string>();

            int index = 0;
            int i = 1;
            int lastSize = 0;

            while (true)
            {
                srcRtb.Select(index, i++);
                string s = srcRtb.SelectedRtf;
                splitRtb.Rtf = s;

                if (splitRtb.Height > MaxRichTextBoxHeight)
                {
                    i -= 2;
                    srcRtb.Select(index, i);
                    list.Add(srcRtb.SelectedRtf);
                    index += i;
                    i = 0;

                    splitRtb.Text = "";
                }
                else
                {
                    if (s.Length == lastSize)
                    {
                        list.Add(srcRtb.SelectedRtf);
                        break;
                    }
                    else
                    {
                        lastSize = s.Length;

                    }
                }
            }
            return list;
        }
    }
}
