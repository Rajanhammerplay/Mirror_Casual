using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PoolManager : MonoBehaviour
{
    public static PoolManager _instance;
    public Dictionary<TroopType, Queue<GameObject>> _UnitPoolDict = new Dictionary<TroopType, Queue<GameObject>>();
    public Dictionary<TroopType, Queue<GameObject>> _UnitPoolDictCpy = new Dictionary<TroopType, Queue<GameObject>>();
    private List<UnitPool> m_ListOfUnitPool = new List<UnitPool>();

    void Start()
    {
        LevelUnitPool lvlunitpool = ScriptableObject.Instantiate(LevelManager.instance._LevelUnitPool[0]);
        m_ListOfUnitPool = lvlunitpool._ListOfUnitPool;
        IntializePool();
        _instance = this;
    }

    public void IntializePool()
    {

        for (int i = 0; i < m_ListOfUnitPool.Count; i++)
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            Queue<GameObject> queuecpy = new Queue<GameObject>();

            for (int j = 0; j < m_ListOfUnitPool[i].PoolSize; j++)
            {
                GameObject unitobject = Instantiate(m_ListOfUnitPool[i].Prefab);
                unitobject.name = m_ListOfUnitPool[i].Type +"_"+j.ToString();
                unitobject.transform.parent = transform;
                unitobject.gameObject.SetActive(false);
                queue.Enqueue(unitobject);
                queuecpy.Enqueue(unitobject);
            }
            _UnitPoolDict.Add(m_ListOfUnitPool[i].Type, queue);
            _UnitPoolDictCpy.Add(m_ListOfUnitPool[i].Type, queuecpy);
        }
    }
    public GameObject GetSpawnableObject(TroopType unittype)
    {
        if (!_UnitPoolDict.ContainsKey(unittype))
        {
            Debug.LogWarning("Type is not exists in Pool");
            return null;
        }
        if (_UnitPoolDict[unittype].Count > 0)
        {
            GameObject spwanableunit = _UnitPoolDict[unittype].Dequeue();
            if (unittype == TroopType.Mirror)
            {
                UpdateMirrorStatus(spwanableunit);
            }
            return spwanableunit;

        }
        else
        {
            Debug.LogWarning("Unit is Out of Stock");
        }

        return null;

    }

    public void UpdateMirrorStatus(GameObject target) 
    {
        foreach (GameObject gameObject in _UnitPoolDictCpy[TroopType.Mirror]) 
        {
            print(gameObject + "==" + target);
            if (gameObject == target)
            {
                gameObject.GetComponent<Mirror>()._IsSelected = true;
            }
            else
            {
                gameObject.GetComponent<Mirror>()._IsSelected = false;
            }
        }
    }


    public void DropTroop(Vector3 pos)
    {
        GameObject unitfrompool = GetSpawnableObject(EventActions._SelectedUnitType);
        if (unitfrompool != null)
        {
            unitfrompool.GetComponent<IIUnityItem>().DropItem(unitfrompool,pos);
        }
            
    }

}

