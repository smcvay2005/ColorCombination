using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ColorComboChallenge
{
    class Program
    {
        static void Main(string[] args){
            
            Console.WriteLine("Please input the beginning marker, end marker, and the series of chip definitions:");
            var input = Console.ReadLine();
            var outputString = Matcher.TryMatch(input);
            Console.WriteLine(outputString);
            Console.ReadLine();
        }
    }
}
