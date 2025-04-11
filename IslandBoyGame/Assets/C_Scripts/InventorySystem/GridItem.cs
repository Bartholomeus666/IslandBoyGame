using UnityEngine;
using UnityEngine.UI;

public class GridItem : MonoBehaviour
{
    [SerializeField] private Image itemImage;

    private InventoryItem itemData;
    private int gridX;
    private int gridY;

    public InventoryItem ItemData => itemData;
    public int GridX => gridX;
    public int GridY => gridY;

    public void Initialize(InventoryItem item, int x, int y, Transform parent, float cellSize)
    {
        itemData = item;
        gridX = x;
        gridY = y;
        itemImage= GetComponent<Image>();
        itemImage.sprite = item.itemSprite;

        RectTransform rectTransform = GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(item.width * cellSize, item.height * cellSize);

        GridGenerator gridGenerator = parent.GetComponent<GridGenerator>();
        Vector2 gridOffset = gridGenerator.GetGridOffset();

        rectTransform.anchoredPosition = new Vector2(gridOffset.x - cellSize / 2 + x * cellSize, gridOffset.y + cellSize / 2 - y * cellSize);

        rectTransform.pivot = new Vector2(0, 1);
    }
}
