using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser
{
    public LineRenderer m_Beam;
    public bool _Raycasted = false;
    public bool _MirrorDeteced = false;
    public GameObject _CurrentTarget;
    public Healer m_healer;
    public GameObject m_BeamObject;

    private List<Vector3> m_BeamIndices = new List<Vector3>();
    private Vector3 pos, currentdir;
    private InputManager m_InputManager;
    private Vector2 m_ShowHealerBounds;
    private float dist = 0;
    private bool m_IsConnectedwithMirror;
    private List<Transform> m_ListOfHitpoints;

    //constructor to intiaize laser
    public Laser(Vector3 starpos, Vector3 rotation, Material Raymaterial, InputManager InputManager,GameObject laserob)
    {
        m_BeamObject = laserob;
        m_BeamObject.name = "LaserBeam";
        m_Beam = m_BeamObject.GetComponent<LineRenderer>();
        m_Beam.sortingLayerName = "Ray";
        this.pos = starpos;
        this.currentdir = rotation;
        this.m_InputManager = InputManager;
        m_ShowHealerBounds = new Vector2(DefaultValues.MIN_HEALER_BOUND, DefaultValues.MAX_HEALER_BOUND);
        m_BeamIndices.Capacity = DefaultValues.MAX_CAP;
        m_ListOfHitpoints = new List<Transform>();
    }

    //update laser positions and linerend index
    public void UpdateLaser(Vector3 startPos, Vector3 direction)
    {
        m_BeamIndices.Clear();
        CastBeam(startPos, direction, m_Beam, Defines.LaserTypes.Normal);
    }

    public void HideLaser(bool show)
    {
        m_BeamObject?.SetActive(show);
    }

 
    public void CastBeam(Vector3 start,Vector3 dir,LineRenderer beam, Defines.LaserTypes castwhile)
    {
        m_BeamIndices.Add(start);
        int excludeRays = castwhile == Defines.LaserTypes.Normal ? (1 << 7 | 1 << 11) : (1 << 7 | 1 << 3 | 1 << 11);
        Ray ray = castwhile == Defines.LaserTypes.Normal ? new Ray(start, dir) : new Ray(start, dir + new Vector3(0f, 0.08f, 0f));
        int maxdist = castwhile == Defines.LaserTypes.Normal ? DefaultValues.NORMAL_RAY_DIST : DefaultValues.REFLECFT_RAY_DIST;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,maxdist,~excludeRays))
        {
            Mirror currentmirror = hit.collider.gameObject.GetComponent<Mirror>();
            if (currentmirror != null && currentmirror.MirrorType == Defines.UnitType.HealerMirror) 
            {
                float dist = Vector3.Dot(hit.transform.forward, (hit.transform.position - m_healer.transform.position));
              //  Debug.Log("distance from center: " + dist);
                if (m_healer && (dist > m_ShowHealerBounds.x && dist < m_ShowHealerBounds.y))
                {
                   // m_healer.transform.position = new Vector3(m_healer.transform.position.x, 0.8f, m_healer.transform.position.z);
                    m_healer.gameObject.SetActive(true);
                }
                else
                {
                    m_healer.gameObject.SetActive(false);
                }
            }
            ReflectMirror(hit,dir,beam);
        }
        else
        {
            m_BeamIndices.Add(ray.GetPoint(30));
            UpdateIndices();
        }
    }

    public void UpdateIndices()
    {
        int count = 0;
        m_Beam.positionCount = m_BeamIndices.Count;

        foreach(var index in m_BeamIndices)
        {
            m_Beam.SetPosition(count, index);
            count++;
        }
    }

    Vector3 hitpoint;
    Transform hitobject;
    //to generate reflections
    public void ReflectMirror(RaycastHit hitinfo,Vector3 direction,LineRenderer laser)
    {
        if(hitinfo.collider.gameObject.tag == "mirror")
        {
            if(hitinfo.collider.GetComponent<Mirror>().MirrorType == Defines.UnitType.HealerMirror)
            {
                _CurrentTarget = hitinfo.collider.gameObject;
            }
            else
            {
                if(!_CurrentTarget.GetComponent<Mirror>())
                {
                    _CurrentTarget = hitinfo.collider.gameObject;
                }
            }
            
            _MirrorDeteced = true;
            Mirror currentmirror = hitinfo.collider.gameObject.GetComponent<Mirror>();
            hitpoint = hitinfo.collider.gameObject.GetComponent<Mirror>()._Mirror.transform.position;
            hitobject = hitinfo.collider.gameObject.GetComponent<Mirror>()._Mirror.transform;
            Vector3 dir = Vector3.Reflect(direction, hitobject.right);
            CastBeam(hitpoint, dir, laser, Defines.LaserTypes.Reflect);
        }
        else if(hitinfo.collider.GetComponent<Troop>())
        {
            m_BeamIndices.Add(hitinfo.point);
            UpdateIndices();
            hitinfo.collider.gameObject.GetComponent<Troop>().KillTroop();
            
        }
        else
        {
            m_BeamIndices.Add(hitinfo.point);
            UpdateIndices();
        }
    }
}
