using UnityEngine;
using UnityEngine.UI;
public class TestItem : MonoBehaviour
{
    [SerializeField] private InventoryGrid grid;
    [SerializeField] private InventoryItemData itemData;

    private void Start()
    {
        // Wait a frame to ensure the grid has been initialized
        Invoke("PlaceTestItem", 0.1f);

    }

    private void PlaceTestItem()
    {
        // Create the inventory item GameObject
        GameObject itemObject = new GameObject(itemData.ItemName);
        InventoryItem item = itemObject.AddComponent<InventoryItem>();

        // Initialize with data (the item will instantiate its visual prefab)
        item.Initialize(itemData);

        // Try to place in grid
        bool success = grid.PlaceItem(1, 1, item);
        if (!success)
        {
            Debug.LogError("Failed to place item there!");
            Destroy(itemObject);
        }
        else
        {
            Debug.Log($"Successfully placed {itemData.ItemName}");
        }
    }
}