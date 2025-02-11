using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    [SerializeField] float HealerMovespeed = 1.6f;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float radius;

    private GameObject m_CurrentObject;
    public int segments = 36;
    public bool _ShowHealer;

    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        m_CurrentObject = this.gameObject;
        m_CurrentObject.SetActive(false);
    }

    //public void DrawCircleAround(Vector3 pos)
    //{
    //    if (_ShowHealer == false)
    //    {
    //        m_CurrentObject.SetActive(_ShowHealer);
    //        return;
    //    }
    //    m_CurrentObject.SetActive(_ShowHealer);
    //    Vector3 newpos = Vector3.Lerp(m_CurrentObject.transform.position, pos, HealerMovespeed);
    //    m_CurrentObject.transform.position = newpos;
    //}

    //private void UpdateStatus(bool stat)
    //{
    //    _ShowHealer = stat;
    //}

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

