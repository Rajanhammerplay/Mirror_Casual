using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Troop : MonoBehaviour
{
    [SerializeField] EnemyCard m_EnemyData;

    [SerializeField] HelathBar m_HealthBar;

    public float m_EnemyHealth;
    private float m_PlayerYPos;
    [Header("Movement Props")]
    Tilemap m_PathTileMap;
    private List<GameObject> m_PathTiles;
    private List<Vector3> m_PathTilePosition = new List<Vector3>();
    [SerializeField] private float m_TroopSpeed;


    private void Start()
    {
        m_EnemyHealth = m_EnemyData._Health;
        m_PlayerYPos = this.transform.position.y;
        m_PathTileMap = GameObject.Find("Pathparent")?.GetComponent<Tilemap>();

        SetPath();
        this.gameObject.transform.position = m_PathTilePosition[0];
        StartCoroutine(MoveEnemy());
        //StartCoroutine(DelayedSetup());
    }

    //private IEnumerator DelayedSetup()
    //{

    //}

    public void KillEnemy()
    {
        if(m_EnemyHealth > 0f)
        {
            m_EnemyHealth -= 0.6f;
            m_HealthBar.UpdateHealth((m_EnemyHealth/m_EnemyData._Health));
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

    public IEnumerator MoveEnemy()
    {
        foreach (Vector3 tiletransformpos in m_PathTilePosition)
        {
            Vector3 Troopos = new Vector3(this.transform.position.x,m_PlayerYPos, this.transform.position.z);
            Vector3 targetpos = new Vector3(tiletransformpos.x,m_PlayerYPos,tiletransformpos.z);
            transform.LookAt(targetpos);
            float movementprogress = 0f;
            while(movementprogress < m_TroopSpeed)
            {
                movementprogress += Time.deltaTime * m_TroopSpeed;
                this.transform.position = Vector3.Lerp(Troopos, targetpos,movementprogress);
                yield return new WaitForEndOfFrame();

            }

        }
    }

}
