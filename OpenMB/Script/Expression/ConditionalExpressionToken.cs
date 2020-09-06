using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Expression
{
    public class ConditionalExpressionToken : Expression
    {
        private string lhr;
        private string rhr;
        private Operator operate;
        private string str;

        public ConditionalExpressionToken(string str)
        {
            this.str = str;
        }

        public ConditionalExpressionToken(string lhr, string rhr, string opt)
        {
            this.lhr = lhr;
            this.rhr = rhr;
            operate = ExpressionManager.Instance.GetOperator(opt);
        }

        public bool Execute()
        {
            if(operate == null)
            {
                return false;
            }

            operate.ExecuteOperator(lhr, rhr);
            if (operate.ReturnValue == "True")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
