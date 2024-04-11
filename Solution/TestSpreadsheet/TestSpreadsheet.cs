// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
using System.Numerics;
using System.Windows.Forms;
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
            this.testSheet.SetCellText(1, 5, "cohesion");

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
            this.testSheet.SetCellText(0, 0, "8");

            // set cell B2 equal to the value of cell A1
            this.testSheet.SetCellText(1, 1, "=A1");

            // check if the value of B2 was correctly evaluated to equal the contents of A1.
            Assert.AreEqual(
                "8",
                this.testSheet.GetCell(1, 1).Value);
        }

        /// <summary>
        /// Tests a normal equation evaluation with multiple operators but no variables.
        /// </summary>
        [Test]
        public void TestEvaluatingMultiOperatorEquation() {
            this.testSheet.SetCellText(0, 0, "=5*4-10/2+1");

            Assert.AreEqual(
                "16",
                this.testSheet.GetCell(0, 0).Value);
        }

        /// <summary>
        /// Tests a edge case equation evaluation with multiple operators and variables.
        /// </summary>
        [Test]
        public void TestEvaluatingVariableEquation() {
            // set some cells to a number
            this.testSheet.SetCellText(1, 1, "10");
            this.testSheet.SetCellText(3, 3, "5");

            // include those cells in the equation
            this.testSheet.SetCellText(0, 0, "=D4*4-B2/2+1");

            Assert.AreEqual(
                "16",
                this.testSheet.GetCell(0, 0).Value);
        }

        /// <summary>
        /// Tests a exception case equation evaluation with a variable that is not a number.
        /// </summary>
        [Test]
        public void TestEquationWithInvalidVariable() {
            // set a cell to a non number input
            this.testSheet.SetCellText(1, 1, "memes");

            // make sure the exception is handled
            Assert.DoesNotThrow(() => this.testSheet.SetCellText(0, 0, "=B2"));
        }

        /// <summary>
        /// Tests a exception case evaluation of an invalid equation.
        /// </summary>
        [Test]
        public void TestInvalidEquation() {
            // make sure the exception is handled
            Assert.DoesNotThrow(() => this.testSheet.SetCellText(0, 0, "=4++6"));
        }

        /// <summary>
        /// Tests a normal case for changing cell color.
        /// </summary>
        [Test]
        public void TestChangingCellColor() {
            uint newColorValue = 0xAAAAAAAA;
            List<Cell> cellsList = new List<Cell>();
            Cell cell = this.testSheet.GetCell(0, 0);
            cellsList.Add(cell);

            Spreadsheet.SetCellColor(cellsList, newColorValue);

            Assert.AreEqual(
                0xAAAAAAAA,
                this.testSheet.GetCell(0, 0).BGColor);
        }

        /// <summary>
        /// Edge case for ensuring that the program can change multiple cell colors in one shot, to different values.
        /// </summary>
        [Test]
        public void TestChangingMultipleCellColor() {
            // create a list of 2 cells
            List<Cell> cellsList = new List<Cell>();
            Cell cell = this.testSheet.GetCell(0, 0);
            cellsList.Add(cell);
            cell = this.testSheet.GetCell(1, 1);
            cellsList.Add(cell);

            // create a list of 2 colors
            List<uint> colors = new List<uint>();
            colors.Add(0x11111111);
            colors.Add(0x11AAFFBB);

            // set the colors
            Spreadsheet.SetCellColor(cellsList, colors);

            // check the color of both cells
            Assert.AreEqual(
                0x11111111,
                this.testSheet.GetCell(0, 0).BGColor);
            Assert.AreEqual(
                0x11AAFFBB,
                this.testSheet.GetCell(1, 1).BGColor);
        }

        /// <summary>
        /// Exception case - trying to undo when there are no actions in the undo stack.
        /// </summary>
        [Test]
        public void TestUndoOnEmptyStack() {
            Assert.Throws<Exception>(() => this.testSheet.ExecuteUndo());
        }

        [Test]
        public void TestCreatingAnXMLFile() {
            string filePath = "testFile.xml";
            this.testSheet.SaveCurrentSheetToFile(filePath);
            Assert.IsTrue(File.Exists(filePath));
        }

        [Test]
        public void TestSavingToFile() {

        }
    }
}
