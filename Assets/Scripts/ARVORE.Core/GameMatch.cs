using ARVORE.AI;
using ARVORE.Player;
using ARVORE.Tilemap;
using ARVORE.TimeTravel;
using UnityEngine;

namespace ARVORE.Core
{
    public enum GameMatchState
    {
        NotStarted,
        InGame,
        Finished
    }

    public class GameMatch : MonoBehaviour
    {
        public GameMatchState State { get; private set; }

        public int snakePlayerIndex;
        public KeyCode playerLeftMoveKey, playerRightMoveKey;

        [HideInInspector] public SnakeController snakePlayerController;
        [HideInInspector] public SnakeAI snakeEnemyAI;
        [HideInInspector] public Block block;


        public void OnDestroy()
        {
            if (snakePlayerController != null) Destroy(snakePlayerController.gameObject);
            if (snakeEnemyAI != null) Destroy(snakeEnemyAI.gameObject);
            if (block != null && block.Snake == null) Destroy(block.gameObject);
        }

        public void LateUpdate()
        {
            if (GameRecorder.Instance != null && GameRecorder.Instance.IsRewinding) return;

            if (snakePlayerController == null || !snakePlayerController.gameObject.activeSelf) State = GameMatchState.Finished;
            else if (State != GameMatchState.NotStarted) State = GameMatchState.InGame;

            bool isMatchFinish = block == null || block.Snake != null;
            isMatchFinish |= snakeEnemyAI != null && !snakeEnemyAI.gameObject.activeSelf;

            if (isMatchFinish) StartNextRound();
            else if (block != null && block.Snake != null) block = null;
        }

        public void StartMatch()
        {
            if (snakePlayerController != null) snakePlayerController.gameObject.SetActive(false);

            SpawnPlayer();

            State = GameMatchState.InGame;

            StartNextRound();
        }

        private void StartNextRound()
        {
            if (block != null && block.Snake == null) block.gameObject.SetActive(false);
            if (snakeEnemyAI != null) snakeEnemyAI.gameObject.SetActive(false);

            if (State != GameMatchState.InGame || SpawnManager.Instance == null) return;

            SpawnBlock();
            SpawnEnemy();
        }

        private void SpawnPlayer()
        {
            if (SpawnManager.Instance == null) return;

            GameObject instance = SpawnManager.Instance.SpawnPlayer(snakePlayerIndex);
            if (instance != null)
            {
                if (instance.TryGetComponent(out snakePlayerController))
                {
                    snakePlayerController.leftMoveKey = playerLeftMoveKey;
                    snakePlayerController.rightMoveKey = playerRightMoveKey;

                    if (instance.TryGetComponent(out GameMatchObject gameMatchObject))
                    {
                        gameMatchObject.gameMatch = this;
                        gameMatchObject.snakeController = snakePlayerController;
                    }
                }
            }
        }

        private void SpawnBlock()
        {
            if (SpawnManager.Instance == null) return;

            GameObject instance = SpawnManager.Instance.SpawnRandomBlock();
            if (instance != null)
            {
                if (instance.TryGetComponent(out block))
                {
                    if (instance.TryGetComponent(out GameMatchObject gameMatchObject))
                    {
                        gameMatchObject.gameMatch = this;
                        gameMatchObject.block = block;
                    }
                }
            }
        }

        private void SpawnEnemy()
        {
            if (SpawnManager.Instance == null || (block == null && snakeEnemyAI == null)) return;

            GameObject instance = SpawnManager.Instance.SpawnRandomEnemy();
            if (instance != null)
            {
                if (instance.TryGetComponent(out snakeEnemyAI))
                {
                    if (block != null) snakeEnemyAI.SetDestination(block.transform.position);

                    if (instance.TryGetComponent(out GameMatchObject gameMatchObject))
                    {
                        gameMatchObject.gameMatch = this;
                        gameMatchObject.snakeAI = snakeEnemyAI;
                    }
                }
            }
        }
    }
}
