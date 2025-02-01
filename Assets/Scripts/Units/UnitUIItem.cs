using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//responsible for showing count of unit items and drop and add 
public class UnitUIItem : MonoBehaviour
{

    public TextMeshProUGUI _CountText;

    public TextMeshProUGUI _SlotCostText;

    public int _InstanceIndex;

    public int _Count;

    public int m_UnitSlotCost;

    public TroopType _TroopType;

    public bool _TroopSelected;

    public UnitItem _UnitItem;

    private UnitUIItem m_CurrentInstance;

    private void Update()
    {
        if(EventActions._SelectedUnitType != TroopType.Mirror)
        {
            UnitsManager.Instance.HighlightMirrorTiles(false);
        }
        m_CurrentInstance = GetComponent<UnitUIItem>();
    }
    public void TriggerAddUnitsCount()
    {
        UnitsManager.Instance.AddUnit(m_CurrentInstance._TroopType);
    }
    public void TriggerRemoveUnitsCount()
    {
        UnitsManager.Instance.DropUnit(m_CurrentInstance._TroopType);
    }

    //to update units count once after drop and add units
    public void UpdateUnitItemCount(bool drop)
    {
        if (UnitsManager.Instance._UnitItemsTempList.Count > 0) 
        { 
            foreach(UnitDetails item in UnitsManager.Instance._UnitItemsTempList)
            {
                if(item.UnitsData._InstanceIndex == _InstanceIndex)
                {
                    if (!drop)
                    {
                        if ((UnitsManager.Instance._TotalTroopCount + m_CurrentInstance.m_UnitSlotCost) <= UnitsManager.Instance._LevelTroopData.maxTroopsSlot)
                        {

                            m_CurrentInstance._Count += 1;
                            if (m_CurrentInstance._CountText)
                            {
                                m_CurrentInstance._CountText.text = m_CurrentInstance._Count.ToString();
                            }
                            if (m_CurrentInstance._Count > 0)
                            {
                                m_CurrentInstance.gameObject.SetActive(true);
                            }
                        }
                    }
                    else
                    {
                        if (m_CurrentInstance._Count > 0)
                        {
                            m_CurrentInstance._Count -= 1;
                        }
                        if (m_CurrentInstance._CountText)
                        {
                            m_CurrentInstance._CountText.text = m_CurrentInstance._Count.ToString();
                        }
                        if (m_CurrentInstance._Count == 0)
                        {
                            m_CurrentInstance.gameObject.SetActive(false);
                            EventActions._SelectedUnitType = TroopType.none;
                        }

                    }

                }
            }
        }
    }

   


    public void TriggerSelectUnit()
    {
        EventActions._SelectedUnitType = _TroopType;
        if(EventActions._SelectedUnitType == TroopType.Mirror)
        {
            UnitsManager.Instance.HighlightMirrorTiles(true);
        }
        else
        {
            UnitsManager.Instance.HighlightMirrorTiles(false);
        }
    }

  
}


