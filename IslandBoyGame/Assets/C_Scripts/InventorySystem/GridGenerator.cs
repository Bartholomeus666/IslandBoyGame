using System;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;

    [Header("Grid Visuals")]
    [SerializeField] private GameObject gridCell;
    [SerializeField] private Transform gridParent;

    [Header("Grid Position")]
    [SerializeField] private Vector2 gridOffset = Vector2.zero;

    private GridCell[,] grid;
    private float cellSize;

    public int Width => gridWidth;
    public int Height => gridHeight;
    public float CellSize => cellSize;
    public Vector2 GridOffset => gridOffset;
    public Transform GridParent => gridParent;

    private void Awake()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        grid = new GridCell[gridWidth, gridHeight];

        // Get cell dimensions to use for spacing
        RectTransform cellRectTransform = gridCell.GetComponent<RectTransform>();
        cellSize = cellRectTransform.rect.width;

        // Change start position to include the offset inputed
        float startX =+ gridOffset.x;
        float startY =+ gridOffset.y;

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                GameObject cellObject = Instantiate(gridCell, Vector2.zero, Quaternion.identity, gridParent);
                cellObject.name = ($"cell ({i}, {j})");

                RectTransform rectTransform = cellObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(startX + i * cellSize, startY - j * cellSize);

                GridCell cellComponent = cellObject.GetComponent<GridCell>();

                cellComponent.Initialize(i, j);
                grid[i, j] = cellComponent;
            }
        }
    }

    public GridCell GetCell(int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            return grid[x, y];
        }
        return null;
    }
}