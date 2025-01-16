using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitsManager : MonoBehaviour
{
    public static UnitsManager Instance;
    public LevelTroops _LevelTroopData;
    public int _TotalTroopCount = 0;

    [SerializeField] private TextMeshProUGUI m_Totalslot;
    [SerializeField] private TextMeshProUGUI m_slot;

    public List<TroopUIItem> UnitsSelected = new List<TroopUIItem>();
    public List<TroopUIItem> UnitsDefault = new List<TroopUIItem>();

    private void Awake()
    {
        Instance = this;

        EventActions._SelectTroop += AddTroopCount;
        EventActions._DropTroop += RemoveTroopCount;

        UpdateTroopSlot();
    }

    public void UpdateTroopSlot()
    {
        m_Totalslot.text = _LevelTroopData.maxTroopsSlot.ToString();
        m_slot.text = _TotalTroopCount.ToString();
    }

    public void AddTroopCount(int index, int slotcost)
    {
        if ((_TotalTroopCount < _LevelTroopData.maxTroopsSlot) && ((_TotalTroopCount + slotcost) <= _LevelTroopData.maxTroopsSlot))
        {
            UnitsSelected[index]._Count += 1;
            UnitsDefault[index]._Count += 1;

            _TotalTroopCount += slotcost;
            UpdateTroopSlot();

            UnitsSelected[index]._CountText.text = UnitsSelected[index]._Count.ToString();
            UnitsDefault[index]._CountText.text = UnitsDefault[index]._Count.ToString();

            if (UnitsSelected[index]._Count > 0)
            {
                UnitsSelected[index].gameObject.SetActive(true);
            }

            if (UnitsDefault[index]._Count > 0)
            {
                UnitsDefault[index].gameObject.SetActive(true);
            }
        }


    }

    public void RemoveTroopCount(int index, int slotcost)
    {
        if (UnitsSelected[index]._Count > 0)
        {
            UnitsSelected[index]._Count -= 1;
            //_TotalTroopCount -= slotcost;
        }

        if (UnitsDefault[index]._Count > 0)
        {
            UnitsDefault[index]._Count -= 1;
        }

        _TotalTroopCount -= slotcost;


        UpdateTroopSlot();
        UnitsSelected[index]._CountText.text = UnitsSelected[index]._Count.ToString();
        UnitsDefault[index]._CountText.text = UnitsDefault[index]._Count.ToString();

        if (UnitsSelected[index]._Count == 0)
        {
            UnitsSelected[index].gameObject.SetActive(false);
        }

        if (UnitsDefault[index]._Count == 0)
        {
            UnitsDefault[index].gameObject.SetActive(false);
        }
    }

    public void UpdateUnitsCount(int instanceindex,int count,bool dropcount)
    {
        foreach(TroopUIItem unitobj in UnitsSelected)
        {
            if(unitobj._InstanceIndex == instanceindex)
            {
                if (dropcount)
                {
                    unitobj._Count -= count;
                }
                else
                {
                    unitobj._Count = count;
                }
                
                unitobj._CountText.text = unitobj._Count.ToString();
            }
        }
    }
}

public enum TroopSlotMethod
{
    Add,
    Remove
}
