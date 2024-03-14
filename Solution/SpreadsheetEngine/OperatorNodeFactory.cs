using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {
    public class OperatorNodeFactory {
        private Dictionary<char, Type> operatorNodeMap = new Dictionary<char, Type>();

        public OperatorNodeFactory() { }

        private delegate void OnOperator(char theOperator, Type type);

        private void TraverseAllOperators(OnOperator onOperator) {

        }

        public List<char> GetOperators() {
            return new List<char> { '(', ')', '[', };
        }

        public BinaryOperatorNode CreateOperatorNode(char theOperator) {
            return new AdditionNode(null, null);
        }

        public int GetPrecedence(char theOperator) {
            return 0;
        }

        public Associativity GetAssociativity(char theOperator) {
            return Associativity.Left;
        }

        public bool IsOperator(char aChar) {
            return false;
        }
    }
}
