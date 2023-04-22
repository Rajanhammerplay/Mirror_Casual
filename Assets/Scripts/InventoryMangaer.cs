using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMangaer : MonoBehaviour
{
    [SerializeField] List<GameObject> Mirrors;

    [SerializeField] GameObject UI_Parent;
    // Start is called before the first frame update
    void Start()
    {
        ShowInvItems();
    }

    public void ShowInvItems()
    {
        foreach(GameObject mirror in Mirrors)
        {
            Instantiate(mirror, UI_Parent.transform);
        }
    }
}
