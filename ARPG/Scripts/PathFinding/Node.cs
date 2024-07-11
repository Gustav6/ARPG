using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public class Node
    {
        public Vector2 Position { get; private set; }
        public int GridX { get; private set; }
        public int GridY { get; private set; }

        public int gCost; // Cost from starting node
        public int hCost; // How far away from end node
        public int fCost; // GCost + hCost
        public Tile ownerOfNode;
        public Node parent; // What node that "owns" the current node
        public bool Walkable { get; private set; }

        public Node(Vector2 position, int gridX, int gridY, bool walkable, Tile ownerOfNode)
        {
            Position = position;
            GridX = gridX;
            GridY = gridY;
            Walkable = walkable;
            this.ownerOfNode = ownerOfNode;
        }

        public void ResetNode()
        {
            gCost = 0;
            hCost = 0;
            fCost = 0;
        }
    }
}
