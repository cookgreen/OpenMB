using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Expression
{
    public class ConditionalOperator
    {
    }

    public class ConditionalEqualOperator : Operator
    {
        private string retValue;
        public override string ReturnValue { get { return retValue; } }
        public override string Name { get { return "=="; } }
        public override void ExecuteOperator(params object[] exeArgs)
        {
            var lhr = exeArgs[0].ToString();
            var rhr = exeArgs[1].ToString();

            if (lhr.StartsWith("%") || lhr.StartsWith("$"))
            {
                string lhrName = lhr.Substring(1);
                if (rhr.StartsWith("%") || rhr.StartsWith("$"))
                {
                    string rhrName = rhr.Substring(1);

                    int lhrAddr = ScriptValueStorage.Instance.GetVariableAddress(lhrName);
                    int rhrAddr = ScriptValueStorage.Instance.GetVariableAddress(rhrName);

                    object lhrValue = ScriptValueStorage.Instance.GetVariableValue(lhrName);
                    object rhrValue = ScriptValueStorage.Instance.GetVariableValue(rhrName);

                    if ((lhrName == rhrName) && (lhrAddr == rhrAddr) && (lhrValue == rhrValue))
                    {
                        retValue = "True";
                    }
                    else
                    {
                        retValue = "False";
                    }
                }
                else
                {
                    object lhrValue = ScriptValueStorage.Instance.GetVariableValue(lhrName);
                    if (lhrValue.ToString() == rhr)
                    {
                        retValue = "True";
                    }
                    else
                    {
                        retValue = "False";
                    }
                }
            }
            else if (rhr.StartsWith("$") || rhr.StartsWith("%"))
            {
                string rhrName = rhr.Substring(1);
                if (rhr.StartsWith("%") || rhr.StartsWith("$"))
                {
                    string lhrName = lhr.Substring(1);

                    int lhrAddr = ScriptValueStorage.Instance.GetVariableAddress(lhrName);
                    int rhrAddr = ScriptValueStorage.Instance.GetVariableAddress(rhrName);

                    object lhrValue = ScriptValueStorage.Instance.GetVariableValue(lhrName);
                    object rhrValue = ScriptValueStorage.Instance.GetVariableValue(rhrName);

                    if ((lhrName == rhrName) && (lhrAddr == rhrAddr) && (lhrValue == rhrValue))
                    {
                        retValue = "True";
                    }
                    else
                    {
                        retValue = "False";
                    }
                }
                else
                {
                    object rhrValue = ScriptValueStorage.Instance.GetVariableValue(rhrName);
                    if (rhrValue.ToString() == lhr)
                    {
                        retValue = "True";
                    }
                    else
                    {
                        retValue = "False";
                    }
                }
            }
            else
            {
                if (lhr == rhr)
                {
                    retValue = "True";
                }
                else
                {
                    retValue = "False";
                }
            }
        }
    }

    public class ConditionalGreaterThanOperator : Operator
    {
        private string retValue;
        public override string ReturnValue { get { return retValue; } }
        public override string Name { get { return ">"; } }
        public override void ExecuteOperator(params object[] exeArgs)
        {
            var lhr = exeArgs[0].ToString();
            var rhr = exeArgs[1].ToString();
            if (int.Parse(lhr) > int.Parse(rhr))
            {
                retValue = "True";
            }
            else
            {
                retValue = "False";
            }
        }
    }

    public class ConditionalLessThanOperator : Operator
    {
        private string retValue;
        public override string ReturnValue { get { return retValue; } }
        public override string Name { get { return "<"; } }
        public override void ExecuteOperator(params object[] exeArgs)
        {
            var lhr = exeArgs[0].ToString();
            var rhr = exeArgs[1].ToString();
            if (int.Parse(lhr) < int.Parse(rhr))
            {
                retValue = "True";
            }
            else
            {
                retValue = "False";
            }
        }
    }

    public class ConditionalAndOperator : Operator
    {
        private string retValue;
        public override string ReturnValue { get { return retValue; } }
        public override string Name { get { return "&&"; } }
        public override void ExecuteOperator(params object[] exeArgs)
        {
            var lhr = exeArgs[0].ToString();
            var rhr = exeArgs[1].ToString();
            if (lhr == "True" && rhr == "False")
            {
                retValue = "False";
            }
            else if (lhr == "True" && rhr == "True")
            {
                retValue = "True";
            }
            else if (lhr == "False" && rhr == "False")
            {
                retValue = "False";
            }
            else if (lhr == "False" && rhr == "True")
            {
                retValue = "False";
            }
            else
            {
                retValue = "Unknown";
            }
        }
    }

    public class ConditionalOrOperator : Operator
    {
        private string retValue;
        public override string ReturnValue { get { return retValue; } }
        public override string Name { get { return "||"; } }
        public override void ExecuteOperator(params object[] exeArgs)
        {
            var lhr = exeArgs[0].ToString();
            var rhr = exeArgs[1].ToString();
            if (lhr == "True" && rhr == "False")
            {
                retValue = "True";
            }
            else if (lhr == "True" && rhr == "True")
            {
                retValue = "True";
            }
            else if (lhr == "False" && rhr == "False")
            {
                retValue = "False";
            }
            else if (lhr == "False" && rhr == "True")
            {
                retValue = "True";
            }
            else
            {
                retValue = "Unknown";
            }
        }
    }
}
