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

    private void DrawCircleAround(Vector3 pos,bool showhealer)
    {
        if (showhealer == false)
        {
            m_CurrentObject.SetActive(showhealer);
            return;
        }
        m_CurrentObject.SetActive(showhealer);
        Vector3 newpos = Vector3.Lerp(m_CurrentObject.transform.position, pos, 1.6f);
        m_CurrentObject.transform.position = newpos;
    }

    Troop troop;

    private void OnTriggerEnter(Collider other)
    {
        troop = other.transform.GetComponent<Troop>();
        if (troop != null) 
        { 
            troop._CanKillTroop = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        troop = other.transform.GetComponent<Troop>();
        if (troop != null)
        {
            troop._CanKillTroop = false;
        }
    }
}

