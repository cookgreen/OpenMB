using OpenMB.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Expression
{
    /// <summary>
    /// Expressions connected with a conditional operator
    /// </summary>
    public class ConditionalExpression : Expression
    {
        private List<ConditionalExpressionToken> tokens;
        private string conditionStr;
        private string[] conditionConnectionsArr =
        {
            "&&",
            "||"
        };
        
        public ConditionalExpression(string conditionStr)
        {
            this.conditionStr = conditionStr;
            tokens = new List<ConditionalExpressionToken>();
            Parse();
        }

        private void Parse()
        {
            ExpressionTreeNode lastTreeNode = null;
            string lastExpressionStr = conditionStr;

            bool hasConditionConnection = false;

            List<Tuple<string, int>> connPosDic = new List<Tuple<string, int>>();

            foreach(var conditionConnectionStr in conditionConnectionsArr)
            {
                if (conditionStr.Contains(conditionConnectionStr))
                {
                    int[] indics = conditionStr.IndexOfAll(conditionStr);
                    foreach (var index in indics)
                    {
                        connPosDic.Add(new Tuple<string, int>(conditionConnectionStr, index));
                    }
                }
            }

            if (!hasConditionConnection)
            {
                tokens.Add(new ConditionalExpressionToken(conditionStr));
            }
            else
            {
                connPosDic = (from pair in connPosDic
                              orderby pair.Item2 descending
                              select pair).ToList();

                int idx = 0;
                foreach (var pair in connPosDic)
                {
                    var part1 = lastExpressionStr.Substring(0, lastExpressionStr.Length - pair.Item2);
                    var part2 = lastExpressionStr.Substring(pair.Item2);

                    ExpressionTreeNode node = new ExpressionTreeNode();
                    node.Str = pair.Item1;
                    node.NodeType = ExpressionTreeNodeType.Operator;
                    ExpressionTreeNode leftNode = new ExpressionTreeNode();
                    leftNode.NodeType = ExpressionTreeNodeType.Expr;
                    leftNode.Str = part2;
                    node.LeftNode = leftNode;

                    if (idx == 0)
                    {
                        exprTree.Root = node;
                    }
                    else if (idx == connPosDic.Count - 1)
                    {
                        ExpressionTreeNode rightNode = new ExpressionTreeNode();
                        rightNode.NodeType = ExpressionTreeNodeType.Expr;
                        rightNode.Str = part2;
                        node.RightNode = rightNode;
                    }
                    else
                    {
                        lastTreeNode.RightNode = node;
                    }

                    lastTreeNode = node;
                    lastExpressionStr = part1;
                }
            }
        }

        public override bool Execute(params object[] exeArgs)
        {
            return ExecuteSub(exprTree.Root, exeArgs);
        }

        private bool ExecuteSub(ExpressionTreeNode treeNode, params object[] exeArgs)
        {
            if (treeNode.RightNode.NodeType == ExpressionTreeNodeType.Operator)
            {
                return ExecuteSub(treeNode.RightNode, exeArgs);
            }
            else
            {
                ConditionalExpressionToken exprTokenLeft = new ConditionalExpressionToken(treeNode.LeftNode.Str);
                ConditionalExpressionToken exprTokenRight = new ConditionalExpressionToken(treeNode.RightNode.Str);
                exprTokenLeft.Execute(exeArgs);
                exprTokenRight.Execute(exeArgs);
                Operator opt = ExpressionManager.Instance.GetOperator(treeNode.Str);
                opt.ExecuteOperator(exprTokenLeft.ReturnValue, exprTokenRight.ReturnValue);
                if(opt.ReturnValue == "True")
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
}
