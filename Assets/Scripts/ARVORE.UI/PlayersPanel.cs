using ARVORE.Core;
using System.Collections.Generic;
using UnityEngine;

namespace ARVORE.UI
{
    public class PlayersPanel : MonoBehaviour
    {
        public GameObject addPlayerTextObject;
        public GameObject startTextObject;
        public Transform panelsTransform;
        public GameObject playerPanelPrefab;

        [Space]
        public KeyCode startKey = KeyCode.Return;
        public Audio startAudio;

        [Space]
        public List<KeyCode> newPlayerKeys;
        public Audio newPlayerAudio;

        private List<KeyCode> _currentAvaliableKeys;
        private List<PlayerPanel> _playerPanels = new List<PlayerPanel>();

        private KeyCode _leftKey, _rightKey;
        private float _t;


        private void OnEnable()
        {
            _currentAvaliableKeys = new List<KeyCode>(newPlayerKeys);
            _playerPanels = new List<PlayerPanel>();

            _leftKey = KeyCode.None;
            _rightKey = KeyCode.None;
            _t = 0f;
        }

        private void OnDisable()
        {
            if (panelsTransform != null)
            {
                foreach (Transform child in panelsTransform)
                {
                    Destroy(child.gameObject);
                }
            }

            if (startTextObject != null) startTextObject.SetActive(false);
        }

        private void Update()
        {
            if (_playerPanels.Count > 0 && Input.GetKeyDown(startKey) && GameManager.Instance != null)
            {
                GameManager.Instance.StartGame();
                startAudio.Play();
                return;
            }

            if (_currentAvaliableKeys != null && _currentAvaliableKeys.Count > 2)
            {
                if (addPlayerTextObject != null) addPlayerTextObject.SetActive(true);

                KeyCode leftKey = KeyCode.None;
                KeyCode rightKey = KeyCode.None;
                foreach (KeyCode keyCode in _currentAvaliableKeys)
                {
                    if (Input.GetKey(keyCode))
                    {
                        if (leftKey == KeyCode.None) leftKey = keyCode;
                        else if (rightKey == KeyCode.None) rightKey = keyCode;
                        else break;
                    }
                }

                if (leftKey == _leftKey && rightKey == _rightKey) _t += Time.deltaTime;
                else _t = 0f;

                _leftKey = leftKey;
                _rightKey = rightKey;

                if (_t >= 1f && _leftKey != KeyCode.None && _rightKey != KeyCode.None)
                    AddPlayerPanel(_leftKey, _rightKey);
            }
            else if (addPlayerTextObject != null) addPlayerTextObject.SetActive(false);

            if (startTextObject != null && panelsTransform != null)
                startTextObject.SetActive(panelsTransform.childCount > 0);
        }

        private void AddPlayerPanel(KeyCode leftKey, KeyCode rightKey)
        {
            if (playerPanelPrefab == null) return;

            GameObject instance = Instantiate(playerPanelPrefab, panelsTransform);
            if (instance.TryGetComponent(out PlayerPanel playerPanel))
            {
                playerPanel.SetControls(leftKey, rightKey);
                _playerPanels.Add(playerPanel);
                _currentAvaliableKeys.Remove(leftKey);
                _currentAvaliableKeys.Remove(rightKey);
            }

            newPlayerAudio.Play();
        }
    }
}
