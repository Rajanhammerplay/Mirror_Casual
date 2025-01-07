using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Troop : MonoBehaviour
{
    [SerializeField] TroopCard m_TroopData;

    [SerializeField] HelathBar m_HealthBar;

    public float m_TroopHealth;
    private float m_PlayerYPos;
    [Header("Movement Props")]
    Tilemap m_PathTileMap;
    private List<GameObject> m_PathTiles;
    private List<Vector3> m_PathTilePosition = new List<Vector3>();
    [SerializeField] private float m_TroopSpeed;


    private void Start()
    {
        m_TroopHealth = m_TroopData._Health;
        m_PlayerYPos = this.transform.position.y;
        m_PathTileMap = GameObject.Find("Pathparent")?.GetComponent<Tilemap>();

        SetPath();
        this.gameObject.transform.position = m_PathTilePosition[0];
        StartCoroutine(MoveTroop());
        //StartCoroutine(DelayedSetup());
    }

    //private IEnumerator DelayedSetup()
    //{

    //}

    public void KillTroop()
    {
        if(m_TroopHealth > 0f)
        {
            m_TroopHealth -= 0.6f;
            m_HealthBar.UpdateHealth((m_TroopHealth/m_TroopData._Health));
            return;
        }
        Destroy(this.gameObject);
    }

    public void SetPath()
    {

        foreach (Transform tiletransform in m_PathTileMap.transform)
        {
            TileObject tileobject = tiletransform.GetComponent<TileObject>();
            m_PathTilePosition.Add(tileobject.GetTileData().tileworldpos);
            print("Tile pos:" + tileobject.GetTileData().tilepos +" world pos: "+ tileobject.GetTileData().tileworldpos);
        }
    }

    public IEnumerator MoveTroop()
    {
        foreach (Vector3 tiletransformpos in m_PathTilePosition)
        {
            Vector3 Troopos = new Vector3(transform.position.x, m_PlayerYPos, transform.position.z);
            Vector3 targetpos = new Vector3(tiletransformpos.x, m_PlayerYPos, tiletransformpos.z);

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
            }

            transform.LookAt(targetpos);
            float movementprogress = 0f;
            while (movementprogress < m_TroopSpeed)
            {
                movementprogress += Time.deltaTime * m_TroopSpeed;
                transform.position = Vector3.Lerp(Troopos, targetpos, movementprogress);
                yield return new WaitForEndOfFrame();
            }

        }
    }

}
