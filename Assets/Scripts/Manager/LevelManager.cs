using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<LevelUnitPool> _LevelUnitPool;
    public static LevelManager instance;

    //
    public Transform _Destination;
    public Transform _StartingPoint;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
