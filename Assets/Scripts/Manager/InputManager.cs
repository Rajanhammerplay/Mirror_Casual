using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class InputManager : MonoBehaviour
{

    public bool isRaycasted = false;

    [SerializeField] private GameObject m_Mirror;


    public float _Radius;

    private Vector3 m_HitPoint;

    int id = 0;

    public int m_MirrorPoolSize;

    public enum While_on{mirror_added,mirror_select};

    public LayerMask LayerMaskToIgnore;

    public GameObject MirrorPicked;

    public GameObject MirrorParent;

    public PoolManager m_PoolManager;

    private Tilemap Tilemap;


    private void Start()
    {
        Tilemap = GetComponent<Tilemap>();
        print("slected inv type by default: " + EventActions._SelectedInvType);

    }

    //method for recieve event
    public void AddInvItem(object sender, InventoryArgs args)
    {
        if (args != null)
        {
            m_Mirror = args.InvItem._MirrorPrefab;
        }
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

            Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos.z = 0;


            Vector3Int tilepos = Tilemap.WorldToCell(mousepos);

            TileBase clickedTile = Tilemap.GetTile(tilepos);

            GameObject troop = Instantiate(m_Mirror);
            troop.transform.position = new Vector3(mousepos.x, 1.708048f, mousepos.z);
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            //{

            //    if (EventSystem.current.IsPointerOverGameObject() || hit.collider.gameObject.GetComponent<Canvas>() != null)
            //    {
            //        return;
            //    }
            //    //else if (hit.collider.gameObject.tag == "mirror")
            //    //{
            //    //    m_PoolManager.SelectMirror(hit.collider.gameObject);
            //    //}
            //    //else if (hit.collider.gameObject.tag == "ground")
            //    //{
            //    //    m_HitPoint = hit.point;
            //    //    //to blocks the player to place mirrors nearby others
            //    //    Collider[] colliders = Physics.OverlapSphere(hit.point, _Radius);
            //    //    if (colliders.Length > 0)
            //    //    {
            //    //        foreach (var collider in colliders)
            //    //        {
            //    //            if (collider.gameObject.tag == "mirror")
            //    //            {
            //    //                return;
            //    //            }
            //    //            else
            //    //            {
            //    //                if (EventActions._SelectedInvType != MirrorVariation.none && m_PoolManager.GetSpawnnableObject(EventActions._SelectedInvType))
            //    //                {
            //    //                    GameObject mirror = m_PoolManager.GetSpawnnableObject(EventActions._SelectedInvType);
            //    //                    print("current mirror: " + mirror.name);
            //    //                    mirror.transform.position = hit.point + new Vector3(0, mirror.GetComponent<Mirror>().yPos, 0);
            //    //                    mirror.SetActive(true);
            //    //                    m_PoolManager.SelectMirror(mirror);
            //    //                    m_PoolManager.DequeuePool(mirror.GetComponent<Mirror>());
            //    //                    EventActions._DropMirror.Invoke(mirror.GetComponent<Mirror>());
            //    //                }

            //    //            }
            //    //        }
            //    //    }

            //    //}
            //    //else if (hit.collider.gameObject.GetComponent<TileObject>() != null) 
            //    //{
            //    //    print("mouse down working on tileobject");
            //    //}
            //}
        }
    }

    
 
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(m_HitPoint, _Radius);
    }
    //to select mirror
    //public void MirrorSelected(GameObject mirror, While_on whileon)
    //{

    //    if (whileon == While_on.mirror_added)        //to make mirror selected while drop
    //    {
    //        if (_MirrorsSelected.Count == 0)
    //        {
    //            _MirrorsSelected.Add(mirror.GetComponent<Mirror>());
    //        }else
    //        {
    //            _MirrorsSelected.Add(mirror.GetComponent<Mirror>());

    //            MakeMirrorSelected(_MirrorsSelected, mirror);
    //        }
    //    }
    //    else //to make mirror selected while select
    //    {
    //        MakeMirrorSelected(_MirrorsSelected, mirror);
    //    }
    //}

    ////to select a mirror
    //public void MakeMirrorSelected(List<Mirror> mirroslist,GameObject mirror)
    //{
    //    foreach (Mirror Mirror in mirroslist)
    //    {
    //        if (Mirror == mirror.GetComponent<Mirror>())
    //        {
    //            Mirror._IsSelected = true;
    //        }
    //        else
    //        {
    //            Mirror._IsSelected = false;
    //        }
    //    }
    //}

    //public void SetMirrorPicked(Mirror mirror)
    //{
    //    MirrorPicked = mirror.gameObject;
    //}
}
