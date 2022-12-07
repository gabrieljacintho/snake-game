using System.Collections.Generic;
using UnityEngine;

namespace ARVORE.Tilemap
{
    public enum MovementMode
    {
        StraightAndDiagonal,
        OnlyStraight,
        OnlyDiagonal
    }

    public class Pathfinding
    {
        private const int StraightMoveCost = 10;
        private const int DiagonalMoveCost = 14;

        public MovementMode movementMode;

        private readonly Grid _grid;
        private PathNode[,] _nodesGrid;

        private List<PathNode> _openList;
        private List<PathNode> _closedList;


        public Pathfinding(Grid grid, MovementMode movementMode = MovementMode.StraightAndDiagonal)
        {
            _grid = grid;
            this.movementMode = movementMode;
        }

        public List<Vector2> FindPath(Vector2 startWorldPosition, Vector2 endWorldPosition)
        {
            if (_grid == null) return null;

            Cell startCell = _grid.WorldToCell(startWorldPosition);
            Cell endCell = _grid.WorldToCell(endWorldPosition);

            List<PathNode> path = FindPath(startCell.x, startCell.y, endCell.x, endCell.y);
            if (path == null) return null;
            else
            {
                List<Vector2> vectorPath = new List<Vector2>();
                foreach (PathNode pathNode in path)
                {
                    vectorPath.Add(_grid.Cells[pathNode.x, pathNode.y].worldPosition);
                }
                return vectorPath;
            }
        }

        public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            CreateNodesGrid();
            if (_nodesGrid == null) return null;

            PathNode startNode = _nodesGrid[startX, startY];
            PathNode endNode = _nodesGrid[endX, endY];

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            _openList = new List<PathNode> { startNode };
            _closedList = new List<PathNode>();

            while (_openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(_openList);
                if (currentNode == endNode) return CalculatePath(endNode);

                _openList.Remove(currentNode);
                _closedList.Add(currentNode);

                foreach (PathNode neighbourNode in GetNeighbourNodes(currentNode))
                {
                    if (_closedList.Contains(neighbourNode)) continue;
                    if (!_grid.Cells[neighbourNode.x, neighbourNode.y].Walkable)
                    {
                        _closedList.Add(neighbourNode);
                        continue;
                    }

                    int gCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if (gCost < neighbourNode.gCost)
                    {
                        neighbourNode.gCost = gCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();
                        neighbourNode.previousNode = currentNode;

                        if (!_openList.Contains(neighbourNode)) _openList.Add(neighbourNode);
                    }
                }
            }

            return null;
        }

        private void CreateNodesGrid()
        {
            if (_grid == null || _grid.Width == 0 || _grid.Height == 0) return;

            _nodesGrid = new PathNode[_grid.Width, _grid.Height];
            for (int x = 0; x < _grid.Width; x++)
            {
                for (int y = 0; y < _grid.Height; y++)
                {
                    _nodesGrid[x, y] = new PathNode(x, y);
                }
            }
        }

        private List<PathNode> GetNeighbourNodes(PathNode pathNode)
        {
            List<PathNode> neighbourNodes = new List<PathNode>();

            if (movementMode == MovementMode.StraightAndDiagonal || movementMode == MovementMode.OnlyStraight)
            {
                neighbourNodes.Add(GetNode(pathNode.x - 1, pathNode.y));
                neighbourNodes.Add(GetNode(pathNode.x + 1, pathNode.y));
                neighbourNodes.Add(GetNode(pathNode.x, pathNode.y - 1));
                neighbourNodes.Add(GetNode(pathNode.x, pathNode.y + 1));
            }

            if (movementMode == MovementMode.StraightAndDiagonal || movementMode == MovementMode.OnlyDiagonal)
            {
                neighbourNodes.Add(GetNode(pathNode.x - 1, pathNode.y - 1));
                neighbourNodes.Add(GetNode(pathNode.x - 1, pathNode.y + 1));
                neighbourNodes.Add(GetNode(pathNode.x + 1, pathNode.y - 1));
                neighbourNodes.Add(GetNode(pathNode.x + 1, pathNode.y + 1));
            }

            return neighbourNodes;
        }

        private PathNode GetNode(int x, int y)
        {
            if (_nodesGrid == null) return null;

            if (x < 0) x = _nodesGrid.GetLength(0) - 1;
            else if (x > _nodesGrid.GetLength(0) - 1) x = 0;

            if (y < 0) y = _nodesGrid.GetLength(1) - 1;
            else if (y > _nodesGrid.GetLength(1) - 1) y = 0;

            return _nodesGrid[x, y];
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();
            path.Add(endNode);

            PathNode currentNode = endNode;
            while (currentNode.previousNode != null)
            {
                path.Add(currentNode.previousNode);
                currentNode = currentNode.previousNode;
            }
            path.Reverse();

            return path;
        }

        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            int xDistance = Mathf.Abs(a.x - b.x);
            int yDistance = Mathf.Abs(a.y - b.y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return (Mathf.Min(xDistance, yDistance) * DiagonalMoveCost) + (remaining * StraightMoveCost);
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostNode = pathNodeList[0];
            for (int i = 1; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].fCost < lowestFCostNode.fCost) lowestFCostNode = pathNodeList[i];
            }

            return lowestFCostNode;
        }
    }
}
