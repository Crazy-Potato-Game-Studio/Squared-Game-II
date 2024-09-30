using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

namespace LevelBuilder
{
    public class GridCursor : MonoBehaviour
    {
        public static GridCursor Singleton;
        public Image gridCursorImage;
        public Color cursorValidColor;
        public Color cursorInvalidColor;
        public Color cursorScaleColor;
        public bool locked = false;
        public bool enable = false;
        public Vector2 cursorDimension;
        public Vector2 startAnchorePos;
        public Vector3 startGridPos;
        RectTransform cursorTransform;
        LevelEditorItem selectedItem;
        public Vector3 gridPos;
        public bool multiGridCursor;
        private void Awake()
        {
            Singleton = this;
        }
        private void Start()
        {
            cursorTransform = GetComponent<RectTransform>();
        }
        public void DisplayCursor(LevelEditorItem sandBoxItem)
        {
            selectedItem = sandBoxItem;
            if (selectedItem == null || selectedItem.cursorType == PlacementType.None)
            {
                gridCursorImage.enabled = false;
                multiGridCursor = false;
                locked = false;
                enable = false;
                gameObject.SetActive(false);
                return;
            }
            cursorDimension = Vector3.one;
            cursorTransform.pivot = Vector2.zero;
            cursorTransform.sizeDelta = cursorDimension * 100;
            gridCursorImage.enabled = true;
            multiGridCursor = false;
            locked = false;
            enable = true;
            gameObject.SetActive(true);
        }

        public void DisplayMultiGridCursor(Vector3Int _startPos,Vector2 dimention)
        {
            multiGridCursor = true;
            transform.position = _startPos;
            startAnchorePos = cursorTransform.anchoredPosition;
            cursorTransform.sizeDelta = dimention * 100;
        }
        private void Update()
        {
            if (!enable) { return; }
            if (locked) {ScaleGridCursor();return; }
            gridCursorImage.color = LevelCreateManager.Singleton.itemPlaceable ? cursorValidColor : cursorInvalidColor;
            if (multiGridCursor) { return; }
            transform.position = LevelCreateManager.Singleton.gridPos;
        }
        public void LockCursor(bool doEnable)
        {
            if (!doEnable)
            {
                cursorDimension = Vector3.one;
                cursorTransform.pivot = Vector2.zero;
                cursorTransform.sizeDelta = cursorDimension * 100;
                locked = false;
            }
            else
            {
                startAnchorePos = cursorTransform.anchoredPosition;
                startGridPos = LevelCreateManager.Singleton.gridPos;
                gridCursorImage.color = cursorScaleColor;
                locked = true;
            }
        }
        private void ScaleGridCursor()
        {
            cursorTransform.pivot = new Vector2(
                    gridPos.x >= startGridPos.x ? 0 : 1,
                    gridPos.y >= startGridPos.y ? 0 : 1);

            Vector2 adjustedStartPos = Vector2.zero;
            cursorTransform.anchoredPosition = startAnchorePos + new Vector2(
                gridPos.x >= startGridPos.x ? 0 : 1,
                gridPos.y >= startGridPos.y ? 0 : 1);
            cursorTransform.anchoredPosition += adjustedStartPos;
            cursorTransform.sizeDelta = CalculateDimension() * 100;
        }
        private Vector2 CalculateDimension()
        {
            Vector2 dimention = Vector2.zero;
            if (selectedItem.cursorType == PlacementType.VarticleGrid)
            {
                dimention = new(1, Mathf.Abs(gridPos.y - startGridPos.y) + 1);
            }
            else if (selectedItem.cursorType == PlacementType.HorizontalGrid)
            {
                dimention = new(Mathf.Abs(gridPos.x - startGridPos.x) + 1, 1);
            }
            else if (selectedItem.cursorType == PlacementType.BoxGrid)
            {
                dimention = new(Mathf.Abs(gridPos.x - startGridPos.x) + 1,
                                Mathf.Abs(gridPos.y - startGridPos.y) + 1);
            }
            return dimention;
        }
    }
}