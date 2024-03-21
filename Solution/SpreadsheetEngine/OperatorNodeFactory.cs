using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {
    /// <summary>
    /// A class whose job is to take in operator characters and create the correct operator node classes.
    /// </summary>
    public class OperatorNodeFactory {
        /// <summary>
        /// A mapping between operators and their type of concrete Node class.
        /// </summary>
        private Dictionary<char, Type> operatorNodeMap = new Dictionary<char, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNodeFactory"/> class.
        /// Populates the operatorNodeMap from all the loaded node sub classes in the assembly.
        /// </summary>
        public OperatorNodeFactory() {
            // call traverseAllOperators with the OnOperator function passed in as a parameter, and use the result to populate the dictionary.
            TraverseAllOperators((op, type) => operatorNodeMap.Add(op, type));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="type"></param>
        private delegate void OnOperator(char oper, Type type);

        /// <summary>
        /// Gets all operatorNode sub classes that are loaded in the assembly using LINQ.
        /// Calls the OnOperator function passed in as a parameter to populate the dictionary.
        /// </summary>
        /// <param name="onOperator"></param>
        private void TraverseAllOperators(OnOperator onOperator) {
            // get the type declaration of the operator node
            Type operatorNodeType = typeof(BinaryOperatorNode);

            // iterate over all loaded assemblies
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                // get all types that inherit from the operator node class using LINQ
                IEnumerable<Type> typesOfOperators = assembly.GetTypes().Where(type => type.IsSubclassOf(operatorNodeType));

                // for each subclass of the operator node
                foreach (var subclass in typesOfOperators) {

                    // get the operator char property of the node
                    PropertyInfo operatorField = subclass.GetProperty("Operator");

                    // error checking
                    if (operatorField != null) {
                        // get the value of the operator
                        object operatorValue = operatorField.GetValue(subclass);

                        // error checking
                        if (operatorValue is char) {
                            // cast it as a char
                            char operatorChar = (char)operatorValue;

                            // invoke the function passed as a parameter with the operator char and the subclass type
                            onOperator(operatorChar, subclass);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<char> GetOperators() {
            return new List<char> { '(', ')', '[', };
        }

        /// <summary>
        /// Takes an operator character, creates and returns the correct operatorNode sub class.
        /// </summary>
        /// <param name="oper">The operator char.</param>
        /// <returns>The created node.</returns>
        /// <exception cref="Exception">If a char is not a recognized operator.</exception>
        public BinaryOperatorNode CreateOperatorNode(char oper, ExpressionTreeNode left, ExpressionTreeNode right) {
            // if the operator is valid
            if (this.operatorNodeMap.ContainsKey(oper)) {
                Console.WriteLine("op is valid");
                // create an object of the corresponding operator sub class
                object operatorNodeSubclass = System.Activator.CreateInstance(this.operatorNodeMap[oper], new object[] { left, right });
                Console.WriteLine("making progress");
                // error checking
                if (operatorNodeSubclass is BinaryOperatorNode) {
                    // return the node
                    return (BinaryOperatorNode)operatorNodeSubclass;
                }
            }

            // else throw an exception
            throw new Exception("Invalid operator");
        }

        /// <summary>
        /// Takes an operator char and returns its precedence.
        /// </summary>
        /// <param name="oper">The operator char.</param>
        /// <returns>The precedence.</returns>
        public int GetPrecedence(char oper) {
            Type nodeType = this.operatorNodeMap[oper];
            PropertyInfo propertyInfo = nodeType.GetProperty("Precedence");
            if (propertyInfo != null) {
                object propertyValue = propertyInfo.GetValue(nodeType);
                if (propertyValue is int) {
                    return (int)propertyValue;
                }
            }

            return 0;
        }

        /// <summary>
        /// Takes an operator char and returns its associativity.
        /// </summary>
        /// <param name="oper">The operator char.</param>
        /// <returns>The associativity.</returns>
        public Associativity GetAssociativity(char oper) {
            Type nodeType = this.operatorNodeMap[oper];
            PropertyInfo propertyInfo = nodeType.GetProperty("Associativity");
            if (propertyInfo != null) {
                object propertyValue = propertyInfo.GetValue(nodeType);
                if (propertyValue is Associativity) {
                    return (Associativity)propertyValue;
                }
            }

            return Associativity.Non;
        }

        /// <summary>
        /// Takes in a char and returns whether it is a valid operator.
        /// </summary>
        /// <param name="character">The input character.</param>
        /// <returns>true or false.</returns>
        public bool IsOperator(char character) {
            return this.operatorNodeMap.ContainsKey(character);
        }
    }
}
