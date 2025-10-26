using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventActions
{

    public static Action<Mirror> _PickMirror;
    public static Action<Mirror> _DropMirror;

    /*units drag and drop actions*/
    public static Defines.UnitType _SelectedUnitType = Defines.UnitType.none;
    public static Action<GameObject> _SelectMirror;
    public static Action<GameObject> _SelectUnitFromPool;
    public static Action<Defines.UnitType> _DropUnitOnGround;
    public static Action<Defines.UnitType> _AddUnit;

    public static Action<Defines.UnitType> _SpawnTroops;

    /*units shop actions*/
    public static Action<int,int> _AddTroop;
    public static Action<int, int> _DropTroop;

    public static Action<bool> CheckCanSwipe;

}
