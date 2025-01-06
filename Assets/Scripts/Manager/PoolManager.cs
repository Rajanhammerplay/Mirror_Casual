using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject m_ParentObject;
    [SerializeField] private InventoryData m_InvData;

    private List<GameObject> m_MirrorQueue = new List<GameObject>();
    List<MirrorTypes> mirrorlist = new List<MirrorTypes>();
    List<GameObject> m_MirrorTempList = new List<GameObject>();

    void Start()
    {

       IntializePool();
    }

    private void IntializePool()
    {

        foreach (var mirrortypes in m_InvData.MirrorInventoryList)
        {
            mirrorlist.Add(mirrortypes.value);
        }

        foreach (var mirrortype in mirrorlist) 
        {
            for(int i = 0;i< mirrortype._MirrorCount; i++)
            {
                GameObject mirror = Instantiate(mirrortype._MirrorPrefab, m_ParentObject.transform);
                mirror.transform.name = mirrortype._MirrorType + "_" + i;
                m_MirrorQueue.Add(mirror);
                m_MirrorTempList.Add(mirror);
            }
        }
    }
    public void SelectMirror(GameObject mirror)
    {
        foreach (var mirrorobj in m_MirrorTempList)
        {
            if(mirrorobj.transform.name == mirror.name)
            {
                mirrorobj.GetComponent<Mirror>()._IsSelected = true;
            }
            else
            {
                mirrorobj.GetComponent<Mirror>()._IsSelected = false;
            }
        }
    }
    public GameObject GetSpawnnableObject(MirrorVariation mirrortype)
    {
        GameObject mirror = null;
        foreach (var mirrorinv in m_MirrorQueue)
        {
            if (mirrorinv.GetComponent<Mirror>()._MirrorType == mirrortype) 
            { 
                 mirror = mirrorinv;
            }
        }
        return mirror;
    }
    public void DequeuePool(Mirror mirror)
    {
        m_MirrorQueue.Remove(mirror.gameObject);
    }

}
