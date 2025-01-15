using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum UnitType
{
    Mirror_1,
    Type2,
    Type3
}

[Serializable]
public class MirrorTypeMapping
{
    public MirrorVariation key;
    public MirrorTypes value;
}


[CreateAssetMenu(fileName = "UnitTypes", menuName = "ScriptableObject/MirrorCasual/UnitsData")]
public class UnitsData : ScriptableObject
{
    public List<MirrorTypeMapping> MirrorInventoryList = new List<MirrorTypeMapping>();
}