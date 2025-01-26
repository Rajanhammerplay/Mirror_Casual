using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour,IIUnityItem
{

    [SerializeField] private VariableJoystick m_JoyStick;
    [SerializeField] private GameObject _Lever;
    [SerializeField] private Vector3 _LeverScaleFactor;
    private float rotangle;
    private Vector3 leverInitalScale;
    Vector2 Inputpos;
    bool canswipe;
    bool canrotate;
    float touchdiff = 0;

    public GameObject _Mirror;
    public float _RotSpeed = 80f;
    public bool _IsSelected = false;
    public float yPos;



    private void Start()
    {
        leverInitalScale = _Lever.transform.localScale;
        print("inital scale: "+leverInitalScale);
    }


    // Update is called once per frame
    void Update()
    {
        GetRoatationAxis();

        if (_IsSelected && canrotate)
        {
            RotateMirror();
        }

    }

    //get rotation axis from swipe
    private void GetRoatationAxis()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Inputpos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);
            print("gameobject hitting: "+hit.collider.gameObject.name);
            if (hit.collider.GetComponent<Mirror>() == this)
            {
                ScaleLever(true);
                canswipe = true;
            }
        }

        if (Input.GetMouseButton(0) && canswipe)
        {
            touchdiff = (Input.mousePosition.x - Inputpos.x) * Time.deltaTime;
            float swipedir = MathF.Sign(touchdiff);
            if (MathF.Abs(touchdiff) > 0.02f)
            {
                canrotate = true;
            }
            else
            {
                canrotate = false;
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            canrotate = false;
            canswipe = false;
            ScaleLever(false);
        }
    }

    public void ScaleLever(bool scale)
    {
        if (scale) 
        {
            _Lever.transform.DOScale((_Lever.transform.localScale + _LeverScaleFactor),0.6f);
        }
        else
        {
           // _Lever.transform.DOScale(leverInitalScale, 0.6f);
            _Lever.transform.localScale = new Vector3(1,1,1);
        }
    }

    //to rotate mirror
    public void RotateMirror()
    {
        float clampedrot = rotangle + touchdiff * _RotSpeed * Time.deltaTime;
        rotangle = Mathf.Clamp(clampedrot, -30f, 30f);
        _Mirror.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, clampedrot));
        _Lever.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, clampedrot));
    }

    public void DropItem(GameObject go, Vector3 p, GameObject lookatobj)
    {
        if (go == this.gameObject)
        {
            this.gameObject.SetActive(true);
            this.gameObject.transform.position = p;
            print("rot "+(lookatobj.transform.localEulerAngles.y-90)+"obj: "+lookatobj.transform.name);
            Vector3 currentrot = this.transform.localEulerAngles;
            Vector3 lookatrot = new Vector3(currentrot.x, (lookatobj.transform.localEulerAngles.y - 90), currentrot.z);
            //Vector3 directionToTarget = (lookatobj.transform.position - this.transform.position);
            //Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            //this.transform.rotation = lookRotation;
            this.transform.eulerAngles = lookatrot;
            UnitsManager.Instance.DropUnit(EventActions._SelectedUnitType);
        }
    }
}

