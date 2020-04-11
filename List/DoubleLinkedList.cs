using System;
using System.Collections;
using System.Collections.Generic;

namespace List
{
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
            { 
                next.prev = nextLink;
                nextLink.next = next;
            }

            next = nextLink;

            nextLink.prev = this;
        }

        public void LinkToPrev(DoubleLinkedNode<T> prevLink)
        {
            if (prev != null)
            { 
                prev.next = prevLink;
                prevLink.prev = prev;
            }
          
            prev = prevLink;

            prevLink.next = this;
        }

        public T GetValue() => value;

        public void SetValue(T value) => this.value = value;

        public DoubleLinkedNode<T> GetNext() => next;
        public DoubleLinkedNode<T> GetPrev() => prev;

        public void SetPrev(DoubleLinkedNode<T> toPrev)
        {
            if (toPrev != null)
                toPrev.next = this;

            prev = toPrev;
        }

        public void SetNext(DoubleLinkedNode<T> toNext)
        {
            if (toNext != null)
                toNext.prev = this;

            next = toNext;
        }

        public static implicit operator T (DoubleLinkedNode<T> node) => node.GetValue();
    }

    class DoubleLinkedList<T> : IEnumerable
        where T : IEquatable<T>
    {
        private DoubleLinkedNode<T> first;
        private DoubleLinkedNode<T> last;
        private int count = 0;

        public void PushFront(T toPush)
        {
            if (first == null)
            {
                first = new DoubleLinkedNode<T>(toPush);
                last = first;
                ++count;
                return;
            }

            first.LinkToPrev(new DoubleLinkedNode<T>(toPush));
            first = first.GetPrev();
            ++count;
        }

        public void PushBack(T toPush)
        {
            if (last == null)
            {
                first = new DoubleLinkedNode<T>(toPush);
                last = first;
                ++count;
                return;
            }

            last.LinkToNext(new DoubleLinkedNode<T>(toPush));
            last = last.GetNext();
            ++count;
        }

        public bool Insert(int index, T toInsert, bool insertLastIfFail = false)
        {
            try
            {
                if (index == 0)
                {
                    PushFront(toInsert);
                    return true;
                }

                var iter = first;

                while (--index > 0)
                    iter = iter.GetNext();

                iter.LinkToNext(new DoubleLinkedNode<T>(toInsert));

                ++count;

                return true;
            }
            catch (Exception)
            {
                if (insertLastIfFail)
                    PushBack(toInsert);
            }
            return false;
        }

        public void Remove(T toRemove, bool removeAllIterations = false)
        {
            var iter = first;

            while (iter != null)
            {
                if (iter.GetValue().Equals(toRemove))
                {
                    if (iter == first)
                    {
                        first = iter.GetNext();
                        first.SetPrev(null);
                    }
                    else if (iter == last)
                    {
                        iter.GetPrev().SetNext(null);
                    }
                    else
                    {
                        iter.GetPrev().SetNext(iter.GetNext());
                    }

                    --count;

                    if (!removeAllIterations)
                        break;
                }

                iter = iter.GetNext();
            }
        }

        public void Clear()
        {
            first = last = null;
            count = 0;
        }

        public int Count() => count;

        public T this[int index]
        {
            get
            {
                var iter = first;

                while (index-- != 0)
                    first = first.GetNext();

                return iter.GetValue();
            }
            set
            {
                var iter = first;

                while (index-- != 0)
                    first = first.GetNext();

                iter.SetValue(value);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new LinkedEnumerator<T>(first);
        }
    }

    class LinkedEnumerator<T> : IEnumerator<T>
    {
        DoubleLinkedNode<T> iterator, start;
        public LinkedEnumerator(DoubleLinkedNode<T> startNode)
        {
           start = startNode;
        }

        public T Current => iterator.GetValue();

        object IEnumerator.Current => iterator.GetValue();

        public void Dispose()
        {
            iterator = null;
        }

        public bool MoveNext()
        {
            if (iterator == null)
            {
                iterator = start;
                return iterator != null;
            }
            iterator = iterator.GetNext();
            return iterator != null;
        }

        public void Reset()
        {
            iterator = start;
        }
    }
}
