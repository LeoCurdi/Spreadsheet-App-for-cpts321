// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {
    /// <summary>
    /// Concrete node for division.
    /// Performs division on left and right children.
    /// </summary>
    public class DivisionNode : BinaryOperatorNode {
        /// <summary>
        /// Initializes a new instance of the <see cref="DivisionNode"/> class.
        /// </summary>
        /// <param name="left">Reference to the left child node.</param>
        /// <param name="right">Reference to the right child node.</param>
        public DivisionNode(ExpressionTreeNode left, ExpressionTreeNode right)
            : base(left, right) {
            precedence = 2;
            this.associativity = Associativity.Left;
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
            if (rightResult != 0) {
                result = leftResult / rightResult;
            }
            else {
                // dividing by zero - throw exception
                throw new Exception("Cannot divide by zero");
            }

            return result;
        }
    }
}
