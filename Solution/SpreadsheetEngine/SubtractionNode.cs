// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

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
        }

        /// <summary>
        /// Gets the precedence of the operator.
        /// Set the value directly rather than in the constructor, since its static.
        /// </summary>
        public static int Precedence => 1;

        /// <summary>
        /// Gets the operator character corresponding to the type of operator node.
        /// </summary>
        public static char Operator => '-';

        /// <summary>
        /// Gets the associativity of the operator.
        /// Set the value directly rather than in the constructor, since its static.
        /// </summary>
        protected static Associativity Associativity => Associativity.Left;

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
