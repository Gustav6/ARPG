using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ARPG
{
    public class AStar
    {
        public static AStar instance = new();

        private Node[] nodes;

        private int gridWidth, gridHeight;

        private Node start, target;

        private List<Node> path = [];
        private HashSet<Node> closedNodes = [];
        private Heap<Node> openNodes;

        private List<Node> hCostNeighbors = [], currentNodeNeighbors = [];

        public void StartFindPath()
        {

        }

        public List<Node> FindPath(Tile[,] grid, Vector2 _start, Vector2 _target)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();

            gridWidth = grid.GetLength(0);
            gridHeight = grid.GetLength(1);

            path.Clear();
            closedNodes.Clear();

            if (openNodes == null || gridWidth * gridHeight > openNodes.Count)
            {
                openNodes = new Heap<Node>(gridWidth * gridHeight);
            }

            if (nodes == null || nodes.Length < grid.Length)
            {
                nodes = new Node[grid.Length];
            }

            for (int i = 0; i < grid.Length; i++)
            {
                nodes[i] = grid[i % gridWidth, i / gridWidth].node;
            }

            start = NodeFromWorldPoint(_target);
            target = NodeFromWorldPoint(_start);

            openNodes.Add(start);

            Node currentNode;

            while (openNodes.Count > 0)
            {
                currentNode = openNodes.RemoveFirst();

                closedNodes.Add(currentNode);

                if (currentNode == target)
                {
                    path = RetracePath(currentNode);

                    sw.Stop();
                    Debug.WriteLine("Path found: " + sw.Elapsed);

                    break;
                }

                #region Check neighbors

                GetNeighbors(currentNode.gridPosition, currentNodeNeighbors);

                foreach (Node neighbor in currentNodeNeighbors)
                {
                    if (closedNodes.Contains(neighbor) || !neighbor.walkable)
                    {
                        continue;
                    }

                    int newPathCost = currentNode.gCost + GetDistance(currentNode, neighbor);

                    if (newPathCost < neighbor.gCost || !openNodes.Contains(neighbor))
                    {
                        neighbor.gCost = newPathCost;
                        neighbor.hCost = GetDistance(neighbor, target);
                        neighbor.parent = currentNode.gridPosition;

                        if (!openNodes.Contains(neighbor))
                        {
                            openNodes.Add(neighbor);
                        }
                        else
                        {
                            openNodes.UpdateItem(neighbor);
                        }
                    }
                }

                #endregion
            }

            return new List<Node>(path);
        }

        private List<Node> RetracePath(Node currentNode)
        {
            path.Clear();
            Node temp = currentNode;

            while (temp.gridPosition != start.gridPosition)
            {
                if (temp.parent != null)
                {
                    path.Add(temp);

                    temp = GetNode(temp.parent.Value);
                }
                else
                {
                    break;
                }
            }

            return path;
        }

        private Node NodeFromWorldPoint(Vector2 worldPosition)
        {
            Point offset = new((int)nodes[0].worldPosition.X / TextureManager.tileSize, (int)nodes[0].worldPosition.Y / TextureManager.tileSize);

            float percentX = (worldPosition.X) / TextureManager.tileSize - offset.X;
            float percentY = (worldPosition.Y) / TextureManager.tileSize - offset.Y;

            percentX = MathHelper.Clamp(percentX, 0, gridWidth - 1);
            percentY = MathHelper.Clamp(percentY, 0, gridHeight - 1);

            return GetNode(new Point((int)percentX, (int)percentY));
        }

        private Node GetNode(Point position)
        {
            int i = position.X + position.Y * gridWidth;

            return nodes[i];
        }

        private int GetDistance(Node NodeA, Node NodeB)
        {
            int distanceX = Math.Abs(NodeA.gridPosition.X - NodeB.gridPosition.X);
            int distanceY = Math.Abs(NodeA.gridPosition.Y - NodeB.gridPosition.Y);

            if (distanceX > distanceY)
            {
                return 14 * distanceY + 10 * (distanceX - distanceY);
            }
            else
            {
                return 14 * distanceX + 10 * (distanceY - distanceX);
            }
        }

        private void GetNeighbors(Point position, List<Node> list)
        {
            list.Clear();
            Node NeighboringNode;

            #region Get neighbors
            if (InBounds(position.X + 1, position.Y))
            {
                NeighboringNode = GetNode(new Point(position.X + 1, position.Y));

                list.Add(NeighboringNode);
            }
            if (InBounds(position.X - 1, position.Y))
            {
                NeighboringNode = GetNode(new Point(position.X - 1, position.Y));

                list.Add(NeighboringNode);
            }
            if (InBounds(position.X, position.Y + 1))
            {
                NeighboringNode = GetNode(new Point(position.X, position.Y + 1));

                list.Add(NeighboringNode);
            }
            if (InBounds(position.X, position.Y - 1))
            {
                NeighboringNode = GetNode(new Point(position.X, position.Y - 1));

                list.Add(NeighboringNode);
            }

            if (InBounds(position.X + 1, position.Y + 1))
            {
                NeighboringNode = GetNode(new Point(position.X + 1, position.Y + 1));

                list.Add(NeighboringNode);
            }
            if (InBounds(position.X + 1, position.Y - 1))
            {
                NeighboringNode = GetNode(new Point(position.X + 1, position.Y - 1));

                list.Add(NeighboringNode);
            }
            if (InBounds(position.X - 1, position.Y - 1))
            {
                NeighboringNode = GetNode(new Point(position.X - 1, position.Y - 1));

                list.Add(NeighboringNode);
            }
            if (InBounds(position.X - 1, position.Y + 1))
            {
                NeighboringNode = GetNode(new Point(position.X - 1, position.Y + 1));

                list.Add(NeighboringNode);
            }
            #endregion
        }

        private bool InBounds(int x, int y)
        {
            return 0 <= y && y < gridHeight && 0 <= x && x < gridWidth;
        }
    }
}
