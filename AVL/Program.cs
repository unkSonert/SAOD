using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVL
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllText("stuff/big.txt").Split(' ', '\n', '\t').Where(t => t != "").ToArray();

            var AVL = new AVL<string, int>();

            var watch = Stopwatch.StartNew();

            foreach (var line in lines)
            {
                if (!AVL.ContainsKey(line))
                {
                    AVL.Add(line, 0);
                }

                AVL[line] += 1;
            }

            watch.Stop();

            Console.WriteLine(AVL["red"]);

            Console.WriteLine();
            Console.WriteLine(watch.ElapsedMilliseconds);

            Console.ReadKey();
        }
    }
}
