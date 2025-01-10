using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TroopManager : MonoBehaviour
{
    public static TroopManager Instance;
    public LevelTroops _LevelTroopData;
    public int _TotalTroopCount = 0;

    [SerializeField] private TextMeshProUGUI m_Totalslot;
    [SerializeField] private TextMeshProUGUI m_slot;

    public List<TroopUIItem> troopSelections = new List<TroopUIItem>();

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
            troopSelections[index]._Count += 1;
            _TotalTroopCount += slotcost;
        }

        UpdateTroopSlot();
        troopSelections[index]._CountText.text = troopSelections[index]._Count.ToString();

        if (troopSelections[index]._Count > 0)
        {
            troopSelections[index].gameObject.SetActive(true);
        }

    }

    public void RemoveTroopCount(int index, int slotcost)
    {
        if (troopSelections[index]._Count >= 0)
        {
            troopSelections[index]._Count -= 1;
            _TotalTroopCount -= slotcost;
        }

        UpdateTroopSlot();
        troopSelections[index]._CountText.text = troopSelections[index]._Count.ToString();

        if (troopSelections[index]._Count == 0)
        {
            troopSelections[index].gameObject.SetActive(false);
        }
    }
}

public enum TroopSlotMethod
{
    Add,
    Remove
}
