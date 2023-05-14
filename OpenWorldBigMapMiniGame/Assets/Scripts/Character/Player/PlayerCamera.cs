using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;
    public PlayerManager player;
    public Camera cameraObject;
    [SerializeField] Transform cameraPivotTransform;

    [Header("Camera Settings")]
    private float cameraSmoothSpeed = 1;  // THE BIGGER THIS NUMBER, THE LONGER FOR THE CAMERA TO REACH ITS POSITION DURING MOVEMENT
    [SerializeField] float leftRightRotationSpeed = 220;
    [SerializeField] float upDownRotationSpeed = 220;
    [SerializeField] float minimumPivot = -30;
    [SerializeField] float maximumPivot = 60;
    [SerializeField] float cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask collideWithLayers;

    [Header("Camera Values")]
    Vector3 cameraVelocity;
    Vector3 cameraObjectPosition;
    [SerializeField] float leftRightLookAngle;
    [SerializeField] float upDownLookAngle;
    // for camera collisions
    float cameraZPosition;
    float targetCameraZPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            // Follow the player
            HandleFollowTarget();

            // Rotate around the player
            HandleRotations();

            // collide with objects
            HandleCollisions();
        }
    }

    private void HandleFollowTarget()
    {
        var targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    private void HandleRotations()
    {
        // IF LOCKED ON£¬FORCE ROTATION TOWARDS TARGET
        // ELSE ROTATE REGULARLY

        //ROTATE LEFT AND RIGHT BASED ON CAMERA HORIZONTAL MOVEMENT
        leftRightLookAngle += (PlayerInputManager.Instance.cameraHorizontalInput * leftRightRotationSpeed) * Time.deltaTime;
        //ROTATE UP AND DOWN BASED ON CAMERA VERTICAL MOVEMENT
        upDownLookAngle -= (PlayerInputManager.Instance.cameraVerticalInput * upDownRotationSpeed) * Time.deltaTime;
        //CLAMP THE UP AND DOWN LOOK ANGLE BETWEEN A MIN AND MAX VALUE
        upDownLookAngle = Mathf.Clamp(upDownLookAngle, minimumPivot, maximumPivot);

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        // ROTATE THIS GAMEOBJECT LEFT AND RIGHT
        cameraRotation.y = leftRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        //ROTATE THE PIVOT GAMEOBJECT UP AND DOWN
        cameraRotation = Vector3.zero;
        cameraRotation.x = upDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCollisions()
    {
        targetCameraZPosition = cameraZPosition;
        RaycastHit hit;
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();
        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
        {
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }

        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }
}
