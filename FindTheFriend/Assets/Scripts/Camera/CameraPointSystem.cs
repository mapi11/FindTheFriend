using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraPointSystem : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float arrivalThreshold = 0.1f;
    public float roomDestroyDelay = 0.5f;

    [Header("References")]
    [SerializeField] private Transform _roomsContainer;

    private List<CameraPoint> _allPoints = new List<CameraPoint>();
    private CameraPoint _currentPoint;
    private bool _isMoving;
    private GameObject _currentActiveRoom;
    private GameObject _roomToDestroy;
    private List<CameraDoorPoint> _usedDoors = new List<CameraDoorPoint>();
    private Vector3 _targetPosition;

    HealthSystem _healthSystem;
    RoomsCounter roomsCounter;

    private void Awake()
    {
        InitializeSystem();
        CreateRoomsContainerIfNeeded();

        _healthSystem = GetComponent<HealthSystem>();
        roomsCounter = GetComponent<RoomsCounter>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isMoving)
            HandleClick();

        if (_isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _targetPosition,
                moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _targetPosition) <= arrivalThreshold)
            {
                transform.position = _targetPosition;
                _isMoving = false;
            }
        }
    }

    private void InitializeSystem()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        FindAllPoints();
        SetInitialPoint();
    }

    private void CreateRoomsContainerIfNeeded()
    {
        if (_roomsContainer == null)
        {
            _roomsContainer = new GameObject("RoomsContainer").transform;
        }
    }

    private void FindAllPoints()
    {
        _allPoints.Clear();
        _allPoints.AddRange(FindObjectsOfType<CameraPoint>());

        foreach (var point in _allPoints)
        {
            point.OnSelected += OnPointSelected;
        }
    }

    private void SetInitialPoint()
    {
        CameraPoint closest = null;
        float minDistance = Mathf.Infinity;

        foreach (var point in _allPoints)
        {
            float dist = Vector3.Distance(transform.position, point.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = point;
            }
        }

        if (closest != null)
        {
            closest.SetSelected(true);
            _currentPoint = closest;
            transform.position = closest.transform.position;
        }
    }

    private void HandleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var door = hit.collider.GetComponent<CameraDoorPoint>();
            if (door != null && door.CanBeUsed())
            {
                StartCoroutine(ProcessDoorTransition(door));
                return;
            }

            var point = hit.collider.GetComponent<CameraPoint>();
            if (point != null && point != _currentPoint)
            {
                StartCoroutine(MoveToPoint(point));
            }
        }
    }

    private IEnumerator ProcessDoorTransition(CameraDoorPoint door)
    {
        if (_usedDoors.Contains(door)) yield break;

        _isMoving = true;
        door.MarkAsUsed();
        _usedDoors.Add(door);

        // Move to approach point (первая точка)
        yield return MoveToPosition(door.approachPoint);

        // Handle room transition
        if (door.roomPrefabs.Length > 0 && door.roomSpawnPoint != null)
        {
            if (_currentActiveRoom != null)
            {
                _roomToDestroy = _currentActiveRoom;
                StartCoroutine(DestroyRoomWithDelay(_roomToDestroy));
            }

            int index = Random.Range(0, door.roomPrefabs.Length);
            _currentActiveRoom = Instantiate(
                door.roomPrefabs[index],
                door.roomSpawnPoint.position,
                door.roomSpawnPoint.rotation,
                _roomsContainer);
        }


        // Вызываем OnPlayerEntered перед движением к exit point (вторая точка)
        DoorEnemyInteraction doorInteraction = door.GetComponent<DoorEnemyInteraction>();
        if (doorInteraction != null)
        {
            doorInteraction.OnPlayerEntered();
        }
        

        // Move to exit point (вторая точка)
        yield return MoveToPosition(door.exitPoint);
        roomsCounter.RoomCount++;


        SetNewCurrentPointAfterDoor();
    }

    private IEnumerator DestroyRoomWithDelay(GameObject room)
    {
        yield return new WaitForSeconds(roomDestroyDelay);

        if (room != null && room != _currentActiveRoom)
        {
            Destroy(room);
        }
    }

    private IEnumerator MoveToPoint(CameraPoint point)
    {
        _isMoving = true;

        if (_currentPoint != null)
            _currentPoint.SetSelected(false);

        yield return MoveToPosition(point.transform);

        _currentPoint = point;
        point.SetSelected(true);
    }

    private IEnumerator MoveToPosition(Transform target)
    {
        _targetPosition = target.position;
        _isMoving = true;

        while (_isMoving)
            yield return null;
    }

    private void SetNewCurrentPointAfterDoor()
    {
        CameraPoint closest = null;
        float minDistance = Mathf.Infinity;

        foreach (var point in _allPoints)
        {
            if (point == _currentPoint) continue;

            float dist = Vector3.Distance(transform.position, point.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = point;
            }
        }

        if (closest != null)
        {
            if (_currentPoint != null)
                _currentPoint.SetSelected(false);

            closest.SetSelected(true);
            _currentPoint = closest;
        }
    }

    private void OnDestroy()
    {
        foreach (var point in _allPoints)
        {
            if (point != null)
                point.OnSelected -= OnPointSelected;
        }
    }

    private void OnPointSelected(CameraPoint point)
    {
        if (_isMoving || point == _currentPoint) return;
        StartCoroutine(MoveToPoint(point));
    }

    public void StopAllMovement()
    {
        StopAllCoroutines();
        _isMoving = false;
    }
}