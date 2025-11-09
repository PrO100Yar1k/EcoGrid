using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public List<HexCell> NeighborTileList = new List<HexCell>();

    [SerializeField] private TileType _tileType = default;

    [Range(0, 100)] public int SoilFertility = default;
    [Range(0, 100)] public int AirPollution = default;
    [Range(0, 100)] public int WaterPollution = default;

    [SerializeField] private float _height = default;
    [SerializeField] private Building _currentBuilding = default;
    private MeshRenderer _meshRenderer = default;

    public bool IsCanBuild = true;

    private void OnEnable()
    {
        GameEvents.instance.OnTileDisorder += DisableEffect;
        InitializeCell();
    }

    private void OnDisable()
    {
        GameEvents.instance.OnTileDisorder -= DisableEffect;
    }

    public void InitializeCell()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        UpdateView();
    }

    private void OnMouseUp()
    {
        GameEvents.instance.TileClicked(this);
        EnableEffect();

        if (PlaySound.Instance != null)
            PlaySound.Instance.PlayClick();
    }

    private void EnableEffect()
        => _meshRenderer.material = Resources.Load<Material>("SelectedTileMaterial");

    private void DisableEffect()
    {
        UpdateView();
    }

    public void ChangeSoil(int change)
    {
        SoilFertility += change;
        SoilFertility = Mathf.Clamp(SoilFertility, 0, 100);

        UpdateView();
    }

    public void ChangeAir(int change)
    {
        AirPollution += change;
        AirPollution = Mathf.Clamp(AirPollution, 0, 100);
    }

    public void ChangeWater(int change)
    {
        WaterPollution += change;
        WaterPollution = Mathf.Clamp(WaterPollution, 0, 100);
    }

    private void UpdateView()
    {
        Material targetMaterial = default;

        if (SoilFertility >= 93)
            targetMaterial = Resources.Load<Material>("Textures/Ground 0-7");
        else if (SoilFertility >= 79 && SoilFertility < 93)
            targetMaterial = Resources.Load<Material>("Textures/Ground 8-21");
        else if (SoilFertility >= 64 && SoilFertility < 79)
            targetMaterial = Resources.Load<Material>("Textures/Ground 22-36");
        else if (SoilFertility >= 37 && SoilFertility < 64)
            targetMaterial = Resources.Load<Material>("Textures/Ground 37-63");
        else if (SoilFertility >= 22 && SoilFertility < 37)
            targetMaterial = Resources.Load<Material>("Textures/Ground 64-78");
        else if (SoilFertility >= 8 && SoilFertility < 22)
            targetMaterial = Resources.Load<Material>("Textures/Ground 79-92");
        else if (SoilFertility < 8)
            targetMaterial = Resources.Load<Material>("Textures/Ground 93-100");

        _meshRenderer.material = targetMaterial;
    }
}

public enum TileType
{
    Grass = 0,
    Water = 1,
    Forest = 2
}