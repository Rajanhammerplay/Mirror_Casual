using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum MirrorType
{
    Mirror,
    Type2,
    Type3
}

[Serializable]
public class MirrorTypeMapping
{
    public MirrorVariation key;
    public MirrorTypes value;
}


[CreateAssetMenu(fileName = "MirrorTypes", menuName = "ScriptableObject/MirrorCasual/InventoryData")]
public class InventoryData : ScriptableObject
{
    public List<MirrorTypeMapping> MirrorInventoryList = new List<MirrorTypeMapping>();
}