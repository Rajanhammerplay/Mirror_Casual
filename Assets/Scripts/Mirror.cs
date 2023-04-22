using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{

    float rotangle;

    public float _RotSpeed = 80f;

    public bool _IsSelected = false;
    // Update is called once per frame
    void Update()
    {
        if (_IsSelected && Input.GetAxis("Horizontal")!=0)
        {
            RotateMirror();
        }
    }

    //to rotate mirror
    public void RotateMirror()
    {
            float clampedrot = rotangle + Input.GetAxis("Horizontal") * _RotSpeed * Time.deltaTime;
            rotangle = Mathf.Clamp(clampedrot, -30f, 30f);
            transform.localRotation = Quaternion.Euler(new Vector3(0, clampedrot, -4.221f));
    }
}
