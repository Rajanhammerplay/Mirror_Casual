using UnityEngine;

public class MobileCameraController : MonoBehaviour
{
    [Header("Camera Movement")]
    [SerializeField] private float panSpeed = 20f;
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float bounceBackSpeed = 2f;

    [Header("Boundaries")]
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minZ = -10f;
    [SerializeField] private float maxZ = 10f;

    [Header("Touch Settings")]
    [SerializeField] private float touchSensitivity = 0.5f;
    [SerializeField] private float minSwipeDistance = 5f;

    private Camera mainCamera;
    private Vector3 targetPosition;
    private Vector3 currentVelocity;
    private Vector2 touchStartPos;
    private bool isDragging;
    private int touchId = -1;
    private bool m_CanSwipe;

    private void Start()
    {
        mainCamera = Camera.main;
        targetPosition = transform.position;
        EventActions.CheckCanSwipe += CheckCanSwipe;
    }

    private void Update()
    {
        if (!m_CanSwipe)
        {
            HandleTouchInput();
            if (isDragging)
            {
                UpdateCameraPosition();
            }
        } 
        
    }

    public void CheckCanSwipe(bool state)
    {
        m_CanSwipe = state;
    }

    private void HandleTouchInput()
    {
        // Handle touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartDrag(touch);
                    break;

                case TouchPhase.Moved:
                    if (isDragging && touch.fingerId == touchId)
                    {
                        UpdateDrag(touch);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (touch.fingerId == touchId)
                    {
                        EndDrag();
                    }
                    break;
            }
        }
        // Handle mouse input for testing in editor
        else if (Application.isEditor)
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    StartDragMouse();
            //}
            //else if (Input.GetMouseButton(0) && isDragging)
            //{
            //    UpdateDragMouse();
            //}
            //else if (Input.GetMouseButtonUp(0))
            //{
            //    EndDrag();
            //}
        }
    }

    private void StartDrag(Touch touch)
    {
        touchId = touch.fingerId;
        touchStartPos = touch.position;
        isDragging = true;
    }

    private void StartDragMouse()
    {
        touchStartPos = Input.mousePosition;
        isDragging = true;
    }

    private void UpdateDrag(Touch touch)
    {
        Vector2 difference = touch.position - touchStartPos;

        if (difference.magnitude > minSwipeDistance)
        {
            // Convert screen movement to world space movement
            Vector3 moveDirection = new Vector3(-difference.x, 0, -difference.y).normalized;
            float moveDistance = difference.magnitude * touchSensitivity;

            // Calculate new target position
            Vector3 newTargetPosition = targetPosition + moveDirection * moveDistance * Time.deltaTime * panSpeed;

            // Clamp to boundaries
            newTargetPosition.x = Mathf.Clamp(newTargetPosition.x, minX, maxX);
            newTargetPosition.z = Mathf.Clamp(newTargetPosition.z, minZ, maxZ);

            targetPosition = newTargetPosition;
            touchStartPos = touch.position;
        }
    }

    private void UpdateDragMouse()
    {
        Vector2 difference = (Vector2)Input.mousePosition - touchStartPos;

        if (difference.magnitude > minSwipeDistance)
        {
            Vector3 moveDirection = new Vector3(difference.x, 0, difference.y).normalized;
            float moveDistance = difference.magnitude * touchSensitivity;

            Vector3 newTargetPosition = targetPosition + moveDirection * moveDistance * Time.deltaTime * panSpeed;
            newTargetPosition.x = Mathf.Clamp(newTargetPosition.x, minX, maxX);
            newTargetPosition.z = Mathf.Clamp(newTargetPosition.z, minZ, maxZ);

            targetPosition = newTargetPosition;
            touchStartPos = Input.mousePosition;
        }
    }

    private void EndDrag()
    {
        isDragging = false;
        touchId = -1;
    }

    private void UpdateCameraPosition()
    {
        // Smoothly move camera to target position
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref currentVelocity,
            smoothTime
        );

        // Bounce back if outside boundaries
        if (transform.position.x < minX || transform.position.x > maxX ||
            transform.position.z < minZ || transform.position.z > maxZ)
        {
            Vector3 clampedPosition = new Vector3(
                Mathf.Clamp(transform.position.x, minX, maxX),
                transform.position.y,
                Mathf.Clamp(transform.position.z, minZ, maxZ)
            );

            transform.position = Vector3.Lerp(
                transform.position,
                clampedPosition,
                Time.deltaTime * bounceBackSpeed
            );
        }
    }

    // Optional: Method to set new boundaries at runtime
    public void SetBoundaries(float newMinX, float newMaxX, float newMinZ, float newMaxZ)
    {
        minX = newMinX;
        maxX = newMaxX;
        minZ = newMinZ;
        maxZ = newMaxZ;
    }
}