using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    public int soilChangeValue = default;
    public int airChangeValue = default;
    public int waterChangeValue = default;

    protected HexCell _currentCell = default;

    public int MoneyProfit = default;

    public void Initialize(HexCell hexCell)
    {
        _currentCell = hexCell;
        if (PlaySound.Instance != null)
            PlaySound.Instance.PlayBuild();
    }

    public void AffectOnNeighbourTile()
    {
        HexCell[] neighbors = GetNeighbourTile();

        foreach (HexCell neighbor in neighbors)
        {
            neighbor.ChangeSoil(soilChangeValue);
            neighbor.ChangeAir(airChangeValue);
            neighbor.ChangeWater(waterChangeValue);
        }
    }

    private void OnMouseUp()
    {
        GameEvents.instance.BuildingClicked(soilChangeValue, airChangeValue, waterChangeValue);
        GameEvents.instance.BuildingClicked2(this);
    }

    public bool isNegativeTile()
        => soilChangeValue < 0 || airChangeValue < 0 || waterChangeValue < 0;

    public HexCell[] GetNeighbourTile()
    {
        List<HexCell> newList = new List<HexCell>(_currentCell.NeighborTileList);
        newList.Add(_currentCell);

        return newList.ToArray();
    }
}

public enum BuildingType
{
    Factory = 0,
    AirCleaner = 1,
    SoilCleaner = 2,
}