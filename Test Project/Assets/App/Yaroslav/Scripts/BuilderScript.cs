using UnityEngine;

public class BuilderScript : MonoBehaviour
{
    [SerializeField] private BuilderMenu _builderMenuContainer = default;

    public HexCell CurrentCell = default;

    private void OnEnable()
    {
        GameEvents.instance.OnTileClicked += OpenBuilderMenu;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnTileClicked -= OpenBuilderMenu;
    }

    private void OpenBuilderMenu(HexCell hexCell)
    {
        CurrentCell = hexCell;

        _builderMenuContainer.EnableMenu(CurrentCell);
    }
}
