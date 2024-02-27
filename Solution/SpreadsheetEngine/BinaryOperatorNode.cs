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
        public BinaryOperatorNode(char operatorChar) {
            this.operatorChar = operatorChar;
        }

        /// <summary>
        /// Implementation of abstract Evaluate.
        /// Evaluates the node to get the resulting value.
        /// </summary>
        /// <returns>A double containing the result of evaluation of the node.</returns>
        public override double Evaluate() {
            throw new NotImplementedException();
        }
    }
}
