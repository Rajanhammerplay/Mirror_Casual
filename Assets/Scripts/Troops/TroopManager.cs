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

    private void Awake()
    {
        Instance = this;
        UpdateTroopSlot();
    }

    public void UpdateTroopSlot()
    {
        m_Totalslot.text = _LevelTroopData.maxTroopsSlot.ToString();
        m_slot.text = _TotalTroopCount.ToString();
    }    
}
