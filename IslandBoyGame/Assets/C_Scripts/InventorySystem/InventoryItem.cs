using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;

    public int width = 1;
    public int height = 1;

    [SerializeField]
    private bool[,] itemShape;

    public void InitializeShape(int w, int h)
    {
        width = w;
        height = h;
        itemShape = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                itemShape[x, y] = true;
            }
        }
    }

    public bool IsCellOccupied(int relativeX, int relativeY)
    {
        if (itemShape == null)
        {
            InitializeShape(width, height);
        }

        if (relativeX >= 0 && relativeX < width && relativeY >= 0 &&relativeY < height)
        {
            return itemShape[relativeX, relativeY];
        }

        return false;
    }

}
