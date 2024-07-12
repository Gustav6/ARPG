using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARPG
{
    public static class AStar
    {
        public static bool FoundPath { get; private set; }
        private static Tile[,] grid;

        private static Node targetNode;
        private static Node startingNode;

        public static List<Node> GetPath(Tile[,] grid, Node start, Node target)
        {
            if (start == target)
                return new List<Node>();

            FindPath(grid, start, target);
            List<Node> path = new();

            if (FoundPath)
            {
                Node currentTile = target.parent;

                if (currentTile == null)
                {
                    return path;
                }

                while (currentTile != startingNode)
                {
                    if (currentTile.parent != null)
                    {
                        path.Add(currentTile);
                        currentTile = currentTile.parent;
                    }
                }

                path.Reverse();
            }

            return path;
        }

        private static void FindPath(Tile[,] _grid, Node start, Node target)
        {
            // Variables needed to run the method
            if (target == null || start == null || _grid == null)
                return;

            startingNode = start;
            targetNode = target;
            grid = _grid;
            FoundPath = false;

            #region Reset map
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    if (grid[x, y].node != targetNode && grid[x, y].node != startingNode)
                    {
                        grid[x, y].node.ResetNode();
                        grid[x, y].node.ownerOfNode.color = Color.White;
                    }
                }
            }
            #endregion

            HashSet<Node> closedNodes = new();
            List<Node> openNodes = new()
            {
                startingNode
            };

            Node currentNode = startingNode;

            while (!FoundPath && openNodes.Count > 0)
            {
                for (int i = 0; i < openNodes.Count; i++)
                {
                    if (currentNode == null)
                    {
                        currentNode = openNodes[i];
                    }
                    else if (currentNode.fCost > openNodes[i].fCost)
                    {
                        currentNode = openNodes[i];
                    }
                    else if (currentNode.fCost == openNodes[i].fCost && currentNode.hCost > openNodes[i].hCost)
                    {

                    }
                }

                #region Check neighbors
                foreach (Node neighbor in GetNeighbors(currentNode.GridX, currentNode.GridY))
                {
                    if (neighbor == targetNode)
                    {
                        FoundPath = true;
                        targetNode.parent = currentNode;
                        break;
                    }
                    else if (closedNodes.Contains(neighbor) || !neighbor.Walkable)
                    {
                        continue;
                    }

                    int newPathCost = currentNode.gCost + GetDistance(currentNode, neighbor);

                    if (newPathCost < neighbor.gCost || !openNodes.Contains(neighbor))
                    {
                        openNodes.Add(neighbor);

                        SetCosts(grid[currentNode.GridX, currentNode.GridY].node, neighbor);
                    }
                }
                #endregion

                closedNodes.Add(currentNode);
                openNodes.Remove(currentNode);
                currentNode = null;
            }
        }

        private static void SetCosts(Node currentNode, Node neighbor)
        {
            #region Set baseCost
            int baseGCost;

            if (currentNode.GridX == neighbor.GridX || currentNode.GridY == neighbor.GridY)
            {
                baseGCost = 10;
            }
            else
            {
                baseGCost = 14;
            }
            #endregion

            #region Set node values
            neighbor.hCost = GetHCost(neighbor);
            neighbor.gCost = currentNode.gCost + baseGCost;
            neighbor.fCost = neighbor.gCost + neighbor.hCost;

            neighbor.parent = currentNode;
            #endregion
        }

        private static int GetHCost(Node currentNode)
        {
            // The cost from the current node to the target
            int totalCost = 0;

            List<Node> hasVisited = new()
            {
                currentNode,
            };

            bool foundTarget = false;
            Node closestNode = currentNode;

            while (!foundTarget)
            {
                int moveCost;
                Node prevNode = closestNode;
                List<Node> neighbors = new();
                neighbors.AddRange(GetNeighbors(closestNode.GridX, closestNode.GridY));

                // "Walk" towards target to calculate final distance

                #region Get the closest node

                foreach (Node neighbor in neighbors)
                {
                    if (hasVisited.Contains(neighbor))
                    {
                        continue;
                    }

                    #region Distance from the new neighboring node to target

                    float xDistance = MathF.Abs(neighbor.Position.X - targetNode.Position.X);
                    float yDistance = MathF.Abs(neighbor.Position.Y - targetNode.Position.Y);

                    #endregion

                    #region Distance from the current closest node to target

                    float tempXDistance = MathF.Abs(closestNode.Position.X - targetNode.Position.X);
                    float tempYDistance = MathF.Abs(closestNode.Position.Y - targetNode.Position.Y);

                    #endregion

                    if (xDistance < tempXDistance || yDistance < tempYDistance)
                    {
                        closestNode = neighbor;
                        hasVisited.Add(closestNode);
                    }
                }
                #endregion

                #region Determine if move is with adjacent node or not
                if (closestNode.GridX == prevNode.GridX || closestNode.GridY == prevNode.GridY)
                {
                    moveCost = 10;
                }
                else
                {
                    moveCost = 14;
                }
                #endregion

                totalCost += moveCost;

                if (closestNode == targetNode)
                {
                    foundTarget = true;
                }
            }

            return totalCost;
        }

        private static int GetDistance(Node NodeA, Node NodeB)
        {
            int distanceX = Math.Abs(NodeA.GridX - NodeB.GridX);
            int distanceY = Math.Abs(NodeA.GridY - NodeB.GridY);

            if (distanceX > distanceY)
            {
                return 14 * distanceY + 10 * (distanceX - distanceY);
            }
            else
            {
                return 14 * distanceX + 10 * (distanceY - distanceX);
            }
        }

        private static List<Node> GetNeighbors(int positionX, int positionY)
        {
            List<Node> result = new();
            Node NeighboringNode;

            #region Get neighbors
            if (InBounds(grid, positionX + 1, positionY))
            {
                NeighboringNode = grid[positionX + 1, positionY].node;

                result.Add(NeighboringNode);
            }
            if (InBounds(grid, positionX - 1, positionY))
            {
                NeighboringNode = grid[positionX - 1, positionY].node;

                result.Add(NeighboringNode);
            }
            if (InBounds(grid, positionX, positionY + 1))
            {
                NeighboringNode = grid[positionX, positionY + 1].node;

                result.Add(NeighboringNode);
            }
            if (InBounds(grid, positionX, positionY - 1))
            {
                NeighboringNode = grid[positionX, positionY - 1].node;

                result.Add(NeighboringNode);
            }

            if (InBounds(grid, positionX + 1, positionY + 1))
            {
                NeighboringNode = grid[positionX + 1, positionY + 1].node;

                result.Add(NeighboringNode);
            }
            if (InBounds(grid, positionX + 1, positionY - 1))
            {
                NeighboringNode = grid[positionX + 1, positionY - 1].node;

                result.Add(NeighboringNode);
            }
            if (InBounds(grid, positionX - 1, positionY - 1))
            {
                NeighboringNode = grid[positionX - 1, positionY - 1].node;

                result.Add(NeighboringNode);
            }
            if (InBounds(grid, positionX - 1, positionY + 1))
            {
                NeighboringNode = grid[positionX - 1, positionY + 1].node;

                result.Add(NeighboringNode);
            }
            #endregion

            return result;
        }

        private static bool InBounds(Tile[,] map, int x, int y)
        {
            return 0 <= y && y < map.GetLength(1) && 0 <= x && x < map.GetLength(0);
        }
    }
}
