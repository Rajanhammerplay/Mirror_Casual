using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Defines;

public class Mirror : MonoBehaviour,IUnitItem
{
    [SerializeField] private VariableJoystick m_JoyStick;
    [SerializeField] private GameObject _Lever;
    [SerializeField] private Vector3 _LeverScaleFactor;
    [SerializeField] private float m_MinThreshHold;
    [SerializeField] private float m_MaxThreshHold;
    [SerializeField] private float _OtherMirrorInRadius;
    
    private float rotangle;
    private float touchdiff = 0;
    private bool canswipe;
    private bool canrotate;
    private Vector3 leverInitalScale;
    private Vector2 Inputpos;
    private Camera m_MainCamera;
    private Ray m_ray;
    private RaycastHit m_Hit;
    private Vector3 m_Intialrot;
    private int touchId = -1;
    private Vector2 startTouchPos;

    private const float MIN_ROTATION = -30f;
    private const float MAX_ROTATION = 30f;
    private const float SWIPE_THRESHOLD = 0.02f;
    private const float SCALE_DURATION = 0.6f;

    public GameObject _Mirror;
    public Defines.UnitType MirrorType;
    public float _RotSpeed = 40f;
    public float yPos;
    public bool _IsSelected = false;
    public float InitAngle = 0;

    private void Awake()
    {
      UnitsManager._Instance.RegisterUnit(this);
    }

    private void Start()
    {
        if(_Lever != null)
        {
            leverInitalScale = _Lever.transform.localScale;
            m_Intialrot = _Lever.transform.localEulerAngles;
        }
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
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        HandleTouchDown(touch);
                        break;
                    case TouchPhase.Moved:
                        if (canswipe && touch.fingerId == touchId)
                        {
                            HandleTouch(touch);
                        }
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        if (touch.fingerId == touchId)
                        {
                            HandleTouchUp();
                        }
                        break;
                }
            }
        }
        // Handle mouse input for editor testing
        else if (Application.isEditor)
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


        }
        if (_IsSelected && canrotate)
        {
            RotateMirror();
        }
    }
    //Input Event Callbacks
    private void HandleTouchDown(Touch touch)
    {
        m_ray = m_MainCamera.ScreenPointToRay(touch.position);
        if (Physics.Raycast(m_ray, out m_Hit))
        {
            if (m_Hit.collider.GetComponent<Mirror>() == this)
            {
                touchId = touch.fingerId;
                startTouchPos = touch.position;
                canswipe = true;
                SetSelected(true);
            }
        }
    }

    private void HandleTouch(Touch touch)
    {
        // Calculate touch difference based on screen width for consistent behavior across devices
        float touchDelta = (touch.position.x - startTouchPos.x) / Screen.width;
        touchdiff = touchDelta * 100f; // Scale factor for more noticeable movement
       // print("touch diff: " + touchdiff);
        if (Mathf.Abs(touchdiff) > SWIPE_THRESHOLD)
        {
            canrotate = true;
        }
        else
        {
            canrotate = false;
        }

        // Update start position for next frame
        startTouchPos = touch.position;
    }

    private void HandleTouchUp()
    {
        canrotate = false;
        canswipe = false;
        touchId = -1;
        SetSelected(false);
        touchdiff = 0f;
    }
    private void HandleMouseDown()
    {
        Inputpos = Input.mousePosition;
        m_ray = m_MainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(m_ray, out m_Hit);
        if (m_Hit.collider.GetComponent<Mirror>() == this)
        {
            canswipe = true;
            SetSelected(true);
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
        SetSelected(false);
    }

    ////select mirror
    //public void SelectMirror(GameObject target)
    //{
    //    //EventActions._SelectMirror.Invoke(target);
    //}

    //private void CheckMirror(GameObject target)
    //{
    //    if (gameObject == target)
    //    {
    //        if (this._IsSelected)
    //        {
    //            return;
    //        }
    //        this._IsSelected = true;
    //        this.ScaleLever(true);
    //    }
    //    else
    //    {
    //        this._IsSelected = false;
    //        this.ScaleLever(false);
    //    }
    //}

    //to scaling lever 
    public void ScaleLever(bool scale)
    {
        if(_Lever == null)
        {
            return;
        }

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

        if (_Lever == null)
        {
            return;
        }
        float clampedrot = rotangle + touchdiff * _RotSpeed * Time.deltaTime;
        rotangle = Mathf.Clamp(clampedrot, MIN_ROTATION, MAX_ROTATION);
        _Mirror.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, clampedrot));
        _Lever.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, clampedrot));
        float dist = Vector3.Distance(m_Intialrot, new Vector3(0, 0, clampedrot));
    }

    //interface methods
    public void SetSelected(bool selected)
    {
        _IsSelected = selected;
        EventActions.CheckCanSwipe.Invoke(_IsSelected);
        ScaleLever(selected);
    }

    public bool IsSelected => _IsSelected;

    public UnitType GetUnitType() => MirrorType;

    public GameObject GetGameObject() => gameObject;

    public bool IsSameItemNearby(Vector3 pos)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, _OtherMirrorInRadius);
        if (colliders.Length > 0)
        {
            foreach (var collider in colliders)
            {
                if (collider.gameObject.tag == "mirror")
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void DropItem(GameObject go, Vector3 p, GameObject lookatobj)  //to Drop mirror Item
    {
        if (go == this.gameObject && IsSameItemNearby(p))
        {
            EventActions.CheckCanSwipe.Invoke(true);
            this.gameObject.SetActive(true);
            this.gameObject.transform.position = p;
            Vector3 currentrot = this.transform.localEulerAngles;
            Vector3 lookatrot = new Vector3(currentrot.x, (lookatobj.transform.localEulerAngles.y - 90), currentrot.z);
            this.transform.eulerAngles = lookatrot;
            UnitsUIManager.Instance.DropUnit(EventActions._SelectedUnitType);
            EventActions.CheckCanSwipe.Invoke(false);
            InitAngle = _Mirror.transform.rotation.y;
        }
    }


}

