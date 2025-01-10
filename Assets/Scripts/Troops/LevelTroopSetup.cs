using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTroopSetup : MonoBehaviour
{
    [SerializeField] LevelTroops m_LevelTroops;
    [SerializeField] GameObject m_TroopUIPrefab;
    [SerializeField] Vector3 m_UIScale;

    public bool _SelectedTroops;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_LevelTroops.TroopsOnShop.Count; i++) 
        {
            GameObject troop = Instantiate(m_TroopUIPrefab);
            troop.transform.parent = transform;
            troop.transform.localScale = m_UIScale;
            troop.transform.Find("TroopImage").GetComponent<Image>().sprite = m_LevelTroops.TroopsOnShop[i]._TroopData.Image;
            troop.gameObject.SetActive(_SelectedTroops);
            troop.GetComponent<TroopUIItem>().InstanceIndex = i;
            troop.GetComponent<TroopUIItem>().m_TroopSlotCost = m_LevelTroops.TroopsOnShop[i]._TroopData.SlotCost;
            if(troop.GetComponent<TroopUIItem>()._SlotCostText != null)
            {
                troop.GetComponent<TroopUIItem>()._SlotCostText.text = m_LevelTroops.TroopsOnShop[i]._TroopData.SlotCost.ToString();
            }
            if(_SelectedTroops == false)
            {
                TroopManager.Instance.troopSelections.Add(troop.GetComponent<TroopUIItem>());
            }
             
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
