using System;
using System.Collections.Generic;
using System.Collections;
namespace Collections
{
    /// <summary>
    /// List of objects
    /// </summary>
    /// <typeparam name="T">Type of objects stored</typeparam>
    class List<T> : IEnumerable<T>
    {
        /// <summary>
        /// First item in list
        /// </summary>
        private Node<T> head;
        /// <summary>
        /// Last item in list
        /// </summary>
        private Node<T> tail;
        /// <summary>
        /// Number of items in list
        /// </summary>
        private int count;

        /// <summary>
        /// Number of Nodes in list
        /// </summary>
        public int Count { get { return count; } }

        /// <summary>
        /// Return the data at the head of the list
        /// </summary>
        public T Head
        {
            get { return head.Data; }
        }

        /// <summary>
        /// Return the data at the end of the list
        /// </summary>
        public T Tail
        {
            get { return tail.Data; }
        }

        /// <summary>
        /// Indexer for List</summary>
        /// <param name="index">Index of List item to get or set</param>
        /// <returns>get: Data at index set: nothing</returns>
        public T this[int index]
        {
            get
            {
                Node<T> temp = head;

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
                Node<T> temp = head;

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
        /// Enumerator for foreach loops
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            //Node<T> temp = head;

            for (int i = 0; i < count; i++)
            {
                yield return this[i];
            }
        }

        ///<summary>
        /// Returns other enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Default Constructor, sets default values
        /// </summary>
        public List()
        {
            count = 0;
            head = null;
            tail = null;
        }

        /// <summary>
        /// Adds item to end of list
        /// </summary>
        /// <param name="data"> Data to add to list</param>
        public void Add(T data)
        {
            Node<T> toBeAdded = new Node<T>(data, count);        // Node to be put into array

            if (head == null)
            {
                //Console.WriteLine( "First Add" );
                head = toBeAdded;
                tail = head;
            }
            else
            {
                //Console.WriteLine( "Not First Add" );
                tail.Next = toBeAdded;
                tail = tail.Next;
            }

            count++;
        }

        /// <summary>
        /// Adds Item to front of List
        /// </summary>
        /// <param name="data">Item to add to front of List </param>
        public void AddFront(T data)
        {
            Node<T> toBeAdded = new Node<T>(data, 0, head);
            Node<T> temp = head;

            head = toBeAdded;
            count++;

            for (int i = 1; i < count; i++)
            {
                temp.Index++;
                temp = temp.Next;
            }
        }

        /// <summary>
        /// remove first node with data
        /// </summary>
        /// <param name="data">object of type T to add</param>
        public void Remove(T data)
        {
            Node<T> current = head;
            Node<T> prev = new Node<T>(data, count);

            while (current != null)// && current.Next != null)
            {
                // The following 2 lines are what I changed to make it work
                // EqualityComparer uses the default .Equals of object, comparing the hash code 
                // of the two objects which will be unique for each instance of an object
                if (EqualityComparer<T>.Default.Equals(data, current.Data))
                //if (current.Data.Equals(data))
                {
                    if (current == head) // if current is head, it has no previous, need to do something different
                    {
                        head = head.Next;
                        count--;
                    }
                    else if (current == tail)
                    {
                        tail = prev;
                        current = null;
                        count--;
                    }
                    else
                    {
                        prev.Next = current.Next;
                        current = null;
                        count--;
                    }
                    ReIndex();
                    return;
                }
                else
                {
                    prev = current;
                    current = current.Next;
                }
            }
        }

        /// <summary>
        /// Sets index of each node to its correct value
        /// </summary>
        private void ReIndex()
        {
            Node<T> temp = head;
            for (int i = 0; i < count; i++)
            {
                temp.Index = i;
                temp = temp.Next;
            }
        }


        /// <summary>
        /// Removes an item at the specified 0-based index
        /// </summary>
        /// <param name="index">The index of the item you wish to remove</param>
        /// <returns>A boolean value indicating whether an item was removed</returns>
        public void RemoveAt(int index)
        {
            //Calls the Remove function using the index
            Remove(this[index]);
        }


        /// <summary>
        /// Gets the index of the node containing T data
        /// </summary>
        /// <param name="data">The data to look for in the list</param>
        /// <returns>Returns the index of the node containing data, or -1 if there is no node containing data</returns>
        public int IndexOf(T data)
        {
            //The current node we are looking at
            Node<T> current = head;

            try
            {
                for (int i = 0; i < Count; i++)
                {
                    if (current.Data.Equals(data))
                        return current.Index;
                    else
                        current = current.Next;
                }

                //If nothing has been returned, return -1 to signal that data is not in the list
                return -1;

            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Returns whether or not the list is empty
        /// </summary>
        /// <returns>Emptyness of list</returns>
        public bool IsEmpty()
        {
            return head == null;
        }


        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }
        /// <summary>
        /// Returns all items in List
        /// </summary>
        /// <returns> A string of all items</returns>
        public override string ToString()
        {
            string returnString = "";
            if (head != null)
            {
                returnString = head.Index + ": " + head.ToString();      // String to add elements to
                Node<T> temp = head;                        // Current Node to be added to string

                for (int i = 0; i < count - 1; i++)
                {
                    temp = temp.Next;                       // Set temp as next
                    returnString += ", " + temp.Index + ": " + temp.ToString(); // Add temp to string
                }
            }

            return returnString;
        }


    }
}
