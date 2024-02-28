﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {

    /// <summary>
    /// A concrete inherited Node containing a raw numerical value.
    /// </summary>
    public class ConstantNode : ExpressionTreeNode {

        /// <summary>
        /// The value of the constant.
        /// </summary>
        private double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantNode"/> class.
        /// </summary>
        /// <param name="value">The value of the Node.</param>
        public ConstantNode(double value) {
            this.value = value;
        }

        /// <summary>
        /// Implementation of abstract Evaluate.
        /// Evaluates the node to get the resulting value.
        /// </summary>
        /// <returns>A double containing the result of evaluation of the node.</returns>
        public override double Evaluate() {
            return this.value;
        }
    }
}
