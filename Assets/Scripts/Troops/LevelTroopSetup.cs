using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTroopSetup : MonoBehaviour
{
    [SerializeField] LevelTroops m_LevelTroops;
    [SerializeField] GameObject m_TroopUIPrefab;
    [SerializeField] Vector3 m_UIScale;

    public bool _SelectedTroopsSetup;
    public bool _DefaultUnitSetup;

    // Start is called before the first frame update
    void Awake()
    {

        if (m_LevelTroops.DefaultUnits.Count > 0 && _DefaultUnitSetup)
        {
            TroopSetup();
        }
        else
        {
            TroopShopSetup();
        }
    }

    private void TroopShopSetup()
    {
        for (int i = 0; i < m_LevelTroops.TroopsOnShop.Count; i++)
        {
            GameObject troop = Instantiate(m_TroopUIPrefab);
            troop.transform.parent = transform;
            troop.transform.localScale = m_UIScale;
            troop.transform.Find("TroopImage").GetComponent<Image>().sprite = m_LevelTroops.TroopsOnShop[i]._TroopData.Image;
            troop.gameObject.SetActive(true);
            troop.GetComponent<TroopUIItem>()._InstanceIndex = i;
            troop.GetComponent<TroopUIItem>().m_UnitSlotCost = m_LevelTroops.TroopsOnShop[i]._TroopData.SlotCost;
            if (troop.GetComponent<TroopUIItem>()._SlotCostText != null)
            {
                troop.GetComponent<TroopUIItem>()._SlotCostText.text = m_LevelTroops.TroopsOnShop[i]._TroopData.SlotCost.ToString();
            }
            if (_SelectedTroopsSetup == false)
            {
                UnitsManager.Instance.UnitsSelected.Add(troop.GetComponent<TroopUIItem>());
            }
        }
    }

    private void TroopSetup()
    {
        var ListOfDistinctUnit = m_LevelTroops.DefaultUnits.GroupBy(n=>n).Select(group => new { Unit = group.Key, Count = group.Count() }).ToList();
        for (int i = 0; i < ListOfDistinctUnit.Count; i++)
        {
            GameObject troop = Instantiate(m_TroopUIPrefab);
            troop.transform.parent = transform;
            troop.transform.localScale = m_UIScale;
            troop.transform.Find("TroopImage").GetComponent<Image>().sprite = ListOfDistinctUnit[i].Unit._TroopData.Image;
            troop.gameObject.SetActive(_SelectedTroopsSetup);
            troop.GetComponent<TroopUIItem>()._InstanceIndex = i;
            troop.GetComponent<TroopUIItem>().m_UnitSlotCost = ListOfDistinctUnit[i].Unit._TroopData.SlotCost;
            troop.GetComponent<TroopUIItem>()._Count = ListOfDistinctUnit[i].Count;
            troop.GetComponent<TroopUIItem>()._TroopType = ListOfDistinctUnit[i].Unit._TroopData.Type;
            if (troop.GetComponent<TroopUIItem>()._CountText != null)
            {
                troop.GetComponent<TroopUIItem>()._CountText.text = ListOfDistinctUnit[i].Count.ToString();
            }
            UnitsManager.Instance.UnitsDefault.Add(troop.GetComponent<TroopUIItem>());
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
