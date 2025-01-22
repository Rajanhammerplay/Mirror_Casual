using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorTileDetector : MonoBehaviour
{
    [SerializeField] float m_DetectionDistance;
    [SerializeField] Vector3 m_DetectionBoxSize;
    [SerializeField] Vector3 m_OriginOffset;
    [SerializeField] Vector3 m_OriginOffset_1;
    [SerializeField] float m_EnemyDetectionRadius;
    [SerializeField] private Transform m_LaserHead;
    [SerializeField] private LaserGenerator m_LaserGenerator;
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
            UnitsManager.Instance._MirrorPlacableTiles.Add(collider2.gameObject);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 BoxCenter = (transform.position + m_OriginOffset) + (transform.forward * m_DetectionDistance / 2);
        Vector3 BoxSize = new Vector3(m_DetectionBoxSize.x, m_DetectionBoxSize.y, m_DetectionDistance);
        Gizmos.DrawCube(BoxCenter,BoxSize);
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(this.transform.position + m_OriginOffset_1, m_EnemyDetectionRadius);
    }

    private void FindTarget()
    {
        Collider[] TargetsInRange = Physics.OverlapSphere(this.transform.position + m_OriginOffset_1, m_EnemyDetectionRadius);
        foreach (Collider Target in TargetsInRange)
        {
            if(Target.GetComponent<Troop>() != null)
            {
                m_LaserGenerator._CanCastLaser = true;
                m_LaserHead.transform.LookAt(Target.transform.position);
            }
            else
            {
                m_LaserGenerator._CanCastLaser = false;
                m_LaserHead.transform.LookAt(Vector3.zero);
            }
        }
    }

}
