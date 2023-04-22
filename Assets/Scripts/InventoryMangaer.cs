using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMangaer : MonoBehaviour
{
    public List<MirrorTypes> Mirrors;

    [SerializeField] GameObject UI_Parent;
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
}
