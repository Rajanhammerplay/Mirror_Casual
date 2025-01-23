using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelathBar : MonoBehaviour
{
    [SerializeField] private Image m_HealthBar;


    // Start is called before the first frame update
    void Start()
    {
        //m_HealthBar.fillAmount = m_Maxhealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealth(float health)
    {
        print("health: "+health);
        m_HealthBar.fillAmount = health;
    }

}
