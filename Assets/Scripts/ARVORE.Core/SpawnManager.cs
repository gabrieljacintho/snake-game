using ARVORE.Tilemap;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ARVORE.Core
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        public List<SnakeType> playerSnakeTypes = new List<SnakeType>();
        public List<GameObject> enemySnakePrefabs = new List<GameObject>();
        public List<GameObject> blockPrefabs;


        protected override void Awake()
        {
            base.Awake();

            Object[] objects = Resources.LoadAll("");
            if (objects != null && objects.Length > 0)
            {
                foreach (Object @object in objects)
                {
                    if (@object is SnakeType snakeType && !playerSnakeTypes.Contains(snakeType)) playerSnakeTypes.Add(snakeType);
                }
            }
        }

        public GameObject SpawnPlayer(int index = 0)
        {
            if (playerSnakeTypes == null || playerSnakeTypes.Count == 0) return null;

            index = Mathf.Clamp(index, 0, playerSnakeTypes.Count);
            if (playerSnakeTypes[index] == null || playerSnakeTypes[index].playerPrefab == null) return null;

            return Instantiate(playerSnakeTypes[index].playerPrefab, GetRandomPosition(), GetRandomRotation());
        }

        public GameObject SpawnRandomEnemy()
        {
            if (enemySnakePrefabs == null || enemySnakePrefabs.Count == 0) return null;

            int index = Random.Range(0, enemySnakePrefabs.Count);
            if (enemySnakePrefabs[index] == null) return null;

            return Instantiate(enemySnakePrefabs[index], GetRandomPosition(), GetRandomRotation());
        }

        public GameObject SpawnRandomBlock()
        {
            if (blockPrefabs == null || blockPrefabs.Count == 0) return null;

            int index = Random.Range(0, blockPrefabs.Count);
            if (blockPrefabs[index] == null) return null;

            return Instantiate(blockPrefabs[index], GetRandomPosition(), Quaternion.identity);
        }

        private Vector3 GetRandomPosition()
        {
            Vector3 randomPosition = Vector3.zero;

            if (GridManager.Instance == null || GridManager.Instance.Grid == null) return randomPosition;

            List<Cell> cells = GridManager.Instance.Grid.EmptyCells;
            if (cells == null || cells.Count == 0) return randomPosition;

            Cell randomCell = cells[Random.Range(0, cells.Count)];
            if (randomCell != null) randomPosition = randomCell.worldPosition;

            return randomPosition;
        }

        private Quaternion GetRandomRotation() => Quaternion.AngleAxis(Random.Range(-1, 2) * 90f, Vector3.forward);
    }
}
