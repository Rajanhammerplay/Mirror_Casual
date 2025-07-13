using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using static Defines;


//responsible for troop 
public class Troop : MonoBehaviour, IUnitItem
{
    [SerializeField] UnitItem m_TroopCard;
    [SerializeField] HelathBar m_HealthBar;
    [SerializeField] float m_TroopSpeed;
    [SerializeField] NavMeshAgent m_NavMeshAgent;
    
    public float m_TroopHealth;
    public float m_healthlose;
    public Canvas m_HealthBarCanvas;
    public bool _CanKillTroop;
    public bool _TroopDead;
    public bool CanMoveTroop;
    public List<Transform> _CornerTiles;
    public Vector3[] m_CornerOffsets;

    [Header("Movement Props")]
    private Tilemap m_PathTileMap;
    private List<GameObject> m_PathTiles = new List<GameObject>();
    private List<Vector3> m_PathTilePosition = new List<Vector3>();
    private Collider[] m_NearbyColliders = new Collider[100];
    private float m_PlayerYPos;
    private Troop m_CurrentIntance;
    private GameObject m_CurrentObject;
    private UnitType m_CurrentUnitType;
    private int cornerId = 1;
    private Vector3[] m_CornerPositions;
    private NavMeshPath newpath;
    private bool m_PlacedOnNavmesh;


    private void Awake()
    {

        m_NavMeshAgent.enabled = false;
        newpath = new NavMeshPath();
        m_CurrentIntance = this.GetComponent<Troop>();
        m_CurrentObject = this.gameObject;
        m_CurrentUnitType = m_TroopCard._UnitData.Type;
        _CornerTiles = new List<Transform>();

    }

    private void Start()
    {
        m_NavMeshAgent.updateRotation = false;
        foreach (Transform corner in TilesManager._Instance._CornerTiles)
        {
            _CornerTiles.Add(corner);
        }
    }
    private void FixedUpdate()
    {
        Quaternion targetrot = GetCorner(transform.position).rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetrot, Time.deltaTime * 4f);
    }

    

    // to move troop once dropped
    public void TriggerMove()
    {
       StartCoroutine(MoveTroop());
    }

    public void KillTroop()
    {
        if (_CanKillTroop)
        {
            return;
        }
        if(m_TroopHealth > 0f)
        {
            m_TroopHealth -= m_healthlose * Time.deltaTime;
            if (m_HealthBar)
            {
                m_HealthBar.UpdateHealth((m_TroopHealth / m_TroopCard._UnitData._Health));
            }
            return;
        }
        if (m_TroopHealth == 0)
        {
            _TroopDead = true;
        }
        ResetTroop();
    }
    public Transform GetCorner(Vector3 pos)
    {
        if (_CornerTiles.Count > 0)
        {
            if (Vector3.Distance(pos, _CornerTiles[0].position) < 4.5f)
            {
                _CornerTiles.RemoveAt(0);
            }
            if (_CornerTiles.Count == 0)
            {
                return TilesManager._Instance._CornerTiles[TilesManager._Instance._CornerTiles.Count - 1];
            }

            return _CornerTiles[0];
        }
        return TilesManager._Instance._CornerTiles[TilesManager._Instance._CornerTiles.Count - 1];
    }
    public IEnumerator MoveTroop()
    {
        Vector3 targetpos = new Vector3(TilesManager._Instance._SpawningPoint.position.x, m_PlayerYPos, TilesManager._Instance._SpawningPoint.position.z);
        this.transform.position = targetpos;
        m_NavMeshAgent.enabled = true;
        yield return new WaitForSeconds(0.5f);
        if (m_NavMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent is not assigned.");
        }
        else if (!m_NavMeshAgent.isActiveAndEnabled)
        {
            Debug.LogError("NavMeshAgent is not active or enabled.");
        }
        else if (!m_NavMeshAgent.isOnNavMesh)
        {
            Debug.LogError("NavMeshAgent is not on the NavMesh.");
        }
        else
        {
            m_PlacedOnNavmesh = true;
            m_NavMeshAgent.SetDestination(TilesManager._Instance._CornerTiles[1].position);
        }
    }

    public void ResetTroop()
    {
        Vector3 targetpos = new Vector3(TilesManager._Instance._SpawningPoint.position.x, m_PlayerYPos, TilesManager._Instance._SpawningPoint.position.z);

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        transform.gameObject.SetActive(false);
        transform.rotation = targetRotation;
        transform.position = targetpos;
        m_TroopHealth = m_TroopCard._UnitData._Health;
        _TroopDead = true;
        m_PlacedOnNavmesh = false;
        m_HealthBar.ResetHealthBar();
        _CornerTiles.Clear();
        foreach (Transform corner in TilesManager._Instance._CornerTiles)
        {
            _CornerTiles.Add(corner);
        }
        PoolManager._instance._UnitPoolDict[m_TroopCard._UnitData.Type].Enqueue(this.gameObject);

    }

    //interface methods 

    private bool _IsSelected;
    public void SetSelected(bool selected)
    {
        _IsSelected = selected;
    }
    public bool IsSelected => _IsSelected;
    public UnitType GetUnitType() => m_CurrentUnitType;
    public GameObject GetGameObject() => gameObject;

    public bool IsSameItemNearby(Vector3 pos)
    {
        m_NearbyColliders = Physics.OverlapSphere(pos, 1f);
        if (m_NearbyColliders.Length > 0)
        {
            foreach (var collider in m_NearbyColliders)
            {
                if (collider.gameObject.GetComponent<Troop>())
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void DropItem(GameObject troop, Vector3 p, GameObject lookatobj)
    {
        if (troop == this.gameObject)
        {
            EventActions.CheckCanSwipe.Invoke(true);
            this.gameObject.SetActive(true);
            UnitsUIManager.Instance.DropUnit(EventActions._SelectedUnitType);
            m_CurrentIntance._TroopDead = false;
            m_CurrentIntance.TriggerMove();
            EventActions.CheckCanSwipe.Invoke(false);
        }

    }

    public void HealTroop()
    {
        print("Healing Troop....");
    }

    private bool HasReachedCorner()
    {
        if (m_NavMeshAgent.hasPath && m_NavMeshAgent.path.corners.Length > 1)
        {
            Vector3 nextCorner = m_NavMeshAgent.path.corners[1];
            float distanceToCorner = Vector3.Distance(transform.position, nextCorner);

            // Consider we've reached corner if we're within a small distance
            return distanceToCorner < 0.1f; // Adjust threshold as needed
        }
        return false;
    }


    private void OnDrawGizmos()
    {
        if (m_NavMeshAgent != null && m_NavMeshAgent.hasPath)
        {
            // Draw path corners
            Gizmos.color = Color.red;
            Vector3 previousCorner = transform.position;
            foreach (Vector3 corner in m_NavMeshAgent.path.corners)
            {
                Gizmos.DrawLine(previousCorner, corner);
                Gizmos.DrawSphere(corner, 2f);
                previousCorner = corner;
            }
        }
    }

}
