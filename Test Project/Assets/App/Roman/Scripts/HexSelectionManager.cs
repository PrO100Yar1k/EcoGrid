using UnityEngine;

public class HexSelectionManager : MonoBehaviour
{
    // Події для UIManager
    public delegate void TileSelectedHandler(GameObject tile);
    public event TileSelectedHandler OnTileSelected;
    public event System.Action OnTileDeselected;
    
    // Налаштування
    public LayerMask hexLayer;
    public Material highlightMaterial;
    
    private GameObject selectedTile;
    private Material originalMaterial;

    void Update()
    {
        HandleMouseInput();
        HandleKeyboardInput();
    }

    void HandleMouseInput()
    {
        // Клік лівою кнопкою миші
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 1000f, hexLayer))
            {
                SelectTile(hit.collider.gameObject);
            }
            else
            {
                DeselectTile();
            }
        }
    }

    void HandleKeyboardInput()
    {
        // ESC для скасування вибору
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeselectTile();
        }
    }

    void SelectTile(GameObject tile)
    {
        // Якщо вже щось вибрано - зняти виділення
        if (selectedTile != null)
        {
            UnhighlightTile(selectedTile);
        }
        
        // Вибрати новий тайл
        selectedTile = tile;
        HighlightTile(selectedTile);
        
        // Повідомити UIManager
        OnTileSelected?.Invoke(selectedTile);
        
        Debug.Log($"Вибрано тайл: {selectedTile.name}");
    }

    void DeselectTile()
    {
        if (selectedTile != null)
        {
            UnhighlightTile(selectedTile);
            selectedTile = null;
            
            // Повідомити UIManager
            OnTileDeselected?.Invoke();
            
            Debug.Log("Тайл знято з вибору");
        }
    }

    void HighlightTile(GameObject tile)
    {
        Renderer renderer = tile.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Зберегти оригінальний матеріал
            originalMaterial = renderer.material;
            
            // Застосувати виділення
            if (highlightMaterial != null)
            {
                renderer.material = highlightMaterial;
            }
            else
            {
                // Якщо немає матеріалу - просто змінити колір
                renderer.material.color = Color.yellow;
            }
        }
    }

    void UnhighlightTile(GameObject tile)
    {
        Renderer renderer = tile.GetComponent<Renderer>();
        if (renderer != null && originalMaterial != null)
        {
            renderer.material = originalMaterial;
        }
    }

    // Метод для UIManager щоб отримати вибраний тайл
    public GameObject GetSelectedTile()
    {
        return selectedTile;
    }

    // Метод для перевірки чи щось вибрано
    public bool IsTileSelected()
    {
        return selectedTile != null;
    }
}