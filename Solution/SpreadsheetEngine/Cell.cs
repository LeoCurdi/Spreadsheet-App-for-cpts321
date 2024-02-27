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
    public abstract class Cell : INotifyPropertyChanged { // notify observers when a cell property is changed

        /// <summary>
        /// The text entered into the cell by the user.
        /// </summary>
        protected string text;

        /// <summary>
        /// The evaluated text, which is actually displayed in the cell.
        /// </summary>
        protected string value;

        /// <summary>
        /// Notify observers whenever a property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

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
        /// Gets or sets the text that is typed into the cell (unevaluated equations etc).
        /// </summary>
        public string Text {
            get {
                return this.text;
            }

            set {
                // if text being set is the same text, don't call the property change event (since the text isn't actually being changed)
                if (this.text == value) {
                    return;
                }

                // if new text is different
                this.text = value; // set new text
                this.PropertyChanged(this, new PropertyChangedEventArgs("Text")); // fire the PropertyChanged event for cell text changed
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

            internal set { this.value = value; } // setter is internal so only spreadsheet and cell can access it
        }

        /// <summary>
        /// Constructor.
        /// Sets row and column index, which can never be changed.
        /// </summary>
        public Cell(int row, int column) {
            this.RowIndex = row;
            this.ColumnIndex = column;
        }
    }
}
