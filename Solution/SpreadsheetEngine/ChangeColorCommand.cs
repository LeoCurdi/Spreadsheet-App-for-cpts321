using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {
    public class ChangeColorCommand : Command {

        private uint oldColor;
        private uint newColor;
        private List<Cell> cellsList;

        public ChangeColorCommand(List<Cell> cellsList, uint newColor) {
            this.oldColor = cellsList[0].BGColor;
            this.newColor = newColor;
            this.cellsList = cellsList;
        }

        public override void Execute() {
            Spreadsheet.SetCellColor(this.cellsList, this.newColor);
        }

        public override void Unexecute() {
            Spreadsheet.SetCellColor(this.cellsList, this.oldColor);
        }
    }
}
