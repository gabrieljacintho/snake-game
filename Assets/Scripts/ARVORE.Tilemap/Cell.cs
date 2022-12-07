using System.Collections.Generic;
using UnityEngine;

namespace ARVORE.Tilemap
{
    public class Cell
    {
        public int x, y;
        public Vector3 worldPosition;
        public List<Tile> tiles = new List<Tile>();

        public Vector2Int GridPosition
        {
            get => new Vector2Int(x, y);
            set
            {
                x = value.x;
                y = value.y;
            }
        }

        public bool Walkable => tiles == null || tiles.Count == 0 || !tiles.Exists(x => x != null && !x.walkable);


        public Cell(int x, int y, Vector3 worldPosition)
        {
            this.x = x;
            this.y = y;
            this.worldPosition = worldPosition;
            tiles = new List<Tile>();
        }

        public override string ToString()
        {
            return x + "," + y + " (" + worldPosition.ToString() + ")";
        }
    }
}
