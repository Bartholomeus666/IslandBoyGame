using System;
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

    private GameObject[,] grid;

    private void Awake()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        grid = new GameObject[gridWidth, gridHeight];

        // Get cell dimensions to use for spacing
        RectTransform cellRectTransform = gridCell.GetComponent<RectTransform>();
        float cellWidth = cellRectTransform.rect.width;
        float cellHeight = cellRectTransform.rect.height;

        // Change start position to include the offset inputed
        float startX =+ gridOffset.x;
        float startY =+ gridOffset.y;

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                grid[i, j] = Instantiate(gridCell, Vector3.zero, Quaternion.identity, gridParent);
                grid[i, j].name = $"Cell ({i}, {j})";

                RectTransform rectTransform = grid[i, j].GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(startX + i * cellWidth, startY - j * cellHeight);
            }
        }
    }
}