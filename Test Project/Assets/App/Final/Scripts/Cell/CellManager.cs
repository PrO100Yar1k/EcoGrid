using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    [SerializeField] private List<HexCell> _cells = new List<HexCell>();

    [SerializeField] private float _updateInterval = 0.25f; // 4 рази на секунду
    private float _timer = 0f;

    private void Awake()
    {
        InitializeHexCells();
        _cells.Clear();
        _cells.AddRange(FindObjectsOfType<HexCell>());
    }

    private void InitializeHexCells()
    {
        foreach (HexCell cell in _cells)
        {
            cell.InitializeCell();
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _updateInterval)
        {
            _timer = 0f;
            UpdateCellColors();
        }
    }

    private void UpdateCellColors()
    {
        foreach (HexCell cell in _cells)
        {
            float pollution = (cell.AirPollution + cell.SoilFertility) / 20f; // середнє, нормалізоване до 0–1
            pollution = Mathf.Clamp01(pollution); // щоб не вийшло за межі
            Color newColor = Color.Lerp(Color.green, Color.gray, pollution);

            Renderer renderer = cell.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = newColor;
            }
        }
    }

    private void GetDataFromSatelite()
    {
        // to do — оновлення даних з супутника
    }
}