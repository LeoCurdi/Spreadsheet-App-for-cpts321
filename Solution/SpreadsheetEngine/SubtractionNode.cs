using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {
    /// <summary>
    /// Concrete node for subtraction.
    /// Performs subtraction on left and right children.
    /// </summary>
    public class SubtractionNode : BinaryOperatorNode {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubtractionNode"/> class.
        /// </summary>
        /// <param name="left">Reference to the left child node.</param>
        /// <param name="right">Reference to the right child node.</param>
        public SubtractionNode(ExpressionTreeNode left, ExpressionTreeNode right)
            : base(left, right) {
            this.precedence = 1;
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
            result = leftResult - rightResult;

            return result;
        }
    }
}
