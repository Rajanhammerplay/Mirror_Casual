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

    public int _InstanceIndex;

    public int _Count;

    public int m_UnitSlotCost;

    public TroopType _TroopType;

    public bool _TroopSelected;

    private void Start()
    {
        EventActions._DropUnitOnGround += DropUnits;
    }

    public void TriggerAddUnitsCount()
    {
        EventActions._SelectTroop.Invoke(_InstanceIndex,m_UnitSlotCost);
    }

    public void TriggerRemoveUnitsCount()
    {
        EventActions._DropTroop.Invoke(_InstanceIndex, m_UnitSlotCost);
    }

    public void TriggerSelectUnit()
    {
        EventActions._SelectedUnitType = _TroopType;
        //EventActions._SelectUnitFromPool.Invoke(this.gameObject);
    }


    public void DropUnits(int instindex)
    {
        if (_TroopSelected) { return; }

        if(instindex == _InstanceIndex)
        {
            EventActions._DropTroop.Invoke(_InstanceIndex, m_UnitSlotCost);
        }
    }

  
}


