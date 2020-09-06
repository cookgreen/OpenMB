using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMB.Script.Expression
{
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
            foreach(var connStr in conditionConnectionsArr)
            {
                if (conditionStr.Contains(connStr))
                {
                    var splitTokens = conditionStr.Split(connStr.ToCharArray());
                    var leftStr = splitTokens[0];
                    var RightStr = splitTokens[1];
                    ExpressionTreeNode node = new ExpressionTreeNode();
                    node.Str = connStr;
                    exprTree.Root = node;
                    ParseSub(leftStr, node);
                    ParseSub(RightStr, node);
                    break;
                }
            }
        }

        private void ParseSub(string condStr, ExpressionTreeNode node)
        {
            bool isContains = false;
            foreach (var connStr in conditionConnectionsArr)
            {
                if (condStr.Contains(connStr))
                {
                    var splitTokens = condStr.Split(connStr.ToCharArray());
                    var leftStr = splitTokens[0];
                    var RightStr = splitTokens[1];
                    ExpressionTreeNode subNode = new ExpressionTreeNode();
                    subNode.Str = connStr;
                    subNode.NodeType = ExpressionTreeNodeType.Operator;
                    node.RightNode = subNode;
                    ParseSub(leftStr, subNode);
                    ParseSub(RightStr, subNode);
                    isContains = true;
                }
            }

            if (!isContains)
            {
                if (node.LeftNode == null)
                {
                    ExpressionTreeNode leftNode = new ExpressionTreeNode();
                    leftNode.Str = condStr;
                    leftNode.NodeType = ExpressionTreeNodeType.Expr;
                    node.LeftNode = leftNode;
                }
                else if (node.RightNode == null)
                {
                    ExpressionTreeNode rightNode = new ExpressionTreeNode();
                    rightNode.Str = condStr;
                    rightNode.NodeType = ExpressionTreeNodeType.Expr;
                    node.RightNode = rightNode;
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
