using System.Collections;
using TMPro;
using UnityEngine;

public class EcologyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalEcologyText = default;

    private int _currentEcology = default;
    private int _previousEcology = default;

    private void Start()
    {
        UpdateEcologyText();
    }

    public void UpdateEcologyText()
    {
        _currentEcology = CalculateTotalEcology();

        if (StepManager.instance.StepCount == 1) //
            _totalEcologyText.text = $"{_currentEcology}%";
        else
            _totalEcologyText.text = $"{_previousEcology}% —> {_currentEcology}%";

        _previousEcology = _currentEcology;

        //if (_currentEcology >= 80)
        //    GameEvents.instance.WinGame();
        if (_currentEcology <= 25)
            StartCoroutine(GameOverCoroutine());
    }

    private int CalculateTotalEcology()
    {
        HexCell[] cells = FindObjectsOfType<HexCell>();

        float avarageSoil = 0;
        float avarageAir = 0;
        float avarageWater = 0;

        foreach (HexCell cell in cells)
        {
            avarageSoil += cell.SoilFertility;
            avarageAir += cell.AirPollution;
            avarageWater += cell.WaterPollution;
        }

        return Mathf.RoundToInt(avarageSoil + avarageAir + avarageWater) / 3 / cells.Length;
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(1);

        GameEvents.instance.GameOver();
    }

    private void Eventss()
    {
        EventBase[] cells = FindObjectsOfType<EventBase>();

    }
}
