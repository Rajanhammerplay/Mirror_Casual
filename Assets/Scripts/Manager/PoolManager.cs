using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PoolManager : MonoBehaviour
{
    public static PoolManager _instance;
    private List<UnitPool> m_ListOfUnitPool = new List<UnitPool>();
    public Dictionary<TroopType, Queue<GameObject>> _UnitPoolDict = new Dictionary<TroopType, Queue<GameObject>>();

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

            for (int j = 0; j < m_ListOfUnitPool[i].PoolSize; j++)
            {
                GameObject unitobject = Instantiate(m_ListOfUnitPool[i].Prefab);
                unitobject.name = m_ListOfUnitPool[i].Type +"_"+j.ToString();
                unitobject.transform.parent = transform;
                unitobject.gameObject.SetActive(false);
                queue.Enqueue(unitobject);
            }
            _UnitPoolDict.Add(m_ListOfUnitPool[i].Type, queue);
        }
    }
    public GameObject GetSpawnableObject(TroopType unittype)
    {
        if (!_UnitPoolDict.ContainsKey(unittype))
        {
            Debug.LogWarning("Type is not exists in Pool");
            return null;
        }
        if(_UnitPoolDict[unittype].Count > 0)
        {
            GameObject spwanableunit = _UnitPoolDict[unittype].Dequeue();
            return spwanableunit;

        }
        else
        {
            Debug.LogWarning("Unit is Out of Stock");
        }
        
        return null;

    }


    public void DropTroop()
    {
        GameObject unitfrompool = GetSpawnableObject(EventActions._SelectedUnitType);
        if (unitfrompool != null)
        {
            unitfrompool.gameObject.SetActive(true);
            UnitsManager.Instance.DropUnit(EventActions._SelectedUnitType);
                                                                                                                            

            if (unitfrompool.GetComponent<Troop>() != null)
            {
                unitfrompool.GetComponent<Troop>().SetupTroop();
                unitfrompool.GetComponent<Troop>().TriggerMove();
            }
        }
            
    }

}

