﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class Node(Vector2 position, Point gridPosition, bool walkable) : IHeapItem<Node>
    {
        public readonly bool walkable = walkable;
        public readonly Vector2 worldPosition = position;
        public readonly Point gridPosition = gridPosition;

        public int gCost; // Cost from starting node
        public int hCost; // How far away from end node
        public int FCost { get { return hCost + gCost; } }

        public Point? parent; // What node that "owns" this node

        public int heapIndex;
        public int HeapIndex
        {
            get { return heapIndex; }
            set { heapIndex = value; }
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = FCost.CompareTo(nodeToCompare.FCost);

            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }

            return -compare;
        }
    }
}
