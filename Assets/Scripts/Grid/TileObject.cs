using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileObject : MonoBehaviour
{
  public Tile3d tile;
  private Tilemap m_Tilemap;

    private void Start()
    {
        m_Tilemap = this.GetComponentInParent<Tilemap>();
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

    public void HighLightTile(float value)
    {
      this.GetComponent<Renderer>().material.SetFloat("_BlendStrength",value);
    }
}

public class TileInfo
{
    public TileData TileData;
    public Vector3Int tilepos;
    public Vector3 tileworldpos;
}
