using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {

    /// <summary>
    /// A class that takes in a string representation of an expression and evaluates it to a numeric value.
    /// </summary>
    public class ExpressionTree {
        /// <summary>
        /// A list of chars for the supported operators.
        /// </summary>
        private char[] operators = { '+', '-', '*', '/' };

        /// <summary>
        /// The entered expression.
        /// </summary>
        private string expression;

        /// <summary>
        /// The root node of the expression tree.
        /// </summary>
        private ExpressionTreeNode rootNode;

        /// <summary>
        /// A lookup table that stores the corresponding numeric value for every variable.
        /// </summary>
        private Dictionary<string, double> variables = new Dictionary<string, double>();


        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// Constructs the tree from an expression.
        /// </summary>
        public ExpressionTree(string expression) {
            
        }

        /// <summary>
        /// Sets the specified variable within the ExpressionTree variables dictionary.
        /// </summary>
        /// <param name="variableName">The name of the target variable.</param>
        /// <param name="variableValue">The value to set the variable to.</param>
        public void SetVariable(string variableName, double variableValue) {

        }

        /// <summary>
        /// Evaluates the expression to a double value.
        /// </summary>
        /// <returns>A double contianing the result of evaluating the tree.</returns>
        public double Evaluate() {
            // make sure root is not null
            if (this.rootNode != null) {
                return this.rootNode.Evaluate();
            }

            return -1;
        }
    }
}
