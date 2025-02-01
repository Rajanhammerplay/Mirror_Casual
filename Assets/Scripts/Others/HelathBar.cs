using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelathBar : MonoBehaviour
{
    [SerializeField] private Image m_HealthBar;
    [SerializeField] private Gradient healthGradient;

    private float initialfillamount = 0;
    private Transform m_CurrentTransform;
    void Start()
    {
        m_CurrentTransform = transform;
        initialfillamount = m_HealthBar.fillAmount;
    }

    void LateUpdate()
    {
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        m_CurrentTransform.rotation = Quaternion.LookRotation(m_CurrentTransform.position - PoolManager._instance.m_UICamera.transform.position);
    }
    public void UpdateHealth(float health)
    {
        m_HealthBar.fillAmount = health;
        UpdateHealthBarColor(health);
    }

    public void ResetHealthBar()
    {
        m_HealthBar.fillAmount = initialfillamount;
    }

    private void UpdateHealthBarColor(float health)
    {
        m_HealthBar.color = healthGradient.Evaluate(health);
    }

}
