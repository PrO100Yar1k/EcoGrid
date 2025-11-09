using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MoneyController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText = default;

    private int _money = 15000;

    #region Singleton Activation

    [HideInInspector] public static MoneyController instance;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    #endregion

    private void Start()
    {
        UpdateMoneyCount();
    }

    public void ChangeMoneyValue(int value)
    {
        _money += value;
        UpdateMoneyCount();
    }

    public int GetMoney()
        => _money;

    private void UpdateMoneyCount()
    {
        if (_money < 0)
        {
            _money = 0;
            StartCoroutine(GameOverCoroutine());
        }
        _moneyText.text = _money == 0 ? "0" : String.Format("{0:### ### ###}", _money);
    }


    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(1);

        GameEvents.instance.GameOver();
    }
}
