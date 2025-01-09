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
    private List<TroopSelection> troopSelections = new List<TroopSelection>();
    public bool _SelectedTroops;
    

    // Start is called before the first frame update
    void Start()
    {
        if (!_SelectedTroops)
        {
            EventActions._SelectTroopIndex += UpdateTroopCount;
        }
        for (int i = 0; i < m_LevelTroops.TroopsOnShop.Count; i++) 
        {
            GameObject troop = Instantiate(m_TroopUIPrefab);
            troop.transform.parent = transform;
            troop.transform.localScale = m_UIScale;
            troop.transform.Find("TroopImage").GetComponent<Image>().sprite = m_LevelTroops.TroopsOnShop[i]._TroopData.Image;
            troop.gameObject.SetActive(_SelectedTroops);
            troop.GetComponent<TroopSelection>().InstanceIndex = i;
            troop.GetComponent<TroopSelection>().m_TroopSlotCost = m_LevelTroops.TroopsOnShop[i]._TroopData.SlotCost;
            if (!_SelectedTroops)
            {
                troopSelections.Add(troop.GetComponent<TroopSelection>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTroopCount(int index,bool add,int slotcost)
    {

        if (add && (TroopManager.Instance._TotalTroopCount < TroopManager.Instance._LevelTroopData.maxTroopsSlot) && ((TroopManager.Instance._TotalTroopCount + slotcost) <= TroopManager.Instance._LevelTroopData.maxTroopsSlot))
        {
            troopSelections[index]._Count += 1;
            TroopManager.Instance._TotalTroopCount += slotcost;
        }
        else
        {
            if (troopSelections[index]._Count >= 0)
            {
                troopSelections[index]._Count -= 1;
                TroopManager.Instance._TotalTroopCount -= slotcost;
            }
        }

        TroopManager.Instance.UpdateTroopSlot();
        
        
        troopSelections[index]._CountText.text = troopSelections[index]._Count.ToString();

        if(troopSelections[index]._Count > 0)
        {
            troopSelections[index].gameObject.SetActive(true);
        }
        else
        {
            troopSelections[index].gameObject.SetActive(false);
        }
    }

}
