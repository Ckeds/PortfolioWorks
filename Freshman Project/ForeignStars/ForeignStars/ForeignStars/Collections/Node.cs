using System;
//
using System.Linq;
using System.Text;

namespace Collections
{
    /// <summary>
    /// Node class stores data and links to next Node
    /// A lot of this comes from Professor Schwartz</summary>
    /// <typeparam name="T">
    /// Type of data stored</typeparam>
    class Node<T>
    {
        /// <summary>
        /// The node's data</summary>
        private T data;
        /// <summary>
        /// A reference to the next node</summary>
        private Node<T> next;
        /// <summary>
        /// Index of Node in List</summary>
        private int index;

        /// <summary>
        /// Value stored at current Node</summary>
        public T Data { get { return data; } set { data = value; } }
        /// <summary>
        /// Next node in list</summary>
        public Node<T> Next { get { return next; } set { next = value; } }
        /// <summary>
        /// Index of node in List</summary>
        public int Index { get { return index; } set { index = value; } }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Node() { }


        public Node(T data)
        {
            this.data = data;
        }

        /// <summary>
        /// Constructs a node with a data element, and null for the next node
        /// </summary>
        /// <param name="data">The data element</param>
        /// <param name="index">Index of element added</param>
        public Node(T data, int index) : this(data, index, null) { }

        /// <summary>
        /// Constructs a node with a data element and a next node
        /// </summary>
        /// <param name="data">The data element</param>
        /// <param name="index">Index of element added</param>
        /// <param name="next">The next node</param>
        public Node(T data, int index, Node<T> next)
        {
            this.data = data;
            this.index = index;
            this.next = next;
        }

        /// <summary>
        /// Returnts current node's data and all nodes after it</summary>
        /// <returns>
        /// Current node's data and all nodes after it</returns>
        public override string ToString()
        {
            return data.ToString();
        }
    }
}
