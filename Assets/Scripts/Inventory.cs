using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CountText;

    [SerializeField] private MirrorTypes m_Mirror;

    [SerializeField] private InventoryMangaer m_InvManager;

    [SerializeField] private MirrorManager m_MirrorManager;



    // Start is called before the first frame update
    void Start()
    {
        if(CountText!=null && m_Mirror != null)
        {
            CountText.text = m_Mirror._MirrorCount.ToString();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //to Pick and reduce inventory count
    public void TapOnInventory()
    {
        print("tapping");
        //foreach(MirrorTypes mirror in m_InvManager.Mirrors)
        //{
        //    if(mirror._TypeId == _Mirror_Types._TypeId)
        //    {
        //        m_MirrorManager.MirrorPicked = mirror._MirrorPrefab;
        //        return;
        //    }
        //}

            m_InvManager.Mirror = m_Mirror;
            m_InvManager.PickInventory();
        
    }

}
