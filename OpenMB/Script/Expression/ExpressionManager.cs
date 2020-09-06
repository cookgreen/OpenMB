using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Expression
{
    public class ExpressionManager
    {
        private static ExpressionManager instance;
        public static ExpressionManager Instance
        {
            get { if (instance == null) { instance = new ExpressionManager(); } return instance; }
        }

        private List<Operator> avaliableOperators;

        public ExpressionManager()
        {
            avaliableOperators.Add(new ConditionalEqualOperator());
            avaliableOperators.Add(new ConditionalGreaterThanOperator());
            avaliableOperators.Add(new ConditionalLessThanOperator());
            avaliableOperators.Add(new ConditionalAndOperator());
        }

        public Operator GetOperator(string opt)
        {
            return avaliableOperators.Where(o=>o.Name == opt).FirstOrDefault();
        }
    }
}
