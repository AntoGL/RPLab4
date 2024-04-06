using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;

namespace OpenData
{
    class Program
    {
        static void Main(string[] args)
        {
            bool b = Choose.Get();

            if (b)
                Console.WriteLine("Ходит 1 игрок");
            else
                Console.WriteLine("Ходит 2 игрок");
        }
    }
}
