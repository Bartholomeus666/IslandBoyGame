using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;

    [Tooltip("Define the shape using a string of 1's and 0's. Each line is a row, 1 = occupied, 0 = empty")]
    [TextArea(3, 10)]
    public string shapeString = "1";

    [HideInInspector]
    public int width;

    [HideInInspector]
    public int height;

    private bool[,] itemShape;

    // This is called when the scriptable object is created or modified in the editor
    private void OnValidate()
    {
        ParseShapeString();
    }

    private void OnEnable()
    {
        // Ensure the shape is parsed when the object is loaded
        if (itemShape == null)
        {
            ParseShapeString();
        }
    }

    private void ParseShapeString()
    {
        if (string.IsNullOrEmpty(shapeString))
        {
            shapeString = "1";
        }

        // Split the string by lines
        string[] rows = shapeString.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        height = rows.Length;

        // Find the maximum width
        width = 0;
        foreach (string row in rows)
        {
            width = Mathf.Max(width, row.Length);
        }

        // Create the shape array
        itemShape = new bool[width, height];

        // Parse each character
        for (int y = 0; y < height; y++)
        {
            string row = rows[y];
            for (int x = 0; x < row.Length; x++)
            {
                itemShape[x, y] = row[x] == '1';
            }
        }
    }

    public bool IsCellOccupied(int relativeX, int relativeY)
    {
        if (itemShape == null)
        {
            ParseShapeString();
        }

        if (relativeX >= 0 && relativeX < width && relativeY >= 0 && relativeY < height)
        {
            return itemShape[relativeX, relativeY];
        }
        return false;
    }
}