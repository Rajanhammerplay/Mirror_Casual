using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//responsible for setup units UI Items ad hadling drop and select
public class LevelUnitSetup : MonoBehaviour
{
    [SerializeField] private LevelTroops m_LevelUnits;
    [SerializeField] private GameObject m_TroopUIPrefab;
    [SerializeField] private Vector3 m_UIScale;
    private int instindex = 0;

    public bool _CanDroopCount;
    public bool _CanAddCount;

    void Start()
    {
        IniTializeUnitsUI();
    }

    //Units setup for Unit UI-Elements
    private void IniTializeUnitsUI()
    {
        foreach (var unititem in UnitsManager.Instance._UnitItemsList)
        {
            GameObject troop = Instantiate(m_TroopUIPrefab);
            troop.transform.parent = transform;
            troop.transform.localScale = m_UIScale;
            troop.transform.Find("TroopImage").GetComponent<Image>().sprite = unititem.Key._UnitData.Image;
            troop.GetComponent<UnitUIItem>()._InstanceIndex = unititem.Key._UnitData._InstanceIndex = instindex;
            troop.GetComponent<UnitUIItem>().m_UnitSlotCost = unititem.Key._UnitData.SlotCost;
            troop.GetComponent<UnitUIItem>()._Count = unititem.Value;
            troop.GetComponent<UnitUIItem>()._TroopType = unititem.Key._UnitData.Type;
            troop.GetComponent<UnitUIItem>()._UnitItem = unititem.Key;
            troop.GetComponent<RectTransform>().position = new Vector3(troop.GetComponent<RectTransform>().position.x, troop.GetComponent<RectTransform>().position.y,0f);
            if (troop.GetComponent<UnitUIItem>()._SlotCostText != null)
            {
                troop.GetComponent<UnitUIItem>()._SlotCostText.text = unititem.Key._UnitData.SlotCost.ToString();
            }
            if (troop.GetComponent<UnitUIItem>()._CountText != null)
            {
                troop.GetComponent<UnitUIItem>()._CountText.text = unititem.Value.ToString();
            }
            instindex++;
        }
    }

    
    

}
