using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Main UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject buildingMenuPanel;
    
    [Header("Main Menu Buttons")]
    public Button buildButton;
    public Button infoButton;
    
    [Header("Building Menu Buttons")]
    public Button factoryButton;
    public Button airCleanerButton;
    public Button soilCleanerButton;
    public Button backButton;
    
    [Header("References")]
    public HexSelectionManager hexSelectionManager;
    public BuildingManager buildingManager;
    
    private bool isTileSelected = false;

    void Start()
    {
        // Початковий стан
        ShowMainMenu();
        UpdateButtonStates(false);
        
        // Підписка на події (якщо hexSelectionManager прив'язаний)
        if (hexSelectionManager != null)
        {
            hexSelectionManager.OnTileSelected += HandleTileSelected;
            hexSelectionManager.OnTileDeselected += HandleTileDeselected;
        }
        
        // Прив'язка кнопок
        if (buildButton != null)
            buildButton.onClick.AddListener(OnBuildButtonClicked);
        
        if (infoButton != null)
            infoButton.onClick.AddListener(OnInfoButtonClicked);
        
        if (factoryButton != null)
            factoryButton.onClick.AddListener(() => OnBuildingSelected(BuildingType.Factory));
        
        if (airCleanerButton != null)
            airCleanerButton.onClick.AddListener(() => OnBuildingSelected(BuildingType.AirCleaner));
        
        if (soilCleanerButton != null)
            soilCleanerButton.onClick.AddListener(() => OnBuildingSelected(BuildingType.SoilCleaner));
        
        if (backButton != null)
            backButton.onClick.AddListener(OnBackButtonClicked);
    }

    void ShowMainMenu()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        
        if (buildingMenuPanel != null)
            buildingMenuPanel.SetActive(false);
    }

    void ShowBuildingMenu()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        
        if (buildingMenuPanel != null)
            buildingMenuPanel.SetActive(true);
    }

    void UpdateButtonStates(bool tileSelected)
    {
        isTileSelected = tileSelected;
        
        // Активність кнопок
        if (buildButton != null)
            buildButton.interactable = tileSelected;
        
        if (infoButton != null)
            infoButton.interactable = tileSelected;
        
        // Візуальний стан (сірість)
        if (buildButton != null)
            SetButtonAlpha(buildButton, tileSelected ? 1f : 0.5f);
        
        if (infoButton != null)
            SetButtonAlpha(infoButton, tileSelected ? 1f : 0.5f);
    }

    void SetButtonAlpha(Button button, float alpha)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            Color color = buttonImage.color;
            color.a = alpha;
            buttonImage.color = color;
        }
        
        // Також змінити прозорість тексту
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            Color textColor = buttonText.color;
            textColor.a = alpha;
            buttonText.color = textColor;
        }
    }

    // Обробники подій вибору тайлу
    void HandleTileSelected(GameObject tile)
    {
        UpdateButtonStates(true);
    }

    void HandleTileDeselected()
    {
        UpdateButtonStates(false);
        ShowMainMenu();
    }

    // Обробники кнопок
    void OnBuildButtonClicked()
    {
        if (isTileSelected)
        {
            ShowBuildingMenu();
        }
    }

    void OnInfoButtonClicked()
    {
        if (isTileSelected)
        {
            Debug.Log("Показати інформацію про тайл");
        }
    }

    void OnBuildingSelected(BuildingType buildingType)
    {
        if (buildingManager != null && hexSelectionManager != null)
        {
            GameObject selectedTile = hexSelectionManager.GetSelectedTile();
            if (selectedTile != null)
            {
                //buildingManager.PlaceBuilding(selectedTile, buildingType);
                ShowMainMenu();
            }
        }
    }

    void OnBackButtonClicked()
    {
        ShowMainMenu();
    }

    void OnDestroy()
    {
        // Відписка від подій
        if (hexSelectionManager != null)
        {
            hexSelectionManager.OnTileSelected -= HandleTileSelected;
            hexSelectionManager.OnTileDeselected -= HandleTileDeselected;
        }
    }
}