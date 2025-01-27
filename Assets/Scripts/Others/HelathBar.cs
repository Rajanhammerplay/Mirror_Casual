using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelathBar : MonoBehaviour
{
    [SerializeField] private Image m_HealthBar;

    private float initialfillamount = 0;
    // Start is called before the first frame update
    void Start()
    {
        //m_HealthBar.fillAmount = m_Maxhealth;
        initialfillamount = m_HealthBar.fillAmount;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.LookRotation(this.transform.position - PoolManager._instance.m_UICamera.transform.position);
    }

    public void UpdateHealth(float health)
    {
        m_HealthBar.fillAmount = health;
    }

    public void ResetHealthBar()
    {
        m_HealthBar.fillAmount = initialfillamount;
    }

}
