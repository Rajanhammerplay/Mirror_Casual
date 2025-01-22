using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileObject : MonoBehaviour
{
    

    private Tilemap m_Tilemap;
    private float edgeintesnse;
    private bool canshowedges;

    public Vector2 EdgeBounds;
    public Tile3d tile;

    private void Start()
    {
        m_Tilemap = this.GetComponentInParent<Tilemap>();

    }

    private void Update()
    {
        if (canshowedges)
        {
            float sineWave = Mathf.Sin(Time.time * 2f); // The 2f factor controls the oscillation speed

            edgeintesnse = Mathf.Lerp(EdgeBounds.x, EdgeBounds.y, (sineWave + 1f) * 0.5f);
            this.GetComponent<Renderer>().material.SetFloat("_EdgeImapctMax", edgeintesnse);
        }
    }




    public TileInfo GetTileData()
    {
        TileData tileData = new TileData();
        TileInfo tileInfo = new TileInfo();
        Vector3Int tilepos = new Vector3Int();

        if (m_Tilemap != null)
        {
            tilepos = m_Tilemap.WorldToCell(this.gameObject.transform.position);
            this.tile.GetTileData(tilepos, m_Tilemap, ref tileData);
            tileInfo.TileData = tileData;
        }

        tileInfo.tilepos = tilepos;
        tileInfo.tileworldpos = this.transform.position;
        return tileInfo;
    }

    public void HighLightTile(bool value)
    {
        if (value) 
        {
            canshowedges = true;
            this.GetComponent<Renderer>().material.EnableKeyword("NOISETEX_ON");
        } 
        else
        {
            canshowedges = false;
            this.GetComponent<Renderer>().material.SetFloat("_EdgeImapctMax",0);
            this.GetComponent<Renderer>().material.DisableKeyword("NOISETEX_ON");
        }

    }
}

public class TileInfo
{
    public TileData TileData;
    public Vector3Int tilepos;
    public Vector3 tileworldpos;
}
