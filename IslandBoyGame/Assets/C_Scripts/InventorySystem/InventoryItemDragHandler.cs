using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private InventoryItem item;
    private InventoryGrid grid;
    private Vector3 startPosition;
    private Vector3 startScale;
    private Transform originalParent;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    // The original cell position in the grid
    private int originalX;
    private int originalY;

    // Visual indicator for valid/invalid placement
    private Color validPlacementColor = new Color(0.5f, 1f, 0.5f, 0.7f); // Green
    private Color invalidPlacementColor = new Color(1f, 0.5f, 0.5f, 0.7f); // Red
    private Color normalColor = Color.white;

    private bool isDragging = false;

    private void Awake()
    {
        item = GetComponent<InventoryItem>();
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();

        // Add a CanvasGroup component if it doesn't exist
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void SetGrid(InventoryGrid inventoryGrid)
    {
        grid = inventoryGrid;
    }

    public void SetOriginalCell(int x, int y)
    {
        originalX = x;
        originalY = y;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null || grid == null) return;

        isDragging = true;

        // Store original position, scale, and parent
        startPosition = transform.position;
        startScale = transform.localScale;
        originalParent = transform.parent;

        // Make the item semi-transparent and ignore raycasts while dragging
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        // Remove the item from its current position in the grid
        grid.RemoveItem(originalX, originalY);

        // DO NOT CHANGE PARENT - just keep the current position and transform

        // Set the item color to the normal dragging color
        item.SetColor(normalColor);

        // Log the scale for debugging
        Debug.Log($"Begin Drag - Scale: {transform.localScale}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        // Move the item with the pointer, but don't change its parent
        Vector3 mousePosition = Input.mousePosition;
        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);

        // Force the original scale
        transform.localScale = startScale;

        // Find the grid cell under the cursor
        Vector2 gridCellPosition;
        InventoryCell targetCell = FindCellUnderPointer(eventData, out gridCellPosition);

        if (targetCell != null)
        {
            int gridX = targetCell.GridX;
            int gridY = targetCell.GridY;

            // Set color based on whether the placement is valid
            if (grid.CanPlaceItem(gridX, gridY, item))
            {
                item.SetColor(validPlacementColor);
            }
            else
            {
                item.SetColor(invalidPlacementColor);
            }
        }
        else
        {
            // Not over a valid cell
            item.SetColor(invalidPlacementColor);
        }

        // Log the scale for debugging
        Debug.Log($"During Drag - Scale: {transform.localScale}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        isDragging = false;

        // Reset transparency and raycast blocking
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Reset color
        item.SetColor(normalColor);

        // Force the original scale
        transform.localScale = startScale;

        // Try to place the item at the new position
        Vector2 gridCellPosition;
        InventoryCell targetCell = FindCellUnderPointer(eventData, out gridCellPosition);

        bool placed = false;

        if (targetCell != null)
        {
            int gridX = targetCell.GridX;
            int gridY = targetCell.GridY;

            // Try to place the item in the grid
            placed = grid.PlaceItem(gridX, gridY, item);

            if (placed)
            {
                // Update the original position reference
                originalX = gridX;
                originalY = gridY;

                // The item is now parented to the cell through the grid's PlaceItem method
                return;
            }
        }

        // If we couldn't place the item, try to return it to its original position
        if (!placed)
        {
            if (grid.CanPlaceItem(originalX, originalY, item))
            {
                // Return to original position
                grid.PlaceItem(originalX, originalY, item);
            }
            else
            {
                // The original position is no longer valid, try to find another spot
                placed = TryPlaceItemSomewhere();

                if (!placed)
                {
                    // Failed to place item anywhere - this could mean the inventory is full
                    Debug.LogWarning("Failed to place item: " + item.ItemData.ItemName);

                    // Just return it to where it started
                    transform.position = startPosition;
                    transform.SetParent(originalParent, true);
                    transform.localScale = startScale;
                }
            }
        }

        // Log the scale for debugging
        Debug.Log($"End Drag - Scale: {transform.localScale}");
    }

    private InventoryCell FindCellUnderPointer(PointerEventData eventData, out Vector2 cellPosition)
    {
        // Raycast to find the cell under the pointer
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        cellPosition = Vector2.zero;

        foreach (RaycastResult result in results)
        {
            // Look for a cell component
            InventoryCell cell = result.gameObject.GetComponent<InventoryCell>();
            if (cell != null)
            {
                cellPosition = result.gameObject.transform.position;
                return cell;
            }
        }

        return null;
    }

    private bool TryPlaceItemSomewhere()
    {
        // Try to find any valid position in the grid
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                if (grid.CanPlaceItem(x, y, item))
                {
                    // Force scale to be correct before placing
                    transform.localScale = startScale;

                    bool placed = grid.PlaceItem(x, y, item);
                    if (placed)
                    {
                        originalX = x;
                        originalY = y;
                        return true;
                    }
                }
            }
        }

        return false;
    }
}