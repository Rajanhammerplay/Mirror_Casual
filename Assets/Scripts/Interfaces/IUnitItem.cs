using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public interface IUnitItem 
{
    bool IsSelected { get; }
    public void SetSelected(bool selected);
    Defines.UnitType GetUnitType();
    GameObject GetGameObject();
    public void DropItem(GameObject go,Vector3 pos,GameObject looatobj);

    public bool IsSameItemNearby(Vector3 curpos);    
}

