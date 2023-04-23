using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryMangaer : MonoBehaviour
{
    public List<MirrorTypes> Mirrors;

    [SerializeField] GameObject UI_Parent;

    public MirrorTypes Mirror;


    public event EventHandler<InventoryArgs> m_InvArgs;
    // Start is called before the first frame update
    void Start()
    {
        ShowInvItems();
    }

    public void ShowInvItems()
    {
        foreach(MirrorTypes mirror in Mirrors)
        {
            Instantiate(mirror._UIMirrorPrefab, UI_Parent.transform);
        }
    }

    //while pick on Inventory
    public void PickInventory()
    {
        if (Mirror != null && m_InvArgs!=null)
        {
            m_InvArgs(this, new InventoryArgs(Mirror));
        }
    }
}
