// adapted from http://users.cs.fiu.edu/~weiss/dsj2/code/code.html (Chap 21)

using System;
using System.Collections.Generic;

namespace Collections.PriorityQueue
{

    /// <summary>
    /// Abstract class for creating min and max heaps.
    /// </summary>
    /// <typeparam name="T">type of element in heap</typeparam>
    public abstract class BinaryHeap<T>
    {

        #region members
        #region fields

        private int currentSize;                   // how many elements in heap
        private T[] array;                         // array stores heap
        private const int DEFAULT_CAPACITY = 100;  // initial array size for heap

        #endregion fields

        # region construction
        /// <summary>
        /// Create the heap with initial size: 100+1.
        /// </summary>
        public BinaryHeap()
        {
            currentSize = 0;
            array = new T[DEFAULT_CAPACITY + 1]; // need default capacity
        }

        /// <summary>
        /// Add unique element to heap.
        /// </summary>
        /// <param name="x">element to add</param>
        protected void put(T x)
        {

            // expand array if we need more space:
            if (currentSize + 1 == array.Length)
                doubleArray();

            //---------------------------------------------------------------
            // percolate up
            //---------------------------------------------------------------
            int hole = ++currentSize; // index pointing to spot just after last element
            array[0] = x;             // put element to add in temp location (0 index)

            // compare x with element at mid-point
            // if x is bigger (assume max heap), put it there
            // keep going until x is smaller than remaining elements
            // (when doing a min heap, compare hole w/x):
            for (; compare(x, array[hole / 2]) > 0; hole /= 2)
                array[hole] = array[hole / 2];
            array[hole] = x;
        }

        # endregion construction

        #region removal and reheap

        /// <summary>
        /// Get element from heap.
        /// </summary>
        /// <returns>top of heap</returns>
        protected T get()
        {
            T minItem = element();

            // return 1st element of array
            array[1] = array[currentSize--];  // grab next element
            percolateDown(1);                 // reheap
            return minItem;                   // will return max when doing max heap
        }

        /// <summary>
        /// Top of heap.
        /// </summary>
        /// <returns>return top of heap</returns>
        protected T element()
        {
            return array[1];
        }
        /// <summary>
        /// Reheap.
        /// </summary>
        /// <param name="hole">position to fill</param>
        private void percolateDown(int hole)
        {
            int child;
            T tmp = array[hole];

            for (; hole * 2 <= currentSize; hole = child)
            {
                child = hole * 2;
                if (child != currentSize && compare(array[child + 1], array[child]) > 0)
                    child++;
                if (compare(array[child], tmp) > 0)
                    array[hole] = array[child];
                else
                    break;
            }
            array[hole] = tmp;
        }

        #endregion removal and reheap

        #region utilities

        /// <summary>
        /// Implement for specific heap type to do min/max comparison.
        /// </summary>
        /// <param name="lhs">element to compare</param>
        /// <param name="rhs">other element to compare</param>
        /// <returns>result of CompareTo</returns>
        protected abstract int compare(T lhs, T rhs);

        /// <summary>
        /// Double array size.
        /// </summary>
        private void doubleArray()
        {
            T[] newArray = new T[array.Length * 2];
            for (int i = 0; i < array.Length; i++)
                newArray[i] = array[i];
            array = newArray;
        }

        #endregion utilities

        #region getters

        /// <summary>
        /// Return size of heap.
        /// </summary>
        /// <returns>size of heap</returns>
        public int size()
        {
            return currentSize;
        }

        /// <summary>
        /// Is heap empty?
        /// </summary>
        /// <returns>true if empty, else false</returns>
        public bool isEmpty()
        {
            return currentSize == 0;
        }

        /// <summary>
        /// "Erase" heap (set size to 0).
        /// </summary>
        public void clear()
        {
            currentSize = 0;
        }

        #endregion getters

        #region ToTree
        /// <summary>
        /// Treeify heap (as string).
        /// </summary>
        /// <returns>string version of heap</returns>
        public string ToTree()
        {
            return ToTree("│   ", "┼───", 1, 0);
        }

        private string ToTree(string blank, string spacing, int index, int count)
        {
            string s = "";
            if (index <= currentSize)
            {  // if (!(array[index].Equals(default(T)))) works except if you want to store "zeros" 
                s += array[index];
                count++;
                s += "\n" + spacing + ToTree(blank, blank + spacing, index * 2, count);      // left child
                s += "\n" + spacing + ToTree(blank, blank + spacing, index * 2 + 1, count);  // right child
            }
            return s;
        }
        #endregion ToTree

        #endregion members

    }

    /// <summary>
    /// MaxHeap.
    /// </summary>
    /// <typeparam name="T">type of element in heap</typeparam>
    public class MaxHeap<T> : BinaryHeap<T> where T : IComparable<T>
    {
        protected override int compare(T lhs, T rhs)
        {
            return lhs.CompareTo(rhs);
        }
    }

    /// <summary>
    /// Minheap.
    /// </summary>
    /// <typeparam name="T">type of element in heap</typeparam>
    public class MinHeap<T> : BinaryHeap<T> where T : IComparable<T>
    {
        protected override int compare(T lhs, T rhs)
        {
            return rhs.CompareTo(lhs); // flip order of lhs & rhs
        }
    }


}
