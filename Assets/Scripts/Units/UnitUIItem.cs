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


    public void TriggerAddUnitsCount()
    {
        UnitsManager.Instance.AddUnit(this._TroopType);
    }
    public void TriggerRemoveUnitsCount()
    {
        UnitsManager.Instance.DropUnit(this._TroopType);
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
                        if ((UnitsManager.Instance._TotalTroopCount + this.m_UnitSlotCost) <= UnitsManager.Instance._LevelTroopData.maxTroopsSlot)
                        {

                            this._Count += 1;
                            if (this._CountText)
                            {
                                this._CountText.text = this._Count.ToString();
                            }
                            if (this._Count > 0)
                            {
                                this.gameObject.SetActive(true);
                            }
                        }
                    }
                    else
                    {
                        if (this._Count > 0)
                        {
                            this._Count -= 1;
                        }
                        if (this._CountText)
                        {
                            this._CountText.text = this._Count.ToString();
                        }
                        if (this._Count == 0)
                        {
                            this.gameObject.SetActive(false);
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


