using UnityEngine;

namespace ARVORE.Player
{
    [RequireComponent(typeof(Snake))]
    public class SnakeController : MonoBehaviour
    {
        public Snake Snake { get; private set; }

        public KeyCode leftMoveKey = KeyCode.A;
        public KeyCode rightMoveKey = KeyCode.S;


        private void Awake()
        {
            Snake = GetComponent<Snake>();
        }

        private void Update()
        {
            if (Snake == null) return;

            if (Input.GetKeyDown(leftMoveKey)) Snake.MoveLeft();
            else if (Input.GetKeyDown(rightMoveKey)) Snake.MoveRight();
        }
    }
}
