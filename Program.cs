using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace angle
{
    class Program
    {
        static void Main(string[] args)
        {
            HandleFile txt = new HandleFile("file1.txt");
            foreach(double e in txt.x)
            {
                Console.Write(e + "______");
            }
            Console.WriteLine();
            foreach (double e in txt.y)
            {
                Console.Write(e + "______");
            }
            Console.Read();
        }
    }
}
