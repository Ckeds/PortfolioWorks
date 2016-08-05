using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collections.PriorityQueue
{
    /// <summary>
    /// A BinaryHeap, which is ALSO a priority queue
    /// A lot of random online sources and research went into this damned thing
    /// Though I have to say, the algorithms of the left, parent, and child node's indexes
    /// made me freak out like a total math nerd
    /// Credit to Chaker Nakhli's Blog for a good portion of the code - although he used the hideous "break" on loops
    /// http://www.javageneration.com/?p=411
    /// Also, after reading through things again, it REALLY looks like I just copy-pasted my code
    /// but thats not really the case, I typed it all out, and had to run it as well to understand it
    /// SO MUCH WASTED PAPER DRAWING TREES
    /// actually this code may be flawed -> I think I needed to add one to count on
    /// Push, because I add something to the list, then I call it, but I never actually increment the BinaryHeaps
    /// internal count, which is what I use...
    /// </summary>
    /// <typeparam name="T">The type of data the Heap holds</typeparam>
    class MyBinaryHeap<T> where T : IComparable<T>
    {
        /// <summary>
        /// The heaps internal list
        /// </summary>
        private List<T> list;
        /// <summary>
        /// The amount of items in the heap
        /// </summary>
        private int count;

        /// <summary>
        /// Create a Binary Heap from an existing list
        /// </summary>
        /// <param name="list">the list to make into a heap</param>
        /// <param name="comparer">The class that implements IComparer to compare T on the heap</param>
        public MyBinaryHeap(List<T> list)
        {

            this.list = list;
            this.count = list.Count;
            Heapify();

        }

        /// <summary>
        /// Create an empty binary heap
        /// </summary>
        /// <param name="comparer">The class that implements IComparer to compare T on the heap</param>
        public MyBinaryHeap()
            : this(new List<T>())
        { }

        /// <summary>
        /// The amount of items on the heap
        /// </summary>
        public int Count { get { return count; } }


        /// <summary>
        /// Removes the root node of the heap
        /// </summary>
        /// <returns>the root node</returns>
        public T Dequeue()
        {
            if (count == 0)
                throw new Exception("Heap is Empty");
            T root = list[0];
            Swap(0, count - 1);
            count--;
            HeapDown(0);
            return root;
        }

        /// <summary>
        /// Look at the root node of the heap
        /// </summary>
        /// <returns>The root node of the heap</returns>
        public T Peek()
        {
            if (list.Count == 0)
                throw new Exception("Heap is Empty");
            return list[0];
        }

        /// <summary>
        /// Adds a node to the heap
        /// </summary>
        /// <param name="data">The item to add to the heap</param>
        public void Enqueue(T data)
        {
            /*if (count >= list.Count)
                list.Add(data);
            else
                list[count] = data;*/
            list.Add(data);
            count++;
            HeapUp(count - 1);
        }

        /// <summary>
        /// Sorts the internal list to resemble the setup of the heap
        /// </summary>
        private void Heapify()
        {
            for (int i = Parent(count - 1); i >= 0; i--)
            {
                HeapDown(i);
            }
        }

        /// <summary>
        /// Sorts a node upward in the heap based on priority
        /// called when a node is pushed into the heap
        /// </summary>
        /// <param name="current"></param>
        private void HeapUp(int current)
        {
            T elt = list[current];
            int parent;
            bool loop = true;
            while (loop)
            {
                parent = Parent(current);
                //changed to less/greater than than from less than or equals
                if (parent > 0 || list[parent].CompareTo(elt) < 0)
                    Swap(current, parent);
                else
                    loop = false;
                current = parent;
            }
        }


        /// <summary>
        /// Sorts a node from the root downward, used when the root node
        /// is popped from the heap in order to place the node with hte next priority at the root
        /// </summary>
        /// <param name="current"></param>
        private void HeapDown(int current)
        {
            bool loop = true;
            int leftChild;
            int rightChild;
            int child;
            while (loop)
            {
                leftChild = LeftChild(current);
                if (leftChild >= 0)
                {
                    rightChild = RightChild(current);
                    if (rightChild < 0)
                        child = leftChild;

                    else
                    {
                        if (list[leftChild].CompareTo(list[rightChild]) > 0)
                            child = leftChild;
                        else
                            child = rightChild;
                    }

                    if (list[child].CompareTo(list[current]) >= 0)
                    {
                        Swap(current, child);
                        current = child;
                    }
                    else
                        loop = false;
                }
                else
                    loop = false;
            }
        }

        /// <summary>
        /// Returns the parent of the specified node in the heap
        /// Returns negative 1 if the parent does not exist
        /// </summary>
        /// <param name="current">The 0-based index of the node to check in the heap</param>
        /// <returns>The 0-based index of the parent node</returns>
        public int Parent(int current)
        {
            return SafeIndex((current - 1) / 2);
        }

        /// <summary>
        /// Returns the left child of the specified node in the heap
        /// Returns negative 1 if the child does not exist
        /// </summary>
        /// <param name="current">The 0-based index of the node to check in the heap</param>
        /// <returns>The 0-based index of the left child node</returns>
        public int LeftChild(int current)
        {
            return SafeIndex(2 * current + 1);
        }

        /// <summary>
        /// Returns the right child of the specified node in the heap
        /// Returns negative 1 if the child does not exist
        /// </summary>
        /// <param name="current">The 0-based index of the node to check in the heap</param>
        /// <returns>The 0-based index of the right child node</returns>
        public int RightChild(int current)
        {
            return SafeIndex(2 * current + 2);
        }


        /// <summary>
        /// returns the index of a node
        /// returns negative 1 if a node does not exist at that index
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int SafeIndex(int i)
        {
            if (i < count)
                return i;
            else
                return -1;

        }

        /// <summary>
        /// Swaps two nodes
        /// </summary>
        /// <param name="swap1">the first node to be swapped</param>
        /// <param name="swap2">the second node to be swapped</param>
        private void Swap(int swap1, int swap2)
        {
            T temp = list[swap1];
            list[swap1] = list[swap2];
            list[swap2] = temp;
        }



    }
}
