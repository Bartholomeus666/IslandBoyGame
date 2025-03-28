using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    public InventoryGrid inventoryGrid;
    public InventoryItem bootItem;
    public InventoryItem squareItem;

    private void Start()
    {
        Invoke("PlaceBoot", 0.1f);
    }

    private void PlaceBoot()
    {
        inventoryGrid.TryPlaceItem(bootItem, 0, 0);
        inventoryGrid.TryPlaceItem(bootItem, 2, 2);
    }
}
