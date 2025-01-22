using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGenerator : MonoBehaviour
{
    private Laser m_laser;

    public Material m_BeamMaterial;

    public InputManager _InputManager;

    public bool _CanCastLaser;
    private void Start()
    {
        m_laser = new Laser(gameObject.transform.position, gameObject.transform.forward, m_BeamMaterial, _InputManager);
    }

    private void Update()
    {
        if (_CanCastLaser)
        {
            m_laser.UpdateLaser(gameObject.transform.position, gameObject.transform.forward);
        }
    }

    public void CastLaser()
    {
        
    }
}
