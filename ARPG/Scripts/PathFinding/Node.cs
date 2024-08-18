using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public struct Node(Vector2 position, Point gridPosition, bool walkable, Tile tile) : IHeapItem<Node>
    {
        public readonly bool Walkable => walkable;
        public readonly Vector2 WorldPosition => position;
        public readonly Point GridPosition => gridPosition;
        public readonly Rectangle Hitbox => tile.Hitbox;

        public int gCost; // Cost from starting node
        public int hCost; // How far away from end node
        public int heapIndex;

        public readonly int FCost => hCost + gCost;

        public Point? parent; // What node that "owns" this node

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
