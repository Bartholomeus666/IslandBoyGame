using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGenerator;

    private void Start()
    {
        gridGenerator.SetCellOccupied(0, 0, true);
        gridGenerator.SetCellOccupied(3, 4, true);
    }
}
