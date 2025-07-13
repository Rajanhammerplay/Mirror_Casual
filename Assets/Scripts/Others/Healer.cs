using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour,ISpecialItem
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

    void ISpecialItem.ShowSpecialItem()
    {
        HealerActivated();
    }

    void ISpecialItem.HideSpecialItem()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator HealerActivated()
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(DefaultValues.HEALER_LIFE_TIME);
        gameObject.SetActive(false);
    }
}

