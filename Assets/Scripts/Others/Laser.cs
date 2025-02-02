using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Laser
{
    public LineRenderer m_Beam;
    public bool _Raycasted = false;
    public bool _MirrorDeteced = false;
    public GameObject _CurrentTarget;

    private GameObject m_BeamObject;
    List<Vector3> m_BeamIndices = new List<Vector3>();
    Vector3 pos, dir;
    private InputManager m_InputManager;

    public Laser(Vector3 starpos, Vector3 rotation, Material Raymaterial, InputManager InputManager)
    {
        m_Beam = new LineRenderer();
        m_BeamObject = new GameObject();
        m_BeamObject.name = "LaserBeam";
        m_Beam = m_BeamObject.AddComponent<LineRenderer>() as LineRenderer;
        m_Beam.sortingLayerName = "Ray";
        m_Beam.startColor = Color.red;
        m_Beam.endColor = Color.white;
        m_Beam.startWidth = 1f;
        m_Beam.endWidth = 0.02f;

        this.pos = starpos;
        this.dir = rotation;
        m_Beam.material = Raymaterial;
        this.m_InputManager = InputManager;

        m_BeamIndices.Capacity = 10;
    }

    public void UpdateLaser(Vector3 startPos, Vector3 direction)
    {
        m_BeamIndices.Clear();
        CastBeam(startPos, direction, m_Beam,"normal");
    }

    public void HideLaser(bool show)
    {
        m_BeamObject?.SetActive(show);
    }

    public void CastBeam(Vector3 start,Vector3 dir,LineRenderer beam,string castwhile)
    {
        m_BeamIndices.Add(start);
        int excludeRays = castwhile == "normal" ? (1 << 7) : (1 << 7 | 1 << 3 | 1 << 11);
        Ray ray = castwhile == "normal" ? new Ray(start, dir) : new Ray(start, dir + new Vector3(0f, 0.08f, 0f));
        int maxdist = castwhile == "normal" ? 30 : 300;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,maxdist,~excludeRays))
        {
            if(castwhile != "normal")
            {
                TileObject tileObject = hit.collider.GetComponent<TileObject>();
                if (tileObject == null) 
                {
                    EventActions._UpdateHealerPos?.Invoke(Vector3.zero, false);
                    return;
                }
                if (tileObject._Tiletype == TileTypes.path) 
                {
                    EventActions._UpdateHealerPos?.Invoke(hit.point,true);
                }
                else
                {
                    EventActions._UpdateHealerPos?.Invoke(Vector3.zero, false);
                }
                
            }
            
            //Collider[] tiles = Physics.OverlapSphere(hit.collider.transform.position, 20f);
            //foreach (Collider col in tiles) 
            //{ 
            //    TileObject to = col.GetComponent<TileObject>();
            //    if (to != null) 
            //    { 
            //        if(to._Tiletype == TileTypes.path)
            //        {
            //        }
            //    }
            //}
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

    //to generate reflections
    public void ReflectMirror(RaycastHit hitinfo,Vector3 direction,LineRenderer laser)
    {
        if(hitinfo.collider.gameObject.tag == "mirror")
        {
            _CurrentTarget = hitinfo.collider.gameObject;
            _MirrorDeteced = true;
            Transform hitpoint = hitinfo.collider.gameObject.GetComponent<Mirror>()._Mirror.transform;
            Vector3 dir = Vector3.Reflect(direction, hitpoint.right);
            CastBeam(hitpoint.position, dir, laser,"reflect");
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
