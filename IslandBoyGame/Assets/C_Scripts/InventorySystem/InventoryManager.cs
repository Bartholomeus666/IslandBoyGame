using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryGrid inventoryGrid;
    [SerializeField] private List<InventoryItemData> availableItems = new List<InventoryItemData>();
    [SerializeField] private Transform itemSpawnPoint;
    [SerializeField] private Transform itemContainer;

    [Header("Test Controls")]
    [SerializeField] private Button addRandomItemButton;

    private void Start()
    {
        if (addRandomItemButton != null)
        {
            addRandomItemButton.onClick.AddListener(AddRandomItem);
        }

        // Find dependencies if not assigned
        if (inventoryGrid == null)
        {
            inventoryGrid = FindObjectOfType<InventoryGrid>();
        }

        if (itemContainer == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                GameObject container = new GameObject("ItemsContainer");
                container.transform.SetParent(canvas.transform, false);
                itemContainer = container.transform;
            }
        }
    }

    public void AddRandomItem()
    {
        if (availableItems.Count == 0)
        {
            Debug.LogWarning("No available items to add!");
            return;
        }

        // Choose a random item from the available ones
        int randomIndex = Random.Range(0, availableItems.Count);
        InventoryItemData itemData = availableItems[randomIndex];

        // Create the item game object
        GameObject itemObject = new GameObject(itemData.ItemName);

        // Add a RectTransform for UI positioning
        RectTransform rectTransform = itemObject.AddComponent<RectTransform>();

        // Set the parent to our items container
        itemObject.transform.SetParent(itemContainer, false);

        // Add and initialize the inventory item component
        InventoryItem item = itemObject.AddComponent<InventoryItem>();
        item.Initialize(itemData);

        // Try to place the item in the inventory
        TryPlaceItemInInventory(item);
    }

    public GameObject CreateItem(InventoryItemData itemData)
    {
        if (itemData == null)
            return null;

        // Create the item game object
        GameObject itemObject = new GameObject(itemData.ItemName);

        // Add a RectTransform for UI positioning
        RectTransform rectTransform = itemObject.AddComponent<RectTransform>();

        // Set the parent to our items container
        itemObject.transform.SetParent(itemContainer, false);

        // Add and initialize the inventory item component
        InventoryItem item = itemObject.AddComponent<InventoryItem>();
        item.Initialize(itemData);

        return itemObject;
    }

    private bool TryPlaceItemInInventory(InventoryItem item)
    {
        if (inventoryGrid == null || item == null)
            return false;

        // Try to find an available spot in the grid
        for (int y = 0; y < inventoryGrid.Height; y++)
        {
            for (int x = 0; x < inventoryGrid.Width; x++)
            {
                if (inventoryGrid.CanPlaceItem(x, y, item))
                {
                    // Place the item at this position
                    bool success = inventoryGrid.PlaceItem(x, y, item);
                    if (success)
                    {
                        // Set up the drag handler
                        InventoryItemDragHandler dragHandler = item.GetComponent<InventoryItemDragHandler>();
                        if (dragHandler == null)
                        {
                            dragHandler = item.gameObject.AddComponent<InventoryItemDragHandler>();
                        }

                        dragHandler.SetGrid(inventoryGrid);
                        dragHandler.SetOriginalCell(x, y);

                        return true;
                    }
                }
            }
        }

        // If we couldn't place the item, destroy it
        Debug.LogWarning("Could not find a spot to place item: " + item.ItemData.ItemName);
        Destroy(item.gameObject);
        return false;
    }

    public void ClearInventory()
    {
        if (inventoryGrid == null)
            return;

        // Go through each cell and remove items
        for (int y = 0; y < inventoryGrid.Height; y++)
        {
            for (int x = 0; x < inventoryGrid.Width; x++)
            {
                InventoryCell cell = inventoryGrid.GetCell(x, y);
                if (cell != null && cell.CurrentItem != null)
                {
                    InventoryItem item = inventoryGrid.RemoveItem(x, y);
                    if (item != null)
                    {
                        Destroy(item.gameObject);
                    }
                }
            }
        }
    }
}