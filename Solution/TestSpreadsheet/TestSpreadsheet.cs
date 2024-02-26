// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
using SpreadsheetEngine;
using System.Numerics;
using NUnit.Framework;

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
    }
}