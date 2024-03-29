// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {
    /// <summary>
    /// Represents and exception that is thrown when an unhandled operator is encountered.
    /// </summary>
    public class UnhandledOperatorException : Exception {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledOperatorException"/> class.
        /// </summary>
        public UnhandledOperatorException() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledOperatorException"/> class.
        /// </summary>
        /// <param name="message">An error message.</param>
        public UnhandledOperatorException(string message)
            : base(message) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledOperatorException"/> class.
        /// </summary>
        /// <param name="message">An error message.</param>
        /// <param name="inner">A reference to an inner exception that is the cause of this exception.</param>
        public UnhandledOperatorException(string message, Exception inner)
            : base(message, inner) {
        }
    }
}
