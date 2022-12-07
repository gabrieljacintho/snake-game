using ARVORE.Core;
using ARVORE.Tilemap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARVORE.AI
{
    [RequireComponent(typeof(Snake))]
    public class SnakeAI : MonoBehaviour
    {
        public Snake Snake { get; private set; }

        [HideInInspector] public Vector2 destination;

        private Pathfinding _pathFinding;
        private List<Vector2> _path;
        private int _currentPathIndex;


        private void Awake()
        {
            Snake = GetComponent<Snake>();

            if (GridManager.Instance != null && GridManager.Instance.Grid != null)
                _pathFinding = new Pathfinding(GridManager.Instance.Grid, MovementMode.OnlyStraight);
        }

        private void OnEnable()
        {
            StartCoroutine(UpdatePath(2));
        }

        private void LateUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (Snake == null || Snake.HeadBlock == null || _path == null) return;

            Transform snakeTransform = Snake.HeadBlock.transform;

            Vector3 nextPosition = _path[_currentPathIndex];
            if (snakeTransform.position != nextPosition)
            {
                Vector2 direction = (nextPosition - snakeTransform.position).normalized;
                direction = snakeTransform.InverseTransformDirection(direction).normalized;

                if (direction.x == -1) Snake.MoveLeft();
                else if (direction.x == 1) Snake.MoveRight();
            }
            else if (_currentPathIndex < _path.Count - 1) _currentPathIndex++;
        }

        public void SetDestination(Vector2 target)
        {
            destination = target;
            UpdatePath();
        }

        public void UpdatePath()
        {
            if (Snake == null || Snake.HeadBlock == null) return;

            _path = _pathFinding.FindPath(Snake.HeadBlock.transform.position, destination);
            if (_path != null && _path.Count > 1) _path.RemoveAt(0);

            _currentPathIndex = 0;
        }

        private IEnumerator UpdatePath(float delay)
        {
            while (true)
            {
                UpdatePath();
                if (_path == null) yield return null;
                else yield return new WaitForSeconds(delay);
            }
        }
    }
}
