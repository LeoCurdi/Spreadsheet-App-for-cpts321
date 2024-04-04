// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {
    /// <summary>
    /// An abstract class for Commands.
    /// </summary>
    public abstract class Command {
        /// <summary>
        /// Gets the command's message.
        /// </summary>
        public abstract string Message { get; }

        /// <summary>
        /// Executes the command.
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Undoes the command.
        /// </summary>
        public abstract void Unexecute();
    }
}
