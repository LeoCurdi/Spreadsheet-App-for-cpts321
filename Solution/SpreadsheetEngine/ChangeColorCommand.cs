// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {
    /// <summary>
    /// Inherited command class for when the user changes the color of cells.
    /// </summary>
    public class ChangeColorCommand : Command {
        /// <summary>
        /// A list of old colors corresponding to the cells.
        /// </summary>
        private List<uint> oldColors;

        /// <summary>
        /// The new color to set the cells to.
        /// </summary>
        private uint newColor;

        /// <summary>
        /// A list of the cells being manipulated.
        /// </summary>
        private List<Cell> cellsList;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeColorCommand"/> class.
        /// </summary>
        /// <param name="cellsList">A list of the cells being manipulated.</param>
        /// <param name="oldCellColors">A list of old colors corresponding to the cells.</param>
        /// <param name="newColor">The new color to set the cells to.</param>
        public ChangeColorCommand(List<Cell> cellsList, List<uint> oldCellColors, uint newColor) {
            this.oldColors = oldCellColors;
            this.newColor = newColor;
            this.cellsList = cellsList;
        }

        /// <summary>
        /// Gets the command's message.
        /// </summary>
        public override string Message { get; } = "changed cell background color";

        /// <summary>
        /// Executes the command.
        /// </summary>
        public override void Execute() {
            Spreadsheet.SetCellColor(this.cellsList, this.newColor);
        }

        /// <summary>
        /// Undoes the command.
        /// </summary>
        public override void Unexecute() {
            Spreadsheet.SetCellColor(this.cellsList, this.oldColors);
        }
    }
}
