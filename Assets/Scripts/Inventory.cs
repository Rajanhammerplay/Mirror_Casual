using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CountText;

    [SerializeField] private MirrorTypes _Mirror_Types;

    [SerializeField] private InventoryMangaer m_InvManager;

    [SerializeField] private MirrorManager m_MirrorManager;


    // Start is called before the first frame update
    void Start()
    {
        CountText.text = _Mirror_Types._MirrorCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //to Pick and reduce inventory count
    public void PickInventory()
    {
        foreach(MirrorTypes mirror in m_InvManager.Mirrors)
        {
            if(mirror._TypeId == _Mirror_Types._TypeId)
            {
                m_MirrorManager.MirrorPicked = mirror._MirrorPrefab;
                return;
            }
        }
    }

}
