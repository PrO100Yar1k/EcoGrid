using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class HexMapPopulator : EditorWindow
{
    [MenuItem("Tools/Hex Map/Populate Hex Cells")]
    public static void PopulateHexCells()
    {
        GameObject hexLayout = GameObject.Find("HexLayout");
        if (hexLayout == null)
        {
            Debug.LogError("HexLayout not found in scene!");
            return;
        }

        HexCell[] hexCells = hexLayout.GetComponentsInChildren<HexCell>();

        // === Дані ===
        int[] soilData = { 47, 46, 68, 64, 30, 27, 45, 40, 72, 70, 40, 30, 30, 33, 45, 30 };
        int[] airData = { 58, 70, 50, 30, 20, 22, 55, 65, 50, 40, 35, 22, 32, 20, 50, 55, 35, 40 };
        int[] waterData = { 38, 50, 30, 10, 10, 12, 45, 55, 40, 30, 25, 12, 32, 20, 50, 45, 25, 30 };

        // === Заповнюємо базові дані ===
        for (int i = 0; i < hexCells.Length; i++)
        {
            int groupIndex = Mathf.Min(i / 4, soilData.Length - 1);

            //hexCells[i].SoilFertility = ApplyRandomOffset(soilData[groupIndex]); // to do
            //hexCells[i].AirPollution = ApplyRandomOffset(airData[groupIndex % airData.Length]); // changed
            //hexCells[i].WaterPollution = ApplyRandomOffset(waterData[groupIndex % waterData.Length]);
        }

        // === Підготовка для пошуку сусідів ===
        Dictionary<(int r, int c), HexCell> cellMap = new Dictionary<(int, int), HexCell>();

        foreach (var cell in hexCells)
        {
            // Ім’я без "(1)"
            string cleanName = cell.name.Split(' ')[0]; 
            // Має бути типу Hex_2_3
            string[] parts = cleanName.Replace("Hex_", "").Split('_');
            if (parts.Length < 2) continue;

            if (int.TryParse(parts[0], out int r) && int.TryParse(parts[1], out int c))
            {
                cellMap[(r, c)] = cell;
            }
            else
            {
                Debug.LogWarning($"Could not parse coordinates from {cell.name}");
            }
        }

        // === Генерація сусідів ===
        foreach (var kvp in cellMap)
        {
            (int row, int col) = kvp.Key;
            HexCell cell = kvp.Value;

            List<(int, int)> neighborCoords = new List<(int, int)>
            {
                (row - 1, col),     // top
                (row + 1, col),     // bottom
                (row, col - 1),     // left
                (row, col + 1),     // right
                (row - 1, col + 1), // top-right (для even-r offset)
                (row + 1, col - 1)  // bottom-left
            };

            cell.NeighborTileList.Clear();

            foreach (var (r, c) in neighborCoords)
            {
                if (cellMap.ContainsKey((r, c)))
                    cell.NeighborTileList.Add(cellMap[(r, c)]);
            }

            EditorUtility.SetDirty(cell);
        }

        Debug.Log("✅ Hex cells updated with stats and neighbor lists!");
    }

    private static int ApplyRandomOffset(int baseValue)
    {
        float offset = Random.Range(-0.1f, 0.1f);
        return Mathf.Clamp(Mathf.RoundToInt(baseValue * (1 + offset)), 0, 100);
    }
}
