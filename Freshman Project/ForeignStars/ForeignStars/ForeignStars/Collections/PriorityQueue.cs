using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collections.PriorityQueue
{
    class MinPriorityQueue<T> : MinHeap<T> where T : IComparable<T>
    {
        public void Enqueue(T x)
        {
            base.put(x);
        }
        public T Peek()
        {
            return base.element();
        }
        public T Dequeue()
        {
            return base.get();
        }
    }
}
