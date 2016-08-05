using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collections
{
    /// <summary>
    /// A node based implementation of a stack
    /// Adapted from prior GSD2 material with many other authors
    /// A lot of this code was written by Professor Schwartz</summary>
    /// <typeparam name="T">
    /// The data type the NodeStack holds</typeparam>
    public class Stack<T>
    {
        #region Fields
        /// <summary>
        /// A reference to the top element in the stack
        /// </summary>
        private Node<T> top;
        /// <summary>
        /// Used to keep track of the number of elements in the stack
        /// </summary>
        private int count;
        /// <summary>
        /// Max size of stack
        /// </summary>
        private int maxSize;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <author>Ryan Conrad</author>
        #region Properties

        /// <summary>
        ///Used to obtain the number of elements in the stack
        /// </summary>
        /// <author>Ryan Conrad</author>
        public int Count { get { return count; } }

        #endregion
        #region Indexer & Enumerator
        /// <summary>
        /// Indexer for List
        /// </summary>
        /// <param name="index">Index of List item to get or set</param>
        /// <returns>get: Data at index set: nothing</returns>
        public T this[int index]
        {
            get
            {
                Node<T> temp = top;

                try
                {
                    for (int i = 0; i < index; i++)
                    {
                        temp = temp.Next;
                    }
                }
                catch (Exception)
                {
                    throw new IndexOutOfRangeException("index is not in List");
                }

                return temp.Data;
            }

            set
            {
                Node<T> temp = top;

                try
                {
                    for (int i = 0; i < index; i++)
                    {
                        temp = temp.Next;
                    }
                }
                catch (Exception)
                {
                    throw new IndexOutOfRangeException("index is not in List");
                }

                temp.Data = value;
            }
        }

        /// <summary>
        /// Enumerator for loops
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            Node<T> temp = top;

            for (int i = 0; i < count; i++)
            {
                yield return temp.Data;

                temp = temp.Next;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Stack() : this(0) { }

        public Stack(int maxSize)
        {
            this.maxSize = maxSize;
            top = null;
        }
        #endregion

        /// <summary>
        /// Adds an element to the stack
        /// </summary>
        /// <param name="data">The element to add</param>
        public void Push(T data)
        {
            Node<T> add = new Node<T>(data, count);

            add.Next = top;
            top = add;

            // If the max size is set, and the count == max size, remove the end
            if (maxSize != 0)
                if (count == maxSize)
                {

                    count--;
                }

            count++;
        }

        /// <summary>
        /// Quietly removes the top element on the stack
        /// </summary>
        public void Pop()
        {
            if (top != null)
            {
                top = top.Next;
                count--;
            }
            else throw new Exception("Stack is empty!");
        }

        /// <summary>
        /// Remove the last element in stack
        /// </summary>
        public void removeEnd()
        {
            Node<T> current = top;

            for (int i = 0; i < count - 1; i++)
            {
                current = current.Next;
            }
            count--;
            current.Next = null;
        }

        /// <summary>
        /// Returns the top element on the stack without removing it
        /// </summary>
        /// <returns>The top element</returns>
        public T Peek()
        {
            try
            {
                return top.Data;
            }
            catch
            {
                throw new Exception("Stack is empty!");
            }
        }

        /// <summary>
        /// Indicates how many elements are in the stack
        /// </summary>
        /// <returns>The number of elements currently in the stack</returns>
        public int Size
        {
            get { return count; }
        }

        /// <summary>
        /// Indicates whether the stack is empty or not
        /// </summary>
        /// <returns>True if the stack is empty, false otherwise</returns>
        public bool isEmpty()
        {
            return count == 0;
        }

        /// <summary>
        /// Indicates whether the stack is full or not
        /// </summary>
        /// <returns>True if the stack is full, false otherwise</returns>
        public bool Full()
        {
            return false;
        }

        /// <summary>
        /// Stringify the stack
        /// </summary>
        /// <returns> [the_stack]</returns>
        public override string ToString()
        {
            string returnString = top.Data.ToString();
            Node<T> current = top;

            for (int i = 0; i < count - 1; i++)
            {
                current = current.Next;
                returnString += ", " + current.Data;
            }

            return returnString;
        }
    }
}
