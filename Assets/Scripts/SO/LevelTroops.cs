using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "LevelTroop", menuName = "ScriptableObject/MirrorCasual/LevelTroop")]
public class LevelTroops : ScriptableObject
{
    public List<TroopCard> TroopsOnShop;
    public int maxTroopsSlot;
}


