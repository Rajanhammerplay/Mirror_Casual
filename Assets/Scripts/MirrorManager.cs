using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorManager : MonoBehaviour
{

    public bool isRaycasted = false;

    [SerializeField] private GameObject m_Mirror;

    public List<Mirror> _MirrorsSelected = new List<Mirror>();

    public List<Mirror> m_MirrorPool = new List<Mirror>();

    private Shader m_NormalShader;

    private Shader m_OutlineShader;

    public float _Radius;

    private Vector3 m_HitPoint;

    int id = 0;

    public int m_MirrorPoolSize;

    public enum While_on{mirror_added,mirror_select};

    private void Start()
    {
        m_NormalShader = Shader.Find("Standard");
        m_OutlineShader = Shader.Find("Ultimate 10+ Shaders/Outline");
        for(int i = 0;i<m_MirrorPoolSize;i++)
        {
            GameObject mirror = Instantiate(m_Mirror,m_Mirror.transform.position, transform.rotation);
            mirror.name = "Mirror_" + i;
            mirror.transform.parent = this.transform;
            mirror.gameObject.SetActive(false);
            m_MirrorPool.Add(mirror.GetComponent<Mirror>());
        }
    }
    // Update is called once per frame
    void Update()
    {
        SpawnMirrorObject();
    }

    public void SpawnMirrorObject()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.tag == "mirror" )
                {
                    if (hit.collider.gameObject != null)
                    {
                        MirrorSelected(hit.collider.gameObject, While_on.mirror_select);
                        return;
                    }
 
                }
                else if (hit.collider.gameObject.tag == "ground")
                {
                    m_HitPoint = hit.point;
                    //to blocks the player to place mirrors nearby others
                    Collider[] colliders  = Physics.OverlapSphere(hit.point, _Radius);
                    if(colliders.Length>0)
                    {
                        foreach(var collider in colliders)
                        {
                            if(collider.gameObject.tag == "mirror")
                            {
                                return;
                            }
                            else
                            {

                                if (GetSpawnedMirror() != null)
                                {
                                    GameObject mirror = GetSpawnedMirror();
                                    mirror.transform.position = new Vector3(m_HitPoint.x, m_HitPoint.y + 0.2f, m_HitPoint.z);
                                    mirror.SetActive(true);
                                    MirrorSelected(mirror,While_on.mirror_added);
                                }
                            }
                        }
                    }

                }
            }
        }

    }
    //to get mirror objects from pool
    private GameObject GetSpawnedMirror()
    {
        for(int i = 0;i<m_MirrorPool.Count;i++)
        {
            if (!m_MirrorPool[i].gameObject.activeSelf)
            {
                return m_MirrorPool[i].gameObject;
            }
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(m_HitPoint, _Radius);
    }
    //to select mirror
    public void MirrorSelected(GameObject mirror, While_on whileon)
    {

        if (whileon == While_on.mirror_added)        //to make mirror selected while drop
        {
            if (_MirrorsSelected.Count == 0)
            {
                _MirrorsSelected.Add(mirror.GetComponent<Mirror>());
            }else
            {
                _MirrorsSelected.Add(mirror.GetComponent<Mirror>());

                MakeMirrorSelected(_MirrorsSelected, mirror);
            }
        }
        else //to make mirror selected while select
        {
            MakeMirrorSelected(_MirrorsSelected, mirror);
        }
    }

    //to select a mirror
    public void MakeMirrorSelected(List<Mirror> mirroslist,GameObject mirror)
    {
        foreach (Mirror Mirror in mirroslist)
        {
            if (Mirror == mirror.GetComponent<Mirror>())
            {
                Mirror._IsSelected = true;
            }
            else
            {
                Mirror._IsSelected = false;
            }
        }
    }


}
