using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ARPG
{
    public class AStar
    {
        public bool FoundPath { get; private set; }
        private Node[] nodes;

        private int gridWidth, gridHeight;

        private Point start, target;

        private List<Node> path = [];
        private HashSet<Point> closedNodes = [];

        //private List<Point> openNodes = [];
        private Heap<Node> openNodes;

        private List<Node> hCostNeighbors = [], currentNodeNeighbors = [];

        public List<Node> FindPath(Tile[,] grid, Node _start, Node _target)
        {
            if (!_target.Walkable)
                return [];

            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();

            start = _start.GridPosition;
            target = _target.GridPosition;

            gridWidth = grid.GetLength(0);
            gridHeight = grid.GetLength(1);

            FoundPath = false;

            path.Clear();
            closedNodes.Clear();

            //openNodes.Clear();
            //openNodes.Add(start);

            openNodes = new Heap<Node>(gridWidth * gridHeight);
            openNodes.Add(ref _start);

            if (nodes == null || nodes.Length < grid.Length)
            {
                nodes = new Node[grid.Length];
            }

            for (int i = 0; i < grid.Length; i++)
            {
                nodes[i] = grid[i % gridWidth, i / gridWidth].node;
            }

            Node currentNode;

            while (openNodes.Count > 0 && !FoundPath)
            {
                currentNode = openNodes.RemoveFirst();

                //currentNode = GetNode(openNodes[0]);

                //#region Get node with lowest f cost

                //for (int i = 0; i < openNodes.Count; i++)
                //{
                //    Node temp = GetNode(openNodes[i]);

                //    if (currentNode.FCost > temp.FCost || currentNode.FCost == temp.FCost && currentNode.hCost > temp.hCost)
                //    {
                //        currentNode = temp;
                //    }
                //}

                //openNodes.Remove(currentNode.GridPosition);
                //#endregion

                closedNodes.Add(currentNode.GridPosition);

                if (currentNode.GridPosition == target)
                {
                    FoundPath = true;
                    path = RetracePath(currentNode);

                    sw.Stop();

                    Debug.WriteLine("Path found: " + sw.Elapsed);

                    break;
                }

                #region Check neighbors

                GetNeighbors(currentNode.GridPosition, currentNodeNeighbors);

                for (int i = 0; i < currentNodeNeighbors.Count; i++)
                {
                    ref Node neighbor = ref System.Runtime.InteropServices.CollectionsMarshal.AsSpan(currentNodeNeighbors)[i];

                    if (closedNodes.Contains(neighbor.GridPosition) || !neighbor.Walkable)
                    {
                        continue;
                    }

                    int newPathCost = currentNode.gCost + GetDistance(currentNode, neighbor);

                    if (newPathCost < neighbor.gCost || !openNodes.Contains(neighbor))
                    {
                        SetCosts(ref currentNode, ref neighbor);

                        if (!openNodes.Contains(neighbor))
                        {
                            openNodes.Add(ref neighbor);
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

        public void SetCosts(ref Node currentNode, ref Node neighborNode)
        {
            #region Get base movement cost
            int baseGCost;

            if (currentNode.GridPosition.X == neighborNode.GridPosition.X || currentNode.GridPosition.Y == neighborNode.GridPosition.Y)
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
            neighborNode.hCost = GetDistance(neighborNode, GetNode(target));
            neighborNode.gCost = currentNode.gCost + baseGCost;

            // Give the neighbor the node that "owns" it
            neighborNode.parent = currentNode.GridPosition;
            #endregion
        }

        private List<Node> RetracePath(Node targetNode)
        {
            path.Clear();
            Node temp = targetNode;

            while (temp.GridPosition != start)
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

            if (path.Count > 0)
            {
                path.Reverse();
            }

            return path;
        }

        private ref Node GetNode(Point position)
        {
            int i = position.X + position.Y * gridWidth;

            return ref nodes[i];
        }

        public int GetDistance(Node NodeA, Node NodeB)
        {
            int distanceX = Math.Abs(NodeA.GridPosition.X - NodeB.GridPosition.X);
            int distanceY = Math.Abs(NodeA.GridPosition.Y - NodeB.GridPosition.Y);

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
