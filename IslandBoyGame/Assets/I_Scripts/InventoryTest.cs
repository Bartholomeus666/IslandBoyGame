using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    public InventoryGrid inventoryGrid;
    public InventoryItem bootItem;
    public InventoryItem squareItem;

    private void Start()
    {
        Invoke("PlaceBoot", 0.1f);
        Invoke("PlaceSquare", 0.1f);
    }

    private void PlaceBoot()
    {
        inventoryGrid.TryPlaceItem(bootItem, 0, 0);
    }

    private void PlaceSquare()
    {
        inventoryGrid.TryPlaceItem(squareItem, 3, 3);
    }
}
