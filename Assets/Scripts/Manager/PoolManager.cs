using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject m_ParentObject;
    [SerializeField] private LevelTroops m_UnitsData;

    private List<GameObject> m_UnitQueue = new List<GameObject>();
    List<GameObject> m_UnitTempList = new List<GameObject>();

    private int m_LastActiveInstanceIndex = -1;

    void Start()
    {
        //EventActions._SelectUnitFromPool += SelectUnit;
        IntializePool();
    }

    private void IntializePool()
    {
        Dictionary<string, int> unitsdata = new Dictionary<string, int>();

        int currentindex = 0;

        for (int i = 0; i < m_UnitsData.DefaultUnits.Count; i++)
        {
            GameObject UnitObject = Instantiate(m_UnitsData.DefaultUnits[i]._TroopData._Prefab, m_ParentObject.transform);
            UnitObject.SetActive(false);

            UnitsManager.Instance._TotalTroopCount += m_UnitsData.DefaultUnits[i]._TroopData.SlotCost;
            UnitsManager.Instance.UpdateTroopSlot();
            UnitObject.GetComponent<Unit>()._SlotCost = m_UnitsData.DefaultUnits[i]._TroopData.SlotCost;

            string unittype = m_UnitsData.DefaultUnits[i]._TroopData.Type.ToString();
            if (!unitsdata.ContainsKey(unittype))
            {
                unitsdata[unittype] = currentindex;
                currentindex++;
            }

            UnitObject.transform.name = m_UnitsData.DefaultUnits[i]._TroopData.Type + "_" + unitsdata[unittype];
            UnitObject.GetComponent<Unit>()._UIInstanceIndex = unitsdata[unittype];

            if (UnitObject.GetComponent<Troop>() != null)
            {
                UnitObject.GetComponent<Troop>().SetupTroop();
            }
            m_UnitQueue.Add(UnitObject);
            m_UnitTempList.Add(UnitObject);
        }
        var ListOfDistinctUnit = m_UnitsData.DefaultUnits.GroupBy(n => n).Select(group => new { Unit = group.Key, Count = group.Count() }).ToList();
        for (int i = 0; i < ListOfDistinctUnit.Count; i++)
        {
            UnitsManager.Instance.UpdateUnitsCount(i, ListOfDistinctUnit[i].Count, false);
        }
    }
    //public void SelectUnit(GameObject Unit)
    //{
    //    foreach (var unitobj in m_UnitTempList)
    //    {
    //        if(unitobj.transform.name == Unit.name)
    //        {
    //            unitobj.GetComponent<TroopUIItem>()._TroopSelected = true;
    //        }
    //        else
    //        {
    //            unitobj.GetComponent<TroopUIItem>()._TroopSelected = false;
    //        }
    //    }
    //}
    public GameObject GetSpawnableObject(TroopType unittype)
    {
        GameObject Unit = null;
        foreach (var unitobj in m_UnitQueue)
        {
            if (unitobj.GetComponent<Unit>()._UnitType == unittype) 
            { 
                 Unit = unitobj;
            }
        }
        return Unit;
    }
    public void DequeuePool(GameObject Unit)
    {
        m_UnitQueue.Remove(Unit);
    }

    public void DropTroop()
    {
        GameObject unitfrompool = GetSpawnableObject(EventActions._SelectedUnitType);
        if (unitfrompool != null)
        {
            unitfrompool.SetActive(true);
            m_LastActiveInstanceIndex = unitfrompool.GetComponent<Unit>()._UIInstanceIndex;
            EventActions._DropUnitOnGround.Invoke(unitfrompool.GetComponent<Unit>()._UIInstanceIndex);
            if(unitfrompool.GetComponent<Troop>() != null)
            {
                unitfrompool.GetComponent<Troop>().TriggerMove();
            }
            UnitsManager.Instance._TotalTroopCount -= unitfrompool.GetComponent<Unit>()._SlotCost;
            UnitsManager.Instance.UpdateTroopSlot();
            UnitsManager.Instance.UpdateUnitsCount(unitfrompool.GetComponent<Unit>()._UIInstanceIndex,1,true);
            DequeuePool(unitfrompool);
        }
        else
        {
            EventActions._DropUnitOnGround.Invoke(m_LastActiveInstanceIndex);
            print("unit from pool: null  ");
        }

    }

}
