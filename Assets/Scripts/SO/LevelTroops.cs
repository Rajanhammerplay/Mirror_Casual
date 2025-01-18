using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "LevelTroop", menuName = "ScriptableObject/MirrorCasual/LevelTroop")]
public class LevelTroops : ScriptableObject
{
    [Header("TroopShopSetup")]
    public List<UnitItem> TroopsOnShop;
    public int maxTroopsSlot;

    [Header("DefaultUnitSetup")]
    public List<UnitItem> DefaultUnits;
    private int TotalUnitsCount = 0;

    public bool CheckTroopSlotExceeds()
    {
        TotalUnitsCount = 0;

        for (int i = 0; i < DefaultUnits.Count; i++)
        {
            if (DefaultUnits[i] != null)
            {
                TotalUnitsCount += DefaultUnits[i]._UnitData.SlotCost;
            }

        }
        return TotalUnitsCount > 0 && TotalUnitsCount > maxTroopsSlot;
    }
    private void OnValidate()
    {
        // Ensure maxTroopsSlot is never negative
        if (maxTroopsSlot < 0) maxTroopsSlot = 0;

        DefaultUnits.RemoveAll(item => item == null);

        if (CheckTroopSlotExceeds())
        {
            Debug.LogWarning("Cannot add more units - exceeds maximum troop slot cost.");
            // Remove units until we're under the limit
            while (CheckTroopSlotExceeds())
            {
                DefaultUnits.RemoveAt(DefaultUnits.Count - 1);
            }
        }
    }
    
}


