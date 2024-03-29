using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {
    public class UnhandledOperatorException : Exception {

        public UnhandledOperatorException() { }

        public UnhandledOperatorException(string message) :base(message) {

        }

        public UnhandledOperatorException(string message, Exception inner) : base(message, inner) {

        }
    }
}
