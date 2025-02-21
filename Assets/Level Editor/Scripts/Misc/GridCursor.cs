using UnityEngine;
using UnityEngine.UI;

public class GridCursor : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] public Image gridCursorImage;
    [SerializeField] public Color cursorValidColor;
    [SerializeField] public Color cursorInvalidColor;
    [SerializeField] public Color cursorScaleColor;

    [Header("Settings")]
    [SerializeField] private Vector2Int gridCellSize = Vector2Int.one;
    [SerializeField] private Vector2Int dynamicScaleLimit = new(20, 20);

    private Vector2 startAnchoredPosition;
    private Vector2 startGridPosition;
    private RectTransform cursorRect;
    private Vector2Int gridPos;
    private bool isCursorIsScaling = false;
    public static GridCursor Singleton;

    private void Awake() => Singleton = this;
    private void Start() => cursorRect = GetComponent<RectTransform>();

    public void SetCursor(Vector2Int gridPos, Vector2Int size, CursorType cursorType = CursorType.Center)
    {
        this.gridPos = gridPos;
        if (cursorType == CursorType.None)
        {
            gridCursorImage.enabled = false;
        }
        else if (cursorType == CursorType.Dynamic)
        {
            ShowDynamicCursor();
        }
        else
        {
            ShowStaticCursor(size, cursorType);
        }
    }

    public void SetScaling(bool value)
    {
        if (value)
        {
            StartCursorScaling();
        }
        else
        {
            EndCursorScaling();
        }
    }

    private void ShowDynamicCursor()
    {
        if (isCursorIsScaling)
        {
            ScaleGridCursor();
        }
        else
        {
            ShowStaticCursor(Vector2Int.one);
        }
    }

    private void ShowStaticCursor(Vector2Int cursorSize, CursorType cursorType = CursorType.Center)
    {
        cursorRect.position = GetBottomLeftPos(gridPos, cursorSize, cursorType);
        cursorRect.sizeDelta = cursorSize * 100;
        cursorRect.pivot = Vector2.zero;
    }

    private void StartCursorScaling()
    {
        startAnchoredPosition = cursorRect.anchoredPosition;
        startGridPosition = gridPos;
        isCursorIsScaling = true;
    }

    private void EndCursorScaling()
    {
        isCursorIsScaling = false;
        cursorRect.sizeDelta = gridCellSize * 100;
        cursorRect.pivot = Vector2.zero;
    }

    private void ScaleGridCursor()
    {
        Vector2 anchorPivot = new(gridPos.x >= startGridPosition.x ? 0 : 1,
                                  gridPos.y >= startGridPosition.y ? 0 : 1);

        Vector2 cursorDimension = new(
            Mathf.Clamp(Mathf.Abs(gridPos.x - startGridPosition.x) + 1, 1, dynamicScaleLimit.x),
            Mathf.Clamp(Mathf.Abs(gridPos.y - startGridPosition.y) + 1, 1, dynamicScaleLimit.y));

        cursorRect.pivot = anchorPivot;
        cursorRect.anchoredPosition = startAnchoredPosition + anchorPivot;
        cursorRect.sizeDelta = cursorDimension * 100;
    }

    public void SetVisibility(bool value)
    {
        float alpha = value ? 100f : 0f;
        gridCursorImage.color = new Color(
            gridCursorImage.color.r,
            gridCursorImage.color.g,
            gridCursorImage.color.b,
            alpha);
    }
    public void ResetCursor()
    {
        isCursorIsScaling = false;
        cursorRect.sizeDelta = gridCellSize * 100;
        cursorRect.pivot = Vector2.zero;
    }
    private Vector3 GetBottomLeftPos(Vector2Int gridPos, Vector2Int dimension, CursorType cursorType)
    {
        Vector3 tileDimensionBottomLeftPos = new();
        if (cursorType == CursorType.BottomCenter)
        {
            tileDimensionBottomLeftPos.x = gridPos.x - (int)dimension.x / 2;
            tileDimensionBottomLeftPos.y = gridPos.y;
        }
        else if (cursorType == CursorType.TopCenter)
        {
            tileDimensionBottomLeftPos.x = gridPos.x - (int)dimension.x / 2;
            tileDimensionBottomLeftPos.y = (gridPos.y - dimension.y) + 1;
        }
        else if (cursorType == CursorType.LeftCenter)
        {
            tileDimensionBottomLeftPos.x = gridPos.x;
            tileDimensionBottomLeftPos.y = gridPos.y - (int)dimension.y / 2;
        }
        else if (cursorType == CursorType.RightCenter)
        {
            tileDimensionBottomLeftPos.x = (gridPos.x - dimension.x) + 1;
            tileDimensionBottomLeftPos.y = gridPos.y - (int)dimension.y / 2;
        }
        else
        {
            tileDimensionBottomLeftPos.x = gridPos.x - (int)dimension.x / 2;
            tileDimensionBottomLeftPos.y = gridPos.y - (int)dimension.y / 2;
        }
        return tileDimensionBottomLeftPos;
    }
}
public enum CursorType
{
    None,
    Center,
    TopCenter,
    BottomCenter,
    LeftCenter,
    RightCenter,
    Dynamic
}