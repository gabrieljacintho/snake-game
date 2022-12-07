using ARVORE.AI;
using ARVORE.Player;
using ARVORE.Tilemap;
using UnityEngine;

namespace ARVORE.Core
{
    public class GameMatchObject : MonoBehaviour
    {
        [HideInInspector] public GameMatch gameMatch;
        [HideInInspector] public SnakeController snakeController;
        [HideInInspector] public SnakeAI snakeAI;
        [HideInInspector] public Block block;


        private void OnEnable()
        {
            if (gameMatch != null)
            {
                if (snakeController != null) gameMatch.snakePlayerController = snakeController;
                else if (snakeAI != null) gameMatch.snakeEnemyAI = snakeAI;
                else if (block != null) gameMatch.block = block;
            }
        }
    }
}
