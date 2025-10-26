using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Defines;
using static UnityEditor.PlayerSettings;

public class UnitsManager : MonoBehaviour
{
    public static UnitsManager _Instance;

    private Dictionary<UnitType, List<IUnitItem>> _unitsByType;
    private Dictionary<UnitType, IUnitItem> _selectedUnits;
    public List<GameObject> _MirrorPlacableTiles = new List<GameObject>();
    public GameObject m_PlayerPlacableTiles;

    private void Awake()
    {
        _Instance = this;
        _unitsByType = new Dictionary<UnitType, List<IUnitItem>>();
        _selectedUnits = new Dictionary<UnitType, IUnitItem>();

        // Initialize lists for each unit type
        foreach (UnitType type in System.Enum.GetValues(typeof(UnitType)))
        {
            _unitsByType[type] = new List<IUnitItem>();
        }
    }

    void Start()
    {

    }
    public void RegisterUnit(IUnitItem unit)
    {
        UnitType type = unit.GetUnitType();
        if (!_unitsByType[type].Contains(unit))
        {
            _unitsByType[type].Add(unit);
        }
    }

    public void UnregisterUnit(IUnitItem unit)
    {
        UnitType type = unit.GetUnitType();
        _unitsByType[type].Remove(unit);

        if (_selectedUnits.ContainsKey(type) && _selectedUnits[type] == unit)
        {
            _selectedUnits.Remove(type);
        }
    }

    public void SelectUnit(IUnitItem unit)
    {
        UnitType type = unit.GetUnitType();

        // Deselect currently selected unit of the same type
        if (_selectedUnits.ContainsKey(type) && _selectedUnits[type] != unit)
        {
            _selectedUnits[type].SetSelected(false);
        }

        _selectedUnits[type] = unit;
        unit.SetSelected(true);
    }

    public IUnitItem GetSelectedUnit(UnitType type)
    {
        return _selectedUnits.ContainsKey(type) ? _selectedUnits[type] : null;
    }

    public List<IUnitItem> GetAllUnitsOfType(UnitType type)
    {
        return _unitsByType[type];
    }

    public bool IsPlacableTile(GameObject obj, UnitType type)
    {
        if (type == Defines.UnitType.NormalMirror || type == Defines.UnitType.HealerMirror)
        {
            if (_MirrorPlacableTiles.Count > 0)
            {
                foreach (var tile in _MirrorPlacableTiles)
                {
                    if (tile == obj)
                    {
                        return true;
                    }
                }
            }
        }
        else if (type == Defines.UnitType.Troop_1 || type == Defines.UnitType.Troop_2)
        {
            if (m_PlayerPlacableTiles == obj)
            {
                return true;
            }
        }
        return false;
    }

    public void HighlighTiles(UnitType type)
    {
        if (type == Defines.UnitType.NormalMirror || type == Defines.UnitType.HealerMirror)
        {
            if (_MirrorPlacableTiles.Count > 0)
            {
                foreach (var tile in _MirrorPlacableTiles)
                {
                    if (!tile.gameObject.GetComponent<TileObject>())
                    {
                        return;
                    }
                    tile.gameObject.GetComponent<TileObject>().HighLightTile(true);
                }
            }
        }
        else
        {
            if (_MirrorPlacableTiles.Count > 0)
            {
                foreach (var tile in _MirrorPlacableTiles)
                {
                    if (!tile.gameObject.GetComponent<TileObject>())
                    {
                        return;
                    }
                    tile.gameObject.GetComponent<TileObject>().HighLightTile(false);
                }
            }
        }
        if (type == Defines.UnitType.Troop_1 || type == Defines.UnitType.Troop_2)
        {
            if (m_PlayerPlacableTiles)
            {
                if (!m_PlayerPlacableTiles.gameObject.GetComponent<TileObject>())
                {
                    return;
                }
                m_PlayerPlacableTiles.gameObject.GetComponent<TileObject>().HighLightTile(true);
            }
        }
        else
        {
            if (m_PlayerPlacableTiles)
            {
                if (!m_PlayerPlacableTiles.gameObject.GetComponent<TileObject>())
                {
                    return;
                }
                m_PlayerPlacableTiles.gameObject.GetComponent<TileObject>().HighLightTile(false);
            }
        }
    }

    public void DropUnit(Vector3 pos, GameObject lookatobj)
    {
        GameObject unitfrompool = PoolManager._instance.GetSpawnableObjectFromPool(EventActions._SelectedUnitType);
        SelectUnit(unitfrompool?.GetComponent<IUnitItem>());
        if (unitfrompool != null)
        {
            unitfrompool.GetComponent<IUnitItem>().DropItem(unitfrompool, pos, lookatobj);
        }

    }

    private void OnEnable()
    {
        EventActions._SpawnTroops += SpawnTroops;
    }

    [Header("Spawn Settings")]
    [SerializeField] private float spawnDelay = 0.5f;
    private Coroutine currentSpawnCoroutine;

    public void SpawnTroops(UnitType unittype)
    {
        // Stop any existing spawn coroutine to avoid overlap
        if (currentSpawnCoroutine != null)
        {
            StopCoroutine(currentSpawnCoroutine);
        }

        currentSpawnCoroutine = StartCoroutine(SpawnTroopsWithDelay(unittype));
    }

    private IEnumerator SpawnTroopsWithDelay(UnitType unittype)
    {
        yield return new WaitForSeconds(2);
        while (PoolManager._instance._UnitPoolDict[unittype].Count > 0)
        {
            GameObject unitobj = PoolManager._instance.GetSpawnableObjectFromPool(unittype);
            unitobj.GetComponent<IUnitItem>().DropItem(unitobj,m_PlayerPlacableTiles.transform.position,m_PlayerPlacableTiles);
            yield return new WaitForSeconds(spawnDelay);
        }

        currentSpawnCoroutine = null;
    }

    private void OnDisable()
    {
        EventActions._SpawnTroops -= SpawnTroops;
    }
}
