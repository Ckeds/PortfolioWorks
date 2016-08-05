using System;
using System.Collections.Generic;

namespace Collections
{
    /// <summary>
    /// Queue class</summary>
    /// <typeparam name="T">
    /// Type of object stored in queue</typeparam>
    class Queue<T>
    {
        /// <summary>
        /// A reference to the front element in the queue
        /// </summary>
        Node<T> front;

        /// <summary>
        /// A reference to the back element in the queue
        /// </summary>
        Node<T> back;

        /// <summary>
        /// Keeps track of how many elements are currently in the queue
        /// </summary>
        private int count;

        #region Indexer & Enumerator
        /// <summary>
        /// Indexer for List</summary>
        /// <param name="index">
        /// Index of List item to get or set</param>
        /// <returns>
        /// get: Data at index set: nothing</returns>
        public T this[int index]
        {
            get
            {
                Node<T> temp = front;

                try
                {
                    for (int ii = 0; ii < index; ii++)
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
                Node<T> temp = front;

                try
                {
                    for (int ii = 0; ii < index; ii++)
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
        /// Enumerator for for loops</summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            Node<T> temp = front;

            for (int ii = 0; ii < count; ii++)
            {
                yield return temp.Data;

                temp = temp.Next;
            }
        }
        #endregion

        /// <summary>
        /// Queue constructor
        /// </summary>
        public Queue()
        {
            count = 0;
            front = null;
            back = null;
        }

        /// <summary>
        /// Adds an element to the back of the queue
        /// </summary>
        /// <param name="data">The element to be added</param>
        public void enqueue(T data)
        {
            Node<T> toBeAdded = new Node<T>(data, count);

            if (front == null)
            {
                front = toBeAdded;
                back = toBeAdded;
            }
            else
            {
                back.Next = toBeAdded;
                back = toBeAdded;
            }


            count++;
        }

        /// <summary>
        /// Removes the front element from the queue
        /// </summary>
        /// <returns>The front element</returns>
        public T dequeue()
        {
            Node<T> temp;

            if (front == null)
            {
                throw new Exception("Queue is empty!");
            }
            else
            {
                temp = front;
                front = front.Next;
                // Count needs to get decremented...So I added this.
                count--;
            }

            return temp.Data;
        }

        /// <summary>
        /// Indicates how many elements are in the queue
        /// </summary>
        /// <returns>The number of elements currently in the queue</returns>
        public int size()
        {
            return count;
        }

        /// <summary>
        /// Indicates whether the queue is empty or not
        /// </summary>
        /// <returns>True if the queue is empty, false otherwise</returns>
        public bool empty()
        {
            return count == 0;
        }

        /// <summary>
        /// Indicates whether the queue is full or not
        /// </summary>
        /// <returns>True if the queue is full, false otherwise</returns>
        public bool full()
        {
            return false;
        }
    }
}
