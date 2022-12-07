using ARVORE.Core;
using UnityEngine;

namespace ARVORE.UI
{
    public class PausePanel : MonoBehaviour
    {
        public KeyCode continueKey = KeyCode.Return;
        public Audio continueAudio;

        [Space]
        public KeyCode homeKey = KeyCode.Escape;
        public Audio homeAudio;


        private void Update()
        {
            if (GameManager.Instance == null) return;

            if (Input.GetKeyDown(continueKey))
            {
                GameManager.Instance.ContinueGame();
                continueAudio.Play();
            }
            else if (Input.GetKeyDown(homeKey))
            {
                GameManager.Instance.BackToHome();
                homeAudio.Play();
            }
        }
    }
}
