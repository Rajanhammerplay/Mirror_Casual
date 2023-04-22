using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CountText;

    [SerializeField] private MirrorTypes _Mirror_Types;

    // Start is called before the first frame update
    void Start()
    {
        CountText.text = _Mirror_Types._MirrorCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
