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
            return false;

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

        for (int y = 0; y < item.shape.GetLength(1); y++)
        {
            for (int x = 0;x < item.shape.GetLength(0); x++)
            {
                if (!item.shape[x, y])
                    continue;

                int gridX = topLeftX + x;
                int gridY = topLeftY + y;

                occupiedCells[gridX, gridY] = true;

                GameObject cellVisual = new GameObject($"Cell_{x}_{y}");
                cellVisual.transform.SetParent(itemObject.transform);

                Image image = cellVisual.AddComponent<Image>();
                image.sprite = item.icon;
                image.color = item.itemColor;

                RectTransform rectTransform = cellVisual.GetComponent<RectTransform>();
                rectTransform.position = slots[gridX, gridY].GetComponent<RectTransform>().position;
                rectTransform.sizeDelta = slots[gridX, gridY].GetComponent <RectTransform>().sizeDelta;
            }
        }
    }
}