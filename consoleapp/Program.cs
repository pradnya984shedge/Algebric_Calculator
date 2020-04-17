using ConsoleApp.Extension;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp
{
    class Program
    {
        private static char[] pattern=new char[] { '+','-','*','/'};
        private static IEnumerable<string> result;
        private static List<string> listOfStr= new List<string>();
        private static string number;

        static void Main(string[] args)
        {

            Console.Write("Enter Input:");
            string input = Console.ReadLine();
            string[] splitInput = input.Split('=');
            int index = char.IsLetter(splitInput[0].Replace(" ", "")[0]) ? 1 : 0;
            string sideWithEquation = splitInput[index];

            if (sideWithEquation.StartsWith('-') || sideWithEquation.StartsWith('+'))
                sideWithEquation = "0" + sideWithEquation;
            if (sideWithEquation.StartsWith('*') || sideWithEquation.StartsWith('/'))
                sideWithEquation = sideWithEquation.Remove(0, 1);


            sideWithEquation = sideWithEquation.Replace(" ", "");
            
            result = EnumerableMethod.SplitAndKeep(sideWithEquation,pattern);
            listOfStr.AddRange(result);
            

            //Compute right hand side
            string[] equation = listOfStr.ToArray();
               
            //compute for * and /
            for (int i = 0; i < equation.Length ; i++)
            {
                string item = equation[i];
                double num = 0;
                switch (item)
                {
                    case "*":
                        num = Convert.ToDouble(equation[i - 1]) * Convert.ToDouble(equation[i + 1]);
                        break;
                    case "/":
                        num = Convert.ToDouble(equation[i - 1]) / Convert.ToDouble(equation[i + 1]);
                        break;
                }
                if (num > 0)
                {
                    equation[i - 1] = "";
                    equation[i] = "";
                    equation[i + 1] = num.ToString();
                }
            }

            //Now compute for + and-
            equation = string.Join(" ", equation).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < equation.Length ; i++)
            {
                string item = equation[i];
                double num = 0;
                switch (item)
                {
                    case "+":
                        num = Convert.ToDouble(equation[i - 1]) + Convert.ToDouble(equation[i + 1]);
                        break;
                    case "-":
                        num = Convert.ToDouble(equation[i - 1]) - Convert.ToDouble(equation[i + 1]);
                        break;
                }
                if (num > 0 || num < 0)
                {
                    equation[i - 1] = "";
                    equation[i] = "";
                    equation[i + 1] = num.ToString();
                }
            }

            
            string total = string.Join("", equation);
           
            //display what x is
            try
            {
                // your code
                total = string.Format("{0:N2}", Convert.ToDouble(total));

                if(!(Fraction.IsWholeNumber(Convert.ToDouble(total))))
                    number = Fraction.DecimalToFraction(Convert.ToDouble(total));
                else
                    number = total;
                Console.WriteLine($"Output = {number}");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error = {e.Message }");
                Console.Read();
                
            }

            Console.WriteLine("------------------------\n");

            // Wait for the user to respond before closing.
            Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
            if (Console.ReadLine() == "n")
            Console.WriteLine("\n"); // Friendly linespacing.
        }
    }
}
    