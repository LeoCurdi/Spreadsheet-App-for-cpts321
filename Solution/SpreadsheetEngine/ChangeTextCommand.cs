// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {
    /// <summary>
    /// Inherited command class for when the user changes the text of a cell.
    /// </summary>
    public class ChangeTextCommand : Command {
        /// <summary>
        /// The previous text of the cell.
        /// </summary>
        private string oldText;

        /// <summary>
        /// The new text of the cell.
        /// </summary>
        private string newText;

        /// <summary>
        /// The cell being manipulated.
        /// </summary>
        private Cell cell;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeTextCommand"/> class.
        /// </summary>
        /// <param name="cell">The cell being manipulated.</param>
        /// <param name="newText">The new text of the cell.</param>
        public ChangeTextCommand(Cell cell, string newText) {
            this.oldText = cell.Text;
            this.newText = newText;
            this.cell = cell;
        }

        /// <summary>
        /// Gets the command's message.
        /// </summary>
        public override string Message { get; } = "changed cell text";

        /// <summary>
        /// Executes the command.
        /// </summary>
        public override void Execute() {
            Spreadsheet.SetCellText(this.cell, this.newText);
        }

        /// <summary>
        /// Undoes the command.
        /// </summary>
        public override void Unexecute() {
            Spreadsheet.SetCellText(this.cell, this.oldText);
        }
    }
}
