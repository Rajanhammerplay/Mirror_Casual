using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CountText;

    public MirrorTypes _MirrorTypeData;

    private int m_Count;

    // Start is called before the first frame update
    void Start()
    {
        
        if(CountText!=null && _MirrorTypeData != null)
        {
            m_Count = _MirrorTypeData._MirrorCount;
            CountText.text = m_Count.ToString();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //to Pick and reduce inventory count
    public void TapOnInventory()
    {
        EventActions._SelectInv.Invoke(_MirrorTypeData._MirrorType);
        print("slected inv: "+EventActions._SelectedInvType);
    }

    public void UpdateInvCount()
    {
        if (m_Count >= 1)
        {
            m_Count -= 1;
            CountText.text = m_Count.ToString();
        }
        else 
        {
            CountText.transform.gameObject.SetActive(false);
        }
    }

}
