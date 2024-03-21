// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
using System.Numerics;
using NUnit.Framework;
using SpreadsheetEngine;

namespace SpreadsheetTests {
    /// <summary>
    /// Tests the functionality of my the ExpressionTree class in my spreadsheet engine.
    /// </summary>
    [TestFixture]
    public class TestExpressionTree {
        /// <summary>
        /// An instance of ExpressionTree to be tested.
        /// </summary>
        private ExpressionTree testTree;

        /// <summary>
        /// The constructor for the test class.
        /// Initializes the data members.
        /// </summary>
        [SetUp]
        public void Setup() {
        }

        /// <summary>
        /// Test adding two numbers.
        /// </summary>
        [Test]
        public void TestAddition() {
            this.testTree = new ExpressionTree("2+2.2");
            Assert.That(
                this.testTree.Evaluate(),
                Is.EqualTo(4.2));
        }

        /// <summary>
        /// Test subtracting two numbers.
        /// </summary>
        [Test]
        public void TestSubtraction() {
            this.testTree = new ExpressionTree("1-55");
            Assert.That(
                this.testTree.Evaluate(),
                Is.EqualTo(-54));
        }

        /// <summary>
        /// Test multiplying two numbers.
        /// </summary>
        [Test]
        public void TestMultiplication() {
            this.testTree = new ExpressionTree("9*0");
            Assert.That(
                this.testTree.Evaluate(),
                Is.EqualTo(0));
        }

        /// <summary>
        /// Test dividing two numbers.
        /// </summary>
        [Test]
        public void TestDivision() {
            this.testTree = new ExpressionTree("9/18");
            Assert.That(
                this.testTree.Evaluate(),
                Is.EqualTo(0.5));
        }

        /// <summary>
        /// Test evaluating a simple expression with a variable.
        /// </summary>
        [Test]
        public void TestExpressionWithVariables() {
            this.testTree = new ExpressionTree("4+hello");
            this.testTree.SetVariable("hello", 8);
            Assert.That(
                this.testTree.Evaluate(),
                Is.EqualTo(12));
        }

        /// <summary>
        /// Test evaluating a complex expression with all operators and a variable.
        /// </summary>
        [Test]
        public void TestComplexExpression() {
            this.testTree = new ExpressionTree("40/hello-0.2*30+4.5*(A1/B3)");
            this.testTree.SetVariable("hello", 2);
            this.testTree.SetVariable("A1", 7);
            this.testTree.SetVariable("B3", 2.5);
            Assert.That(
                this.testTree.Evaluate(),
                Is.EqualTo(26.6));
        }

        /// <summary>
        /// Test evaluating an invalid expression.
        /// </summary>
        [Test]
        public void TestInvalidExpression() {
            // check if it throws an exception
            Assert.Throws<ArgumentException>(() => new ExpressionTree("2-*30"));
        }

        /// <summary>
        /// Test dividing by zero.
        /// </summary>
        [Test]
        public void TestDivideByZero() {
            this.testTree = new ExpressionTree("5/0");
            Assert.Throws<ArgumentException>(() => this.testTree.Evaluate());
        }

        /// <summary>
        /// Uses the factory to create an addition operator node, and checks if its type additionNode.
        /// </summary>
        [Test]
        public void TestCreateOperatorNode() {
            OperatorNodeFactory factory = new OperatorNodeFactory();
            BinaryOperatorNode node = factory.CreateOperatorNode('+');
            Assert.IsTrue(node is AdditionNode);
        }

        /// <summary>
        /// Tests getting the precedence of a type of operator.
        /// </summary>
        [Test]
        public void TestGetPrecedence() {
            OperatorNodeFactory factory = new OperatorNodeFactory();
            Assert.That(
                factory.GetPrecedence('*'),
                Is.EqualTo(2));
        }

        /// <summary>
        /// Tests getting the associativity of a type of operator.
        /// </summary>
        [Test]
        public void TestGetAssociativity() {
            OperatorNodeFactory factory = new OperatorNodeFactory();
            Assert.That(
                factory.GetAssociativity('+'),
                Is.EqualTo(Associativity.Left));
        }

        /// <summary>
        /// Tests whether a char is an operator.
        /// </summary>
        [Test]
        public void TestIsOperator() {
            OperatorNodeFactory factory = new OperatorNodeFactory();
            Assert.That(
                factory.IsOperator('a'),
                Is.EqualTo(false));
        }
    }
}
