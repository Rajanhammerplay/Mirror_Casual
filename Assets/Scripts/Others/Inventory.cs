using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CountText;

    public MirrorTypes _MirrorTypeData;

    // Start is called before the first frame update
    void Start()
    {
        
        if(CountText!=null && _MirrorTypeData != null)
        {
            //TroopManager.Instance._SelectedTroopCount = _MirrorTypeData._MirrorCount;
            //CountText.text = TroopManager.Instance._SelectedTroopCount.ToString();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //to Pick and reduce inventory count
    public void TapOnInventory()
    {
        //EventActions._SelectInv.Invoke(_MirrorTypeData._MirrorType);
        print("slected inv: "+EventActions._SelectedUnitType);
    }

    public void UpdateInvCount()
    {
        //if (TroopManager.Instance._SelectedTroopCount >= 1)
        //{
        //    //TroopManager.Instance._SelectedTroopCount -= 1;
        //    //CountText.text = TroopManager.Instance._SelectedTroopCount.ToString();
        //}
        //else 
        //{
        //    CountText.transform.gameObject.SetActive(false);
        //}
    }

}
