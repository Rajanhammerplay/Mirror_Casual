using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour,IIUnityItem
{

    float rotangle;

    public float _RotSpeed = 80f;

    public bool _IsSelected = false;

    public float yPos;



    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        print("joy stick input:" + PoolManager._instance._JoyStick.input.x);
        if (_IsSelected && PoolManager._instance._JoyStick.input.x != 0)
        {
            RotateMirror();
        }

        
    }

    //to rotate mirror
    public void RotateMirror()
    {
        float clampedrot = rotangle + PoolManager._instance._JoyStick.input.x * _RotSpeed * Time.deltaTime;
        rotangle = Mathf.Clamp(clampedrot, -30f, 30f);
        transform.localRotation = Quaternion.Euler(new Vector3(0, clampedrot, -4.221f));
    }

    public void DropItem(GameObject go, Vector3 p)
    {
        if (go == this.gameObject)
        {
            this.gameObject.SetActive(true);
            PoolManager._instance._JoyStick.gameObject.SetActive(true);
            this.gameObject.transform.position = p;
            UnitsManager.Instance.DropUnit(EventActions._SelectedUnitType);
        }
    }
}

