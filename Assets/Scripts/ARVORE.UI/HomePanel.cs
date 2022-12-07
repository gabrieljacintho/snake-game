using ARVORE.Core;
using UnityEngine;

namespace ARVORE.UI
{
    public class HomePanel : MonoBehaviour
    {
        public KeyCode playKey = KeyCode.Return;
        public Audio playAudio;

        [Space]
        public KeyCode exitKey = KeyCode.Escape;


        private void Update()
        {
            if (Input.GetKeyDown(playKey) && UIManager.Instance != null)
            {
                UIManager.Instance.SwitchPanel(Panel.Players);
                playAudio.Play();
            }
            else if (Input.GetKeyDown(exitKey)) Application.Quit();
        }
    }
}
