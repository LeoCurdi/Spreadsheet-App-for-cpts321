// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel;
using SpreadsheetEngine;

namespace Spreadsheet_Leonardo_Curdi {
    /// <summary>
    /// Inherits from Form class.
    /// Provides functionality for the spreadsheet application using the SpreadsheetEngine DLL.
    /// </summary>
    public partial class Form1 : Form {

        /// <summary>
        /// Number of rows in the spreadsheet.
        /// </summary>
        public static int Rows = 50;

        /// <summary>
        /// Number of columns in the spreadsheet.
        /// </summary>
        public static int Columns = 26;

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
            this.spreadsheet = new Spreadsheet(Rows, Columns);
            this.spreadsheet.CellPropertyChanged += this.Cell_PropertyChanged;
        }

        /// <summary>
        /// Event handler for when a cell's value has changed.
        /// Updates the text of the cell in the DataGridView.
        /// </summary>
        /// <param name="sender">This is the object that is triggering an event.</param>
        /// <param name="e">The arguments associated with the event.</param>
        private void Cell_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            // get the cell whos value has changed
            SpreadsheetCell cell = (SpreadsheetCell)sender;

            // update the value of the corresponding cell in the DataGridView
            this.cellGrid.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = cell.Value;
        }

        /// <summary>
        /// Initializes the data grid in the GUI.
        /// Generates all rows and columns and gives them header names.
        /// </summary>
        public void InitializeDataGrid() {
            // start by clearing the spreadsheet
            // we can access the data grid in the spreadsheet designer using its property name
            this.cellGrid.Rows.Clear();
            this.cellGrid.Columns.Clear();

            // create n columns programatically
            int asciiOffset = 65;
            for (int charAscii = asciiOffset; charAscii < asciiOffset + Form1.Columns; charAscii++) { // use ascii value for capital letters A - Z
                string columnName = ((char)charAscii).ToString(); // cast from ascii code value to char, then convert to a string, since Add() takes strings
                this.cellGrid.Columns.Add(columnName, columnName); // use the Add() to add a column to the data grid
            }

            // create m rows
            this.cellGrid.RowCount = Form1.Rows;

            // name the m rows programatically
            for (int row = 1; row <= Form1.Rows; row++) {
                this.cellGrid.Rows[row - 1].HeaderCell.Value = row.ToString(); // set the value of the header cell
            }
        }

        /// <summary>
        /// An event handler for when the user enters text into a cell in the GUI.
        /// </summary>
        /// <param name="sender">This is the object that is triggering an event.</param>
        /// <param name="e">The arguments associated with the event.</param>
        private void cellGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0) { // for some reason you have to check if the indexes are not negative.
                string enteredText = this.cellGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(); // get the entered text from the user.
                this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Text = enteredText; // update the text of the cell. (the text will be evaluated in the engine then bubbled back up to the listener in the form to update the value in the GUI)
            }
        }
    }
}
