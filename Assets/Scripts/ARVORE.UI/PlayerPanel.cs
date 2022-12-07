using ARVORE.Core;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ARVORE.UI
{
    public class PlayerPanel : MonoBehaviour
    {
        public TextMeshProUGUI controlsText;
        public Transform snakeImageTransform;

        [Space]
        public Audio changeSnakeAudio;

        private GameMatch _gameMatch;
        private List<GameObject> _snakeImageObjects;

        private KeyCode _leftKey, _rightKey;


        private void Awake()
        {
            SpawnSnakeImages();
        }

        private void OnEnable()
        {
            if (GameManager.Instance != null) _gameMatch = GameManager.Instance.CreateGameMatch();
            SetSnakeIndex(0);
        }

        private void Update()
        {
            if (_gameMatch == null) return;

            if (Input.GetKeyDown(_leftKey))
            {
                SetSnakeIndex(_gameMatch.snakePlayerIndex - 1);
                changeSnakeAudio.Play();
            }
            else if (Input.GetKeyDown(_rightKey))
            {
                SetSnakeIndex(_gameMatch.snakePlayerIndex + 1);
                changeSnakeAudio.Play();
            }
        }

        private void SpawnSnakeImages()
        {
            if (snakeImageTransform == null || SpawnManager.Instance == null) return;

            List<SnakeType> snakeTypes = SpawnManager.Instance.playerSnakeTypes;
            if (snakeTypes == null) return;

            _snakeImageObjects = new List<GameObject>();
            snakeTypes.FindAll(x => x != null && x.imagePrefab != null)
                .ForEach(snakeType => _snakeImageObjects.Add(Instantiate(snakeType.imagePrefab, snakeImageTransform)));
        }

        public void SetControls(KeyCode leftKey, KeyCode rightKey)
        {
            _leftKey = leftKey;
            _rightKey = rightKey;

            string leftKeyText = leftKey.ToString();
            leftKeyText = leftKeyText[^1].ToString();

            string rightKeyText = rightKey.ToString();
            rightKeyText = rightKeyText[^1].ToString();

            if (controlsText != null) controlsText.text = leftKeyText + rightKeyText;
            if (_gameMatch != null)
            {
                _gameMatch.playerLeftMoveKey = _leftKey;
                _gameMatch.playerRightMoveKey = _rightKey;
            }
        }

        public void SetSnakeIndex(int index)
        {
            if (_snakeImageObjects == null || _snakeImageObjects.Count == 0) return;

            if (index < 0) index = _snakeImageObjects.Count - 1;
            else if (index >= _snakeImageObjects.Count) index = 0;

            for (int i = 0; i < _snakeImageObjects.Count; i++)
            {
                _snakeImageObjects[i].SetActive(i == index);
            }

            if (_gameMatch != null) _gameMatch.snakePlayerIndex = index;
        }
    }
}
