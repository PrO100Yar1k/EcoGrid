using TMPro;
using UnityEngine;

public class SateliteData : MonoBehaviour
{
    [SerializeField] private GameObject _sateliteContainer = default;

    [SerializeField] private TextMeshProUGUI _soilDataText = default;
    [SerializeField] private TextMeshProUGUI _airDataText = default;
    [SerializeField] private TextMeshProUGUI _waterDataText = default;

    private void OnEnable()
    {
        GameEvents.instance.OnTileClicked += EnableMenu;
        GameEvents.instance.OnEndStep += ResolveConflict;
        GameEvents.instance.OnBuildingClicked += BuildingMenuOptions;
    }

    private void OnDestroy()
    {
        GameEvents.instance.OnTileClicked -= EnableMenu;
        GameEvents.instance.OnEndStep -= ResolveConflict;
        GameEvents.instance.OnBuildingClicked -= BuildingMenuOptions;
    }

    public void EnableMenu(HexCell hexCell)
    {
        _sateliteContainer.SetActive(true);
        AssignTextValue(hexCell.SoilFertility, hexCell.AirPollution, hexCell.WaterPollution);
    }

    public void DisableMenu()
    {
        _sateliteContainer.SetActive(false);
    }

    public void AssignTextValue(int soilValue, int airValue, int waterValue) // in %
    {
        _soilDataText.text = $"{soilValue}%";
        _airDataText.text = $"{airValue}%";
        _waterDataText.text = $"{waterValue}%";
    }

    private void BuildingMenuOptions(int soilValue, int airValue, int waterValue)
    {
        _soilDataText.text = Mathf.Sign(soilValue) > 0 ? $"<color=#00FF00>{soilValue}%</color>" : $"<color=#AB0101>{soilValue}%</color>";
        _airDataText.text = Mathf.Sign(airValue) > 0 ? $"<color=#00FF00>{airValue}%</color>" : $"<color=#AB0101>{airValue}%</color>";
        _waterDataText.text = Mathf.Sign(waterValue) > 0 ? $"<color=#00FF00>{waterValue}%</color>" : $"<color=#AB0101>{waterValue}%</color>";

        _sateliteContainer.SetActive(true);
    }

    private void ResolveConflict(MonoBehaviour monoBehaviour)
    {
        Building building = monoBehaviour as Building;
        HexCell hexcell = monoBehaviour as HexCell;

        if (building != null)
            BuildingMenuOptions(building.soilChangeValue, building.airChangeValue, building.waterChangeValue);
        else if (hexcell != null)
            AssignTextValue(hexcell.SoilFertility, hexcell.AirPollution, hexcell.WaterPollution);
    }
}
