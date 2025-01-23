using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    public bool isRaycasted = false;
    public int m_MirrorPoolSize;
    public LayerMask LayerMaskToIgnore;
    public PoolManager m_PoolManager;
    public float _Radius;


    [SerializeField] private GameObject m_Mirror;
    [SerializeField] private GameObject m_TroopSelectionUI;

    private Vector3 m_HitPoint;
    private Tilemap Tilemap;
    private bool TroopSelectionActiveted;
    private void Start()
    {
        Tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
    }

    public void HandleInputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI() || TroopSelectionActiveted)
            {
                return;
            }

            Vector3 pos = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);
            if (hit.collider.GetComponent<Mirror>())
            {
                PoolManager._instance.UpdateMirrorStatus(hit.collider.gameObject);
            }

            if(EventActions._SelectedUnitType != TroopType.none)
            {
                if (EventActions._SelectedUnitType == TroopType.Mirror)
                {

                    if (hit.collider.transform.GetComponent<TileObject>())
                    {
                        Vector3 tilepos = hit.collider.transform.GetComponent<TileObject>().GetTileData().tileworldpos;
                        if (UnitsManager.Instance.IsMirrorPlacable(hit.collider.gameObject) && UnitsManager.Instance.IsMirrorDetected(tilepos))
                        {
                            pos = new Vector3(tilepos.x, 1.708048f, tilepos.z);
                            m_PoolManager.DropTroop(pos);
                            print("current type:" + EventActions._SelectedUnitType + "  " + hit.collider.transform.GetComponent<TileObject>().GetTileData().tileworldpos);
                        }
                    }
                }
                else
                {
                    if (hit.collider.GetComponent<Mirror>())
                    {
                        return;
                    }
                    m_PoolManager.DropTroop(pos);
                }
            }
            
        }
    }

    //to check only clicking on UI
    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData pointerevendata = new PointerEventData(EventSystem.current);
        pointerevendata.position = Input.mousePosition;
        List<RaycastResult> raycastresult = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerevendata, raycastresult);
        for (int i = 0; i < raycastresult.Count; i++)
        {
            if (raycastresult[i].gameObject.GetComponent<UnityEngine.UI.Image>() != null)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(m_HitPoint, _Radius);
    }

    //to open and close unit Selection Panel
    public void UnitSelectionUIInteractions(bool active)
    {
        TroopSelectionActiveted = active;
        if (active)
        {
            m_TroopSelectionUI.gameObject.transform.DOScale(new Vector3(0.96f, 0.94f, 0f), 0.8f);
        }
        else
        {
            m_TroopSelectionUI.gameObject.transform.DOScale(Vector3.zero, 0.1f);
        }
        
    }
}


