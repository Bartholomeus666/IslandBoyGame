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
    [SerializeField] private GameObject itemPrefab;

    [Header("Grid Position")]
    [SerializeField] private Vector2 gridOffset = Vector2.zero;

    private GridCell[,] grid;
    private float cellSize;

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

    public bool IsCellOccupied(int x, int y)
    {
        GridCell cell = GetCell(x, y);
        return cell != null && cell.IsOccupied;
    }

    public void SetCellOccupied(int x, int y, bool occupied)
    {
        GridCell cell = GetCell(x, y);
        if (cell != null)
        {
            cell.SetOccupied(occupied);
            if (occupied)
            {
                cell.name = ($"cell ({x}, {y}) (occupied)");
            }
            else
            {
                cell.name = ($"cell ({x}, {y})");
            }
        }
    }

    public bool PlaceItem(InventoryItem item, int startX, int startY)
    {
        if (!CanPlaceItem(item, startX, startY))
        {
            Debug.Log($"Cannot place item at position {startX}, {startY}");
            return false;
        }

        for (int x = 0;  x < item.width; x++)
        {
            for (int y = 0; y < item.height; y++)
            {
                if (item.IsCellOccupied(x, y))
                {
                    SetCellOccupied(startX + x, startY + y, true);
                }
            }
        }

        GameObject itemObject = Instantiate(itemPrefab, Vector2.zero, Quaternion.identity, gridParent);
        itemObject.name = item.itemName;

        GridItem gridItem = itemObject.GetComponent<GridItem>();
        if (gridItem == null)
        {
            gridItem = itemObject.AddComponent<GridItem>();
        }

        gridItem.Initialize(item, startX, startY, gridParent, cellSize);

        return true;
    }

    private bool CanPlaceItem(InventoryItem item, int startX, int startY)
    {
        if (startX < 0 || startX + item.width > gridWidth || startY < 0 || startY + item.height > gridHeight)
        {
            return false;
        }

        for (int x = 0; x < item.width; x++)
        {
            for (int y = 0; y < item.height; y++)
            {
                if (item.IsCellOccupied(x, y) && IsCellOccupied(startX + x, startY + y))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public Vector2 GetGridOffset()
    {
        return gridOffset;
    }
}