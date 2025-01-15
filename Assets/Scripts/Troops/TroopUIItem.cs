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

    public TroopType _TroopType;

    public bool _TroopSelected;

    private void Start()
    {
        EventActions._DropUnitOnGround += DropUnits;
    }

    public void TriggerAddTroopCount()
    {
        EventActions._SelectTroop.Invoke(InstanceIndex,m_TroopSlotCost);
    }

    public void TriggerRemoveTroopCount()
    {
        EventActions._DropTroop.Invoke(InstanceIndex, m_TroopSlotCost);
    }

    public void TriggerSelectUnit()
    {
        EventActions._SelectedUnitType = _TroopType;
        //EventActions._SelectUnitFromPool.Invoke(this.gameObject);
    }


    public void DropUnits(int instindex)
    {
        if(instindex == InstanceIndex)
        {
            if (_Count > 0)
            {
                _Count -= 1;
                _CountText.text = _Count.ToString();
            }
        }
        
    }

  
}


