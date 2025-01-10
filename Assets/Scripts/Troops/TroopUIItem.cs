using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TroopUIItem : MonoBehaviour
{

    public TextMeshProUGUI _CountText;

    public TextMeshProUGUI _SlotCostText;

    public int InstanceIndex;

    public int _Count;

    public int m_TroopSlotCost;


    public void TriggerAddTroopCount()
    {
        EventActions._SelectTroop.Invoke(InstanceIndex,m_TroopSlotCost);
    }

    public void TriggerRemoveTroopCount()
    {
        EventActions._DropTroop.Invoke(InstanceIndex, m_TroopSlotCost);
    }
}


