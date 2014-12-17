using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProgram
{
    public class Calculator
    {
        public int Perform(string operation, int value1, int value2)
        {            
            switch(operation)
            {
                case "Add":
                case "+":
                    {
                         return (value1 + value2);                        
                    }
                case "substract":
                case "-":
                    {
                        return (value1 - value2);                                  
                    }
                case "multipy":
                case "*":
                    {
                        return (value1 * value2); 
                    }
                case "divide":
                case "/":
                    {
                        return (value1 / value2); 
                    }
                default:
                    return 0;
            }
        }
    }
}
