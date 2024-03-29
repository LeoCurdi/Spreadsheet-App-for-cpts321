// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using SpreadsheetEngine;
using static System.Net.Mime.MediaTypeNames;

namespace ExpressionTreeDemoApp {
    /// <summary>
    /// The main program class.
    /// </summary>
    internal static class Program {
        /// <summary>
        /// The default starting expression for the program.
        /// </summary>
        private static string expression = "A1+B1+C1";

        /// <summary>
        /// The expression tree field.
        /// </summary>
        private static ExpressionTree expressionTree = new ExpressionTree(expression);

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        public static void Main() {
            int menuSelection = 0;
            do {
                // print the menu
                PrintMenu(expression);

                // get the user's selection
                string userInput = Console.ReadLine();

                // try to convert to an integer, then handle selection if successful
                if (int.TryParse(userInput, out menuSelection)) {
                    // handle menu selection
                    switch (menuSelection) {
                        case 1:
                            // 1.The option to enter an expression string. You may assume that only valid expressions will be entered with no whitespaces.
                            // Simplified expressions are used for this assignment and the assumptions you are allowed to make are discussed later on.
                            Console.Write("Enter new expression: ");
                            string newExpression = Console.ReadLine();
                            expressionTree = new ExpressionTree(newExpression!);
                            expression = newExpression;
                            break;
                        case 2:
                            // 2.The option to set a variable value in the expression.
                            Console.Write("Enter variable name: ");
                            string variableName = Console.ReadLine();
                            Console.Write("Enter variable value: ");
                            double variableValue = Convert.ToDouble(Console.ReadLine());
                            expressionTree.SetVariable(variableName, variableValue);
                            break;
                        case 3:
                            // 3.The option to evaluate to the expression to a numerical value.
                            double result = expressionTree.Evaluate();
                            Console.WriteLine("Result: " + result);
                            break;
                        case 4:
                            // 4. The option to quit the program.
                            Console.WriteLine("\n\n\n\n\n\n\n\n\n\t\t\t\t\t\tExiting . . .\n\n\n\n\n\n\n\n\n");
                            break;
                        default:
                            // ignore any invalid input
                            break;
                    }
                }

                // if input was not an integer, ignore it
            } while (menuSelection != 4);
        }

        /// <summary>
        /// Prints the menu to the console.
        /// </summary>
        /// <param name="expression">The current expression stored in the tree.</param>
        private static void PrintMenu(string expression) {
            Console.WriteLine("Menu (current expression = \"{0}\")", expression);
            Console.WriteLine("  1 = Enter a new expression");
            Console.WriteLine("  2 = Set a variable value");
            Console.WriteLine("  3 = Evaluate tree");
            Console.WriteLine("  4 = Quit");
        }
    }
}
