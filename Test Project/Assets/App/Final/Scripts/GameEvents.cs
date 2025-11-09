using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    #region Singleton Activation

    [HideInInspector] public static GameEvents instance;

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

    public event Action<HexCell> OnTileClicked = default;
    public void TileClicked(HexCell hexCell) => OnTileClicked?.Invoke(hexCell);

    public event Action<int, int, int> OnBuildingClicked = default;
    public void BuildingClicked(int soil, int air, int water) => OnBuildingClicked?.Invoke(soil, air, water);

    public event Action<Building> OnBuildingClicked2 = default;
    public void BuildingClicked2(Building building) => OnBuildingClicked2?.Invoke(building);

    public event Action OnTileDisorder = default;
    public void TileDisorder() => OnTileDisorder?.Invoke();

    public event Action OnBuildingCreated = default;
    public void BuildingCreated() => OnBuildingCreated?.Invoke();

    public event Action<MonoBehaviour> OnEndStep = default;
    public void EndStep(MonoBehaviour monoBehaviour) => OnEndStep?.Invoke(monoBehaviour);



    public event Action OnGameOver = default;
    public void GameOver() => OnGameOver?.Invoke();


    public event Action OnWinGame = default;
    public void WinGame() => OnWinGame?.Invoke();
}
