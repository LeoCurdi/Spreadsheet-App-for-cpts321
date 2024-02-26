// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
using System.Numerics;
using NUnit.Framework;
using SpreadsheetEngine;

namespace SpreadsheetTests {
    /// <summary>
    /// Tests the functionality of my spreadsheet engine.
    /// </summary>
    [TestFixture]
    public class TestSpreadsheet {
        /// <summary>
        /// An instance of Spreadsheet to be tested.
        /// </summary>
        private Spreadsheet testSheet;


        /// <summary>
        /// The constructor for the test class.
        /// Initializes the data members.
        /// </summary>
        [SetUp]
        public void Setup() {
            this.testSheet = new Spreadsheet(50, 26);
        }

        /// <summary>
        /// Tests RowCount property in Spreadsheet for accuracy.
        /// </summary>
        [Test]
        public void TestRowCount() {
            Assert.AreEqual(
                50,
                this.testSheet.RowCount);
        }

        /// <summary>
        /// Tests ColumnCount property in Spreadsheet for accuracy.
        /// </summary>
        [Test]
        public void TestColumnCount() {
            Assert.AreEqual(
                26,
                this.testSheet.ColumnCount);
        }

        /// <summary>
        /// Changes the text of a cell, then checks if the value was updated.
        /// </summary>
        [Test]
        public void TestChangingCellText() {
            // set the text of a cell.
            this.testSheet.GetCell(1, 5).Text = "cohesion";
            // check if the value matches.
            Assert.AreEqual(
                "cohesion",
                this.testSheet.GetCell(1, 5).Value);
        }

        /// <summary>
        /// Changes the text of a cell to an equation, checks if the equation was evaluated properly.
        /// </summary>
        [Test]
        public void TestEnteringEquation() {
            // set the text of cell A1
            this.testSheet.GetCell(0, 0).Text = "cohesion";
            // set cell B2 equal to the value of cell A1
            this.testSheet.GetCell(1, 1).Text = "=A1";
            // check if the value of B2 was correctly evaluated to equal the contents of A1.
            Assert.AreEqual(
                "cohesion",
                this.testSheet.GetCell(1, 1).Value);
        }
    }
}