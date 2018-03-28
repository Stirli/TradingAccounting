using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var i in (new List<int>{1,2,3,4,5}).Skip(6).Take(3))
            {
                Console.WriteLine(i);
            }
            Console.ReadKey();
        }
    }
}
