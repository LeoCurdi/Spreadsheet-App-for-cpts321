using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {

    /// <summary>
    /// A concrete inherited Node containing a variable in the expression.
    /// </summary>
    public class VariableNode : ExpressionTreeNode {

        /// <summary>
        /// The raw text of the variable.
        /// </summary>
        private string name;

        /// <summary>
        /// A reference to the variable dictionary in ExpressionTree class.
        /// </summary>
        private Dictionary<string, double> variables;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// </summary>
        /// <param name="name">The name of the variable.</param>
        /// <param name="variables">A reference to the dictionary that is in ExpressionTree class.</param>
        public VariableNode(string name, Dictionary<string, double> variables) {
            this.name = name;
            this.variables = variables;
        }

        /// <summary>
        /// Implementation of abstract Evaluate.
        /// Evaluates the node to get the resulting value.
        /// </summary>
        /// <returns>A double containing the result of evaluation of the node.</returns>
        public override double Evaluate() {
            // use the dictionary to get the value associated with the variable
            return this.variables[this.name];
        }
    }
}
