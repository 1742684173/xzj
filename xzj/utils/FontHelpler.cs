using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xzj.utils
{
    class FontHelpler
    {
        // B I U
        // regular
        // B
        // I
        // U
        // BI
        // BU
        // IU
        // BIU

        private static void toRegular(RichTextBox rtb) {
            rtb.SelectionFont = new Font(rtb.Font.FontFamily, rtb.Font.Size, FontStyle.Regular);
        }
        private static void toB(RichTextBox rtb)
        {
            rtb.SelectionFont = new Font(rtb.Font.FontFamily, rtb.Font.Size, FontStyle.Bold);
        }
        private static void toI(RichTextBox rtb)
        {
            rtb.SelectionFont = new Font(rtb.Font.FontFamily, rtb.Font.Size, FontStyle.Italic);
        }
        private static void toU(RichTextBox rtb)
        {
            rtb.SelectionFont = new Font(rtb.Font.FontFamily, rtb.Font.Size, FontStyle.Underline);
        }
        
        private static void toBI(RichTextBox rtb)
        {
            rtb.SelectionFont = new Font(rtb.Font.FontFamily, rtb.Font.Size, FontStyle.Bold | FontStyle.Italic);
        }
        private static void toBU(RichTextBox rtb)
        {
            rtb.SelectionFont = new Font(rtb.Font.FontFamily, rtb.Font.Size, FontStyle.Bold | FontStyle.Underline);
        }
        private static void toIU(RichTextBox rtb)
        {
            rtb.SelectionFont = new Font(rtb.Font.FontFamily, rtb.Font.Size, FontStyle.Italic | FontStyle.Underline);
        }
        private static void toBIU(RichTextBox rtb)
        {
            rtb.SelectionFont = new Font(rtb.Font.FontFamily, rtb.Font.Size, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
        }



        public static void changeFontTo(RichTextBox rtb, string toName)
        {

            Boolean isBold = rtb.SelectionFont.Bold;
            Boolean isItalic = rtb.SelectionFont.Italic;
            Boolean isUnderline = rtb.SelectionFont.Underline;
            // Font f = this.rtbSSLR_SSJL.SelectionFont;


            if (toName.Equals("bold"))
            {
                if (isBold)
                {
                    if (isItalic)
                    {
                        if (isUnderline)
                        {
                            toIU(rtb);
                        }
                        else
                        {
                            toI(rtb);
                        }
                    }
                    else
                    {
                        if (isUnderline)
                        {
                            toU(rtb);
                        }
                        else
                        {
                            toRegular(rtb);
                        }
                    }
                }
                else
                {
                    if (isItalic)
                    {
                        if (isUnderline)
                        {
                            toBIU(rtb);
                        }
                        else
                        {
                            toBI(rtb);
                        }
                    }
                    else
                    {
                        if (isUnderline)
                        {
                            toBU(rtb);
                        }
                        else
                        {
                            toB(rtb);
                        }
                    }

                }
            }





            if (toName.Equals("italic"))
            {
                if (isBold)
                {
                    if (isItalic)
                    {
                        if (isUnderline)
                        {
                            toBU(rtb);
                        }
                        else
                        {
                            toB(rtb);
                        }
                    }
                    else
                    {
                        if (isUnderline)
                        {
                            toBIU(rtb);
                        }
                        else
                        {
                            toBI(rtb);
                        }
                    }
                }
                else
                {
                    if (isItalic)
                    {
                        if (isUnderline)
                        {
                            toU(rtb);
                        }
                        else
                        {
                            toRegular(rtb);
                        }
                    }
                    else
                    {
                        if (isUnderline)
                        {
                            toIU(rtb);
                        }
                        else
                        {
                            toI(rtb);
                        }
                    }
                }
            }



            if (toName.Equals("underline"))
            {
                if (isBold)
                {
                    if (isItalic)
                    {
                        if (isUnderline)
                        {
                            toBI(rtb);
                        }
                        else
                        {
                            toBIU(rtb);
                        }
                    }
                    else
                    {
                        if (isUnderline)
                        {
                            toB(rtb);
                        }
                        else
                        {
                            toBU(rtb);
                        }
                    }
                }
                else
                {
                    if (isItalic)
                    {
                        if (isUnderline)
                        {
                            toI(rtb);
                        }
                        else
                        {
                            toIU(rtb);
                        }
                    }
                    else
                    {
                        if (isUnderline)
                        {
                            toRegular(rtb);
                        }
                        else
                        {
                            toU(rtb);
                        }
                    }
                }
            }


        }
    }
}
