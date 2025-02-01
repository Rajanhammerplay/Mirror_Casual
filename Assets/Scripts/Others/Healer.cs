using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float radius;

    private GameObject m_CurrentObject;
    public int segments = 36;
    private void Awake()
    {
        EventActions._UpdateHealerPos += DrawCircleAround;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_CurrentObject = this.gameObject;
        m_CurrentObject.SetActive(false);
    }

    private void DrawCircleAround(Vector3 pos)
    {
        m_CurrentObject.SetActive(true);
        Vector3 newpos = Vector3.Lerp(m_CurrentObject.transform.position, pos, 0.5f);
        m_CurrentObject.transform.position = newpos;
    }
}
