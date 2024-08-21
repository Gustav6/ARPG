using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ARPG
{
    public class AStar
    {
        public static AStar instance = new();

        private Node[,] nodes;

        private int gridWidth, gridHeight;

        private Node start, target;

        private List<Vector2> path = [];
        private List<Node> currentNodeNeighbors = [];
        private HashSet<Node> closedNodes = [];
        private Heap<Node> openNodes;

        public int amountOfRequests;
        public TimeSpan totalTimeTaken;

        public void StartFindPath()
        {

        }

        public List<Vector2> FindPath(Node[,] grid, GameObject requester, Vector2 _start, Vector2 _target)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();

            amountOfRequests++;

            gridWidth = grid.GetLength(0);
            gridHeight = grid.GetLength(1);

            path.Clear();
            closedNodes.Clear();

            if (openNodes == null || gridWidth * gridHeight > openNodes.length)
            {
                openNodes = new Heap<Node>(gridWidth * gridHeight);
            }
            else
            {
                openNodes.Clear();
            }

            nodes = grid;

            start = NodeFromWorldPoint(_start, requester);
            target = NodeFromWorldPoint(_target, Library.playerInstance);

            openNodes.Add(start);

            Node currentNode;

            while (openNodes.Count > 0)
            {
                currentNode = openNodes.RemoveFirst();

                closedNodes.Add(currentNode);

                if (currentNode == target)
                {
                    path = RetracePath();

                    sw.Stop();
                    //Debug.WriteLine("Time to find path was: " + sw.Elapsed);

                    totalTimeTaken += sw.Elapsed;

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

            return new List<Vector2>(path);
        }

        private List<Vector2> RetracePath()
        {
            path.Clear();
            Node temp = target;

            while (temp.gridPosition != start.gridPosition)
            {
                if (temp.parent != null)
                {
                    path.Add(temp.worldPosition);

                    temp = nodes[temp.parent.Value.X, temp.parent.Value.Y];
                }
                else
                {
                    break;
                }
            }

            path.Reverse();

            return path;
        }

        private Node NodeFromWorldPoint(Vector2 worldPosition, GameObject requester)
        {
            Node firstNode = nodes[0, 0];

            Point offset = new((int)firstNode.worldPosition.X / TextureManager.tileSize, (int)firstNode.worldPosition.Y / TextureManager.tileSize);

            float x = ((worldPosition.X + requester.Texture.Width / 2) / TextureManager.tileSize) - offset.X;
            float y = ((worldPosition.Y + requester.Texture.Height / 2) / TextureManager.tileSize) - offset.Y;

            x = MathHelper.Clamp(x, 0, gridWidth - 1);
            y = MathHelper.Clamp(y, 0, gridHeight - 1);

            return nodes[(int)x, (int)y];
        }

        //private Node GetNode(Point position)
        //{
        //    int i = position.X + position.Y * gridWidth;
        //
        //    return nodes[position.X, position.Y];
        //}

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
                NeighboringNode = nodes[position.X + 1, position.Y];

                list.Add(NeighboringNode);
            }
            if (InBounds(position.X - 1, position.Y))
            {
                NeighboringNode = nodes[position.X - 1, position.Y];

                list.Add(NeighboringNode);
            }
            if (InBounds(position.X, position.Y + 1))
            {
                NeighboringNode = nodes[position.X, position.Y + 1];

                list.Add(NeighboringNode);
            }
            if (InBounds(position.X, position.Y - 1))
            {
                NeighboringNode = nodes[position.X, position.Y - 1];

                list.Add(NeighboringNode);
            }

            if (InBounds(position.X + 1, position.Y + 1))
            {
                NeighboringNode = nodes[position.X + 1, position.Y + 1];

                list.Add(NeighboringNode);
            }
            if (InBounds(position.X + 1, position.Y - 1))
            {
                NeighboringNode = nodes[position.X + 1, position.Y - 1];

                list.Add(NeighboringNode);
            }
            if (InBounds(position.X - 1, position.Y - 1))
            {
                NeighboringNode = nodes[position.X - 1, position.Y - 1];

                list.Add(NeighboringNode);
            }
            if (InBounds(position.X - 1, position.Y + 1))
            {
                NeighboringNode = nodes[position.X - 1, position.Y + 1];

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
