using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SpawnTroops();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnTroops()
    {
        EventActions._SpawnTroops?.Invoke(Defines.UnitType.Troop_1);
    }
}
