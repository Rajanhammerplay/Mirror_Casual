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
    private float touchdiff = 0;
    private bool canswipe;
    private bool canrotate;
    private Vector3 leverInitalScale;
    private Vector2 Inputpos;
    private Camera m_MainCamera;
    private Ray m_ray;
    private RaycastHit m_Hit;

    private const float MIN_ROTATION = -30f;
    private const float MAX_ROTATION = 30f;
    private const float SWIPE_THRESHOLD = 0.02f;
    private const float SCALE_DURATION = 0.6f;

    public GameObject _Mirror;
    public float _RotSpeed = 40f;
    public float yPos;
    public bool _IsSelected = false;

    private void Start()
    {
        leverInitalScale = _Lever.transform.localScale;
        m_MainCamera = Camera.main;
    }


    // Update is called once per frame
    void Update()
    {
        GetRoatationAxis();
    }

    //get rotation axis from swipe
    private void GetRoatationAxis()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
        }
        else if (Input.GetMouseButton(0) && canswipe)
        {
            HandleMouse();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleMouseUp();
        }

        if (_IsSelected && canrotate)
        {
            RotateMirror();
        }
    }
    //Input Event Callbacks
    private void HandleMouseDown()
    {
        Inputpos = Input.mousePosition;
        m_ray = m_MainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(m_ray, out m_Hit);
        if (m_Hit.collider.GetComponent<Mirror>() == this)
        {
            ScaleLever(true);
            canswipe = true;
        }
    }

    private void HandleMouse()
    {
        touchdiff = (Input.mousePosition.x - Inputpos.x) * Time.deltaTime;
        float swipedir = MathF.Sign(touchdiff);
        if (MathF.Abs(touchdiff) > SWIPE_THRESHOLD)
        {
            canrotate = true;
        }
        else
        {
            canrotate = false;
        }
    }

    private void HandleMouseUp()
    {
        canrotate = false;
        canswipe = false;
        ScaleLever(false);
    }

    //to scaling lever 
    public void ScaleLever(bool scale)
    {
        if (scale) 
        {
            _Lever.transform.DOScale((_Lever.transform.localScale + _LeverScaleFactor),SCALE_DURATION);
        }
        else
        {
            _Lever.transform.localScale = new Vector3(1,1,1);
        }
    }

    //to rotate mirror
    public void RotateMirror()
    {
        float clampedrot = rotangle + touchdiff * _RotSpeed * Time.deltaTime;
        rotangle = Mathf.Clamp(clampedrot, MIN_ROTATION, MAX_ROTATION);
        _Mirror.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, clampedrot));
        _Lever.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, clampedrot));
    }

    //to Drop mirror Item
    public void DropItem(GameObject go, Vector3 p, GameObject lookatobj)
    {
        if (go == this.gameObject)
        {
            this.gameObject.SetActive(true);
            this.gameObject.transform.position = p;
            Vector3 currentrot = this.transform.localEulerAngles;
            Vector3 lookatrot = new Vector3(currentrot.x, (lookatobj.transform.localEulerAngles.y - 90), currentrot.z);
            this.transform.eulerAngles = lookatrot;
            UnitsManager.Instance.DropUnit(EventActions._SelectedUnitType);
        }
    }
}

