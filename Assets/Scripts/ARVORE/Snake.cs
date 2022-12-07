using ARVORE.Core;
using ARVORE.Tilemap;
using ARVORE.TimeTravel;
using System.Collections.Generic;
using UnityEngine;

namespace ARVORE
{
    public class Snake : MonoBehaviour
    {
        public List<Block> Blocks { get; private set; } = new List<Block>();
        public Block HeadBlock => Blocks != null && Blocks.Count > 0 ? Blocks[0] : null;

        public float moveSpeed = 5f;
        public bool canUpdateScore = true;
        public GameObject[] initialBlocksPrefabs = new GameObject[3];

        [Space]
        public Audio pickBlockAudio;
        public Audio hitAudio;
        public Audio deathAudio;

        private Dictionary<Block, float> _timeByTimeTravelBlock = new Dictionary<Block, float>();
        private Block _timeTravelBlock;

        private Vector2Int _direction = Vector2Int.up;

        private float _initialMoveSpeed;
        private float _moveSpeedDecreasePerBlock;
        private float _t;


        private void Awake()
        {
            _initialMoveSpeed = moveSpeed;

            if (GridManager.Instance != null)
                _moveSpeedDecreasePerBlock = moveSpeed / GridManager.Instance.Grid.Length;

            CreateSnake();
        }

        private void OnDisable()
        {
            if (Blocks != null) Blocks.FindAll(x => x != null).ForEach(x => x.gameObject.SetActive(false));
        }

        private void OnDestroy()
        {
            if (Blocks != null) Blocks.FindAll(x => x != null).ForEach(x => Destroy(x.gameObject));
        }

        private void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.State != GameState.InGame) return;
            if (GameRecorder.Instance != null && GameRecorder.Instance.IsRewinding) return;

            if (_timeTravelBlock != null)
            {
                RemoveBlock(_timeTravelBlock);
                Destroy(_timeTravelBlock.gameObject);

                if (UIManager.Instance != null) UIManager.Instance.SwitchPanel(Panel.Gameplay);
            }

            if (GridManager.Instance != null)
            {
                if (moveSpeed > 0f)
                {
                    if (_t <= GridManager.Instance.cellSize) _t += Time.deltaTime * moveSpeed;
                    else
                    {
                        _t = 0f;
                        Move();
                        CheckCollision();
                    }
                }
            }

            UpdateMoveSpeed();

            if (GameManager.Instance != null && GridManager.Instance != null)
            {
                if (Blocks.Count == GridManager.Instance.Grid.Length)
                    GameManager.Instance.GameOver();
            }
        }

        private void CreateSnake()
        {
            if (initialBlocksPrefabs != null && initialBlocksPrefabs.Length > 0)
            {
                for (int i = initialBlocksPrefabs.Length - 1; i >= 0; i--)
                {
                    GameObject instance = Instantiate(initialBlocksPrefabs[i], transform.position, transform.rotation);
                    if (instance.TryGetComponent(out Block block)) AddBlock(block);
                }
            }
        }

        private void Move()
        {
            if (Blocks == null || Blocks.Count == 0 || Blocks[0] == null) return;

            for (int i = Blocks.Count - 1; i > 0; i--)
            {
                Blocks[i].transform.SetPositionAndRotation(Blocks[i - 1].transform.position, Blocks[i - 1].transform.rotation);
            }
            Blocks[0].Move(_direction);

            _direction = Vector2Int.up;
        }

        private void UpdateMoveSpeed()
        {
            moveSpeed = _initialMoveSpeed - ((Blocks.Count - 1) * _moveSpeedDecreasePerBlock);

            if (Blocks != null && Blocks.Count > 0)
            {
                foreach (Block block in Blocks)
                {
                    if (block.type == BlockType.EnginePowerBlock)
                        moveSpeed += _moveSpeedDecreasePerBlock * block.moveSpeedIncreaseScale;
                }
            }
        }

        public void MoveLeft() => _direction = Vector2Int.left;

        public void MoveRight() => _direction = Vector2Int.right;

        private void AddBlock(Block block)
        {
            if (Blocks == null || Blocks.Contains(block)) return;

            if (Blocks.Count > 0)
            {
                block.transform.rotation = Blocks[0].transform.rotation;

                for (int i = 0; i < Blocks.Count - 1; i++)
                {
                    Blocks[i].transform.position = Blocks[i + 1].transform.position;
                }
                Blocks[Blocks.Count - 1].Move(Vector2Int.down);
            }

            Blocks.Insert(0, block);
            block.walkable = false;
            block.Snake = this;

            if (block.type == BlockType.TimeTravelBlock && block.TimeBody != null)
            {
                TimePoint timePoint = Blocks[1].TimeBody != null ? Blocks[1].TimeBody.TimePoints[0] : block.TimeBody.TimePoints[0];
                _timeByTimeTravelBlock.Add(block, timePoint.time);
            }
        }

        private void RemoveBlock(Block block)
        {
            block.walkable = true;
            block.Snake = null;
            block.gameObject.SetActive(false);

            if (Blocks == null || !Blocks.Contains(block)) return;

            if (Blocks.Count > 0)
            {
                for (int i = Blocks.Count - 1; i > Blocks.IndexOf(block); i--)
                {
                    Blocks[i].transform.SetPositionAndRotation(Blocks[i - 1].transform.position, Blocks[i - 1].transform.rotation);
                }
                Blocks.Remove(block);
            }

            if (block.type == BlockType.TimeTravelBlock && _timeByTimeTravelBlock.ContainsKey(block))
                _timeByTimeTravelBlock.Remove(block);
        }

        private void CheckCollision()
        {
            if (Blocks == null || Blocks.Count == 0 || Blocks[0] == null) return;

            Cell cell = GridManager.Instance.Grid.WorldToCell(Blocks[0].transform.position);
            if (cell == null || cell.tiles == null) return;

            cell.tiles.FindAll(tile => tile is Block block && block != Blocks[0]).ForEach(tile => OnCollision((Block)tile));
        }

        public bool HasBlockType(BlockType blockType) => Blocks != null && Blocks.Exists(x => x.type == blockType);

        private void OnCollision(Block block)
        {
            if (block.Snake == null)
            {
                if (canUpdateScore && GameManager.Instance != null) GameManager.Instance.IncreaseScore(block.score);
                AddBlock(block);
                pickBlockAudio.Play();
            }
            else if (HasBlockType(BlockType.BatteringRamBlock))
            {
                RemoveBlock(Blocks.Find(x => x.type == BlockType.BatteringRamBlock));
                hitAudio.Play();
            }
            else if (HasBlockType(BlockType.TimeTravelBlock) && GameRecorder.Instance != null)
            {
                Block timeTravelBlock = Blocks.Find(x => x.type == BlockType.TimeTravelBlock);
                GameRecorder.Instance.StartRewind(_timeByTimeTravelBlock[timeTravelBlock]);
                _timeTravelBlock = timeTravelBlock;

                if (UIManager.Instance != null) UIManager.Instance.SwitchPanel(Panel.TimeRewinding);
                hitAudio.Play();
            }
            else
            {
                gameObject.SetActive(false);
                deathAudio.Play();
            }
        }
    }
}
