using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTileUI : MonoBehaviour
{
    [SerializeField] private Image _targetImage = default;
    [SerializeField] private GameObject _targetPrefab = default;
    [SerializeField] private Sprite _targetSprite = default;

    [SerializeField] private TextMeshProUGUI _priceText = default;
    [SerializeField] private int price = default;

    private HexCell _currentTile = default;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(InstantiatePrefab);
    }

    public void Initialize(HexCell hexCell)
    {
        _priceText.text = String.Format("{0:### ### ###}", price);

        _targetImage.sprite = _targetSprite;
        _currentTile = hexCell;
    }

    private void InstantiatePrefab()
    {
        if (_currentTile == null)
        {
            if (PlaySound.Instance != null)
                PlaySound.Instance.PlayError();
            return;
        }

        if (!_currentTile.IsCanBuild)
        {
            if (PlaySound.Instance != null)
                PlaySound.Instance.PlayError();
            return;
        }

        if (MoneyController.instance.GetMoney() < price)
        {
            if (PlaySound.Instance != null)
                PlaySound.Instance.PlayError();
            return;
        }

        Vector3 position = new Vector3(_currentTile.transform.position.x, _currentTile.transform.position.y + 0.15f, _currentTile.transform.position.z);
        GameObject Prefab = Instantiate(_targetPrefab, position, _currentTile.transform.rotation);
        Prefab.GetComponent<Building>()?.Initialize(_currentTile);

        _currentTile.IsCanBuild = false;

        MoneyController.instance.ChangeMoneyValue(-price);
        GameEvents.instance.BuildingCreated();
    }
}
