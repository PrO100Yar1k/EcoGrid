using UnityEngine;

public class DroughtEvent : EventBase
{
    public override void CompleteWithChance()
    {
        int randomChance = Random.Range(0, 100);

        if (randomChance < 30)
        {
            HexCell[] cells = FindObjectsOfType<HexCell>();

            HexCell randomCell = cells[Random.Range(0, cells.Length)];

            // to do
        }
    }
}
