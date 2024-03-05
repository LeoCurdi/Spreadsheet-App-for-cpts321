using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {

    /// <summary>
    /// A concrete inherited Node containing a binary operator.
    /// </summary>
    public abstract class BinaryOperatorNode : ExpressionTreeNode {

        /// <summary>
        /// The operation of the Node (+, -, *, /).
        /// </summary>
        //protected char operatorChar;

        /// <summary>
        /// The precedence of the operator (the importance in order of operations).
        /// </summary>
        public static int precedence;
        public int testField;
        public int Precedence { get { return precedence; } }

        protected enum Associativity {
            Left,
            Non,
            Right,
        };

        protected Associativity associativity;

        /// <summary>
        /// the left child Node.
        /// </summary>
        protected ExpressionTreeNode leftNode;

        /// <summary>
        /// The right child Node.
        /// </summary>
        protected ExpressionTreeNode rightNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperatorNode"/> class.
        /// </summary>
        /// <param name="operatorChar">The operator of the node.</param>
        /// <param name="left">The operator node's left child.</param>
        /// <param name="right">The operator node's right child.</param>
        public BinaryOperatorNode(/*char operatorChar, */ExpressionTreeNode left, ExpressionTreeNode right) {
            //this.operatorChar = operatorChar;
            this.leftNode = left;
            this.rightNode = right;
        }

        // were not required to impelement Evaluate() here, since this is also an abstract class.
    }
}
