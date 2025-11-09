using System.Collections.Generic;
using UnityEngine;

public class BuilderMenu : MonoBehaviour
{
    [SerializeField] private GameObject _builderMenuContainer = default;
    [SerializeField] private SateliteData _builderSateliteData = default;

    [SerializeField] private List<BuildingTileUI> _buildingTileUIList = new List<BuildingTileUI>();

    public HexCell CurrentCell = default;

    private void OnEnable()
    {
        GameEvents.instance.OnBuildingCreated += DisableMenu;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnBuildingCreated -= DisableMenu;
    }

    private void Start()
    {
        DisableMenu();
    }

    public void EnableMenu(HexCell hexCell)
    {
        CurrentCell = hexCell;

        _builderMenuContainer.SetActive(true);
        _builderSateliteData.EnableMenu(hexCell);

        foreach (BuildingTileUI buildingTileUI in _buildingTileUIList)
        {
            buildingTileUI.Initialize(hexCell);
        }
    }

    public void DisableMenu()
    {
        CurrentCell = null;

        _builderMenuContainer.SetActive(false);
        _builderSateliteData.DisableMenu();

        GameEvents.instance.TileDisorder();
    }
}
