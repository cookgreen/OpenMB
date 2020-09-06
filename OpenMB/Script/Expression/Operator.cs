using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Expression
{
    public class Operator
    {
        public virtual string ReturnValue { get; }
        public virtual string Name { get; }

        public virtual void ExecuteOperator(params object[] exeArgs)
        {

        }
    }
}
