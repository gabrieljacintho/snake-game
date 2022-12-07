using ARVORE.Core;
using UnityEngine;

namespace ARVORE.UI
{
    public class GameplayPanel : MonoBehaviour
    {
        public KeyCode pauseKey = KeyCode.Escape;
        public Audio pauseAudio;


        private void Update()
        {
            if (GameManager.Instance == null || GameManager.Instance.State != GameState.InGame) return;

            if (Input.GetKeyDown(pauseKey))
            {
                GameManager.Instance.PauseGame();
                pauseAudio.Play();
            }
        }
    }
}
