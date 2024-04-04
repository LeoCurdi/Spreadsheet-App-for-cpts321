// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Specialized;
using System.ComponentModel;

namespace SpreadsheetEngine {
    /// <summary>
    /// This class represents one cell in the worksheet.
    /// It is an abstract base class, because we are creating a library, so we want our library to be usable in many applications thus classes should be able to inherit from a basic cell class.
    /// public so that projects that use the library can access it.
    /// </summary>
#pragma warning disable SA1505 // Opening braces should not be followed by blank line
    public abstract class Cell {
#pragma warning restore SA1505 // Opening braces should not be followed by blank line

        /// <summary>
        /// The text entered into the cell by the user.
        /// Protected so that SpreadsheetCell can inherit it.
        /// </summary>
#pragma warning disable SA1401 // Fields should be private
        protected string text = "\0";

        /// <summary>
        /// The evaluated text, which is actually displayed in the cell.
        /// Protected so that SpreadsheetCell can inherit it.
        /// </summary>
        protected string value = "\0";

        /// <summary>
        /// The background color of the cell.
        /// </summary>
        protected uint bGColor = 0xFFFFFFFF;
#pragma warning restore SA1401 // Fields should be private

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="row">Row index of the new cell.</param>
        /// <param name="column">Column index of the new cell.</param>
        public Cell(int row, int column) {
            this.RowIndex = row;
            this.ColumnIndex = column;
        }

        /// <summary>
        /// Gets read only property for row index.
        /// </summary>
        public int RowIndex {
            get;
        }

        /// <summary>
        /// Gets read only property for column index.
        /// </summary>
        public int ColumnIndex {
            get;
        }

        /// <summary>
        /// Gets the text that is typed into the cell (unevaluated equations etc).
        /// </summary>
        public string Text {
            get {
                return this.text;
            }
        }

        /// <summary>
        /// Gets this represets the actual text that is displayed in the cell.
        /// It will just be the text property if the text doesn't start with '='. OW it will be the evaluation of an equation.
        /// Designed so only the spreadsheet class can set it, but anything can view it.
        /// </summary>
        public string Value {
            get {
                return this.value;
            }
        }

        /// <summary>
        /// Gets the BGcolor.
        /// </summary>
        public uint BGColor {
            get {
                return this.bGColor;
            }
        }
    }
}
