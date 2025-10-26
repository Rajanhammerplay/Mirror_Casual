using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{
    public bool isRaycasted = false;
    public int m_MirrorPoolSize;
    public LayerMask LayerMaskToIgnore;
    public PoolManager m_PoolManager;
    public float _Radius;
    public static InputManager _Instance;

    [SerializeField] private GameObject m_Mirror;
    [SerializeField] private GameObject m_TroopSelectionUI;
    [SerializeField] private TileObject m_PlayerPlacable;

    private Vector3 m_HitPoint;
    private Tilemap Tilemap;
    private bool m_TroopSelectionActiveted;
    private Camera m_MainCamera;
    private Ray m_ray;
    private RaycastHit m_Hit;
    private Vector3 uiShowScale = new Vector3(0.96f, 0.94f, 0f);
    private Vector3 uiHideScale = Vector3.zero;
    private float showDuration = 0.8f;
    private float hideDuration = 0.1f;
    private PointerEventData m_PointerEventData;
    private List<RaycastResult> m_RaycastResults;

    private void Awake()
    {
        m_PointerEventData = new PointerEventData(EventSystem.current);
        m_RaycastResults = new List<RaycastResult>();
        _Instance = GetComponent<InputManager>();
    }
    private void Start()
    {
        Tilemap = GetComponent<Tilemap>();
        m_MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        HandleInputs();
    }

    public void HandleInputs()
    {
        if (EventActions._SelectedUnitType == Defines.UnitType.Troop_1 || EventActions._SelectedUnitType == Defines.UnitType.Troop_2)
        {
            m_PlayerPlacable.HighLightTile(true);
        }
        else
        {
            m_PlayerPlacable.HighLightTile(false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI() || m_TroopSelectionActiveted)
            {
                return;
            }

            Vector3 pos = Vector3.zero;
            m_ray = m_MainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(m_ray, out m_Hit,300);

            if (EventActions._SelectedUnitType != Defines.UnitType.none)
            {
                if (Defines.IsMirrorType(EventActions._SelectedUnitType))
                {
                    if (m_Hit.collider.transform.GetComponent<TileObject>())
                    {
                        Vector3 tilepos = m_Hit.collider.transform.GetComponent<TileObject>().GetTileData().tileworldpos;
                        GameObject tilerot = m_Hit.collider.gameObject;

                        if (UnitsManager._Instance.IsPlacableTile(m_Hit.collider.gameObject, EventActions._SelectedUnitType))
                        {
                            pos = new Vector3(tilepos.x, 1.708048f, tilepos.z);
                            UnitsManager._Instance.DropUnit(pos,tilerot);
                        }
                    }
                }
                else
                {
                    if (m_Hit.collider.GetComponent<Mirror>())
                    {
                        return;
                    }
                    if (UnitsManager._Instance.IsPlacableTile(m_Hit.collider.gameObject, EventActions._SelectedUnitType))
                    {
                        //Vector3 tilepos = m_PlayerPlacable.GetTileData().tileworldpos;
                        //pos = new Vector3(tilepos.x, 1.708048f, tilepos.z);
                        //UnitsManager._Instance.DropUnit(pos, this.gameObject);
                    }
                }
            }

        }
    }

    //to check only clicking on UI
    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;

        m_PointerEventData.position = Input.mousePosition;
        m_RaycastResults.Clear();
        EventSystem.current.RaycastAll(m_PointerEventData, m_RaycastResults);
        for (int i = 0; i < m_RaycastResults.Count; i++)
        {
            if (m_RaycastResults[i].gameObject.GetComponent<UnityEngine.UI.Image>() != null)
            {
                EventActions.CheckCanSwipe.Invoke(true);
                return true;
            }
        }
        EventActions.CheckCanSwipe.Invoke(false);
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(m_HitPoint, _Radius);
    }

    //to open and close unit Selection Panel
    public void UnitSelectionUIInteractions(bool active)
    {
        m_TroopSelectionActiveted = active;
        var targetScale = active ? uiShowScale : uiHideScale;
        var duration = active ? showDuration : hideDuration;
        m_TroopSelectionUI.transform.DOScale(targetScale, duration);
    }
}



