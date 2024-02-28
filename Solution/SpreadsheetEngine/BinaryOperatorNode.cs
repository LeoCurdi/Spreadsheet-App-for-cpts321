using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {

    /// <summary>
    /// A concrete inherited Node containing a binary operator.
    /// </summary>
    public class BinaryOperatorNode : ExpressionTreeNode {

        /// <summary>
        /// The operation of the Node (+, -, *, /).
        /// </summary>
        private char operatorChar;

        /// <summary>
        /// the left child Node.
        /// </summary>
        private ExpressionTreeNode leftNode;

        /// <summary>
        /// The right child Node.
        /// </summary>
        private ExpressionTreeNode rightNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperatorNode"/> class.
        /// </summary>
        /// <param name="operatorChar">The operator of the node.</param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public BinaryOperatorNode(char operatorChar, ExpressionTreeNode left, ExpressionTreeNode right) {
            this.operatorChar = operatorChar;
            this.leftNode = left;
            this.rightNode = right;
        }

        /// <summary>
        /// Implementation of abstract Evaluate.
        /// Evaluates the node to get the resulting value.
        /// </summary>
        /// <returns>A double containing the result of evaluation of the node.</returns>
        public override double Evaluate() {
            double result = 0;

            // evaluate the left and right child
            double leftResult = this.leftNode.Evaluate();
            double rightResult = this.rightNode.Evaluate();

            // perform the operation on the left and right values
            switch (this.operatorChar) {
                case '+':
                    result = leftResult + rightResult;
                    break;
                case '-':
                    result = leftResult - rightResult;
                    break;
                case '*':
                    result = leftResult * rightResult;
                    break;
                case '/':
                    if (rightResult != 0) {
                        result = leftResult / rightResult;
                        break;
                    }
                    else {
                        // dividing by zero - throw exception
                        throw new Exception("Cannot divide by zero");
                    }

                default:
                    // invalid operator - throw exception
                    throw new Exception("Invalid operator: " + this.operatorChar);
            }

            return result;
        }
    }
}
