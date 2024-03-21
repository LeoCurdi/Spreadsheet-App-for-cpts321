using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {
    public class OperatorNodeFactory {
        private Dictionary<char, Type> operatorNodeMap = new Dictionary<char, Type>();

        public OperatorNodeFactory() { }

        private delegate void OnOperator(char theOperator, Type type);

        /// <summary>
        /// Iterates all operator classes that are loaded.
        /// 
        /// </summary>
        /// <param name="onOperator"></param>
        private void TraverseAllOperators(OnOperator onOperator) {

        }

        public List<char> GetOperators() {
            return new List<char> { '(', ')', '[', };
        }

        public BinaryOperatorNode CreateOperatorNode(char oper) {

            return new SubtractionNode(null, null);
        }

        public int GetPrecedence(char oper) {
            return 0;
        }

        public Associativity GetAssociativity(char oper) {
            return Associativity.Right;
        }

        public bool IsOperator(char character) {
            return false;
        }
    }
}
