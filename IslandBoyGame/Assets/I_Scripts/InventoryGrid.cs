using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGrid : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private int gridWidth = 8;
    [SerializeField] private int gridHeight = 8;

    private GameObject[,] slots;
    private bool[,] occupiedCells;

    private void Start()
    {
        GenerateGrid();
        occupiedCells = new bool[gridWidth, gridHeight];
    }

    private void GenerateGrid()
    {
        slots = new GameObject[gridWidth, gridHeight];

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                GameObject slot = Instantiate(slotPrefab, transform);
                slot.name = $"Slot_{x}_{y}";
                slots[y, x] = slot;
            }
        }
    }
    public bool TryPlaceItem(InventoryItem item, int topLeftX, int topLeftY)
    {
        if (!CanPlaceItem(item, topLeftX, topLeftY))
        {
            Debug.Log("unable to place item there..");
            return false;
        }

        PlaceItem(item, topLeftX, topLeftY);
        return true;
    }

    private bool CanPlaceItem(InventoryItem item, int topLeftX, int topLeftY)
    {
        for (int y = 0; y < item.shape.GetLength(1); y++)
        {
            for (int x = 0; x < item.shape.GetLength(0); x++)
            {
                if (!item.shape[x, y])
                    continue;

                int gridX = topLeftX + x;
                int gridY = topLeftY + y;

                if (gridX < 0 || gridX >= gridWidth || gridY < 0 || gridY >= gridHeight)
                    return false;

                if (occupiedCells[gridX, gridY])
                    return false;
            }
        }

        return true;
    }

    private void PlaceItem(InventoryItem item, int topLeftX, int topLeftY)
    {
        GameObject itemObject = new GameObject(item.itemName);
        itemObject.transform.SetParent(transform);

        // Create a single image for the entire item
        GameObject itemVisual = new GameObject("ItemVisual");
        itemVisual.transform.SetParent(itemObject.transform);

        // Add Image component
        Image image = itemVisual.AddComponent<Image>();
        image.sprite = item.icon;
        image.color = item.itemColor;

        RectTransform rectTransform = itemVisual.GetComponent<RectTransform>();

        // Calculate bounds
        int minX = int.MaxValue, minY = int.MaxValue;
        int maxX = int.MinValue, maxY = int.MinValue;

        // First pass: determine item boundaries and mark cells as occupied
        for (int y = 0; y < item.shape.GetLength(1); y++)
        {
            for (int x = 0; x < item.shape.GetLength(0); x++)
            {
                if (!item.shape[x, y])
                    continue;

                int gridX = topLeftX + x;
                int gridY = topLeftY + y;

                // Mark as occupied
                occupiedCells[gridX, gridY] = true;

                // Track min/max for calculating bounds
                minX = Mathf.Min(minX, gridX);
                minY = Mathf.Min(minY, gridY);
                maxX = Mathf.Max(maxX, gridX);
                maxY = Mathf.Max(maxY, gridY);
            }
        }

        // Calculate width and height in grid cells
        int cellWidth = maxX - minX + 1;
        int cellHeight = maxY - minY + 1;

        // Get reference to the first cell's position (top-left)
        RectTransform firstCellRect = slots[minX, minY].GetComponent<RectTransform>();

        // Get size of a single cell
        Vector2 cellSize = firstCellRect.sizeDelta;

        // Set position to the center of all occupied cells
        rectTransform.position = firstCellRect.position;

        // Adjust position to account for the pivot (assuming pivot is at 0.5, 0.5)
        float offsetX = (cellWidth - 1) * cellSize.x * 0.5f;
        float offsetY = (cellHeight - 1) * cellSize.y * 0.5f;

        rectTransform.position = new Vector3(
            firstCellRect.position.x + offsetX,
            firstCellRect.position.y - offsetY,
            firstCellRect.position.z
        );

        // Set the size to cover all occupied cells
        rectTransform.sizeDelta = new Vector2(
            cellWidth * cellSize.x,
            cellHeight * cellSize.y
        );

        // Optionally: Create invisible placeholders for each occupied cell for debugging
        // or to handle cell-specific events
        for (int y = 0; y < item.shape.GetLength(1); y++)
        {
            for (int x = 0; x < item.shape.GetLength(0); x++)
            {
                if (!item.shape[x, y])
                    continue;

                int gridX = topLeftX + x;
                int gridY = topLeftY + y;

                GameObject cellPlaceholder = new GameObject($"Cell_{x}_{y}");
                cellPlaceholder.transform.SetParent(itemObject.transform);

                RectTransform cellRect = cellPlaceholder.AddComponent<RectTransform>();
                cellRect.position = slots[gridX, gridY].GetComponent<RectTransform>().position;
                cellRect.sizeDelta = slots[gridX, gridY].GetComponent<RectTransform>().sizeDelta;

                // Optional: Add a transparent image for raycasting purposes
                /*
                Image placeholderImage = cellPlaceholder.AddComponent<Image>();
                placeholderImage.color = new Color(0, 0, 0, 0); // Fully transparent
                */
            }
        }
    }
}