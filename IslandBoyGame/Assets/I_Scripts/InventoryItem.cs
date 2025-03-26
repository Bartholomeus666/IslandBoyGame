using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool[,] shape;
    public Color itemColor = Color.white;

    [SerializeField] private int width = 3;
    [SerializeField] private int height = 3;
    [SerializeField] private string shapeString = "110\n100\n110";

    public void UpdateShapeFromString()
    {
        string[] rows = shapeString.Split('\n');

        // Make sure we have valid data
        if (rows.Length == 0 || string.IsNullOrEmpty(rows[0]))
        {
            Debug.LogError("Invalid shape string format");
            return;
        }

        height = rows.Length;
        width = rows[0].Length;

        // Initialize the shape array
        shape = new bool[width, height];

        // Fill the shape array
        for (int y = 0; y < height; y++)
        {
            string row = rows[y];

            // Make sure this row isn't too short
            if (row.Length < width)
            {
                Debug.LogWarning($"Row {y} is shorter than expected. Padding with zeros.");
                row = row.PadRight(width, '0');
            }

            for (int x = 0; x < width; x++)
            {
                // Safely access the character
                if (x < row.Length)
                {
                    shape[x, y] = row[x] == '1';
                }
                else
                {
                    shape[x, y] = false;
                }
            }
        }

        Debug.Log($"Updated shape: {width}x{height}");
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateShapeFromString();
    }
#endif
}


