using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StepManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stepCountText = default;
    [SerializeField] private EcologyManager ecologyManager = default;
    [SerializeField] private BuildingController buildingController = default;

    [SerializeField] private GameObject background = default;
    [SerializeField] private Button finishStep = default;

    public int StepCount { get; private set; } = 0;

    #region Singleton Activation

    [HideInInspector] public static StepManager instance;

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

    private void OnEnable()
    {
        GameEvents.instance.OnEndStep += EnableButton;

    }

    private void OnDisable()
    {
        GameEvents.instance.OnEndStep -= EnableButton;
    }

    private void Start()
    {
        finishStep.onClick.AddListener(delegate { EndStep(); });
        UpdateStepCountText();
    }

    public void EndStep()
    {
        Cycle();
    }

    private void Cycle()
    {
        DisableButton();

        CheckAllBuildings();
        UpdateStepCountText();

        buildingController.Call();
        ecologyManager.UpdateEcologyText();

        CheckWinOrDefeat();
    }

    private void UpdateStepCountText()
    {
        StepCount += 1;

        stepCountText.text = $"STEP: {StepCount}";
    }

    private void CheckAllBuildings()
    {
        Building[] buildings = FindObjectsOfType<Building>();

        int revenue = 0;

        foreach (Building building in buildings)
        {
            building.AffectOnNeighbourTile();
            revenue += building.MoneyProfit;
        }

        MoneyController.instance.ChangeMoneyValue(revenue);
    }

    private void EnableButton(MonoBehaviour monoBehaviour)
    {
        background.SetActive(false);
        finishStep.interactable = true;
    }

    private void DisableButton()
    {
        background.SetActive(true);
        finishStep.interactable = false;
    }

    private void CheckWinOrDefeat()
    {

    }
}
