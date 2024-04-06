using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;

namespace OpenData
{
    class Program
    {
        static void Main(string[] args)
        {
            var b = Choose.Get();

            Console.WriteLine(b[0] + " " + b[1]);
        }
    }
}
