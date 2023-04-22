using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser 
{
    private LineRenderer m_Beam;

    private GameObject m_BeamObject;

    List<Vector3> m_BeamIndices = new List<Vector3>();

    Vector3 pos, dir;

    public bool _Raycasted = false;

    private MirrorManager m_MirrorManager;
    
    public Laser(Vector3 starpos,Vector3 rotation,Material Raymaterial,MirrorManager mirrormanager)
    {
        m_Beam = new LineRenderer();
        m_BeamObject = new GameObject();
        m_BeamObject.name = "LaserBeam";
        m_Beam = m_BeamObject.AddComponent<LineRenderer>() as LineRenderer;
        m_Beam.startColor = Color.red;
        m_Beam.endColor = Color.red;
        m_Beam.startWidth = 0.02f;
        m_Beam.endWidth = 0.02f;
        this.pos = starpos;
        this.dir = rotation;
        m_Beam.material = Raymaterial;
        this.m_MirrorManager = mirrormanager;

        CastBeam(this.pos, this.dir, m_Beam);
    }

    void CastBeam(Vector3 start,Vector3 dir,LineRenderer beam)
    {
        m_BeamIndices.Add(start);

        Ray ray = new Ray(start, dir);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,30,1))
        {
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
            this.m_MirrorManager.isRaycasted = true;
            Vector3 dir = Vector3.Reflect(direction, hitinfo.normal);
            CastBeam(hitinfo.point, dir, laser);
        }else if(hitinfo.collider.gameObject.tag == "enemy")
        {
            m_BeamIndices.Add(hitinfo.point);
            UpdateIndices();
            hitinfo.collider.gameObject.GetComponent<Enemy>().KillEnemy();
        }
        else
        {
            m_BeamIndices.Add(hitinfo.point);
            UpdateIndices();
        }



    }
}
