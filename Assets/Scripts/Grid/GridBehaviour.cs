using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridBehaviour : MonoBehaviour
{
    [Header("Grid Data")]
    [SerializeField] private int m_Height;
    [SerializeField] private int m_Length;

    [SerializeField] private int m_TowerRow;
    [SerializeField] private int m_TowerColumn;

    [Header("Tilemap and grid")]
    [SerializeField] private TileBase m_GroundTileBase;
    [SerializeField] private TileBase m_TowerTileBase;
    [SerializeField] private List<GameObject> m_GridObjects;

    Tilemap m_Tilemap;
    GridMap m_Grid;

    // Start is called before the first frame update
    void Start()
    {
        m_Tilemap = GetComponent<Tilemap>();
        m_Grid = GetComponent<GridMap>();

        m_Grid.init(m_Height, m_Length);
        //m_Grid.Set(m_TowerRow, m_TowerColumn,true);
        //UpdateGrid();
    }

    //public void SetupGridData()
    //{
    //    (int rowCount, int columnCount) = GetGridDimensions(gameObjects);
    //    Debug.Log("Rows: " + rowCount + ", Columns: " + columnCount);
    //}

    // Function to calculate grid dimensions based on GameObjects positions
    (int, int) GetGridDimensions(List<GameObject> gameObjects)
    {
        if (gameObjects.Count == 0)
        {
            return (0, 0);  // Return 0, 0 if the list is empty
        }

        float minX = float.MaxValue, maxX = float.MinValue;
        float minZ = float.MaxValue, maxZ = float.MinValue;

        // Loop through the game objects and find the min/max X and Z coordinates
        foreach (GameObject obj in gameObjects)
        {
            Vector3 worldPos = obj.transform.position;
            minX = Mathf.Min(minX, worldPos.x);
            maxX = Mathf.Max(maxX, worldPos.x);
            minZ = Mathf.Min(minZ, worldPos.z);
            maxZ = Mathf.Max(maxZ, worldPos.z);
        }

        // Calculate the number of rows and columns based on the min/max coordinates
        int rowCount = Mathf.FloorToInt(maxX - minX) + 1;
        int columnCount = Mathf.FloorToInt(maxZ - minZ) + 1;

        return (rowCount, columnCount);
    }

    public void UpdateGrid()
    {
        for(int r = 0; r < m_Grid._Length; r++)
        {
            for (int c = 0; c < m_Grid._Height; c++)
            {
                Vector3Int cellPosition = new Vector3Int(r, c, 0);
                if (m_Grid.Get(r,c) == true)
                {
                    m_Tilemap.SetTile(cellPosition, m_TowerTileBase);
                    
                }
                else
                {
                    m_Tilemap.SetTile(cellPosition, m_GroundTileBase);
                }
            }
        }
    }

    public void SetTileState(int r,int c,bool state)
    {
        m_Grid.Set(r,c,state);
    }

}
