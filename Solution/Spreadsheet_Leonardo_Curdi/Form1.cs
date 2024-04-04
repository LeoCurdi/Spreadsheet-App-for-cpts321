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
                string enteredText = this.CellGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(); // get the entered text from the user.
                this.spreadsheet.SetCellText(e.RowIndex, e.ColumnIndex, enteredText); // update the text of the cell. (the text will be evaluated in the engine then bubbled back up to the listener in the form to update the value in the GUI)
            }
        }

        /// <summary>
        /// Event that is called when the Demo button is clicked.
        /// </summary>
        /// <param name="sender">This is the object that is triggering an event.</param>
        /// <param name="e">The arguments associated with the event.</param>
        private void DemoButton_Click(object sender, EventArgs e) {
            this.PerformDemo();
        }

        /// <summary>
        /// Runs the spreadsheet demo.
        /// </summary>
        private void PerformDemo() {
            // create an object of Random class
            Random random = new Random();
            string[] randomText = { "high cohesion", "low coupling" };

            // set the text of 50 random cells to a text string.
            for (int i = 0; i < 100; i++) {
                // get a random int for row and column
                int row = random.Next(0, 50); // the upper bound is exclusive, so we will get 0-49
                int column = random.Next(0, 26);
                int text = random.Next(0, 2);

                // set the text of the randomly selected cell
                this.spreadsheet.SetCellText(row, column, randomText[text]);
            }

            // set the text in every cell in column B to "This is cell B#"
            for (int i = 0; i < 50; i++) {
                this.spreadsheet.SetCellText(i, 1, "This is cell B" + (i + 1));
            }

            // set the text in every cell in column A to "=B#"
            for (int i = 0; i < 50; i++) {
                string text = "=B" + (i + 1);
                this.spreadsheet.SetCellText(i, 0, text);
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
                uint colorValue = (uint)enteredColor.ToArgb();

                // get and update the color for each of the cells that the user currently has selected
                // since multiple cells can be changed in one action, we want to call the color change once, with a list of cells
                List<Tuple<int, int>> changedCellsList = new List<Tuple<int, int>>();
                foreach (DataGridViewCell cell in this.CellGrid.SelectedCells) {
                    Tuple<int, int> coords = new Tuple<int, int>(cell.RowIndex, cell.ColumnIndex);
                    changedCellsList.Add(coords);
                }

                // set the color of all the cells in the logic layer
                this.spreadsheet.SetCellColor(changedCellsList, colorValue);
            }
        }
    }
}
