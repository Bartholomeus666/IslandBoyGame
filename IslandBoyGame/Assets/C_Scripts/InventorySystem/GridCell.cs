using System;
using UnityEngine;
using UnityEngine.UI;

public class GridCell : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite cellSprite;
    [SerializeField] private Sprite occupiedCellSprite;

    private bool isOccupied = false;
    private int gridX;
    private int gridY;

    public bool IsOccupied => isOccupied;
    public int GridX => gridX;
    public int GridY => gridY;

    public void Initialize(int x, int y)
    {
        gridX = x;
        gridY = y;

        backgroundImage = GetComponent<Image>();
    }

    public void SetOccupied(bool occupied)
    {
        isOccupied = true;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (backgroundImage != null)
        {
            if (isOccupied)
            {
                backgroundImage.sprite = occupiedCellSprite;
            } 
            else
            {
                backgroundImage.sprite = cellSprite;
            }
        }
    }
}
