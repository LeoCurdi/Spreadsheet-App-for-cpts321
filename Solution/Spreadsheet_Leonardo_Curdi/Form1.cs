// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel;
using System.Data.Common;
using SpreadsheetEngine;
using static System.Net.Mime.MediaTypeNames;

namespace Spreadsheet_Leonardo_Curdi {
    /// <summary>
    /// Inherits from Form class.
    /// Provides functionality for the spreadsheet application using the SpreadsheetEngine DLL.
    /// </summary>
    public partial class Form1 : Form {
        /// <summary>
        /// Number of rows in the spreadsheet.
        /// </summary>
        private static int rows = 50;

        /// <summary>
        /// Number of columns in the spreadsheet.
        /// </summary>
        private static int columns = 26;

        /// <summary>
        /// An instance of the Spreadsheet class from the SpreadSheetEngine DLL.
        /// </summary>
        private Spreadsheet spreadsheet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// Constructor for the UI class.
        /// </summary>
        public Form1() {
            this.InitializeComponent();
            this.InitializeDataGrid();

            // initialize the spreadsheet and sub to the cell property changed event.
            this.spreadsheet = new Spreadsheet(rows, columns);
            this.spreadsheet.CellPropertyChanged += this.Cell_PropertyChanged!;
            this.spreadsheet.StackPropertyChanged += this.Stack_PropertyChanged;
            this.spreadsheet.UndoTopChanged += this.Undo_TopChanged;
            this.spreadsheet.RedoTopChanged += this.Redo_TopChanged;

            // the undo and redo button should initially default to disabled
            this.undoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        /// Initializes the data grid in the GUI.
        /// Generates all rows and columns and gives them header names.
        /// </summary>
        public void InitializeDataGrid() {
            // start by clearing the spreadsheet
            // we can access the data grid in the spreadsheet designer using its property name
            this.CellGrid.Rows.Clear();
            this.CellGrid.Columns.Clear();

            // create n columns programatically
            int asciiOffset = 65;
            for (int charAscii = asciiOffset; charAscii < asciiOffset + Form1.columns; charAscii++) { // use ascii value for capital letters A - Z
                string columnName = ((char)charAscii).ToString(); // cast from ascii code value to char, then convert to a string, since Add() takes strings
                this.CellGrid.Columns.Add(columnName, columnName); // use the Add() to add a column to the data grid
            }

            // create m rows
            this.CellGrid.RowCount = Form1.rows;

            // name the m rows programatically
            for (int row = 1; row <= Form1.rows; row++) {
                this.CellGrid.Rows[row - 1].HeaderCell.Value = row.ToString(); // set the value of the header cell
            }
        }

        /// <summary>
        /// Event handler for when a stack's emptiness has changed.
        /// Determines to enable or disable undo or redo button.
        /// </summary>
        /// <param name="sender">This is the object that is triggering an event.</param>
        /// <param name="e">The arguments associated with the event.</param>
        private void Stack_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            string message = e.PropertyName;
            switch (message) {
                case "Undos not empty":
                    this.undoToolStripMenuItem.Enabled = true;
                    break;
                case "Undos empty":
                    this.undoToolStripMenuItem.Enabled = false;
                    break;
                case "Redos not empty":
                    this.redoToolStripMenuItem.Enabled = true;
                    break;
                case "Redos empty":
                    this.redoToolStripMenuItem.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Event handler for when the top item on the undo stack changes.
        /// </summary>
        /// <param name="sender">This is the object that is triggering an event.</param>
        /// <param name="e">The arguments associated with the event.</param>
        private void Undo_TopChanged(object sender, PropertyChangedEventArgs e) {
            string text = "Undo " + e.PropertyName;
            this.undoToolStripMenuItem.Text = text;
        }

        /// <summary>
        /// Event handler for when the top item on the redo stack changes.
        /// </summary>
        /// <param name="sender">This is the object that is triggering an event.</param>
        /// <param name="e">The arguments associated with the event.</param>
        private void Redo_TopChanged(object sender, PropertyChangedEventArgs e) {
            string text = "Redo " + e.PropertyName;
            this.redoToolStripMenuItem.Text = text;
        }

        /// <summary>
        /// Event handler for when a cell's property has changed.
        /// Updates the text of the cell in the DataGridView or the cell's BGcolor.
        /// </summary>
        /// <param name="sender">This is the object that is triggering an event.</param>
        /// <param name="e">The arguments associated with the event.</param>
        private void Cell_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            // get the cell whos property has changed
            Cell cell = (Cell)sender;

            // determine which property has changed
            if (e.PropertyName == "Text") {
                // update the value of the corresponding cell in the DataGridView
                this.CellGrid.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = cell.Value;
            }

            if (e.PropertyName == "BGColor") {
                // convert the color from uint to system.drawing.color
                Color color = Color.FromArgb((int)cell.BGColor);

                // update the bg color of the cell in the DataGridView
                this.CellGrid.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Style.BackColor = color;
            }
        }

        /// <summary>
        /// An event handler for when the user double clicks into a cell to change the text.
        /// Makes the cell display the unevaluated text.
        /// </summary>
        /// <param name="sender">This is the object that is triggering an event.</param>
        /// <param name="e">The arguments associated with the event.</param>
        private void CellGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e) {
            string text = this.spreadsheet.GetCellText(e.RowIndex, e.ColumnIndex);
            this.CellGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = text;
        }

        /// <summary>
        /// An evelnt handler for when the user finishes editing the text of a cell.
        /// Sets the text of the corresponding cell in the spreadsheet engine, which will be evaluated, then the result will bubble up to display in the UI.
        /// </summary>
        /// <param name="sender">This is the object that is triggering an event.</param>
        /// <param name="e">The arguments associated with the event.</param>
        private void CellGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0) { // for some reason you have to check if the indexes are not negative.
                // get the entered text from the user.
                string enteredText = this.CellGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                // create a command for text changed
                Cell cell = this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex);
                ChangeTextCommand command = new ChangeTextCommand(cell, enteredText);

                // call addUndo to change the text of the cell in the logic layer and add the undo
                this.spreadsheet.AddUndo(command);
            }
        }

        /// <summary>
        /// Called when the user clicks change color button.
        /// Prompts the user for a color.
        /// Changes the color of the cell in the logic layer.
        /// </summary>
        /// <param name="sender">This is the object that is triggering an event.</param>
        /// <param name="e">The arguments associated with the event.</param>
        private void ChangeBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e) {
            // pull up a ColorDialog (prompt the user for a color)
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK) { // error checking
                // get the color
                Color enteredColor = colorDialog.Color;

                // convert it to uint
                uint newColorValue = (uint)enteredColor.ToArgb();

                // get a list of the selected cells and their old colors
                List<Cell> changedCellsList = new List<Cell>();
                List<uint> oldCellColors = new List<uint>();
                foreach (DataGridViewCell cell in this.CellGrid.SelectedCells) {
                    Cell c = this.spreadsheet.GetCell(cell.RowIndex, cell.ColumnIndex);
                    changedCellsList.Add(c);
                    oldCellColors.Add(c.BGColor);
                }

                // create a new command for color changed
                ChangeColorCommand command = new ChangeColorCommand(changedCellsList, oldCellColors, newColorValue);

                // call AddUndo() with the new command to perform the task in the logic layer and add it to the list of undos
                this.spreadsheet.AddUndo(command);
            }
        }

        /// <summary>
        /// Called when the user clicks the undo button.
        /// Undoes the latest action.
        /// </summary>
        /// <param name="sender">This is the object that is triggering an event.</param>
        /// <param name="e">The arguments associated with the event.</param>
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e) {
            this.spreadsheet.ExecuteUndo();
        }

        /// <summary>
        /// Called when the user clicks the redo button.
        /// redoes the latest undone action.
        /// </summary>
        /// <param name="sender">This is the object that is triggering an event.</param>
        /// <param name="e">The arguments associated with the event.</param>
        private void RedoToolStripMenuItem_Click(object sender, EventArgs e) {
            this.spreadsheet.ExecuteRedo();
        }

        /// <summary>
        /// Called when the save to file button is clicked.
        /// Saves the current spreadsheet content to a file on the user's computer.
        /// </summary>
        /// <param name="sender">This is the object that is triggering an event.</param>
        /// <param name="e">The arguments associated with the event.</param>
        private void SaveFileToolStripMenuItem_Click(object sender, EventArgs e) {
            // create an instance of the save file dialog class
            SaveFileDialog sfd = new SaveFileDialog();

            // set the directory to "c:\"
            sfd.InitialDirectory = "c:\\";

            // filter the file dialog to only allow XML files or all files
            sfd.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

            // If the user selected a file and clicked OK in the file dialog
            if (sfd.ShowDialog() == DialogResult.OK) {
                // get the file path from the file dialog
                string filePath = sfd.FileName;

                // pass the file path to the spreadsheet to save itself in XML format to the file
                this.spreadsheet.SaveCurrentSheetToFile(filePath);
            }
        }

        /// <summary>
        /// Called when the load from file button is clicked.
        /// Opens an XML file from the user's computer and passes it to the spreadsheet.
        /// </summary>
        /// <param name="sender">This is the object that is triggering an event.</param>
        /// <param name="e">The arguments associated with the event.</param>
        private void OpenFileToolStripMenuItem_Click(object sender, EventArgs e) {
            // create an instance of the open file dialog class
            OpenFileDialog ofd = new OpenFileDialog();

            // set the directory to "c:\"
            ofd.InitialDirectory = "c:\\";

            // filter the file dialog to only allow XML files or all files
            ofd.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

            // If the user selected a file and clicked OK in the file dialog
            if (ofd.ShowDialog() == DialogResult.OK) {
                // open a file stream based on the selected file
                Stream fileStream = ofd.OpenFile();

                // pass the stream to the spreadsheet
                this.spreadsheet.LoadSheet(fileStream);





                // create a stream reader to read from the filestream
                using (StreamReader sr = new StreamReader(fileStream)) { // StreamReader inherits from TextReader and has a constructor that takes a file name
                }
            }
        }
    }
}
