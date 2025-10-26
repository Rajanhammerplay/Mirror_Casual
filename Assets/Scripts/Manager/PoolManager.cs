using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PoolManager : MonoBehaviour
{
    public static PoolManager _instance;
    public Dictionary<Defines.UnitType, Queue<GameObject>> _UnitPoolDict = new Dictionary<Defines.UnitType, Queue<GameObject>>();
    public Dictionary<Defines.UnitType, Queue<GameObject>> _UnitPoolDictCpy = new Dictionary<Defines.UnitType, Queue<GameObject>>();
    public GameObject _Pathparent;
    public Camera m_UICamera;
    public GameObject _HealerObj;

    private List<UnitPool> m_ListOfUnitPool = new List<UnitPool>();

    private void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        LevelUnitPool lvlunitpool = ScriptableObject.Instantiate(LevelManager.instance._LevelUnitPool[0]);
        m_ListOfUnitPool = lvlunitpool._ListOfUnitPool;
        IntializePool();
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
                if (unitobject.GetComponent<Troop>()?.m_HealthBarCanvas != null)
                {
                    unitobject.GetComponent<Troop>().m_HealthBarCanvas.worldCamera = m_UICamera;
                }

                queue.Enqueue(unitobject);
                queuecpy.Enqueue(unitobject);
            }
            _UnitPoolDict.Add(m_ListOfUnitPool[i].Type, queue);
            _UnitPoolDictCpy.Add(m_ListOfUnitPool[i].Type, queuecpy);
        }
    }
    public GameObject GetSpawnableObjectFromPool(Defines.UnitType unittype)
    {
        if (!_UnitPoolDict.ContainsKey(unittype))
        {
            return null;
        }
        if (_UnitPoolDict[unittype].Count > 0)
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

    public bool IsUnitAvailableInPool(Defines.UnitType unittype)
    {
        if (!_UnitPoolDict.ContainsKey(unittype))
        {
            return false;
        }
        if (_UnitPoolDict[unittype].Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}

