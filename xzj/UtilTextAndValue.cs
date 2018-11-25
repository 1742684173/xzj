using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xzj
{
    class UtilTextAndValue
    {
        private int _RealValue;
        private string _DisplayText;

        public string DisplayText
        {
            get
            {
                return _DisplayText;
            }
        }

        public int RealValue
        {
            get
            {
                return _RealValue;
            }
        }

        public UtilTextAndValue(string ShowText, int RealVal)
        {
            _DisplayText = ShowText;
            _RealValue = RealVal;
        }

        public override string ToString()
        {
            return _RealValue.ToString();
        }
    }
}
