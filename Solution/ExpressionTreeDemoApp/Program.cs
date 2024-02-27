// Copyright (c) Leonardo Curdi - 11704166. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;
using System;
using static System.Net.Mime.MediaTypeNames;
using SpreadsheetEngine;

namespace ExpressionTreeDemoApp {
    /// <summary>
    /// The main program class.
    /// </summary>
    internal static class Program {

        static ExpressionTree expressionTree = new ExpressionTree();

        static string expression = "A1+B1+C1";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        public static void Main() {
            int menuSelection = 0;
            do {
                // print the menu
                printMenu(expression);

                // get the user's selection
                string userInput = Console.ReadLine();

                // try to convert to an integer, then handle selection if successful
                if (int.TryParse(userInput, out menuSelection)) {
                    // handle menu selection
                    switch (menuSelection) {
                        case 1:
                            //            1.The option to enter an expression string.You may assume that only valid expressions will be
                            //entered with no whitespaces. Simplified expressions are used for this assignment and the
                            //assumptions you are allowed to make are discussed later on.
                            break;
                        case 2:
                            //2.The option to set a variable value in the expression.This must prompt for both the variable
                            //name and then the variable value.
                            break;
                        case 3:
                            //3.The option to evaluate to the expression to a numerical value.
                            break;
                        case 4:
                            break;
                        default:
                            // ignore any invalid input
                            break;
                    }
                }

                // if input was not an integer, ignore it
            } while (menuSelection != 4);
        }

        private static void printMenu(string expression) {
            Console.WriteLine("Menu (current expression = \"{0}\")", expression);
            Console.WriteLine("  1 = Enter a new expression");
            Console.WriteLine("  2 = Set a variable value");
            Console.WriteLine("  3 = Evaluate tree");
            Console.WriteLine("  4 = Quit");
        }
    }
}
