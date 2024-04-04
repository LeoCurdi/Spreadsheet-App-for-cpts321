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

        public ChangeTextCommand(Cell cell, string newText) {
            this.oldText = cell.Text;
            this.newText = newText;
            this.cell = cell;
        }

        public override void Execute() {
            throw new NotImplementedException();
        }

        public override void Unexecute() {
            throw new NotImplementedException();
        }
    }
}
