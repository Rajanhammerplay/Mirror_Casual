using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    public List<Transform> _CornerTiles;
    public Transform _SpawningPoint;
    private int cornerid = 0;
    public static TilesManager _Instance;

    private void Awake()
    {
        _Instance = this;
    }

}
