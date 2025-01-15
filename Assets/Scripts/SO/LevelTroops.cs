using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "LevelTroop", menuName = "ScriptableObject/MirrorCasual/LevelTroop")]
public class LevelTroops : ScriptableObject
{
    [Header("TroopShopSetup")]
    public List<TroopCard> TroopsOnShop;
    public int maxTroopsSlot;

    [Header("DefaultUnitSetup")]
    public List<TroopCard> DefaultUnits;
    private int TotalUnitsCount = 0;


    public bool CheckTroopSlotExceeds()
    {
        TotalUnitsCount = 0;

        for (int i = 0; i < DefaultUnits.Count; i++)
        {
            if (DefaultUnits[i] != null)
            {
                TotalUnitsCount += DefaultUnits[i]._TroopData.SlotCost;
            }

        }
        return TotalUnitsCount > 0 && TotalUnitsCount > maxTroopsSlot;
    }
    private void OnValidate()
    {
        // Ensure maxTroopsSlot is never negative
        if (maxTroopsSlot < 0) maxTroopsSlot = 0;

        DefaultUnits.RemoveAll(item => item == null);

        //if(TotalUnitsCount == 0)
        //{
        //    Debug.LogError("Default Unit Item should include Troop");
        //}

        Debug.Log("check troop slot: "+CheckTroopSlotExceeds()+"total units count: "+TotalUnitsCount);

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


