using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {
    public class ChangeColorCommand : Command {

        private List<uint> oldColors;
        private uint newColor;
        private List<Cell> cellsList;

        public override string Message { get; } = "changed cell background color";

        public ChangeColorCommand(List<Cell> cellsList, List<uint> oldCellColors, uint newColor) {
            this.oldColors = oldCellColors;
            this.newColor = newColor;
            this.cellsList = cellsList;
        }

        public override void Execute() {
            Spreadsheet.SetCellColor(this.cellsList, this.newColor);
        }

        public override void Unexecute() {
            Spreadsheet.SetCellColor(this.cellsList, this.oldColors);
        }
    }
}
