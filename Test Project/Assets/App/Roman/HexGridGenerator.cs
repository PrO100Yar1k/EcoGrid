using System.Collections.Generic;
using UnityEngine;

public class HexGridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    public GameObject hexPrefab;        // Префаб гексу
    public int width = 6;               // Кількість гексів по горизонталі
    public int height = 8;              // Кількість гексів по вертикалі
    public float hexWidth = 1f;         // Базова ширина гексу
    public float hexHeight = 0.866f;    // Висота одного гексу (sqrt(3)/2 для правильного гекса)
    public float heightVariation = 0.5f; // Максимальна випадкова висота (Y)

    private HexCell[,] hexGrid;

    void Start()
    {
        if (hexPrefab == null)
        {
            Debug.LogError("Hex Prefab is not assigned in HexGridGenerator!");
            return;
        }

        GenerateGrid();
        AssignNeighbors();
    }

    void GenerateGrid()
    {
        hexGrid = new HexCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Зсув гекса з урахуванням збільшеної відстані (2x)
                float xPos = x * hexWidth * 0.75f * 2f; // множимо на 2
                float zPos = y * hexHeight * 2f;        // множимо на 2
                if (x % 2 == 1)
                    zPos += hexHeight;                  // половина висоти * 2 = hexHeight

                // Додати випадкову висоту
                float yPos = Random.Range(0f, heightVariation);

                GameObject hexGO = Instantiate(hexPrefab, new Vector3(xPos, yPos, zPos), Quaternion.Euler(0f, 90f, 0f), transform);
                hexGO.name = $"Hex_{x}_{y}";
                hexGrid[x, y] = hexGO.GetComponent<HexCell>();
            }
        }
    }

    void AssignNeighbors()
    {
        int[,] evenOffsets = new int[,]
        {
            {+1, 0}, {0, +1}, {-1, +1},
            {-1, 0}, {-1, -1}, {0, -1}
        };

        int[,] oddOffsets = new int[,]
        {
            {+1, 0}, {+1, +1}, {0, +1},
            {-1, 0}, {0, -1}, {+1, -1}
        };

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                HexCell cell = hexGrid[x, y];
                List<HexCell> neighbors = new List<HexCell>();
                int[,] offsets = (x % 2 == 0) ? evenOffsets : oddOffsets;

                for (int i = 0; i < 6; i++)
                {
                    int nx = x + offsets[i, 0];
                    int ny = y + offsets[i, 1];
                    if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                        neighbors.Add(hexGrid[nx, ny]);
                }

                var field = typeof(HexCell).GetField("_neighborTileList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                field.SetValue(cell, neighbors);
            }
        }
    }
}
