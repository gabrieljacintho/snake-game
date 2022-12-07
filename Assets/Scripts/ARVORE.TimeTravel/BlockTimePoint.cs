using UnityEngine;

namespace ARVORE.TimeTravel
{
    public class BlockTimePoint : TimePoint
    {
        public readonly bool walkable;
        public readonly Snake snake;
        public readonly int snakeBlockIndex;


        public BlockTimePoint(Vector3 position, Quaternion rotation, bool activeSelf, bool walkable, Snake snake, int snakeBlockIndex) : base(position, rotation, activeSelf)
        {
            this.walkable = walkable;
            this.snake = snake;
            this.snakeBlockIndex = snakeBlockIndex;
        }
    }
}
