using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ForeignStars
{
    public class PathNode : IComparable<PathNode>
    {
        private Point node;
        private PathNode source;
        private Point destination;
        private int hCost;

        public PathNode(Tile start, Tile end)
        {
            node = start.GridwiseLocation;
            source = null;
            destination = end.GridwiseLocation;
            hCost += Math.Abs(destination.X - node.X);
            hCost += Math.Abs(destination.Y - node.Y);
        }

        public PathNode(Point current, PathNode source, Point destination)
        {
            this.node = current;
            this.source = source;
            this.destination = destination;
            hCost += Math.Abs(destination.X - current.X);
            hCost += Math.Abs(destination.Y - current.Y);
        }


        public int GCost
        {
            get
            {
                if (source != null)
                    return source.GCost + 1;
                else
                    return 0;
            }
        }

        public int FCost
        {
            get { return (GCost + hCost); }
        }

        public int HCost
        {
            get { return hCost; }
        }


        public int CompareTo(PathNode N)
        {
            if (FCost > N.FCost)
            {
                return 1;
            }
            else if (FCost < N.FCost)
            {
                return -1;
            }
            else
            {/*
                if (hCost > N.HCost)
                {
                    return 1;
                }
                else if(hCost < N.HCost)
                {
                    return -1;
                }
                else
                {
                return 0;
                }*/
                return 0;
            }
        }

        public Point Current
        {
            get { return node; }
        }
        public PathNode Source
        {
            get { return source; }
            set { source = value; }
        }

        public Stack<PathNode> GetPath()
        {
            Stack<PathNode> path = new Stack<PathNode>();
            path.Push(this);
            do
            {
                if (path.Peek().Source == null)
                { break; }
                path.Push(path.Peek().source);
            } while (path.Peek().source != null);
            return path;
        }
    }
}
