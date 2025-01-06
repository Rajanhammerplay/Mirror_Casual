using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine.Tilemaps;
using UnityEngine;

[CreateAssetMenu(fileName = "New 3D Tile", menuName = "Tiles/3D Tile")]
public class Tile3d : Tile
{
    public GameObject tilePrefab;
    public Tilemap tilemap;

    public override void RefreshTile(Vector3Int position, ITilemap itilemap)
    {
        base.RefreshTile(position, itilemap);
        tilemap = itilemap.GetComponent<Tilemap>();
        Vector3 worldPosition = tilemap.CellToWorld(position);
        GameObject tileObject = GameObject.Instantiate(tilePrefab, worldPosition, Quaternion.identity);
        tileObject.transform.SetParent(tilemap.transform);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
    }
}