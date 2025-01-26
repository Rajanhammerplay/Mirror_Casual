using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGenerator : MonoBehaviour
{
    private Laser m_laser;

    public Material m_BeamMaterial;

    public InputManager _InputManager;

    public bool _CanCastLaser;

    public GameObject _CurrentTarget;
    private void Start()
    {
        m_laser = new Laser(gameObject.transform.position, gameObject.transform.forward, m_BeamMaterial, _InputManager);
    }

    private void Update()
    {
        if (_CanCastLaser || _CurrentTarget != null)
        {
            m_laser.HideLaser(true);
            m_laser.UpdateLaser(gameObject.transform.position, (_CurrentTarget.transform.position - gameObject.transform.position).normalized);
        }
        else
        {
            m_laser.HideLaser(false);
        }
    }

    public void CastLaser()
    {
        
    }
}
