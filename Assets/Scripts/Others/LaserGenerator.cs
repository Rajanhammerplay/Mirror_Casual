using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unified laser system that combines target detection and laser beam generation
/// </summary>
public class LaserGenerator : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float targetDetectionRadius = 5f;
    [SerializeField] private Vector3 detectionOffset = Vector3.zero;
    [SerializeField] private float detectionBoxDistance = 5f;
    [SerializeField] private Vector3 detectionBoxSize = new Vector3(3f, 1f, 3f);
    [SerializeField] private Vector3 detectionBoxOffset = Vector3.zero;
    [SerializeField] private LayerMask targetLayers;

    [Header("Laser Settings")]
    [SerializeField] private Material laserMaterial;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private InputManager inputManager;

    // Core state
    private GameObject currentTarget;
    private TargetType currentTargetType = TargetType.None;
    private Troop currentTroop;
    private GameObject persistentMirrorTarget;
    private bool isMirrorPlaced = false;
    private bool canCastLaser = true;

    // Cached components
    private Transform cachedTransform;
    private Transform laserHead;
    private Laser laser;

    // Performance optimization
    private readonly Collider[] detectionResults = new Collider[30];
    private readonly Collider[] targetsInRange = new Collider[15];

    private void Start()
    {
        cachedTransform = transform;
        laserHead = transform.parent;
        InitializeLaser();
        InitializeTilesInFront();
    }

    private void Update()
    {
        UpdateLaserBeam();
        UpdateTargetDetection();
    }

    private void InitializeLaser()
    {
        GameObject laserObj = Instantiate(laserPrefab);
        laser = new Laser(transform.position, transform.forward, laserMaterial, inputManager, laserObj);
    }

    private void InitializeTilesInFront()
    {
        Vector3 boxCenter = transform.position + (transform.forward * detectionBoxDistance) + detectionBoxOffset;
        int detectedCount = Physics.OverlapBoxNonAlloc(boxCenter, detectionBoxSize, detectionResults, transform.rotation);

        for (int i = 0; i < detectedCount; i++)
        {
            if (detectionResults[i] == null) continue;

            TileObject tileObj = detectionResults[i].GetComponent<TileObject>();
            if (tileObj != null) tileObj._LookatObject = gameObject;

            UnitsManager._Instance?._MirrorPlacableTiles.Add(detectionResults[i].gameObject);
        }
    }

    private void UpdateLaserBeam()
    {
        if (currentTarget == null || !canCastLaser)
        {
            laser?.HideLaser(false);
            return;
        }

        laser.HideLaser(true);

        Vector3 direction = (isMirrorPlaced && currentTargetType == TargetType.Mirror)
            ? (currentTarget.transform.position - transform.position).normalized
            : (currentTarget.transform.position - transform.position).normalized;

        // Clear laser detection flags before updating if mirror is already placed
        if (isMirrorPlaced)
        {
            laser._MirrorDeteced = false;
            laser._CurrentTarget = null;
        }

        laser.UpdateLaser(transform.position, direction);
    }

    private void UpdateTargetDetection()
    {
       // print("Updating Target Detection..." + currentTarget + " mirror placed: " + isMirrorPlaced + " target type: " + currentTargetType + " persistent mirror: " + persistentMirrorTarget);

        // ABSOLUTE PRIORITY: If mirror is placed, lock everything down
        if (isMirrorPlaced && persistentMirrorTarget != null)
        {
            // Force lock to mirror target
            if (currentTarget != persistentMirrorTarget || currentTargetType != TargetType.Mirror)
            {
                print("Correcting target back to persistent mirror");
                currentTarget = persistentMirrorTarget;
                currentTargetType = TargetType.Mirror;
            }
            return; // HARD STOP - no further processing when mirror is placed
        }

        // If mirror was placed but persistentMirrorTarget is null, clear mirror state
        if (isMirrorPlaced && persistentMirrorTarget == null)
        {
            print("ERROR: Mirror placed but persistentMirrorTarget is null! Clearing mirror state.");
            isMirrorPlaced = false;
            currentTargetType = TargetType.None;
            currentTarget = null;
        }

        // Priority 2: Laser detected a new mirror - set it once and lock onto it
        // BUT ONLY if we don't already have a mirror placed
        if (!isMirrorPlaced && laser._MirrorDeteced && laser._CurrentTarget != null)
        {
           // print("Laser detected new mirror, setting it");
            SetMirrorTarget(laser._CurrentTarget);
            return;
        }

        // Priority 3: Valid troop target
        if (currentTargetType == TargetType.Troop && IsTargetValid(currentTroop))
        {
            AimAtTarget();
            return;
        }

        // Priority 4: Find new troop target (only if no mirror is placed)
        if (currentTargetType == TargetType.Troop) ResetTarget();
       // print("Looking for new target...");
        FindNewTarget();
    }

    private void FindNewTarget()
    {
        // Safety check: Don't look for new targets if mirror is placed
        if (isMirrorPlaced)
        {
            print("ERROR: FindNewTarget called while mirror is placed! Aborting.");
            return;
        }

        Vector3 detectionCenter = cachedTransform.position + detectionOffset;
        int targetsCount = Physics.OverlapSphereNonAlloc(detectionCenter, targetDetectionRadius, targetsInRange);

        float closestDistance = float.MaxValue;
        Troop closestTroop = null;

        for (int i = 0; i < targetsCount; i++)
        {
            if (targetsInRange[i] == null) continue;

            Troop troopComponent = targetsInRange[i].GetComponent<Troop>();
            if (troopComponent != null && !troopComponent._TroopDead)
            {
                float distance = Vector3.Distance(detectionCenter, troopComponent.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTroop = troopComponent;
                }
            }
        }

        if (closestTroop != null)
        {
            print("Found new troop target: " + closestTroop.name);
            SetTarget(closestTroop.gameObject, TargetType.Troop);
            currentTroop = closestTroop;
            AimAtTarget();
        }
    }

    private void AimAtTarget()
    {
        if (laserHead == null || currentTarget == null) return;

        if (currentTargetType == TargetType.Troop && !isMirrorPlaced)
        {
            laserHead.LookAt(currentTarget.transform.position);
        }
        else if (currentTargetType == TargetType.Mirror && !isMirrorPlaced)
        {
            laserHead.LookAt(currentTarget.transform.position);
            isMirrorPlaced = true;
        }
    }

    private bool IsTargetValid(Troop target)
    {
        if (target == null || target._TroopDead) return false;

        float distance = Vector3.Distance(cachedTransform.position + detectionOffset, target.transform.position);
        return distance < targetDetectionRadius;
    }

    private void ResetTarget()
    {
        if (isMirrorPlaced && currentTargetType == TargetType.Mirror && currentTarget == persistentMirrorTarget)
            return;

        if (currentTargetType == TargetType.Troop)
        {
            currentTarget = null;
            currentTroop = null;
            currentTargetType = TargetType.None;
        }

        if (laser != null && !isMirrorPlaced)
        {
            laser._CurrentTarget = null;
            laser._MirrorDeteced = false;
        }
    }

    // Public Interface
    public void SetTarget(GameObject target, TargetType type)
    {
       // print($"SetTarget called with {target?.name} and type {type}");

        // If we're trying to set a troop target but mirror is placed, ignore it
        if (type == TargetType.Troop && isMirrorPlaced)
        {
            print("ERROR: Attempting to set troop target while mirror is placed! Ignoring.");
            return;
        }

        // If we're setting the same mirror target that's already persistent, just update current references
        if (type == TargetType.Mirror && isMirrorPlaced && persistentMirrorTarget == target)
        {
         //   print("Updating references for existing persistent mirror");
            currentTarget = target;
            currentTargetType = type;
            return;
        }

        currentTarget = target;
        currentTargetType = type;
        canCastLaser = true;

        if (type == TargetType.Mirror)
        {
            isMirrorPlaced = true;
            persistentMirrorTarget = target;
            print($"Mirror target set to: {target?.name}");
        }
        else if (type == TargetType.Troop)
        {
            // Only clear mirror state if we're explicitly setting a troop target
            // and no mirror was previously placed
            if (!isMirrorPlaced)
            {
                persistentMirrorTarget = null;
            }
        }

        if (laser != null)
        {
            laser._CurrentTarget = target;
            laser._MirrorDeteced = (type == TargetType.Mirror);
        }

        currentTroop = (type == TargetType.Troop) ? target.GetComponent<Troop>() : null;
    }

    public void SetMirrorTarget(GameObject mirror)
    {
        // If we already have this mirror as persistent target, don't reset it
        if (isMirrorPlaced && persistentMirrorTarget == mirror)
        {
           // print("Mirror already set as persistent target, ignoring duplicate call");
            return;
        }

        print($"Setting new mirror target: {mirror?.name}");
        SetTarget(mirror, TargetType.Mirror);
        isMirrorPlaced = true;
        persistentMirrorTarget = mirror;
    }

    public void ClearTarget()
    {
        isMirrorPlaced = false;
        persistentMirrorTarget = null;
        ResetTarget();
        canCastLaser = false;
    }

    public void RemoveMirror()
    {
        isMirrorPlaced = false;
        persistentMirrorTarget = null;

        if (currentTargetType == TargetType.Mirror)
            ResetTarget();

        if (laser != null)
        {
            laser._MirrorDeteced = false;
            laser._CurrentTarget = null;
        }
    }

    public bool HasTarget() => currentTarget != null;
    public bool HasMirrorTarget() => currentTargetType == TargetType.Mirror && currentTarget != null;

    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Mirror placement detection box
            Gizmos.color = Color.yellow;
            Vector3 boxCenter = transform.position + (transform.forward * detectionBoxDistance) + detectionBoxOffset;
            Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, detectionBoxSize);

            // Enemy detection sphere
            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.DrawWireSphere(transform.position + detectionOffset, targetDetectionRadius);
        }
    #endif
}

public enum TargetType { None, Troop, Mirror }