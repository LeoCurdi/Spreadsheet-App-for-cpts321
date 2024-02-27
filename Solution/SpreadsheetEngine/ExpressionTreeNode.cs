using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {

    /// <summary>
    /// Abstract base Node class.
    /// </summary>
    public abstract class ExpressionTreeNode {
        /// <summary>
        /// Abstract function for evaulating the current node.
        /// Function must be overridden in inherited classes.
        /// </summary>
        /// <returns>A double containing the result of evaluation of the node.</returns>
        public abstract double Evaluate();
    }
}
