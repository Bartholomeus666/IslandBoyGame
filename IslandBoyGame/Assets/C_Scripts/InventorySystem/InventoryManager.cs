using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Canvas canvas; // Reference to the canvas for proper drag positioning

    private List<GridItem> placedItems = new List<GridItem>();
    private GridItem selectedItem = null;
    private Vector2 spriteOffset; // Offset to center the sprite on the cursor
    private int originalX, originalY; // Original grid position before selecting

    void Update()
    {
        if (selectedItem != null)
        {
            // Move the item with the mouse, centered on the cursor
            Vector2 mousePosition = Input.mousePosition;
            selectedItem.transform.position = mousePosition - spriteOffset;

            // Calculate grid position based on mouse position
            UpdateSelectedItemGridPosition();

            // Handle right-click for rotation while item is selected
            if (Input.GetMouseButtonDown(1))
            {
                RotateItemClockwise(selectedItem);
                // Recalculate the sprite offset when rotating
                CalculateSpriteOffset();
            }
        }

        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedItem == null)
            {
                // No item is currently selected, try to pick one up
                SelectItem();
            }
            else
            {
                // Item is already selected, try to place it
                PlaceSelectedItem();
            }
        }
    }

    private void CalculateSpriteOffset()
    {
        if (selectedItem == null) return;

        // Use the item's Width and Height (in grid cells) instead of raw sprite pixels
        float cellSize = gridGenerator.CellSize;
        float itemWidthInPixels = selectedItem.Width * cellSize;
        float itemHeightInPixels = selectedItem.Height * cellSize;

        // Calculate offset to center the sprite on the cursor
        spriteOffset = new Vector2(itemWidthInPixels / 2, -itemHeightInPixels / 2);
    }

    private void SelectItem()
    {
        // Check if we clicked on an item
        Vector2 mousePosition = Input.mousePosition;

        // Convert mouse position to grid coordinates
        RectTransformUtility.ScreenPointToLocalPointInRectangle(gridGenerator.GridParent as RectTransform, mousePosition, null, out Vector2 localPoint);

        Vector2 gridOffset = gridGenerator.GridOffset;
        float cellSize = gridGenerator.CellSize;

        int gridX = Mathf.FloorToInt((localPoint.x - gridOffset.x + cellSize / 2) / cellSize);
        int gridY = Mathf.FloorToInt(-(localPoint.y - gridOffset.y - cellSize / 2) / cellSize);

        // Get the item at this position
        GridItem item = GetItemAt(gridX, gridY);

        if (item != null)
        {
            // Store the original position
            originalX = item.GridX;
            originalY = item.GridY;

            // Clear the cells occupied by this item
            ClearItemOccupation(item);

            // Select this item
            selectedItem = item;

            // Calculate the sprite offset
            CalculateSpriteOffset();

            // Move the selected item to the top of the hierarchy for visual clarity
            item.transform.SetAsLastSibling();
        }
    }

    private void UpdateSelectedItemGridPosition()
    {
        if (selectedItem == null) return;

        // Convert mouse position to grid coordinates
        Vector2 mousePosition = Input.mousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(gridGenerator.GridParent as RectTransform, mousePosition, null, out Vector2 localPoint);

        Vector2 gridOffset = gridGenerator.GridOffset;
        float cellSize = gridGenerator.CellSize;

        int gridX = Mathf.FloorToInt((localPoint.x - gridOffset.x + cellSize / 2) / cellSize);
        int gridY = Mathf.FloorToInt(-(localPoint.y - gridOffset.y - cellSize / 2) / cellSize);

        // Clear old previews
        ClearPlacementPreviews();

        // Update grid position
        selectedItem.gridX = gridX;
        selectedItem.gridY = gridY;

        // Show placement preview
        UpdatePlacementPreviews();
    }

    private void ClearPlacementPreviews()
    {
        // Clear all preview cells
        for (int x = 0; x < gridGenerator.Width; x++)
        {
            for (int y = 0; y < gridGenerator.Height; y++)
            {
                GridCell cell = gridGenerator.GetCell(x, y);
                if (cell != null)
                {
                    cell.SetPlacementPreview(false);
                }
            }
        }
    }

    private void UpdatePlacementPreviews()
    {
        if (selectedItem == null) return;

        // Check if current position is valid
        bool canPlace = CanPlaceItemWithRotation(selectedItem);

        // Show placement previews
        for (int x = 0; x < selectedItem.Width; x++)
        {
            for (int y = 0; y < selectedItem.Height; y++)
            {
                if (selectedItem.IsCellOccupied(x, y))
                {
                    int gridX = selectedItem.GridX + x;
                    int gridY = selectedItem.GridY + y;

                    if (gridX >= 0 && gridX < gridGenerator.Width &&
                        gridY >= 0 && gridY < gridGenerator.Height)
                    {
                        GridCell cell = gridGenerator.GetCell(gridX, gridY);
                        if (cell != null && !cell.IsOccupied)
                        {
                            cell.SetPlacementPreview(true);
                        }
                    }
                }
            }
        }
    }

    private void PlaceSelectedItem()
    {
        if (selectedItem == null) return;

        // Clear previews
        ClearPlacementPreviews();

        // Check if we can place the item at the current position
        if (CanPlaceItemWithRotation(selectedItem))
        {
            // Place the item
            OccupyCells(selectedItem);

            // Update visual position
            selectedItem.Initialize(selectedItem.ItemData, selectedItem.GridX, selectedItem.GridY, gridGenerator);

            // Deselect the item
            selectedItem = null;
        }
        else
        {
            // Return to original position
            selectedItem.gridX = originalX;
            selectedItem.gridY = originalY;

            // Re-occupy original cells
            OccupyCells(selectedItem);

            // Update visual position
            selectedItem.Initialize(selectedItem.ItemData, selectedItem.GridX, selectedItem.GridY, gridGenerator);

            // Deselect the item
            selectedItem = null;
        }
    }

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
        if (!CanPlaceItemAtPosition(item, startX, startY))
        {
            Debug.Log($"Cannot place item at position {startX}, {startY}");
            return false;
        }

        // Create the visual representation first
        GameObject itemObject = Instantiate(itemPrefab, Vector2.zero, Quaternion.identity, gridGenerator.GridParent);
        itemObject.name = item.itemName;

        GridItem gridItem = itemObject.GetComponent<GridItem>();
        if (gridItem == null)
        {
            gridItem = itemObject.AddComponent<GridItem>();
        }

        gridItem.Initialize(item, startX, startY, gridGenerator);

        // Mark cells as occupied
        OccupyCells(gridItem);

        placedItems.Add(gridItem);

        return true;
    }

    private void OccupyCells(GridItem gridItem)
    {
        for (int x = 0; x < gridItem.Width; x++)
        {
            for (int y = 0; y < gridItem.Height; y++)
            {
                if (gridItem.IsCellOccupied(x, y))
                {
                    SetCellOccupied(gridItem.GridX + x, gridItem.GridY + y, true);
                }
            }
        }
    }

    public void ClearItemOccupation(GridItem gridItem)
    {
        for (int x = 0; x < gridItem.Width; x++)
        {
            for (int y = 0; y < gridItem.Height; y++)
            {
                if (gridItem.IsCellOccupied(x, y))
                {
                    SetCellOccupied(gridItem.GridX + x, gridItem.GridY + y, false);
                }
            }
        }
    }

    public bool UpdateItemPlacement(GridItem gridItem)
    {
        // Check if the item can be placed at its current position with its current rotation
        if (!CanPlaceItemWithRotation(gridItem))
        {
            return false;
        }

        // Mark cells as occupied
        OccupyCells(gridItem);

        return true;
    }

    private bool CanPlaceItemWithRotation(GridItem gridItem)
    {
        int startX = gridItem.GridX;
        int startY = gridItem.GridY;

        // Check boundaries
        if (startX < 0 || startX + gridItem.Width > gridGenerator.Width ||
            startY < 0 || startY + gridItem.Height > gridGenerator.Height)
        {
            return false;
        }

        // Check for collisions with other items
        for (int x = 0; x < gridItem.Width; x++)
        {
            for (int y = 0; y < gridItem.Height; y++)
            {
                if (gridItem.IsCellOccupied(x, y))
                {
                    // Check if this cell is occupied by another item
                    GridCell cell = gridGenerator.GetCell(startX + x, startY + y);
                    if (cell.IsOccupied)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public bool RemoveItem(GridItem gridItem)
    {
        if (gridItem == null || !placedItems.Contains(gridItem))
        {
            return false;
        }

        // Clear occupied cells
        ClearItemOccupation(gridItem);

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

            for (int i = 0; i < item.Width; i++)
            {
                for (int j = 0; j < item.Height; j++)
                {
                    if (item.IsCellOccupied(i, j) &&
                        itemX + i == x && itemY + j == y)
                    {
                        return item;
                    }
                }
            }
        }

        return null;
    }

    public void RotateItemClockwise(GridItem item)
    {
        if (item != null && placedItems.Contains(item))
        {
            // If this is the selected item, we just rotate it without placing
            if (item == selectedItem)
            {
                // Apply rotation to the shape
                item.Rotate90ClockwiseDuringDrag();

                // Update the placement preview
                ClearPlacementPreviews();
                UpdatePlacementPreviews();
            }
            else
            {
                // Normal rotation for placed items
                item.Rotate90Clockwise(this);
                // Update visual
                item.Initialize(item.ItemData, item.GridX, item.GridY, gridGenerator);
            }
        }
    }

    private bool CanPlaceItemAtPosition(InventoryItem item, int startX, int startY)
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