using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayLauncher : MonoBehaviour
{
    [SerializeField] float m_DetectionDistance;
    [SerializeField] Vector3 m_DetectionBoxSize;
    [SerializeField] Vector3 m_OriginOffset;
    [SerializeField] Vector3 m_OriginOffset_1;
    [SerializeField] float m_EnemyDetectionRadius;
    [SerializeField] private Transform m_LaserHead;
    [SerializeField] private LaserGenerator m_LaserGenerator;

    private Troop currentTarget = null;
    // Start is called before the first frame update
    void Start()
    {
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
        Vector3 BoxSize = new Vector3(m_DetectionBoxSize.x, m_DetectionBoxSize.y, m_DetectionDistance);

        Collider[] collider = Physics.OverlapBox(BoxCenter,BoxSize/2,transform.rotation);

        foreach (Collider collider2 in collider)
        {
            if(collider2.transform.GetComponent<TileObject>() != null)
            {
                collider2.transform.GetComponent<TileObject>()._LookatObject = this.gameObject;
            }
            UnitsManager.Instance._MirrorPlacableTiles.Add(collider2.gameObject);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 BoxCenter = (transform.position + m_OriginOffset) + (transform.forward * m_DetectionDistance / 2);
        Vector3 BoxSize = new Vector3(m_DetectionBoxSize.x, m_DetectionBoxSize.y, m_DetectionDistance);
        Gizmos.DrawCube(BoxCenter,BoxSize);
        //Gizmos.color = Color.white;
        //Gizmos.DrawSphere(this.transform.position + m_OriginOffset_1, m_EnemyDetectionRadius);
    }

    
    private void FindTarget()
    {
        
        if(currentTarget != null && IsTargetInRange() == false)
        {
            ResetLauncher();
        }

        if (currentTarget == null) 
        { 
            FindNewTarget();
        }

        if(currentTarget != null)
        {
            ShootTarget();
        }
    }

    //method to find new target
    private void FindNewTarget()
    {
        
        Collider[] TargetsInRange = Physics.OverlapSphere(this.transform.position + m_OriginOffset_1, m_EnemyDetectionRadius);
        foreach (Collider Target in TargetsInRange)
        {
            if (Target.GetComponent<Troop>() != null)
            {
                if (currentTarget == null)
                {
                    currentTarget = Target.GetComponent<Troop>();
                    m_LaserGenerator._CurrentTarget = currentTarget.gameObject;
                    m_LaserGenerator._CanCastLaser = true;
                    //print("Target in distance: " + Vector3.Distance(this.transform.position + m_OriginOffset_1, currentTarget.transform.position) + "<" + m_EnemyDetectionRadius);
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
        m_LaserGenerator._CurrentTarget = null;
        m_LaserGenerator._CanCastLaser = false;
       // m_LaserHead.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    //to track target in Range
    private bool IsTargetInRange()
    {
        if(currentTarget != null)
        {
            if (Vector3.Distance(this.transform.position + m_OriginOffset_1, currentTarget.transform.position) < m_EnemyDetectionRadius)
            {
                return true;
            }
        }
        
        return false;
    }
}
