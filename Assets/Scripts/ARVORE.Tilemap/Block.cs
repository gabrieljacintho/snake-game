using ARVORE.Core;
using ARVORE.TimeTravel;
using UnityEngine;

namespace ARVORE.Tilemap
{
    public enum BlockType
    {
        StandardBlock,
        EnginePowerBlock,
        BatteringRamBlock,
        TimeTravelBlock
    }

    public class Block : Tile
    {
        public Snake Snake { get; set; }
        public TimeBody TimeBody { get; private set; }

        public BlockType type;
        public int score = 7;
        [Tooltip("Just for Engine Power Block.")]
        public float moveSpeedIncreaseScale = 1.5f;


        protected override void Awake()
        {
            base.Awake();
            TimeBody = GetComponent<TimeBody>();
        }

        public void Move(Vector2Int direction)
        {
            if (GridManager.Instance == null) return;

            Grid grid = GridManager.Instance.Grid;
            if (grid == null) return;

            transform.rotation *= Quaternion.AngleAxis(direction.x * -90f, Vector3.forward);

            direction = direction.y >= 0 ? Vector2Int.up : Vector2Int.down;
            Vector2 worldDirection = transform.TransformDirection((Vector2)direction);

            Cell cell = grid.WorldToCell(transform.position);

            int x = cell.x + (int)worldDirection.x;
            int y = cell.y + (int)worldDirection.y;

            int gridWidth = GridManager.Instance.Grid.Width;
            int gridHeight = GridManager.Instance.Grid.Height;

            if (x < 0) x = gridWidth - 1;
            else if (x > gridWidth - 1) x = 0;

            if (y < 0) y = gridHeight - 1;
            else if (y > gridHeight - 1) y = 0;

            cell = GridManager.Instance.Grid.Cells[x, y];
            if (cell == null) return;

            transform.position = cell.worldPosition;

            grid.UpdateCellsTiles();
        }
    }
}
