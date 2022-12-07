using ARVORE.Core;
using ARVORE.Tilemap;
using UnityEngine;

namespace ARVORE.TimeTravel
{
    [RequireComponent(typeof(Block))]
    public class TimeBlock : TimeBody
    {
        public Block Block { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            Block = GetComponent<Block>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Block != null && Block.Snake != null) Block.Snake.Blocks.Remove(Block);
        }

        public override void Rewind(TimePoint timePoint)
        {
            base.Rewind(timePoint);

            if (Block != null)
            {
                BlockTimePoint blockTimePoint = (BlockTimePoint)timePoint;
                Block.walkable = blockTimePoint.walkable;
                if (Block.Snake != null && blockTimePoint.snake == null)
                {
                    if (Block.Snake.canUpdateScore && GameManager.Instance != null)
                        GameManager.Instance.DecreaseScore(Block.score);

                    Block.Snake.Blocks.Remove(Block);
                    Block.Snake = null;
                }
                else if (Block.Snake == null && blockTimePoint.snake != null)
                {
                    Block.Snake = blockTimePoint.snake;
                    Block.Snake.Blocks.Insert(blockTimePoint.snakeBlockIndex, Block);

                    if (Block.Snake.canUpdateScore && GameManager.Instance != null)
                        GameManager.Instance.IncreaseScore(Block.score);
                }
            }
        }

        public override void Record()
        {
            bool walkable = Block == null || Block.walkable;
            Snake snake = Block != null ? Block.Snake : null;
            int snakeBlockIndex = snake != null ? snake.Blocks.IndexOf(Block) : 0;
            TimePoints.Insert(0, new BlockTimePoint(transform.position, transform.rotation, gameObject.activeSelf, walkable, snake, snakeBlockIndex));
        }
    }
}
