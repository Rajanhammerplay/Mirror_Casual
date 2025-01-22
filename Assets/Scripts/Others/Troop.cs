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
    

    private float m_PlayerYPos;
    public float m_TroopHealth;
    public Canvas m_HealthBarCanvas;

    [Header("Movement Props")]
    Tilemap m_PathTileMap;
    private List<GameObject> m_PathTiles = new List<GameObject>();
    private List<Vector3> m_PathTilePosition = new List<Vector3>();

    public void SetupTroop()
    {
        m_TroopHealth = m_TroopCard._UnitData._Health;
        m_PathTileMap = GameObject.Find("Pathparent")?.GetComponent<Tilemap>();
        SetPath();
        this.transform.position = new Vector3(m_PathTilePosition[0].x, 1.708048f, m_PathTilePosition[0].z);
        m_PlayerYPos = this.transform.position.y;
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
            m_TroopHealth -= 0.6f;
            m_HealthBar.UpdateHealth((m_TroopHealth/m_TroopCard._UnitData._Health));
            return;
        }
        Destroy(this.gameObject);
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
            transform.rotation = targetRotation;

            Collider[] nearbyColliders = Physics.OverlapSphere(Troopos, 1f);
            if (nearbyColliders.Length > 0)
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

                targetRotation = Quaternion.LookRotation(m_PathTiles[i].transform.forward, Vector3.up);
                transform.rotation = targetRotation;
            }
            float movementprogress = 0f;
            while (movementprogress < m_TroopSpeed)
            {
                movementprogress += Time.deltaTime * m_TroopSpeed;
                transform.position = Vector3.Lerp(Troopos, targetpos, movementprogress);
                yield return new WaitForEndOfFrame();
            }
            i++;

        }
        if (i == m_PathTilePosition.Count)
        {
            ResetTroop();
        }
    }

    public void ResetTroop()
    {
        Vector3 targetpos = new Vector3(m_PathTilePosition[0].x, m_PlayerYPos, m_PathTilePosition[0].z);

        Quaternion targetRotation = Quaternion.LookRotation(m_PathTiles[0].transform.forward, Vector3.up);
        transform.gameObject.SetActive(false);
        transform.rotation = targetRotation;
        transform.position = targetpos;
        this.ClearPath();
        PoolManager._instance._UnitPoolDict[m_TroopCard._UnitData.Type].Enqueue(this.gameObject);
    }

    public void DropItem(GameObject troop, Vector3 p)
    {
        if (troop == this.gameObject)
        {
            this.gameObject.gameObject.SetActive(true);
            UnitsManager.Instance.DropUnit(EventActions._SelectedUnitType);

            this.gameObject.GetComponent<Troop>().SetupTroop();
            this.gameObject.GetComponent<Troop>().TriggerMove();
        }

    }

}
