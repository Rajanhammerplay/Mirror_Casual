using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject m_ParentObject;
    [SerializeField] private LevelTroops m_UnitsData;

    private List<GameObject> m_UnitQueue = new List<GameObject>();
    List<GameObject> m_UnitTempList = new List<GameObject>();

    void Start()
    {
        //EventActions._SelectUnitFromPool += SelectUnit;
        IntializePool();
    }

    private void IntializePool()
    {

            for(int i = 0;i< m_UnitsData.DefaultUnits.Count; i++)
            {
                GameObject UnitObject = Instantiate(m_UnitsData.DefaultUnits[i]._TroopData._Prefab, m_ParentObject.transform);
                UnitObject.transform.name = m_UnitsData.DefaultUnits[i]._TroopData.Type + "_" + i;
                UnitObject.GetComponent<Unit>()._UIInstanceIndex = i;
                m_UnitQueue.Add(UnitObject);
                m_UnitTempList.Add(UnitObject);
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

    public void DropTroop(Vector3 pos)
    {
        GameObject unitfrompool = GetSpawnableObject(EventActions._SelectedUnitType);
        if (unitfrompool != null)
        {
            unitfrompool.SetActive(true);
        }

        EventActions._DropUnitOnGround.Invoke(unitfrompool.GetComponent<Unit>()._UIInstanceIndex);
        DequeuePool(unitfrompool);
    }

}
