using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileObject : MonoBehaviour
{
    private const string NOISETEX_ON = "NOISETEX_ON";
    private const string EDGE_IMPACTMAX = "_EdgeImapctMax";

    private Tilemap m_Tilemap;
    private float edgeintesnse;
    private bool canshowedges;
    private TileObject m_TileObject;
    private Renderer m_Renderer;

    public Vector2 EdgeBounds;
    public Tile3d tile;
    public GameObject _LookatObject;
    public TileTypes _Tiletype;

    private void Start()
    {
        m_TileObject = GetComponent<TileObject>();
        m_Tilemap = m_TileObject.GetComponentInParent<Tilemap>();
        m_Renderer = m_TileObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        if (canshowedges)
        {
           float sineWave = Mathf.Sin(Time.time * 2f); // The 2f factor controls the oscillation speed
           edgeintesnse = Mathf.Lerp(EdgeBounds.x, EdgeBounds.y, (sineWave + 1f) * 0.5f);
           m_Renderer.material.SetFloat("_EdgeImapctMax", edgeintesnse);
        }
    }




    public TileInfo GetTileData()
    {
        TileData tileData = new TileData();
        TileInfo tileInfo = new TileInfo();
        Vector3Int tilepos = new Vector3Int();

        if (m_Tilemap != null)
        {
            tilepos = m_Tilemap.WorldToCell(m_TileObject.gameObject.transform.position);
            
            m_TileObject.tile.GetTileData(tilepos, m_Tilemap, ref tileData);
            
            tileInfo.TileData = tileData;
        }

        tileInfo.tilepos = tilepos;
        tileInfo.tileworldpos = m_TileObject.transform.position;
        tileInfo.tilerot = m_TileObject.transform.localEulerAngles;
        return tileInfo;
    }

    public void HighLightTile(bool value)
    {
        if (value) 
        {
           canshowedges = true;
           m_Renderer.material.EnableKeyword(NOISETEX_ON);
        } 
        else
        {
           canshowedges = false;
           m_Renderer.material.SetFloat(EDGE_IMPACTMAX, 0);
           m_Renderer.material.DisableKeyword(NOISETEX_ON);
        }

    }
}

public class TileInfo
{
    public TileData TileData;
    public Vector3Int tilepos;
    public Vector3 tileworldpos;
    public Vector3 tilerot;
}

[Serializable]
public enum TileTypes
{
    ground,
    path,
    tower
}