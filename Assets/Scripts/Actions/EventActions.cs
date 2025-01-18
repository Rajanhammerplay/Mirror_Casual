using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventActions
{

    public static Action<Mirror> _PickMirror;
    public static Action<Mirror> _DropMirror;

    /*units drag and drop actions*/
    public static TroopType _SelectedUnitType = TroopType.none;
    public static Action<GameObject> _SelectUnitFromPool;
    public static Action<TroopType> _DropUnitOnGround;
    public static Action<TroopType> _AddUnit;

    /*units shop actions*/
    public static Action<int,int> _AddTroop;
    public static Action<int, int> _DropTroop;
}
