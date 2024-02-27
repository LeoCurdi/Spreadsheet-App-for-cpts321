using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {
    /// <summary>
    /// A concrete class for cells.
    /// We need a concrete inherited class since abstract base classes cant be instantiated.
    /// </summary>
    public class SpreadsheetCell : Cell {

        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetCell"/> class.
        /// </summary>
        /// <param name="row">Row index of the new cell.</param>
        /// <param name="column">Column index of the new cell.</param>
        public SpreadsheetCell(int row, int column) : base(row, column) { }

    }
}
