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
        ///
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public SpreadsheetCell(int row, int column) : base(row, column) { }

    }
}
