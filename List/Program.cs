using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace List
{
    class Program
    {
        static void Main(string[] args)
        {
            DoubleLinkedList<int> test = new DoubleLinkedList<int>();
            
            for (int i = 0; i < 10; ++i) 
                test.PushBack(i);

            test.Insert(4, 228);
            test.Insert(6, 228);
            test.Insert(0, 228);
            test.Insert(1, 228);
            test.Insert(100, 228, true);

            foreach (var iter in test)
            {
                Console.WriteLine(iter);
            }

            test.Remove(228, true);

            foreach (var iter in test)
            {
                Console.WriteLine(iter);
            }

            Console.ReadKey();

            var memesCheck = new LinkedList<int>();
            for (int i = 0; i < 20; ++i)
                memesCheck.AddLast(i);

            var index = memesCheck.IndexOf(10);
            Console.WriteLine(index);
        }
    }
}
