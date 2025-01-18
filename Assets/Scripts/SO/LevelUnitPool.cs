using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelUnitPool", menuName = "ScriptableObject/MirrorCasual/LevelUnitPool")]
public class LevelUnitPool:ScriptableObject
{
    public List<UnitPool> _ListOfUnitPool;
}

[System.Serializable]
public class UnitPool
{
    public TroopType Type;
    public GameObject Prefab;
    public int PoolSize;
}
