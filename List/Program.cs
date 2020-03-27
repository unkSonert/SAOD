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

            foreach (var iter in test)
            {
                foreach(var iter2 in test)
                {
                    Console.WriteLine("magic: " + iter2);
                }
                Console.WriteLine("no magic: " + iter);
            }
            Console.ReadKey();
        }
    }

    class DoubleLinkedNode<T>
    {
        private T value;

        private DoubleLinkedNode<T> next;
        private DoubleLinkedNode<T> prev;

        public DoubleLinkedNode(T value, DoubleLinkedNode<T> next = null, DoubleLinkedNode<T> prev = null)
        {
            this.value = value;
            this.next = next;
            this.prev = prev;
        }

        public void LinkToNext(DoubleLinkedNode<T> nextLink)
        {
            if (next != null)
                next.prev = nextLink;

            next = nextLink;
        }

        public void LinkToPrev(DoubleLinkedNode<T> prevLink)
        {
            if (prev != null)
                prev.next = prevLink;

            prev = prevLink;
        }

        public T GetValue() => value;

        public DoubleLinkedNode<T> GetNext() => next;
        public DoubleLinkedNode<T> GetPrev() => prev;
    }

    class DoubleLinkedList<T> : IEnumerator<T>, IEnumerable
    {
        private DoubleLinkedNode<T> first;
        private DoubleLinkedNode<T> last;
        private DoubleLinkedNode<T> enumerable;

        public T Current => enumerable.GetValue();

        object IEnumerator.Current => enumerable.GetValue();

        public void PushFront(T toPush)
        {
            if (first == null)
            {
                first = new DoubleLinkedNode<T>(toPush);
                last = first;
                enumerable = first;
                return;
            }

            first.LinkToPrev(new DoubleLinkedNode<T>(toPush));
            first = first.GetPrev();
        }

        public void PushBack(T toPush)
        {
            if (last == null)
            {
                first = new DoubleLinkedNode<T>(toPush);
                last = first;
                enumerable = first;
                return;
            }

            last.LinkToNext(new DoubleLinkedNode<T>(toPush));
            last = last.GetNext();
        }

        public void Dispose()
        {
            enumerable = null;
        }

        public bool MoveNext()
        {
            enumerable = enumerable.GetNext();
            return enumerable != null;
        }

        public void Reset()
        {
            enumerable = first;
        }

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator)MemberwiseClone();
        }
    }
}
