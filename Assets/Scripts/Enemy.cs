using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyCard m_EnemyData;

    [SerializeField] HelathBar m_HealthBar;

    public float m_EnemyHealth;


    private void Start()
    {
        m_EnemyHealth = m_EnemyData._Health;
    }
    public void KillEnemy()
    {
        if(m_EnemyHealth > 0f)
        {
            m_EnemyHealth -= 0.6f;
            m_HealthBar.UpdateHealth((m_EnemyHealth/m_EnemyData._Health));
            return;
        }
        Destroy(this.gameObject);
    }
}
