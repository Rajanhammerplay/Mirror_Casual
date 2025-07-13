using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser
{
    public LineRenderer m_Beam;
    public LineRenderer m_HealingBeam; // New healing beam
    public bool _Raycasted = false;
    public bool _MirrorDeteced = false;
    public GameObject _CurrentTarget;
    public Healer m_healer;
    public GameObject m_BeamObject;
    public GameObject m_HealingBeamObject; // New healing beam object

    private List<Vector3> m_BeamIndices = new List<Vector3>();
    private List<Vector3> m_HealingBeamIndices = new List<Vector3>(); // New healing beam indices
    private Vector3 pos, currentdir;
    private InputManager m_InputManager;
    private Vector2 m_ShowHealerBounds;
    private float dist = 0;
    private bool m_IsConnectedwithMirror;
    private List<Transform> m_ListOfHitpoints;

    // Healing ray parameters
    public float healingRayDeviation = 45f; // Deviation angle in degrees
    public float healingRayDistance = 10f; // Distance of healing ray

    //constructor to intiaize laser
    public Laser(Vector3 starpos, Vector3 rotation, Material Raymaterial, InputManager InputManager, GameObject laserob)
    {
        m_BeamObject = laserob;
        m_BeamObject.name = "LaserBeam";
        m_Beam = m_BeamObject.GetComponent<LineRenderer>();
        m_Beam.sortingLayerName = "Ray";

        // Create healing beam object
        m_HealingBeamObject = GameObject.Instantiate(laserob);
        m_HealingBeamObject.name = "HealingBeam";
        m_HealingBeam = m_HealingBeamObject.GetComponent<LineRenderer>();
        m_HealingBeam.sortingLayerName = "Ray";
        m_HealingBeam.material = Raymaterial; // You might want a different material for healing
        //m_HealingBeam.color = Color.green; // Green color for healing

        this.pos = starpos;
        this.currentdir = rotation;
        this.m_InputManager = InputManager;
        m_ShowHealerBounds = new Vector2(DefaultValues.MIN_HEALER_BOUND, DefaultValues.MAX_HEALER_BOUND);
        m_BeamIndices.Capacity = DefaultValues.MAX_CAP;
        m_HealingBeamIndices.Capacity = DefaultValues.MAX_CAP;
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


    public void CastBeam(Vector3 start, Vector3 dir, LineRenderer beam, Defines.LaserTypes castwhile)
    {
        m_BeamIndices.Add(start);
        int excludeRays = castwhile == Defines.LaserTypes.Normal ? (1 << 7 | 1 << 11) : (1 << 7 | 1 << 3 | 1 << 11);
        Ray ray = castwhile == Defines.LaserTypes.Normal ? new Ray(start, dir) : new Ray(start, dir);
        int maxdist = castwhile == Defines.LaserTypes.Normal ? DefaultValues.NORMAL_RAY_DIST : DefaultValues.REFLECFT_RAY_DIST;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxdist, ~excludeRays))
        {
            Mirror currentmirror = hit.collider.gameObject.GetComponent<Mirror>();

            // Get the reflection direction first
            Vector3 reflectionDirection = GetReflectionDirection(hit, dir);

            reflectionDirection = AdjustHealingRayDirection(reflectionDirection,-0.08f); // Adjust direction to avoid ground

            // Calculate the end point of the reflected ray for visualization
            Vector3 reflectionEndPoint = hit.point + (reflectionDirection * DefaultValues.REFLECFT_RAY_DIST); // 10f is just for visualization length
            if (currentmirror != null)
            {
                m_BeamIndices.Add(hitpoint);
                m_BeamIndices.Add(reflectionEndPoint);
                UpdateIndices();

            }
            ReflectMirror(hit, dir, beam);
        }
    }

    public void UpdateIndices()
    {
        int count = 0;
        m_Beam.positionCount = m_BeamIndices.Count;

        foreach (var index in m_BeamIndices)
        {
            m_Beam.SetPosition(count, index);
            count++;
        }
    }

    Vector3 hitpoint;
    Transform hitobject;
    //to generate reflections
    public void ReflectMirror(RaycastHit hitinfo, Vector3 direction, LineRenderer laser)
    {
     //   Debug.Log($"ReflectMirror called with: {hitinfo.collider.name}, tag: {hitinfo.collider.tag}");
        if (hitinfo.collider.gameObject.tag == "mirror")
        {
            _CurrentTarget = hitinfo.collider.gameObject;
            _MirrorDeteced = true;
            Mirror currentmirror = hitinfo.collider.gameObject.GetComponent<Mirror>();
            hitpoint = hitinfo.collider.gameObject.GetComponent<Mirror>()._Mirror.transform.position;
            hitobject = hitinfo.collider.gameObject.GetComponent<Mirror>()._Mirror.transform;

            Vector3 dir = Vector3.Reflect(direction, hitobject.right);
            CastHealingRay(hitpoint, dir);
            CastBeam(hitpoint, dir, laser, Defines.LaserTypes.Reflect);
            return;
        }
        else if (hitinfo.collider.GetComponent<Troop>())
        {
            m_BeamIndices.Add(hitinfo.point);
            UpdateIndices();
            hitinfo.collider.gameObject.GetComponent<Troop>().KillTroop();
            return;

        }
        else
        {
            m_BeamIndices.Add(hitinfo.point);
            UpdateIndices();
        }
    }

    // Helper method to get reflection direction
    private Vector3 GetReflectionDirection(RaycastHit hitInfo, Vector3 incomingDirection)
    {
        // Check if it's a mirror
        if (hitInfo.collider.gameObject.tag == "mirror")
        {
            Mirror mirror = hitInfo.collider.gameObject.GetComponent<Mirror>();
            if (mirror != null && mirror._Mirror != null)
            {
                // Get the mirror's normal (assuming it's the right vector)
                Vector3 mirrorNormal = mirror._Mirror.transform.right;

                // Calculate reflection
                Vector3 reflectedDirection = Vector3.Reflect(incomingDirection, mirrorNormal);
                return reflectedDirection;
            }
        }

        // If not a mirror, use the surface normal
        return Vector3.Reflect(incomingDirection, hitInfo.normal);
    }

    public void CastHealingRay(Vector3 startPoint, Vector3 reflectedDirection)
    {
        //reflectedDirection = AdjustHealingRayDirection(reflectedDirection); // Adjust direction to avoid ground
        // Just cast the healing ray without visual beam indices
        reflectedDirection = AdjustHealingRayDirection(reflectedDirection, -0.08f); // Adjust direction to avoid ground
        CastHealingBeam(startPoint, reflectedDirection, Defines.LaserTypes.Reflect);
    }

    // New method to adjust healing ray direction to avoid ground
    private Vector3 AdjustHealingRayDirection(Vector3 originalDirection,float height)
    {
        // Add upward lift to prevent hitting ground
        Vector3 adjustedDirection = originalDirection;

        adjustedDirection.y = height;

        return adjustedDirection.normalized;
    }

    // Simplified healing beam casting - single ray, no reflections
    public void CastHealingBeam(Vector3 start, Vector3 dir, Defines.LaserTypes castwhile)
    {
        int excludeRays = castwhile == Defines.LaserTypes.Normal ? (1 << 7 | 1 << 11) : (1 << 7 | 1 << 3 | 1 << 11);
        Ray ray = castwhile == Defines.LaserTypes.Normal ? new Ray(start, dir) : new Ray(start, dir);
        int maxdist = castwhile == Defines.LaserTypes.Normal ? DefaultValues.NORMAL_RAY_DIST : DefaultValues.REFLECFT_RAY_DIST;
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * maxdist, Color.blue); // Draw the healing ray for debugging
        if (Physics.Raycast(ray, out hit, maxdist, ~excludeRays))
        {
            if (hit.collider.gameObject.GetComponent<Troop>())
            {
                Troop troop = hit.collider.gameObject.GetComponent<Troop>();
                if (troop != null)
                {
                    // Heal the troop
                    troop.HealTroop();
                }
            }

        }
    }

}
