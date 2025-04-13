using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private GameObject itemPrefab;

    private List<GridItem> placedItems = new List<GridItem>();

    public bool IsCellOccupied(int x, int y)
    {
        GridCell cell = gridGenerator.GetCell(x, y);
        return cell != null && cell.IsOccupied;
    }

    public void SetCellOccupied(int x, int y, bool occupied)
    {
        GridCell cell = gridGenerator.GetCell(x, y);
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

        // Mark cells as occupied
        for (int x = 0; x < item.width; x++)
        {
            for (int y = 0; y < item.height; y++)
            {
                if (item.IsCellOccupied(x, y))
                {
                    SetCellOccupied(startX + x, startY + y, true);
                }
            }
        }

        // Create the visual representation
        GameObject itemObject = Instantiate(itemPrefab, Vector2.zero, Quaternion.identity, gridGenerator.GridParent);
        itemObject.name = item.itemName;

        GridItem gridItem = itemObject.GetComponent<GridItem>();
        if (gridItem == null)
        {
            gridItem = itemObject.AddComponent<GridItem>();
        }

        gridItem.Initialize(item, startX, startY, gridGenerator);
        placedItems.Add(gridItem);

        return true;
    }

    public bool RemoveItem(GridItem gridItem)
    {
        if (gridItem == null || !placedItems.Contains(gridItem))
        {
            return false;
        }

        // Clear occupied cells
        InventoryItem item = gridItem.ItemData;
        int startX = gridItem.GridX;
        int startY = gridItem.GridY;

        for (int x = 0; x < item.width; x++)
        {
            for (int y = 0; y < item.height; y++)
            {
                if (item.IsCellOccupied(x, y))
                {
                    SetCellOccupied(startX + x, startY + y, false);
                }
            }
        }

        // Remove from list and destroy object
        placedItems.Remove(gridItem);
        Destroy(gridItem.gameObject);
        return true;
    }

    public GridItem GetItemAt(int x, int y)
    {
        foreach (var item in placedItems)
        {
            int itemX = item.GridX;
            int itemY = item.GridY;

            for (int i = 0; i < item.ItemData.width; i++)
            {
                for (int j = 0; j < item.ItemData.height; j++)
                {
                    if (item.ItemData.IsCellOccupied(i, j) &&
                        itemX + i == x && itemY + j == y)
                    {
                        return item;
                    }
                }
            }
        }

        return null;
    }

    private bool CanPlaceItem(InventoryItem item, int startX, int startY)
    {
        if (startX < 0 || startX + item.width > gridGenerator.Width ||
            startY < 0 || startY + item.height > gridGenerator.Height)
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
}
