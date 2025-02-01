using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//responsible for troop 
public class Troop : MonoBehaviour, IIUnityItem
{
    [SerializeField] UnitItem m_TroopCard;
    [SerializeField] HelathBar m_HealthBar;
    [SerializeField] float m_TroopSpeed;
    
    public float m_TroopHealth;
    public float m_healthlose;
    public Canvas m_HealthBarCanvas;

    [Header("Movement Props")]

    private Tilemap m_PathTileMap;
    private List<GameObject> m_PathTiles = new List<GameObject>();
    private List<Vector3> m_PathTilePosition = new List<Vector3>();
    private Collider[] m_NearbyColliders = new Collider[100];
    private float m_PlayerYPos;
    private Troop m_CurrentIntance;
    private GameObject m_CurrentObject;

    private void Awake()
    {
        m_CurrentIntance = this.GetComponent<Troop>();
        m_CurrentObject = this.gameObject;
    }

    public void SetupTroop()
    {
        m_TroopHealth = m_TroopCard._UnitData._Health;
        m_PathTileMap = PoolManager._instance._Pathparent?.GetComponent<Tilemap>();

        SetPath();

        m_CurrentObject.transform.position = new Vector3(m_PathTilePosition[0].x, 2.08048f, m_PathTilePosition[0].z);
        m_PlayerYPos = m_CurrentObject.transform.position.y;
    }

    // to move troop once dropped
    public void TriggerMove()
    {
        StartCoroutine(MoveTroop());
    }

    public void KillTroop()
    {

        if(m_TroopHealth > 0f)
        {
            m_TroopHealth -= m_healthlose * Time.deltaTime;
            if (m_HealthBar)
            {
                m_HealthBar.UpdateHealth((m_TroopHealth / m_TroopCard._UnitData._Health));
            }
            return;
        }
        ResetTroop();
    }

    public void SetPath()
    {
        foreach (Transform tiletransform in m_PathTileMap.transform)
        {
            TileObject tileobject = tiletransform.GetComponent<TileObject>();
            m_PathTiles.Add(tiletransform.gameObject);
            m_PathTilePosition.Add(tileobject.GetTileData().tileworldpos);
        }
    }

    public void ClearPath()
    {
        m_PathTiles.Clear();
        m_PathTilePosition.Clear(); 
    }

    public IEnumerator MoveTroop()
    {
        int i = 0;
        foreach (Vector3 tiletransformpos in m_PathTilePosition)
        {
            Vector3 Troopos = new Vector3(transform.position.x, m_PlayerYPos, transform.position.z);
            Vector3 targetpos = new Vector3(tiletransformpos.x, m_PlayerYPos, tiletransformpos.z);

            Quaternion targetRotation = Quaternion.LookRotation( m_PathTiles[i].transform.forward, Vector3.up);
            m_CurrentObject.transform.rotation = targetRotation;

            m_NearbyColliders = Physics.OverlapSphere(Troopos, 1f);
            if (m_NearbyColliders.Length > 0)
            {
                // Add a random offset to avoid overlapping
                float randomAngle = Random.Range(0f, 360f);
                float offsetDistance = 1.5f; // Adjust this value for spacing
                Vector3 offset = new Vector3(
                    Mathf.Cos(randomAngle * Mathf.Deg2Rad) * offsetDistance,
                    0f,
                    Mathf.Sin(randomAngle * Mathf.Deg2Rad) * offsetDistance
                );
                targetpos += offset;
            }

            float movementprogress = 0f;
            while (Vector3.Distance(m_CurrentObject.transform.position, targetpos) > 0.01f)
            {
                movementprogress += Time.deltaTime * m_TroopSpeed;
                m_CurrentObject.transform.position = Vector3.Lerp(Troopos, targetpos, Mathf.Clamp01(movementprogress));
                yield return new WaitForEndOfFrame();
            }
            i++;

        }
    }

    public void ResetTroop()
    {
        Vector3 targetpos = new Vector3(m_PathTilePosition[0].x, m_PlayerYPos, m_PathTilePosition[0].z);

        Quaternion targetRotation = Quaternion.LookRotation(m_PathTiles[0].transform.forward, Vector3.up);
        m_CurrentObject.transform.gameObject.SetActive(false);
        m_CurrentObject.transform.rotation = targetRotation;
        m_CurrentObject.transform.position = targetpos;
        m_TroopHealth = m_TroopCard._UnitData._Health;
        m_HealthBar.ResetHealthBar();
        m_CurrentIntance.ClearPath();
        PoolManager._instance._UnitPoolDict[m_TroopCard._UnitData.Type].Enqueue(m_CurrentObject);
    }

    public void DropItem(GameObject troop, Vector3 p, GameObject lookatobj)
    {
        if (troop == m_CurrentObject)
        {
            m_CurrentObject.gameObject.SetActive(true);
            UnitsManager.Instance.DropUnit(EventActions._SelectedUnitType);

            m_CurrentIntance.SetupTroop();
            m_CurrentIntance.TriggerMove();
        }

    }

}
