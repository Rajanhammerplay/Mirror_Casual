using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TroopData",menuName = "ScriptableObject/MirrorCasual/TroopData")]
public class UnitItem : ScriptableObject
{
    public UnitsData _UnitData;
}



[System.Serializable]
public class UnitsData
{
    public Defines.UnitType Type;
    public string Name;
    public string Description;
    public int Level;
    public int Cost;
    public int SlotCost;
    public Sprite Image;
    public float _Health;
    public GameObject _Prefab;
    public int _InstanceIndex;
    public int _Count;
}
