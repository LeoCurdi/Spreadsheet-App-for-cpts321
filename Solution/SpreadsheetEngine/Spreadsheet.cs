// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine {

    /// <summary>
    /// Serves as a container for a 2D array of cells, and a factory for cells (cells will be created here.
    /// </summary>
    public class Spreadsheet {

        /// <summary>
        /// A 2D array of cells.
        /// </summary>
        public Cell[,] cellArray;

        public event PropertyChangedEventHandler CellPropertyChanged = delegate { };

        /// <summary>
        /// Gets a property that returns the number of columns in the spreadhseet.
        /// </summary>
        public int ColumnCount {
            get {
                return this.cellArray.GetLength(1); // get the length of the column dimension in the array
            }
        }

        /// <summary>
        /// Gets a property that returns the number of rows in the spreadhseet.
        /// </summary>
        public int RowCount {
            get {
                return this.cellArray.GetLength(0); // get the length of the row dimension in the array
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// Constructor.
        /// Allocates space for a 2D array of cells.
        /// Initializes the array of cells.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        public Spreadsheet(int rows, int columns) {
            // initialize the cellArray
            this.cellArray = new Cell[rows, columns];
            // initialize each cell and subscribe the sheet to cell's property changed event for every cell
            for (int row = 0; row < rows; row++) {
                for (int column = 0; column < columns; column++) {
                    this.cellArray[row, column] = new SpreadsheetCell(row, column);
                    this.cellArray[row, column].PropertyChanged += this.Cell_PropertyChanged;
                }
            }
        }

        /// <summary>
        /// If the text of a cell changes, we need to evaluate the text and change the cell value here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cell_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            

        }

        /// <summary>
        /// Finds the cell located at row and column.
        /// </summary>
        /// <param name="rowIndex">The row of the cell.</param>
        /// <param name="columnIndex">The column of the cell.</param>
        /// <returns>a reference to the cell at a specified row and column.</returns>
        public Cell GetCell(int rowIndex, int columnIndex) {
            return new SpreadsheetCell(0,0);
        }

    }
}
