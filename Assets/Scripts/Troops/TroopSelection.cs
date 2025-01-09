using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TroopSelection : MonoBehaviour
{

    public TextMeshProUGUI _CountText;

    public int InstanceIndex;

    public int _Count;

    public int m_TroopSlotCost;


    public void TriggerUpdateTroopCount(bool add)
    {
        EventActions._SelectTroopIndex.Invoke(InstanceIndex,add,m_TroopSlotCost);
    }
}


