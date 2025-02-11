using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGenerator : MonoBehaviour
{
    public Laser _laser;
    public Material m_BeamMaterial;
    public InputManager _InputManager;
    public bool _CanCastLaser;
    public GameObject _laserobj;

    public float defualtypos;

    GameObject heal;
    private Healer _Healer;
    private void Start()
    {
        GameObject lobject = Instantiate(_laserobj);
        _laser = new Laser(gameObject.transform.position, gameObject.transform.forward, m_BeamMaterial, _InputManager,lobject);

        if (_laser.m_BeamObject != null)
        {
            if (PoolManager._instance._HealerObj != null)
            {
                heal = Instantiate(PoolManager._instance._HealerObj);
                heal.transform.parent = lobject.transform;
                Vector3 endpos = _laser.m_Beam.GetPosition(_laser.m_Beam.positionCount - 1);
                heal.transform.position = new Vector3( endpos.x ,-3.8f, endpos.z);
               _Healer = heal.GetComponent<Healer>();
            }

        }

        if (_Healer != null)
        {
            _laser.m_healer = _Healer;
        }
    }

    private void Update()
    {
        HandleLaser();
    }

    private void HandleLaser()
    {
        if( _laser._CurrentTarget == null)
        {
            return;
        }

        if (_CanCastLaser  || (!_laser._CurrentTarget.GetComponent<Troop>() && !_laser._CurrentTarget.GetComponent<Troop>()._TroopDead))
        {
            _laser.HideLaser(true);
            _laser.UpdateLaser(gameObject.transform.position, (_laser._CurrentTarget.transform.position - gameObject.transform.position).normalized);
            if (_laser.m_BeamObject != null)
            {
                Vector3 tailpos = _laser.m_Beam.GetPosition(_laser.m_Beam.positionCount - 1);
                heal.transform.position = new Vector3(tailpos.x,defualtypos, tailpos.z);
            }
        }
        else
        {
            _laser.HideLaser(false);
        }
    }


}


