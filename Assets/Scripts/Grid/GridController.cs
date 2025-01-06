using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    [SerializeField] Tilemap m_TargetTilemap;
    [SerializeField] GridBehaviour m_GridBehaviour;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetTile();
        }
    }
    public void SetTile()
    {
        Vector3Int MousePointGridPos = m_TargetTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        m_GridBehaviour.SetTileState(MousePointGridPos.x, MousePointGridPos.y,true);
        m_GridBehaviour.UpdateGrid();
    }

    public void GetTile()
    {
        Vector3Int MousePointGridPos = m_TargetTilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}
