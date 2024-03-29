// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SpreadsheetEngine {
    /// <summary>
    /// Serves as a container for a 2D array of cells, and a factory for cells (cells will be created here.
    /// </summary>
    public class Spreadsheet {
        /// <summary>
        /// A 2D array of cells.
        /// </summary>
        private Cell[,] cellArray;

        /// <summary>
        /// An instance of the expressionTree class for evaluating cell formulas.
        /// </summary>
        private ExpressionTree expressionTree;

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// Constructor.
        /// Allocates space for a 2D array of cells.
        /// Initializes the array of cells.
        /// </summary>
        /// <param name="rows">The number of rows for the new spreadsheet.</param>
        /// <param name="columns">The number of columns for the new spreadsheet.</param>
        public Spreadsheet(int rows, int columns) {
            // initialize the cellArray
            this.cellArray = new Cell[rows, columns];

            // initialize each cell and subscribe the sheet to cell's property changed event for every cell
            for (int row = 0; row < rows; row++) {
                for (int column = 0; column < columns; column++) {
                    this.cellArray[row, column] = new SpreadsheetCell(row, column);

                    // cast the cell to a SpreadsheetCell to subscribe to the property changed event
                    Cell cell = this.cellArray[row, column]; // get the cell
                    if (cell is SpreadsheetCell) { // ensure it is of type SpreadsheetCell
                        SpreadsheetCell spreadsheetCell = (SpreadsheetCell)cell; // cast it
                        spreadsheetCell.PropertyChanged += this.Cell_PropertyChanged!;
                    }
                }
            }
        }

        /// <summary>
        /// Notify observers whenever a property changes.
        /// </summary>
        public event PropertyChangedEventHandler CellPropertyChanged = (sender, e) => { };

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
        /// Finds the cell located at row and column.
        /// </summary>
        /// <param name="rowIndex">The row of the cell.</param>
        /// <param name="columnIndex">The column of the cell.</param>
        /// <returns>a reference to the cell at a specified row and column.</returns>
        public Cell GetCell(int rowIndex, int columnIndex) {
            // error case
            if (rowIndex < 0 || rowIndex >= this.RowCount || columnIndex < 0 || columnIndex >= this.ColumnCount) {
                return null!;
            }

            return this.cellArray[rowIndex, columnIndex];
        }

        /// <summary>
        /// Sets the text of a cell.
        /// </summary>
        /// <param name="rowIndex">The row of the cell.</param>
        /// <param name="columnIndex">The column of the cell.</param>
        /// <param name="text">The text to set the cell to.</param>
        public void SetCellText(int rowIndex, int columnIndex, string text) {
            Cell cell = this.cellArray[rowIndex, columnIndex]; // get the cell
            if (cell is SpreadsheetCell) { // ensure it is of type SpreadsheetCell
                SpreadsheetCell spreadsheetCell = (SpreadsheetCell)cell; // cast it to a spreadsheet cell (since cell doesn't have setters)
                spreadsheetCell.Text = text; // now we can set the text
            }
        }

        /// <summary>
        /// Gets the text content of a cell.
        /// </summary>
        /// <param name="rowIndex">The row of the cell.</param>
        /// <param name="columnIndex">The column of the cell.</param>
        /// <returns>A string.</returns>
        public string GetCellText(int rowIndex, int columnIndex) {
            Cell cell = this.cellArray[rowIndex, columnIndex]; // get the cell
            string text = cell.Text; // get the text
            return text;
        }

        /// <summary>
        /// If the text of a cell changes, we need to evaluate the text and change the cell value here.
        /// </summary>
        /// <param name="sender">This is the object that is triggering an event.</param>
        /// <param name="e">The arguments associated with the event.</param>
        private void Cell_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            // sender is the cell whos text just changed
            Cell cell = (Cell)sender;
            if (cell is SpreadsheetCell) { // ensure it is of type SpreadsheetCell
                SpreadsheetCell spreadsheetCell = (SpreadsheetCell)cell; // cast it

                // if the entered text is an equation - evaluate it
                if (spreadsheetCell.Text[0] == '=') {
                    try {
                        // give the equation to the expression tree
                        this.expressionTree = new ExpressionTree(spreadsheetCell.Text);

                        // get the list of variables in the equation
                        List<string> variableNames = this.expressionTree.GetVariableList();

                        // set the value of every variable in the equation in the tree
                        foreach (string variableName in variableNames) {
                            // get the cell from the name
                            int column = variableName[0] - 65; // get the column
                            int row = int.Parse(variableName.Substring(1)) - 1; // get the row: get a substring containing the row and parse it to an integer

                            // get the value of the target cell
                            string targetValue = this.GetCell(row, column).Value;

                            // try to parse it to a double
                            if (double.TryParse(targetValue, out double valueDouble)) {
                                // if successful - set value of variable in tree
                                this.expressionTree.SetVariable(variableName, valueDouble);
                            } else {
                                // else throw exception
                                throw new Exception("Value of target cell is not a number");
                            }
                        }

                        // get the evaluation
                        double evaluation = this.expressionTree.Evaluate();

                        // copy the result to the current cell
                        spreadsheetCell.Value = evaluation.ToString();
                    } catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                        spreadsheetCell.Value = ex.Message;
                    }
                }

                // if it is just text
                else {
                    spreadsheetCell.Value = cell.Text;
                }

                this.CellPropertyChanged(sender, e); // fire the PropertyChanged event for cell text changed

                // temporary brute force method for updating dependent cells
                for (int i = 0; i < this.RowCount; i++) {
                    for (int j = 0; j < this.ColumnCount; j++) {
                        cell = this.GetCell(i, j);
                        if (cell is SpreadsheetCell) { // ensure it is of type SpreadsheetCell
                            SpreadsheetCell dependentCell = (SpreadsheetCell)cell; // cast it

                            // update every cell that has an equation
                            if (dependentCell.Text[0] == '=') {
                                try {
                                    // give the equation to the expression tree
                                    this.expressionTree = new ExpressionTree(dependentCell.Text);

                                    // get the list of variables in the equation
                                    List<string> variableNames = this.expressionTree.GetVariableList();

                                    // set the value of every variable in the equation in the tree
                                    foreach (string variableName in variableNames) {
                                        // get the cell from the name
                                        int column = variableName[0] - 65; // get the column
                                        int row = int.Parse(variableName.Substring(1)) - 1; // get the row: get a substring containing the row and parse it to an integer

                                        // get the value of the target cell
                                        string targetValue = this.GetCell(row, column).Value;

                                        // try to parse it to a double
                                        if (double.TryParse(targetValue, out double valueDouble)) {
                                            // if successful - set value of variable in tree
                                            this.expressionTree.SetVariable(variableName, valueDouble);
                                        } else {
                                            // else throw exception
                                            throw new Exception("Value of target cell is not a number");
                                        }
                                    }

                                    // get the evaluation
                                    double evaluation = this.expressionTree.Evaluate();

                                    // copy the result to the current cell
                                    dependentCell.Value = evaluation.ToString();

                                    this.CellPropertyChanged(dependentCell, e); // fire the PropertyChanged event for cell text changed
                                }
                                catch (Exception ex) {
                                    Console.WriteLine(ex.Message);
                                    dependentCell.Value = ex.Message;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// A concrete class for cells.
        /// We need a concrete inherited class since abstract base classes cant be instantiated.
        /// This class must be in Spreadsheet and private becuase no other program should use this class.
        /// </summary>
        private class SpreadsheetCell : Cell, INotifyPropertyChanged {
            /// <summary>
            /// Initializes a new instance of the <see cref="SpreadsheetCell"/> class.
            /// </summary>
            /// <param name="row">Row index of the new cell.</param>
            /// <param name="column">Column index of the new cell.</param>
            public SpreadsheetCell(int row, int column)
                : base(row, column) {
            }

            /// <summary>
            /// Notify observers whenever a property changes.
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

            /// <summary>
            /// Gets or sets the text that is typed into the cell (unevaluated equations etc).
            /// </summary>
            public new string Text {
                get {
                    return this.text;
                }

                set {
                    // if new text is different
                    this.text = value; // set new text
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Text")); // fire the PropertyChanged event for cell text changed
                }
            }

            /// <summary>
            /// Gets or sets this represets the actual text that is displayed in the cell.
            /// It will just be the text property if the text doesn't start with '='. OW it will be the evaluation of an equation.
            /// Designed so only the spreadsheet class can set it, but anything can view it.
            /// </summary>
            public new string Value {
                get {
                    return this.value;
                }

                set {
                    this.value = value;
                }
            }
        }
    }
}
