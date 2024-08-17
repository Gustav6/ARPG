using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public struct Node(Vector2 position, Point gridPosition, bool walkable, Tile tile)
    {
        public readonly bool Walkable => walkable;
        public readonly Vector2 Position => position;
        public readonly Point GridPosition => gridPosition;
        public readonly Rectangle Hitbox => tile.Hitbox;

        public int gCost; // Cost from starting node
        public int hCost; // How far away from end node
        public readonly int FCost => hCost + gCost;

        public Point? parent; // What node that "owns" this node

        public void SetCosts(Node prevNode, Node targetNode)
        {
            #region Get base movement cost
            int baseGCost;

            if (prevNode.GridPosition.X == GridPosition.X || prevNode.GridPosition.Y == GridPosition.Y)
            {
                baseGCost = 10;
            }
            else
            {
                baseGCost = 14;
            }
            #endregion

            #region Set values
            // Give the costs for node
            hCost = Library.AStarManager.GetDistance(this, targetNode);
            gCost = prevNode.gCost + baseGCost;

            // Give the neighbor the node that "owns" it
            parent = prevNode.GridPosition;
            #endregion
        }
    }
}
