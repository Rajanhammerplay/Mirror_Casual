using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TroopData",menuName = "ScriptableObject/MirrorCasual/TroopData")]
public class TroopCard : ScriptableObject
{
    public TroopData _TroopData;
}

public enum TroopType
{
    Mirror,
    Troop_1,
    Troop_2,
    Jammer,
    none
}

[System.Serializable]
public class TroopData
{
    public TroopType Type;
    public string Name;
    public string Description;
    public int Level;
    public int Cost;
    public int SlotCost;
    public Sprite Image;
    public float _Health;
    public GameObject _Prefab;
}
