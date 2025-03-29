using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool isInUse;
    [SerializeField] private InventoryItem currentItem;
    [SerializeField] private int gridX, gridY;
    [SerializeField] private Image backgroundImage; // Optional: For visual indication

    // Reference to the grid this cell belongs to
    private InventoryGrid parentGrid;

    // Static reference to the items container
    private static Transform itemsContainer;

    public bool IsInUse => isInUse;
    public InventoryItem CurrentItem => currentItem;
    public int GridX => gridX;
    public int GridY => gridY;

    // Find or create the items container
    private Transform GetItemsContainer()
    {
        if (itemsContainer == null)
        {
            // Try to find existing container
            GameObject container = GameObject.Find("Items");
            // Create it if it doesn't exist
            if (container == null)
            {
                container = new GameObject("Items");
                // If this is a UI-based inventory, make sure the items container is in the Canvas
                Canvas canvas = FindObjectOfType<Canvas>();
                if (canvas != null)
                {
                    container.transform.SetParent(canvas.transform, false);
                    // Make sure it has a RectTransform
                    if (container.GetComponent<RectTransform>() == null)
                    {
                        container.AddComponent<RectTransform>();
                    }
                    // Make sure it's positioned properly in the UI hierarchy
                    // This ensures items are rendered on top of the grid
                    container.transform.SetAsLastSibling();
                }
            }
            itemsContainer = container.transform;
        }
        return itemsContainer;
    }

    // Initialize the cell
    public void Initialize(int x, int y, bool inUse, InventoryGrid grid)
    {
        gridX = x;
        gridY = y;
        isInUse = inUse;
        parentGrid = grid;

        // Optional: Find background image if not set
        if (backgroundImage == null)
            backgroundImage = GetComponent<Image>();
    }

    // Set an item in this cell
    // Set an item in this cell
    // Set an item in this cell
    public void SetItem(InventoryItem item)
    {
        currentItem = item;
        isInUse = true;

        if (item != null && item.gameObject != null)
        {
            // Set the parent to the items container instead of this cell
            item.transform.SetParent(GetItemsContainer(), false);

            // Always maintain original scale
            item.transform.localScale = Vector3.one;

            // For centered positioning, we need to offset from the cell center
            float cellSize = 100f; // This should match your grid's cell size

            // Position the item with an offset to account for multi-cell items
            // This positions the center of the item at the correct position
            float offsetX = cellSize * (item.Width - 1) / 2;
            float offsetY = -cellSize * (item.Height - 1) / 2;

            // Apply the position with the calculated offset
            Vector3 position = transform.position;
            position.x += offsetX;
            position.y += offsetY;
            item.transform.position = position;

            // Set up the drag handler
            InventoryItemDragHandler dragHandler = item.GetComponent<InventoryItemDragHandler>();
            if (dragHandler == null)
            {
                dragHandler = item.gameObject.AddComponent<InventoryItemDragHandler>();
            }

            dragHandler.SetGrid(parentGrid);
            dragHandler.SetOriginalCell(gridX, gridY);
        }

        UpdateVisual();
    }

    // Mark the cell as occupied (for multi-cell items)
    public void MarkAsOccupied(bool occupied)
    {
        isInUse = occupied;
        // If we're clearing the cell, also clear the item
        if (!occupied)
            currentItem = null;
        UpdateVisual();
    }

    // Get the item in this cell
    public InventoryItem GetItem()
    {
        return currentItem;
    }

    // Clear the cell
    public void ClearCell()
    {
        currentItem = null;
        isInUse = false;
        UpdateVisual();
    }

    // Update the visual appearance
    private void UpdateVisual()
    {
        // This is optional - you can use it to show cells as occupied/free
        if (backgroundImage != null)
        {
            // For example, you could change the cell color based on its state
            if (isInUse)
            {
                // Occupied cell - slightly darker
                backgroundImage.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            }
            else
            {
                // Free cell - normal color
                backgroundImage.color = Color.white;
            }
        }
    }

    // Implement IDropHandler to handle item dropping directly on cells
    public void OnDrop(PointerEventData eventData)
    {
        // This is a placeholder to enable drops on the cell
        // The actual logic is handled in the InventoryItemDragHandler's OnEndDrag

        // But we could add some additional logic here if needed
        Debug.Log($"Item dropped on cell {gridX}, {gridY}");
    }
}