using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayLauncher : MonoBehaviour
{
    [SerializeField] private float m_DetectionDistance;
    [SerializeField] private Vector3 m_DetectionBoxSize;
    [SerializeField] private Vector3 m_OriginOffset;
    [SerializeField] private Vector3 m_EnemyDetectionOffset;
    [SerializeField] private float m_EnemyDetectionRadius;
    [SerializeField] private Transform m_LaserHead;
    [SerializeField] private LaserGenerator m_LaserGenerator;

    private Troop currentTarget = null;
    private Transform m_CurrentTransform;
    private Collider[] m_DetectioResults = new Collider[100];
    private Collider[] m_TargetsInRange = new Collider[100];

    void Start()
    {
        m_CurrentTransform = transform;
        DetectTilesInfornt();
    }
    private void Update()
    {
        FindTarget();
    }
    public void DetectTilesInfornt()
    {
        Vector3 direction = transform.forward;

        Vector3 BoxCenter = (transform.position + m_OriginOffset) + (direction * m_DetectionDistance / 2);
        Vector3 BoxSize = new Vector3(m_DetectionBoxSize.x, m_DetectionBoxSize.y, m_DetectionBoxSize.z);

        m_DetectioResults = Physics.OverlapBox(BoxCenter,BoxSize/2,transform.rotation);

        foreach (Collider result in m_DetectioResults)
        {
            if(result.transform.GetComponent<TileObject>() != null)
            {
                result.transform.GetComponent<TileObject>()._LookatObject = this.gameObject;
            }
            UnitsManager.Instance._MirrorPlacableTiles.Add(result.gameObject);
        }

    }

   #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 BoxCenter = (transform.position + m_OriginOffset) + (transform.forward * m_DetectionDistance / 2);
        Vector3 BoxSize = new Vector3(m_DetectionBoxSize.x, m_DetectionBoxSize.y, m_DetectionBoxSize.z);
        Gizmos.DrawCube(BoxCenter,BoxSize);
    }
   #endif

    private void FindTarget()
    {
        if (m_LaserGenerator._laser._MirrorDeteced == true)
        {
            return;
        }
        if (currentTarget != null && IsTargetInRange() == false)
        {
            ResetLauncher();
        }

        if (currentTarget == null)
        {
            FindNewTarget();
        }

        if (currentTarget != null)
        {
            ShootTarget();
        }

    }

    //method to find new target
    private void FindNewTarget()
    {

        
        m_TargetsInRange = Physics.OverlapSphere(m_CurrentTransform.position + m_EnemyDetectionOffset, m_EnemyDetectionRadius);
        foreach (Collider Target in m_TargetsInRange)
        {
            if (Target.GetComponent<Troop>() != null)
            {
                if (currentTarget == null)
                {
                    currentTarget = Target.GetComponent<Troop>();
                    m_LaserGenerator._laser._CurrentTarget = currentTarget.gameObject;
                    m_LaserGenerator._CanCastLaser = true;
                    break;
                }

            }
        }
    }

    //to look Target
    private void ShootTarget()
    {
        m_LaserHead.transform.LookAt(currentTarget.transform.position);
    }

    //to reset launcher
    private void ResetLauncher()
    {
        currentTarget = null;
        m_LaserGenerator._laser._CurrentTarget = null;
        m_LaserGenerator._CanCastLaser = false;
    }

    //to track target in Range
    private bool IsTargetInRange()
    {
        if(currentTarget != null)
        {
            if (Vector3.Distance(m_CurrentTransform.position + m_EnemyDetectionOffset, currentTarget.transform.position) < m_EnemyDetectionRadius)
            {
                return true;
            }
        }
        
        return false;
    }
}
