// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {
    /// <summary>
    /// The associativity type of an operator.
    /// </summary>
    public enum Associativity {
        /// <summary>
        /// Left associativity.
        /// </summary>
        Left,

        /// <summary>
        /// non associativity.
        /// </summary>
        Non,

        /// <summary>
        /// right associativity.
        /// </summary>
        Right,
    }

    /// <summary>
    /// A concrete inherited Node containing a binary operator.
    /// </summary>
    public abstract class BinaryOperatorNode : ExpressionTreeNode {
#pragma warning disable SA1401 // Fields should be private
        /// <summary>
        /// the left child Node.
        /// </summary>
        protected ExpressionTreeNode leftNode;

        /// <summary>
        /// The right child Node.
        /// </summary>
        protected ExpressionTreeNode rightNode;
#pragma warning restore SA1401 // Fields should be private

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperatorNode"/> class.
        /// </summary>
        /// <param name="left">The operator node's left child.</param>
        /// <param name="right">The operator node's right child.</param>
        public BinaryOperatorNode(ExpressionTreeNode left, ExpressionTreeNode right) {
            this.leftNode = left;
            this.rightNode = right;
        }

        // were not required to impelement Evaluate() here, since this is also an abstract class.
    }
}
