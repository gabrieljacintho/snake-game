using UnityEngine;
using Grid = ARVORE.Tilemap.Grid;

namespace ARVORE.Core
{
    public class GridManager : Singleton<GridManager>
    {
        public Grid Grid { get; private set; }

        public RectTransform rectTransform;
        public float cellSize = 0.5f;


        protected override void Awake()
        {
            base.Awake();

            ResizeRectTransform();
            CreateGrid();
        }

        private void ResizeRectTransform()
        {
            if (rectTransform == null) return;

            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            float worldWidth = Vector2.Distance(corners[0], corners[3]);
            float worldHeight = Vector2.Distance(corners[0], corners[1]);

            float newWorldWidth = worldWidth - (worldWidth % cellSize);
            float newWorldHeight = worldHeight - (worldHeight % cellSize);

            float width = rectTransform.rect.width * newWorldWidth / worldWidth;
            float height = rectTransform.rect.height * newWorldHeight / worldHeight;

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        private void CreateGrid()
        {
            if (rectTransform == null) return;

            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            int width = Mathf.RoundToInt(Vector2.Distance(corners[0], corners[3]) / cellSize);
            int height = Mathf.RoundToInt(Vector2.Distance(corners[0], corners[1]) / cellSize);

            Grid = new Grid(corners[0], width, height, cellSize);
        }
    }
}
