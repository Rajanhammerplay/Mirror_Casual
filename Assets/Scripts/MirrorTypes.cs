using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MirrorTypes", menuName = "ScriptableObject/MirrorTypes")]
public class MirrorTypes : ScriptableObject
{
    public string _MirrorType;

    public int _MirrorCount;

    public int _TypeId;

    public GameObject _MirrorPrefab;

    public GameObject _UIMirrorPrefab;
}
