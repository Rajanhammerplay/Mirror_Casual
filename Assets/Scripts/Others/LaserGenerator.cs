using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGenerator : MonoBehaviour
{
    public Laser _laser;

    public Material m_BeamMaterial;

    public InputManager _InputManager;

    public bool _CanCastLaser;


    private void Start()
    {
        _laser = new Laser(gameObject.transform.position, gameObject.transform.forward, m_BeamMaterial, _InputManager);
    }

    private void Update()
    {

        if (_CanCastLaser || _laser._CurrentTarget != null)
        {
            _laser.HideLaser(true);
            _laser.UpdateLaser(gameObject.transform.position, (_laser._CurrentTarget.transform.position - gameObject.transform.position).normalized);
        }
        else
        {
            _laser.HideLaser(false);
        }
    }

    public void CastLaser()
    {
        
    }
}
