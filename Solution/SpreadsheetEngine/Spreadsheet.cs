﻿// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpreadsheetEngine {
    /// <summary>
    /// Serves as a container for a 2D array of cells, and a factory for cells (cells will be created here.
    /// </summary>
    public class Spreadsheet {
        /// <summary>
        /// A bool to check if a spreadsheet is currently being loaded in, so that formulas are not prematurely evaluated during loading.
        /// </summary>
        private bool isLoading = false;

        /// <summary>
        /// A 2D array of cells.
        /// </summary>
        private Cell[,] cellArray;

        /// <summary>
        /// An instance of the expressionTree class for evaluating cell formulas.
        /// </summary>
        private ExpressionTree expressionTree;

        /// <summary>
        /// A stack containing history of actions performed by the user.
        /// </summary>
        private Stack<Command> undos = new ();

        /// <summary>
        /// A stack containing history of undone actions performed by the user.
        /// </summary>
        private Stack<Command> redos = new Stack<Command>();

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
                        spreadsheetCell.TellSheetDependentCellChanged += this.Cell_DependentCellChanged;
                    }
                }
            }
        }

        /// <summary>
        /// Notify observers whenever a cell property changes.
        /// </summary>
        public event PropertyChangedEventHandler CellPropertyChanged = (sender, e) => { };

        /// <summary>
        /// Notify observers when a stack property changes.
        /// </summary>
        public event PropertyChangedEventHandler StackPropertyChanged = (sender, e) => { };

        /// <summary>
        /// Notify observers when the top of the undo stack changes.
        /// </summary>
        public event PropertyChangedEventHandler UndoTopChanged = (sender, e) => { };

        /// <summary>
        /// Notify observers when the top of the redo stack changes.
        /// </summary>
        public event PropertyChangedEventHandler RedoTopChanged = (sender, e) => { };

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
        /// Sets the text of a cell.
        /// Static so that Command can access it without needing an instance of Spreadsheet.
        /// </summary>
        /// <param name="cell">The target cell.</param>
        /// <param name="text">The new text.</param>
        public static void SetCellText(Cell cell, string text) {
            if (cell is SpreadsheetCell) { // ensure it is of type SpreadsheetCell
                SpreadsheetCell spreadsheetCell = (SpreadsheetCell)cell; // cast it to a spreadsheet cell (since cell doesn't have setters)
                spreadsheetCell.Text = text; // now we can set the text
            }
        }

        /// <summary>
        /// Sets the BGColor of multiple cells.
        /// Static so that Command can access it without needing an instance of Spreadsheet.
        /// </summary>
        /// <param name="cellList">A list of every cell whos color needs to be changed.</param>
        /// <param name="newColor">The new color.</param>
        public static void SetCellColor(List<Cell> cellList, uint newColor) {
            // change the color for each cell
            foreach (Cell cell in cellList) {
                if (cell is SpreadsheetCell) { // ensure it is of type SpreadsheetCell
                    SpreadsheetCell spreadsheetCell = (SpreadsheetCell)cell; // cast it to a spreadsheet cell (since cell doesn't have setters)
                    spreadsheetCell.BGColor = newColor; // now we can set the color
                }
            }
        }

        /// <summary>
        /// Overload.
        /// Sets the BGColor of multiple cells to multiple different colors, mapped between the cell list and the color list.
        /// Static so that Command can access it without needing an instance of Spreadsheet.
        /// </summary>
        /// <param name="cellList">A list of every cell whos color needs to be changed.</param>
        /// <param name="colorList">A list contianing the new colors, mapped to the cell list.</param>
        public static void SetCellColor(List<Cell> cellList, List<uint> colorList) {
            // change the color for each cell
            int i = 0;
            foreach (Cell cell in cellList) {
                if (cell is SpreadsheetCell) { // ensure it is of type SpreadsheetCell
                    SpreadsheetCell spreadsheetCell = (SpreadsheetCell)cell; // cast it to a spreadsheet cell (since cell doesn't have setters)
                    spreadsheetCell.BGColor = colorList[i]; // set the corresponding color
                    i++;
                }
            }
        }

        /// <summary>
        /// Takes in a file path, generates the XML data for the current sheet, and writes it to the file path.
        /// </summary>
        /// <param name="filePath">The target file path.</param>
        public void SaveCurrentSheetToFile(string filePath) {
            // create an xmlWriter to write the sheet data to the file
            using (XmlWriter xmlWriter = XmlWriter.Create(filePath)) {
                // start the document
                xmlWriter.WriteStartDocument();

                // create a spreadsheet attribute
                xmlWriter.WriteStartElement("Spreadsheet");

                // visit each cell in the sheet
                foreach (Cell cell in this.cellArray) {
                    // check if the cell has any non default values
                    if (cell.Text != "\0" || cell.BGColor != 0xFFFFFFFF) {
                        // get the name of the cell
                        char column = (char)(cell.ColumnIndex + 65);
                        string cellName = $"{column}{cell.RowIndex + 1}";

                        // write the cell to the xmlwriter
                        xmlWriter.WriteStartElement("Cell");
                        xmlWriter.WriteAttributeString("name", cellName);
                        if (cell.Text != "\0") {
                            xmlWriter.WriteElementString("Text", cell.Text);
                        }

                        if (cell.BGColor != 0xFFFFFFFF) {
                            xmlWriter.WriteElementString("Bgcolor", cell.BGColor.ToString());
                        }

                        xmlWriter.WriteEndElement();
                    }
                }

                // close the spreadsheet attribute
                xmlWriter.WriteEndElement();

                // finish writing the document to the file
                xmlWriter.WriteEndDocument();
            }
        }

        /// <summary>
        /// Takes in a file path containing XML, clears the current spreadsheet and builds a new one from the XML file.
        /// </summary>
        /// <param name="filePath">The target file path.</param>
        public void LoadSheetFromFile(string filePath) {
            // Clear all spreadsheet data before loading file data, including undo/redo stacks
            this.ClearSheet();

            // load the data into the spreadsheet
            this.isLoading = true;

            // use a try to make sure the xml is not malformed
            try {
                using (XmlReader xmlReader = XmlReader.Create(filePath)) {
                    // read through the whole file
                    while (xmlReader.Read()) {
                        // if the element is a cell
                        if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Cell") {
                            // get the cell's name
                            string name = xmlReader.GetAttribute("name");
                            int column = name[0] - 65;
                            int row = int.Parse(name.Substring(1)) - 1;

                            // read through each element of the cell
                            string text = "\0";
                            uint bgColor = 0xFFFFFFFF;
                            xmlReader.Read(); // putting the read in the while parameter is a bad call becuase there are 1 too many reads which causes elements to be skipped
                            while (!xmlReader.EOF) {
                                // if its an opening element
                                if (xmlReader.NodeType == XmlNodeType.Element) {
                                    // operate based on the element type
                                    switch (xmlReader.Name) {
                                        case "Text":
                                            text = xmlReader.ReadElementContentAsString();
                                            break;
                                        case "Bgcolor":
                                            string color = xmlReader.ReadElementContentAsString();
                                            bgColor = uint.Parse(color);
                                            break;
                                        default:
                                            // skip all unimportant elements
                                            xmlReader.Skip();
                                            break;
                                    }
                                }

                                // if its a closing cell - break the reading loop
                                if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "Cell") {
                                    break;
                                }
                            }

                            // set the data for the new cell
                            this.SetCellText(row, column, text);
                            this.SetCellColor(row, column, bgColor);
                        }
                    }
                }
            }
            catch (Exception e) {
                throw new Exception("XML file is malformed", e);
            }

            this.isLoading = false;

            // Make sure formulas are properly evaluated after loading.
        }

        /// <summary>
        /// Executes an action, and adds it to the undos stack.
        /// </summary>
        /// <param name="command">A Command representing the action to be executed.</param>
        public void AddUndo(Command command) {
            // execute the command
            command.Execute();

            // push the command to the undos stack
            this.undos.Push(command);

            // fire the PropertyChanged event to notify the UI layer that the undos is not empty
            this.StackPropertyChanged(this, new PropertyChangedEventArgs("Undos not empty"));

            // notify the UI layer of the most recently done action
            string message = this.undos.Peek().Message;
            this.UndoTopChanged(this, new PropertyChangedEventArgs(message));
        }

        /// <summary>
        /// Undoes the most recent action and saves it to the redos stack.
        /// </summary>
        public void ExecuteUndo() {
            // error case - empty stack
            if (this.undos.Count == 0) {
                throw new Exception("undo stack is empty.");
            }

            // undo the action and pop it from the undos stack
            Command command = this.undos.Pop();
            command.Unexecute();

            // push the undo onto the redo stack
            this.redos.Push(command);

            // if undos is empty - notify the UI layer that the undos is empty, give UI emtpy message for undo button
            if (this.undos.Count == 0) {
                this.StackPropertyChanged(this, new PropertyChangedEventArgs("Undos empty"));
                this.UndoTopChanged(this, new PropertyChangedEventArgs(string.Empty));
            }

            // if undos is not empty - notify the UI layer of the message for most recently done action
            else {
                this.UndoTopChanged(this, new PropertyChangedEventArgs(this.undos.Peek().Message));
            }

            // notify the UI layer that the redos is not empty, give UI the message to display
            this.StackPropertyChanged(this, new PropertyChangedEventArgs("Redos not empty"));
            this.RedoTopChanged(this, new PropertyChangedEventArgs(this.redos.Peek().Message));
        }

        /// <summary>
        /// Redoes the most recently undone action and saves it to the undos stack.
        /// </summary>
        public void ExecuteRedo() {
            // redo the action and pop it from the redos stack
            Command command = this.redos.Pop();
            command.Execute();

            // push the command to the undos stack
            this.undos.Push(command);

            // if redos is empty - notify the UI layer that the redos is empty, give UI empty message for redo button
            if (this.redos.Count == 0) {
                this.StackPropertyChanged(this, new PropertyChangedEventArgs("Redos empty"));
                this.RedoTopChanged(this, new PropertyChangedEventArgs(string.Empty));
            }

            // if redos is not empty - notify the UI layer of the message for most recently undone action
            else {
                this.RedoTopChanged(this, new PropertyChangedEventArgs(this.redos.Peek().Message));
            }

            // notify the UI layer that the undos is not empty, give UI the message to display
            this.StackPropertyChanged(this, new PropertyChangedEventArgs("Undos not empty"));
            this.UndoTopChanged(this, new PropertyChangedEventArgs(this.undos.Peek().Message));
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
        /// Sets the text of a cell.
        /// This overloaded version is now only used by the old test cases.
        /// </summary>
        /// <param name="rowIndex">The row of the cell.</param>
        /// <param name="columnIndex">The column of the cell.</param>
        /// <param name="text">The new text.</param>
        public void SetCellText(int rowIndex, int columnIndex, string text) {
            Cell cell = this.cellArray[rowIndex, columnIndex]; // get the cell
            if (cell is SpreadsheetCell) { // ensure it is of type SpreadsheetCell
                SpreadsheetCell spreadsheetCell = (SpreadsheetCell)cell; // cast it to a spreadsheet cell (since cell doesn't have setters)
                spreadsheetCell.Text = text; // now we can set the text
            }
        }

        /// <summary>
        /// Sets the color of a cell.
        /// </summary>
        /// <param name="rowIndex">The row of the cell.</param>
        /// <param name="columnIndex">The column of the cell.</param>
        /// <param name="newColor">The new color.</param>
        public void SetCellColor(int rowIndex, int columnIndex, uint newColor) {
            Cell cell = this.cellArray[rowIndex, columnIndex]; // get the cell
            if (cell is SpreadsheetCell) { // ensure it is of type SpreadsheetCell
                SpreadsheetCell spreadsheetCell = (SpreadsheetCell)cell; // cast it to a spreadsheet cell (since cell doesn't have setters)
                spreadsheetCell.BGColor = newColor;
            }
        }

        /// <summary>
        /// Takes a variable string and parses the cell row and column from it.
        /// </summary>
        /// <param name="variable">The target variabel.</param>
        /// <returns>A row and column int.</returns>
        /// <exception cref="Exception">The variable is malformed, or contains a cell that is out of range.</exception>
        private (int row, int column) GetRowAndColFromString(string variable) {
            int row, column;

            // get the column
            char c = variable[0];
            if (char.IsUpper(c)) {
                column = c - 65;
            } else {
                column = c - 97;
            }

            if (column < 0 || column >= this.ColumnCount) {
                throw new Exception("!(Bad reference)");
            }

            // get the row: get a substring containing the row and parse it to an integer
            if (int.TryParse(variable.Substring(1), out row)) {
                row -= 1; // adjust for 0-indexed
            } else {
                throw new Exception("!(Bad reference)");
            }

            if (row < 0 || row >= this.RowCount) {
                throw new Exception("!(Bad reference)");
            }

            return (row, column);
        }

        /// <summary>
        /// Takes a row and column and returns the string equivalent of the cell.
        /// </summary>
        /// <param name="row">The row of the cell.</param>
        /// <param name="column">The column of the cell.</param>
        /// <returns>A string representation of the cell name.</returns>
        private string GetCellNameFromIndices(int row, int column) {
            char columnC = (char)(column + 65);
            string cellName = $"{columnC}{row + 1}";
            return cellName;
        }

        /// <summary>
        /// Takes a cell and calls a helper recursive method, which checks if the cell contains a circular reference.
        /// </summary>
        /// <param name="cell">The target cell.</param>
        /// <returns>True or false.</returns>
        private bool HasCircularReference(SpreadsheetCell cell) {
            // initialize a set of visited cells
            List<string> visitedCells = new List<string>();

            // call recursive DFS to see if it has a circular ref
            return this.HasCircularReferenceRecursive(cell, visitedCells);
        }

        /// <summary>
        /// Takes in a target cell and uses DFS with cycle detection to return whether there is a cycle.
        /// </summary>
        /// <param name="currentCell">The target cell.</param>
        /// <param name="visitedCells">A list of already visited cells.</param>
        /// <returns>True or false.</returns>
        private bool HasCircularReferenceRecursive(SpreadsheetCell currentCell, List<string> visitedCells) {
            // get the current cell's name
            string cellName = this.GetCellNameFromIndices(currentCell.RowIndex, currentCell.ColumnIndex);

            // check if the current cell has already been visited
            if (visitedCells.Contains(cellName)) {
                return true;
            }

            // mark the current cell as visited
            visitedCells.Add(cellName);

            // if the current cell is not an equation - return false
            if (currentCell.Text[0] != '=') {
                visitedCells.Remove(cellName);
                return false;
            }

            // get a list of all cells referenced by the current cell
            ExpressionTree tempExpressionTree = new ExpressionTree(currentCell.Text);
            List<string> variableNames = tempExpressionTree.GetVariableList();

            // call DFS on each dependent cell
            foreach (string variableName in variableNames) {
                // get the cell
                (int row, int column) = this.GetRowAndColFromString(variableName);
                Cell nextCell = this.GetCell(row, column);
                SpreadsheetCell sNextCell = (SpreadsheetCell)nextCell;

                // call DFS
                bool hasCirc = false;
                hasCirc = this.HasCircularReferenceRecursive(sNextCell, visitedCells);

                // if a cycle was detected, return true (else keep looking)
                if (hasCirc) {
                    return true;
                }
            }

            // if no cycle was detected in dependent cells, return false
            visitedCells.Remove(cellName);
            return false;
        }

        /// <summary>
        /// Takes in a cell containing a formula.
        /// Builds an expression tree for the cell.
        /// Evaluates the formula to get the value.
        /// </summary>
        /// <param name="spreadsheetCell">The cell.</param>
        /// <exception cref="Exception">The formula contains a target cell who's value is not a number.</exception>
        private void EvaluateFormula(SpreadsheetCell spreadsheetCell) {
            // give the equation to the expression tree and get the list of variables back
            this.expressionTree = new ExpressionTree(spreadsheetCell.Text);
            List<string> variableNames = this.expressionTree.GetVariableList();

            // check every variable in the formula for self reference
            foreach (string variable in variableNames) {
                (int row, int column) = this.GetRowAndColFromString(variable);
                if (spreadsheetCell.RowIndex == row && spreadsheetCell.ColumnIndex == column) {
                    throw new Exception("!(self reference)");
                }
            }

            // unsub current cell from all old dependencies
            foreach (SpreadsheetCell sc in spreadsheetCell.CurrentDependentCells) {
                sc.ValuePropertyChanged -= spreadsheetCell.Cell_DependentCellChanged;
            }

            spreadsheetCell.CurrentDependentCells.Clear();

            // check cell for circular reference
            if (this.HasCircularReference(spreadsheetCell)) {
                // we still have to sub to dependencies
                foreach (string variableName in variableNames) {
                    // get the cell from the name
                    (int row, int column) = this.GetRowAndColFromString(variableName);

                    // subscribe to the cell
                    Cell dependentCell = this.GetCell(row, column);
                    SpreadsheetCell sDependentCell = (SpreadsheetCell)dependentCell;
                    sDependentCell.ValuePropertyChanged += spreadsheetCell.Cell_DependentCellChanged;
                    spreadsheetCell.CurrentDependentCells.Add(sDependentCell); // keep track of each cell subbed to, so that they can be unsubbed
                }

                throw new Exception("!(circular reference)");
            }

            // set the value of every variable in the equation in the tree
            foreach (string variableName in variableNames) {
                // get the cell from the name
                (int row, int column) = this.GetRowAndColFromString(variableName);

                // subscribe to the cell
                Cell dependentCell = this.GetCell(row, column);
                SpreadsheetCell sDependentCell = (SpreadsheetCell)dependentCell;
                sDependentCell.ValuePropertyChanged += spreadsheetCell.Cell_DependentCellChanged;
                spreadsheetCell.CurrentDependentCells.Add(sDependentCell); // keep track of each cell subbed to, so that they can be unsubbed

                // get the value of the target cell
                string targetValue = this.GetCell(row, column).Value;

                // try to parse it to a double
                if (double.TryParse(targetValue, out double valueDouble)) {
                    // if successful - set value of variable in tree
                    this.expressionTree.SetVariable(variableName, valueDouble);
                } else {
                    // else give default value 0
                    this.expressionTree.SetVariable(variableName, 0);
                }
            }

            // get the evaluation
            double evaluation = this.expressionTree.Evaluate();

            // copy the result to the current cell
            spreadsheetCell.Value = evaluation.ToString();
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

                // if the entered text is an equation - only evaluate it if the spreadsheet is not in loading phase
                if (spreadsheetCell.Text[0] == '='/* && !this.isLoading*/) {
                    try {
                        this.EvaluateFormula(spreadsheetCell);
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
            }
        }

        /// <summary>
        /// Called by a cell, whos equation contains a cell whos value has been updated.
        /// Re evalutes the value of the cell associated with this event.
        /// </summary>
        /// <param name="sender">This is the cell that needs to be re-evaluated.</param>
        /// <param name="e">The arguments associated with the event.</param>
        private void Cell_DependentCellChanged(object sender, PropertyChangedEventArgs e) {
            // sender is the cell whos dependent cell changed
            Cell cell = (Cell)sender;
            if (cell is SpreadsheetCell) { // ensure it is of type SpreadsheetCell
                SpreadsheetCell spreadsheetCell = (SpreadsheetCell)cell; // cast it

                // update the cell whos dependent cell changed
                if (spreadsheetCell.Text[0] == '=') {
                    try {
                        this.EvaluateFormula(spreadsheetCell);

                        this.CellPropertyChanged(spreadsheetCell, e); // fire the PropertyChanged event for cell text changed
                    } catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                        spreadsheetCell.Value = ex.Message;
                    }
                }
            }
        }

        /// <summary>
        /// Clears all cell and stack data.
        /// </summary>
        private void ClearSheet() {
            // visit each cell in the sheet
            foreach (Cell cell in this.cellArray) {
                if (cell is SpreadsheetCell) {
                    SpreadsheetCell spreadsheetCell = (SpreadsheetCell)cell; // cast it to a spreadsheet cell (since cell doesn't have setters)
                    spreadsheetCell.Text = "\0"; // reset the text to null string
                    spreadsheetCell.BGColor = 0xFFFFFFFF; // reset the color to default
                }
            }

            // clear the stacks
            this.undos.Clear();
            this.StackPropertyChanged(this, new PropertyChangedEventArgs("Undos empty"));
            this.redos.Clear();
            this.StackPropertyChanged(this, new PropertyChangedEventArgs("Redos empty"));
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
            /// Notify observers whenever the value changes.
            /// Used for updating dependent cells.
            /// </summary>
            public event PropertyChangedEventHandler ValuePropertyChanged = (sender, e) => { };

            /// <summary>
            /// Notify the spreadsheet that a cell in this cell's formula has changed, and to reevalute this cell.
            /// </summary>
            public event PropertyChangedEventHandler TellSheetDependentCellChanged = (sender, e) => { };

            /// <summary>
            /// Gets or Sets.
            /// Keeps a list of references to all cells that this cell has subscribed to, so that it can unsubscribe from them eventually.
            /// </summary>
            public List<SpreadsheetCell> CurrentDependentCells { get; set; } = new List<SpreadsheetCell>();

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
                    this.ValuePropertyChanged(this, new PropertyChangedEventArgs("Value"));
                }
            }

            /// <summary>
            /// Gets or sets BGColor.
            /// </summary>
            public new uint BGColor {
                get {
                    return this.bGColor;
                }

                set {
                    this.bGColor = value;
                    this.PropertyChanged(this, new PropertyChangedEventArgs("BGColor")); // fire the PropertyChanged event for cell color changed
                }
            }

            /// <summary>
            /// This is called when another cell in this cell's formula is updated, so that this cell can re evaluate itself.
            /// </summary>
            /// <param name="sender">This is the cell whos value has changed.</param>
            /// <param name="e">The arguments associated with the event.</param>
            public void Cell_DependentCellChanged(object sender, PropertyChangedEventArgs e) {
                // if the dependent cell was changed to an error - dont notify listeners
                SpreadsheetCell spreadsheetCell = (SpreadsheetCell)sender; // cast it
                if (spreadsheetCell.Value[0] == '!') return;

                this.TellSheetDependentCellChanged(this, new PropertyChangedEventArgs("Text"));
            }
        }
    }
}
