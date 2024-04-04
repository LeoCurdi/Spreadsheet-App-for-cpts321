using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {
    public class ChangeTextCommand : Command {

        private string oldText;
        private string newText;
        private Cell cell;

        public override string Message { get; } = "changed cell text";

        public ChangeTextCommand(Cell cell, string newText) {
            this.oldText = cell.Text;
            this.newText = newText;
            this.cell = cell;
        }

        public override void Execute() {
            Spreadsheet.SetCellText(this.cell, this.newText);
        }

        public override void Unexecute() {
            Spreadsheet.SetCellText(this.cell, this.oldText);
        }
    }
}
