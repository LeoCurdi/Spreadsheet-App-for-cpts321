using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace SpreadsheetEngine {

    /// <summary>
    /// A class that takes in a string representation of an expression and evaluates it to a numeric value.
    /// </summary>
    public class ExpressionTree {
        /// <summary>
        /// A list of chars for the supported operators, and their importance in order of operations.
        /// </summary>
        private Dictionary<char, int> OperatorPrecedences = new Dictionary<char, int> {
            { '+', 1 },
            { '-', 1 },
            { '*', 2 },
            { '/', 2 },
        };

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
            // save the entered expression
            this.expression = expression;

            // build the tree
            this.BuildExpressionTree();
        }

        /// <summary>
        /// Sets the specified variable within the ExpressionTree variables dictionary.
        /// </summary>
        /// <param name="variableName">The name of the target variable.</param>
        /// <param name="variableValue">The value to set the variable to.</param>
        public void SetVariable(string variableName, double variableValue) {
            // if the variable exists in our dictionary
            if (this.variables.ContainsKey(variableName)) {
                // set the value
                this.variables[variableName] = variableValue;
            }
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

        /// <summary>
        /// Called by the constructor.
        /// Takes the given expression and builds the expression tree.
        /// </summary>
        private void BuildExpressionTree() {
            // tokenize the infix expression
            List<string> infixTokens = TokenizeInfixExpression(this.expression);

            // convert the tokenized expression from infix to postfix
            List<string> postfixTokens = ConvertExpressionToPostfix(infixTokens);

            // load the postfix expression into the tree
            this.rootNode = LoadPostfixTokensIntoTree(postfixTokens);
        }

        private List<string> TokenizeInfixExpression(string expression) {
            List<string> infixTokens = new List<string>();


            return infixTokens;
        }

        private List<string> ConvertExpressionToPostfix(List<string> infixTokens) {
            List<string> postfixTokens = new List<string>();

            return postfixTokens;
        }

        private ExpressionTreeNode LoadPostfixTokensIntoTree(List<string> postfixTokens) {

            return new VariableNode("memes", variables);
        }
    }
}
