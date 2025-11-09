using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    private Building currentBuilding;

    public BuilderMenu builderMenu;

    private void OnEnable()
    {
        GameEvents.instance.OnBuildingClicked2 += SetupActiveCell;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnBuildingClicked2 -= SetupActiveCell;
    }

    public void Call()
        => StartCoroutine(AllEnable());

    private IEnumerator AllEnable()
    {
        Building[] buildings = FindObjectsOfType<Building>();

        List<HexCell> negative = new List<HexCell>();
        List<HexCell> positive = new List<HexCell>();

        Material targetMaterial = default;

        foreach (Building building in buildings)
        {
            if (building.isNegativeTile())
                negative.AddRange(building.GetNeighbourTile());
            else positive.AddRange(building.GetNeighbourTile());
        }

        if (negative.Count > 0)
        {
            targetMaterial = Resources.Load<Material>("DamagedMaterial");
            yield return StartCoroutine(DamageEffect(negative.ToArray(), targetMaterial));
        }

        if (positive.Count > 0 && negative.Count > 0)
            yield return new WaitForSeconds(1);

        if (positive.Count > 0)
        {
            targetMaterial = Resources.Load<Material>("HealMaterial");
            yield return StartCoroutine(DamageEffect(positive.ToArray(), targetMaterial));
        }

        if (builderMenu.CurrentCell != null) //
            GameEvents.instance.EndStep(builderMenu.CurrentCell); //
        else GameEvents.instance.EndStep(currentBuilding); // 
    }

    private IEnumerator DamageEffect(HexCell[] neighbors, Material targetMaterial)
    {
        List<MeshRenderer> renderers = new List<MeshRenderer>();
        List<Material> defaultMaterials = new List<Material>();

        for (int i = 0; i < neighbors.Length; i++)
        {
            MeshRenderer neighborsRenderer = neighbors[i].GetComponent<MeshRenderer>();

            renderers.Add(neighborsRenderer);
            defaultMaterials.Add(renderers[i].material);
        }

        for (int flash = 0; flash < 3; flash++)
        {
            foreach (MeshRenderer renderer in renderers)
            {
                renderer.material = targetMaterial;
            }

            yield return new WaitForSeconds(0.3f);

            for (int i = 0; i < renderers.Count; i++)
            {
                renderers[i].material = defaultMaterials[i];
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    private void SetupActiveCell(Building building)
    {
        currentBuilding = building;
    }
}
