using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
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
        /// A mapping between operators and their type of concrete Node class.
        /// </summary>
        private Dictionary<char, Type> operatorNodeMap = new Dictionary<char, Type>();

        /// <summary>
        /// A lookup table for the precedences of each operator.
        /// Note: this was meant to be removed with the refactoring, which will have to wait until hw6.
        /// </summary>
        private Dictionary<char, int> operatorPrecedences = new Dictionary<char, int>() {
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

            // populate the operator map
            operatorNodeMap.Clear();
            operatorNodeMap['+'] = typeof(AdditionNode);
            operatorNodeMap['-'] = typeof(SubtractionNode);
            operatorNodeMap['*'] = typeof(MultiplicationNode);
            operatorNodeMap['/'] = typeof(DivisionNode);

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

        /// <summary>
        /// 
        /// Works for expressions with parentheses.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private List<string> TokenizeInfixExpression(string expression) {
            Console.WriteLine("Tokenizing: " + expression);
            Console.Write("Infix tokens: ");

            List<string> infixTokens = new List<string>();

            // Regex stuff (Code help from: https://www.bytehide.com/blog/regex-csharp)
            // brackets are like parens, | means or, * means 0 or more, \ is an escape code, ? means the item is optional.
            // Operators: "[\+\-\*\/\(\)]": finds any single char operators (+, -, *, /, (, )).
            // Variables: "[a-zA-Z][a-zA-Z0-9]*": finds any variables that start with 1 letter followed by any combination of 0 or more letters or numbers.
            // Constants: "\d+(\.\d+)?": finds numeric values of one or more digits with an optional dot followed by one or more digits for decimal numbers.
            string regexPattern = @"([\+\-\*\/\(\)])|([a-zA-Z][a-zA-Z0-9]*)|(\d+(\.\d+)?)"; // Regex pattern for operators, variables, and numbers. @"(operators)|(variables)|(constants)"
            MatchCollection matches = Regex.Matches(expression, regexPattern); // use Regex to get all the individual items from the expression (assuming they are valid)
            foreach (Match m in matches) { // for each parsed item
                Console.Write(m.ToString() + " ");
                infixTokens.Add(m.Value); // add each item to the list of tokens
            } // End of code help from: https://www.bytehide.com/blog/regex-csharp
            Console.WriteLine();
            return infixTokens;
        }

        /// <summary>
        /// Uses Shunting Yard algorithm to convert infix to postfix.
        /// </summary>
        /// <param name="infixTokens"></param>
        /// <returns></returns>
        private List<string> ConvertExpressionToPostfix(List<string> infixTokens) {
            Console.Write("Postfix tokens: ");

            // We need a list to store the postfix tokens and a stack for the algorithm
            List<string> postfixTokens = new List<string>();
            Stack<char> operatorStack = new Stack<char>();

            // iterate over every token in the input list
            foreach (string token in infixTokens) {
                // if token is an operator
                if (this.IsOperator(token)) {
                    // while there are operators on top of the stack with greater or equal precedence, pop them into the result list.
                    while (operatorStack.Count > 0) {
                        string stackTop = operatorStack.Peek().ToString(); // get the top of the stack
                        if (IsOperator(stackTop)) { // if its an operator
                            // get the precedence of current token and stack top
                            //Type nodeType = operatorNodeMap[stackTop[0]];
                            //FieldInfo fieldInfo = nodeType.GetField("testField");
                            //if (fieldInfo != null) Console.WriteLine("not null");
                            //int stackTopPrecedence = (int)fieldInfo.GetValue(null);

                            //System.Reflection.MemberInfo info = typeof(AdditionNode);
                            //object[] attributes = info.GetCustomAttributes(true);
                            //for (int i = 0; i < attributes.Length; i++) {
                            //    System.Console.WriteLine(attributes[i]);
                            //}

                            //FieldInfo fieldInfo = nodeType.GetField("precedence", BindingFlags.Public | BindingFlags.Static);
                            //if (fieldInfo != null) {
                            //    int precedenceValue = (int)fieldInfo.GetValue(null);
                            //    // Use precedenceValue as needed
                            //} else {
                            //    Console.WriteLine("Precedence field not found.");
                            //}

                            //int stackTopPrecedence = (int)nodeType.GetProperty("Precedence", BindingFlags.Static | BindingFlags.Public).GetValue(null);
                            //nodeType = operatorNodeMap[token[0]];
                            //int tokenPrecedence = (int)nodeType.GetProperty("Precedence", BindingFlags.Static | BindingFlags.Public).GetValue(null);

                            //Object value = nodeType.GetProperty("Precedence");
                            //Console.WriteLine("\nvalue: " + value.ToString() + "\n");

                            //int stackTopPrecedence = nodeType.GetProperty("Precedence").GetValue(null);
                            //nodeType = operatorNodeMap[token[0]];
                            //int tokenPrecedence = (int)nodeType.GetProperty("Precedence").GetValue(null);

                            if (operatorPrecedences[stackTop[0]] >= operatorPrecedences[token[0]]) { // if the tops precedence is greater equal
                                postfixTokens.Add(operatorStack.Pop().ToString()); // pop it from the stack into the result list
                            }
                            else { // if top is not equal or greater precedence stop the loop
                                break;
                            }
                        } else { // if top is not equal or greater precedence stop the loop
                            break;
                        }
                    }

                    // push the current operator to the stack
                    operatorStack.Push(token[0]);
                }

                // if token is an opening paren
                else if (token == "(") {
                    // push the opening paren to the stack
                    operatorStack.Push(token[0]);
                }

                // if token is a closing paren
                else if (token == ")") {
                    // while the top of the stack isnt an opening paren, pop each operator into the result list.
                    while (operatorStack.Count > 0 && operatorStack.Peek() != '(') {
                        postfixTokens.Add(operatorStack.Pop().ToString());
                    }

                    // pop and discard the opening  paren
                    operatorStack.Pop();
                }

                // if token is an operand
                else if (this.IsConstant(token) || this.IsVariable(token)) {
                    // just insert operands directly to the result
                    postfixTokens.Add(token);
                }
            }

            // pop any remaining operators onto the end of the result
            while (operatorStack.Count > 0) {
                postfixTokens.Add(operatorStack.Pop().ToString());
            }

            foreach (string token in postfixTokens) {
                Console.Write(token.ToString() + " ");
            }
            Console.WriteLine();

            return postfixTokens;
        }

        private ExpressionTreeNode LoadPostfixTokensIntoTree(List<string> postfixTokens) {
            // use a stack to get the postfix tokens in reverse
            Stack<ExpressionTreeNode> stack = new Stack<ExpressionTreeNode>();

            // iterate over each token
            foreach (string token in postfixTokens) {
                // if its an operator
                if (this.IsOperator(token)) {
                    // take the top two tokens in the stack as the right and left children of the operator
                    if (stack.Count < 2) { // error case
                        throw new ArgumentException("Expression is invalid.");
                    }

                    ExpressionTreeNode right = stack.Pop();
                    ExpressionTreeNode left = stack.Pop();

                    // create the correct operator node and push it to the stack
                    Type operatorNodetype = this.operatorNodeMap[token[0]]; // get the type of node corresponding to the operator
                    ExpressionTreeNode operatorNode = (ExpressionTreeNode)Activator.CreateInstance(operatorNodetype, new object[] { left, right }); // create an instance of the type of node, then cast it as a base class since we don't know what the instance type is
                    stack.Push(operatorNode);
                }

                // if its a variable
                else if (this.IsVariable(token)) {
                    stack.Push(new VariableNode(token, this.variables)); // create a varibale node and push it to the stack
                }

                // if its a constant
                else if (this.IsConstant(token)) {
                    stack.Push(new ConstantNode(double.Parse(token))); // create a constant node and push it to the stack
                }
            }

            // error case
            if (stack.Count != 1) {
                throw new ArgumentException("Expression is invalid.");
            }

            // return the root node
            return stack.Pop();
        }

        /// <summary>
        /// Helper for checking if a token is an operator.
        /// Checks if the token exists in the operator dictionary.
        /// </summary>
        /// <param name="token">The token that we want to see is an operator.</param>
        /// <returns>A true or false.</returns>
        private bool IsOperator(string token) {
            return operatorNodeMap.ContainsKey(token[0]);
        }

        private bool IsVariable(string token) {
            return Regex.IsMatch(token, @"([a-zA-Z][a-zA-Z0-9]*)");
        }

        private bool IsConstant(string token) {
            return Regex.IsMatch(token, @"(\d+(\.\d+)?)");
        }
    }
}
