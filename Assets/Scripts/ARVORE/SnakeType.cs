using UnityEngine;

namespace ARVORE
{
    [CreateAssetMenu(fileName = "New Snake Type", menuName = "Snake Type")]
    public class SnakeType : ScriptableObject
    {
        public GameObject imagePrefab;
        public GameObject playerPrefab;
    }
}
