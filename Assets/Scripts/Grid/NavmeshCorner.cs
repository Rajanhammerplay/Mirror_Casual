using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshCorner : MonoBehaviour
{
    [SerializeField] private NavMeshSurface m_navmeshsurface;

    private void Start()
    {
        SetupCorners();
    }
    private void SetupCorners()
    {
       NavMeshTriangulation navmtri = NavMesh.CalculateTriangulation();
        foreach (Vector3 pos in navmtri.vertices) 
        {

        }
    }
}
