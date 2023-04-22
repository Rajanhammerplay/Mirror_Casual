using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGenerator : MonoBehaviour
{
    private Laser m_laser;

    public Material m_BeamMaterial;

    public MirrorManager _MirrorManger;
    private void Update()
    {
       Destroy(GameObject.Find("LaserBeam"));
       m_laser = new Laser(gameObject.transform.position,gameObject.transform.forward,m_BeamMaterial,_MirrorManger);
    }
}
