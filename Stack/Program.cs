using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SAOD
{
    class Program
    {
        private const bool SonertTop = true;

        static void Main(string[] args)
        {
            var n = 100000000;

            var AllTime = 0L;
            var stack = new MyStack<int>(4, 2);

            for (var j = 0; j < 50; ++j)
            {

                var watch = Stopwatch.StartNew();

                for (var i = 0; i != n; ++i)
                {
                    stack.Push(i);
                }
                //Console.WriteLine(stack.Count);
                for (var i = 0; i != n; ++i)
                {
                    stack.Peek();
                    stack.Pop();
                }
                // Console.WriteLine(stack.Count);
                watch.Stop();
                AllTime += watch.ElapsedMilliseconds;
            }
            Console.WriteLine(AllTime / 50);

            while (SonertTop)
            {
                try
                {
                    ReversePolish(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Exit");
                    break;
                }
            }

            Console.ReadKey();
        }

        private static int Priority(char c)
        {
            int a = 0;
            switch (c)
            {
                case '^': a = 3; break;
                case '*': a = 2; break;
                case '/': a = 2; break;
                case '+': a = 1; break;
                case '-': a = 1; break;
            }
            return a;
        }
    

        private static void ReversePolish(string sample)
        {
            var result = string.Empty;

            var stack = new MyStack<char>();

            for (var i = 0; i < sample.Length; i++)
            {
                if (char.IsNumber(sample[i]))
                {
                    if (i != 0 && !char.IsNumber(sample[i - 1]))
                    {
                        result += ' ';
                    }

                    result += sample[i];
                }
                else
                    switch (sample[i])
                    {
                        case '(':
                            stack.Push(sample[i]);
                            break;
                        case ')':
                            {
                                while (stack.Peek() != '(' && Priority(stack.Peek()) >= Priority(sample[i]))
                                {
                                    result += $" {stack.Peek()}";
                                    stack.Pop();
                                }

                                stack.Pop();
                                break;
                            }
                        default:
                            {
                                while (!stack.Empty() && Priority(stack.Peek()) >= Priority(sample[i]))
                                {
                                    result += $" {stack.Peek()}";
                                    stack.Pop();
                                }

                                stack.Push(sample[i]);
                                break;
                            }
                    }
            }

            while (!stack.Empty())
            {
                result += $" {stack.Peek()}";
                stack.Pop();
            }

            Console.WriteLine(result);
        }
    }

    class MemoryBlockDoubleQueue<T>
    {
        public MemoryBlockDoubleQueue<T> prevBlock { get; private set; }
        public MemoryBlockDoubleQueue<T> nextBlock { get; set; }

        public T[] Memory { get; private set; }

        public MemoryBlockDoubleQueue(int size, MemoryBlockDoubleQueue<T> prev, MemoryBlockDoubleQueue<T> next = null)
        {
            Memory = new T[size];
            prevBlock = prev;
        }
    }
    class MyStack<T>
    {
        private int blockIter = -1;
        private int blockCapacity = 8;
        private int mult = 4;
        public int Capacity { get; private set; } = 8;
        public int Count { get; private set; } = 0;
        public T IO { get => Peek(); set => Push(value); }

        MemoryBlockDoubleQueue<T> memoryBlock;

        private void BlockDrop()
        {
            memoryBlock.prevBlock.nextBlock = memoryBlock;
            memoryBlock = memoryBlock.prevBlock;

            Capacity -= blockCapacity;
            blockCapacity /= mult;
            blockIter = blockCapacity - 1;
        }

        private void BlockAdd()
        {
            blockCapacity *= mult;
            Capacity += blockCapacity;
            memoryBlock = memoryBlock.nextBlock ?? new MemoryBlockDoubleQueue<T>(blockCapacity, memoryBlock);
            blockIter = 0;
        }

        public void Pop()
        {
            if (Count < 0)
                throw new Exception("Meow");

            --Count;

            if (--blockIter >= 0 || Count == 0)
                return;

            BlockDrop();
        }

        public T PopEx()
        {
            var toOut = Peek();
            Pop();
            return toOut;
        }

        public T Peek()
        {
            return memoryBlock.Memory[blockIter];
        }

        public void Push(T toPush)
        {
            if (++blockIter >= blockCapacity)
                BlockAdd();

            memoryBlock.Memory[blockIter] = toPush;

            ++Count;
        }

        public MyStack(int defaultSize = 4, int multiplier = 4)
        {
            blockCapacity = Capacity = defaultSize;
            mult = multiplier;
            memoryBlock = new MemoryBlockDoubleQueue<T>(blockCapacity, null);
        }

        public void Clear()
        {
            memoryBlock = null;
        }

        public bool Empty() => Count == 0;

        public static implicit operator bool(MyStack<T> stack) => stack.Empty();

        public static explicit operator int(MyStack<T> stack) => stack.Count;
    }

}
