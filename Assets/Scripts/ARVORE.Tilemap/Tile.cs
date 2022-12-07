using ARVORE.Core;
using UnityEngine;

namespace ARVORE.Tilemap
{
    public class Tile : MonoBehaviour
    {
        public bool walkable = true;


        protected virtual void Awake()
        {
            if (GridManager.Instance != null) transform.localScale = Vector3.one * GridManager.Instance.cellSize;
        }

        protected virtual void OnEnable()
        {
            if (GridManager.Instance != null && GridManager.Instance.Grid != null)
            {
                GridManager.Instance.Grid.AddTile(this);
            }
        }

        protected virtual void OnDisable()
        {
            if (GridManager.Instance != null && GridManager.Instance.Grid != null)
            {
                GridManager.Instance.Grid.RemoveTile(this);
            }
        }
    }
}
