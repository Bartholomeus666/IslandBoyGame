using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private InventoryItem featherPen;
    [SerializeField] private InventoryItem potion;

    private void Start()
    {
        gridGenerator.PlaceItem(featherPen, 3, 3);
        gridGenerator.PlaceItem(potion, 3, 3);
    }
}
