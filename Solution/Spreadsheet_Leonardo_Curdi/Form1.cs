// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using SpreadsheetEngine;
using System.ComponentModel;

namespace Spreadsheet_Leonardo_Curdi {
    /// <summary>
    /// 
    /// </summary>
    public partial class Form1 : Form {

        /// <summary>
        /// 
        /// </summary>
        static int rows = 50;

        /// <summary>
        /// 
        /// </summary>
        static int columns = 26;

        /// <summary>
        /// 
        /// </summary>
        private Spreadsheet spreadsheet;

        // constructor for the UI class
        public Form1() {
            this.InitializeComponent();
            this.InitializeDataGrid();

            this.spreadsheet = new Spreadsheet(rows, columns);
            this.spreadsheet.CellPropertyChanged += this.Cell_PropertyChanged;
        }

        /// <summary>
        /// Event handler for when a cell's value has changed.
        /// Updates the text of the cell in the DataGridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cell_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            // get the cell whos value has changed
            SpreadsheetCell cell = (SpreadsheetCell)sender;
            // update the value of the corresponding cell in the DataGridView
            this.cellGrid.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = cell.Value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cellGrid"></param>
        public void InitializeDataGrid() {
            // start by clearing the spreadsheet
            // we can access the data grid in the spreadsheet designer using its property name
            this.cellGrid.Rows.Clear();
            this.cellGrid.Columns.Clear();

            // create n columns programatically
            int asciiOffset = 65;
            for (int charAscii = asciiOffset; charAscii < asciiOffset + Form1.columns; charAscii++) { // use ascii value for capital letters A - Z
                string columnName = ((char)charAscii).ToString(); // cast from ascii code value to char, then convert to a string, since Add() takes strings
                this.cellGrid.Columns.Add(columnName, columnName); // use the Add() to add a column to the data grid
            }

            // create m rows
            this.cellGrid.RowCount = Form1.rows;
            // name the m rows programatically
            for (int row = 1; row <= Form1.rows; row++) {
                this.cellGrid.Rows[row - 1].HeaderCell.Value = (row).ToString(); // set the value of the header cell
            }
        }

        private void cellGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0) {
                string enteredText = this.cellGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Text = enteredText;
            }
        }
    }
}
