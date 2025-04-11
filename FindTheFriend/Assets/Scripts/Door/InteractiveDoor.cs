using System.Collections.Generic;
using UnityEngine;

public class InteractiveDoor : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private Transform doorTransform;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;
    [SerializeField] private float closeDelay = 0.5f;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraFirstPoint;
    [SerializeField] private Transform cameraSecondPoint;
    [SerializeField] private float cameraMoveSpeed = 2f;
    [SerializeField] private float cameraRotationSpeed = 5f;
    [SerializeField] private float positionThreshold = 0.1f;

    private enum AnimationState { Idle, Opening, MovingToFirstPoint, MovingToSecondPoint, Closing }
    private AnimationState currentState = AnimationState.Idle;

    private Quaternion initialDoorRotation;
    private Quaternion openDoorRotation;
    private Camera mainCamera;
    private bool isInteractable = true;

    private void Start()
    {
        initialDoorRotation = doorTransform.rotation;
        openDoorRotation = initialDoorRotation * Quaternion.Euler(0, openAngle, 0);
        mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        if (currentState == AnimationState.Idle && isInteractable)
        {
            currentState = AnimationState.Opening;
            isInteractable = false;
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case AnimationState.Opening:
                UpdateDoorOpening();
                break;

            case AnimationState.MovingToFirstPoint:
                MoveCameraToTarget(cameraFirstPoint);
                break;

            case AnimationState.MovingToSecondPoint:
                MoveCameraToTarget(cameraSecondPoint);
                break;

            case AnimationState.Closing:
                UpdateDoorClosing();
                break;
        }
    }

    private void UpdateDoorOpening()
    {
        doorTransform.rotation = Quaternion.Lerp(doorTransform.rotation, openDoorRotation, openSpeed * Time.deltaTime);

        if (Quaternion.Angle(doorTransform.rotation, openDoorRotation) < 5f)
        {
            currentState = AnimationState.MovingToFirstPoint;
        }
    }

    private void MoveCameraToTarget(Transform target)
    {
        mainCamera.transform.position = Vector3.Lerp(
            mainCamera.transform.position,
            target.position,
            cameraMoveSpeed * Time.deltaTime
        );

        Quaternion targetRotation = Quaternion.LookRotation(target.position - mainCamera.transform.position);
        mainCamera.transform.rotation = Quaternion.Slerp(
            mainCamera.transform.rotation,
            targetRotation,
            cameraRotationSpeed * Time.deltaTime
        );

        if (Vector3.Distance(mainCamera.transform.position, target.position) < positionThreshold)
        {
            if (currentState == AnimationState.MovingToFirstPoint)
            {
                currentState = AnimationState.MovingToSecondPoint;
            }
            else if (currentState == AnimationState.MovingToSecondPoint)
            {
                currentState = AnimationState.Closing;
                Invoke("StartClosingDoor", closeDelay);
            }
        }
    }

    private void StartClosingDoor()
    {
        currentState = AnimationState.Closing;
    }

    private void UpdateDoorClosing()
    {
        doorTransform.rotation = Quaternion.Lerp(doorTransform.rotation, initialDoorRotation, openSpeed * Time.deltaTime);

        if (Quaternion.Angle(doorTransform.rotation, initialDoorRotation) < 1f)
        {
            doorTransform.rotation = initialDoorRotation;
            currentState = AnimationState.Idle;
            isInteractable = true;
        }
    }
}
