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
    public static Action<int> _DropUnitOnGround;

    /*units shop actions*/
    public static Action<int,int> _SelectTroop;
    public static Action<int, int> _DropTroop;
}
