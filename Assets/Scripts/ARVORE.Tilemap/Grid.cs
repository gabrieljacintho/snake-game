using System.Collections.Generic;
using UnityEngine;

namespace ARVORE.Tilemap
{
    public class Grid
    {
        public float CellSize { get; private set; }

        public Cell[,] Cells { get; private set; }
        public List<Cell> EmptyCells => GetEmptyCells();

        public List<Tile> Tiles { get; private set; } = new List<Tile>();

        public int Width => Cells.GetLength(0);
        public int Height => Cells.GetLength(1);
        public int Length => Width * Height;


        public Grid(Vector2 startPoint, int width, int height, float cellSize)
        {
            Cells = new Cell[width, height];
            CellSize = cellSize;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Vector2 worldPosition = startPoint;
                    worldPosition += CellSize / 2 * Vector2.one;
                    worldPosition += new Vector2(CellSize * x, CellSize * y);

                    Cells[x, y] = new Cell(x, y, worldPosition);
                }
            }
        }

        public void AddTile(Tile tile)
        {
            if (Tiles == null || Tiles.Contains(tile)) return;

            Tiles.Add(tile);
            UpdateCellsTiles();
        }

        public void RemoveTile(Tile tile)
        {
            if (Tiles == null || !Tiles.Contains(tile)) return;

            Tiles.Remove(tile);
            UpdateCellsTiles();
        }

        public void UpdateCellsTiles()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (Cells[x, y] != null) Cells[x, y].tiles = Tiles.FindAll(tile => tile.transform.position == Cells[x, y].worldPosition);
                }
            }
        }

        public Cell WorldToCell(Vector2 worldPosition)
        {
            if (Cells == null || Cells.Length == 0) return default;

            Cell cell = default;
            float cellDistance = Mathf.Infinity;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float distance = Vector2.Distance(worldPosition, Cells[x, y].worldPosition);
                    if (distance < cellDistance)
                    {
                        cell = Cells[x, y];
                        cellDistance = distance;
                    }
                }
            }

            return cell;
        }

        private List<Cell> GetEmptyCells()
        {
            if (Cells == null || Cells.Length == 0) return new List<Cell>();

            List<Cell> emptyCells = new List<Cell>();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (Cells[x, y].tiles == null || Cells[x, y].tiles.Count == 0)
                        emptyCells.Add(Cells[x, y]);
                }
            }

            return emptyCells;
        }
    }
}
