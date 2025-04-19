using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraPointSystem : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float arrivalThreshold = 0.1f;
    public float roomDestroyDelay = 0.5f;

    [Header("Footstep Sounds")]
    public AudioClip[] footstepSounds;
    [Range(0f, 1f)] public float footstepsVolume = 0.5f;
    [Range(0.5f, 2f)] public float footstepsPitch = 1f;
    [Range(0.1f, 1f)] public float footstepsInterval = 0.4f;

    [Header("References")]
    [SerializeField] private Transform _roomsContainer;

    private List<CameraPoint> _allPoints = new List<CameraPoint>();
    private CameraPoint _currentPoint;
    private bool _isMoving;
    private GameObject _currentActiveRoom;
    private GameObject _roomToDestroy;
    private List<CameraDoorPoint> _usedDoors = new List<CameraDoorPoint>();
    private Vector3 _targetPosition;
    private AudioSource _audioSource;
    private Coroutine _footstepsCoroutine;
    private Coroutine _movementCoroutine;
    private int _lastFootstepIndex = -1;

    private HealthSystem _healthSystem;
    private RoomsCounter _roomsCounter;

    private OpenMenu _openMenu;

    private void Awake()
    {
        _openMenu = FindAnyObjectByType<OpenMenu>(); // Или получить ссылку другим способом
        SetupAudioSource();
        InitializeSystem();
    }

    private void SetupAudioSource()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.volume = footstepsVolume;
        _audioSource.pitch = footstepsPitch;
    }

    private void InitializeSystem()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _healthSystem = GetComponent<HealthSystem>();
        _roomsCounter = GetComponent<RoomsCounter>();

        CreateRoomsContainerIfNeeded();
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

    #region Footsteps System
    private void StartFootsteps()
    {
        if (footstepSounds == null || footstepSounds.Length == 0 || _footstepsCoroutine != null) return;
        _footstepsCoroutine = StartCoroutine(PlayFootstepsLoop());
    }

    private void StopFootsteps()
    {
        if (_footstepsCoroutine != null)
        {
            StopCoroutine(_footstepsCoroutine);
            _footstepsCoroutine = null;
        }
    }

    private IEnumerator PlayFootstepsLoop()
    {
        while (true)
        {
            PlayRandomFootstep();
            yield return new WaitForSeconds(footstepsInterval);
        }
    }

    private void PlayRandomFootstep()
    {
        if (footstepSounds.Length == 0) return;

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, footstepSounds.Length);
        }
        while (randomIndex == _lastFootstepIndex && footstepSounds.Length > 1);

        _lastFootstepIndex = randomIndex;
        AudioClip clip = footstepSounds[randomIndex];

        if (clip != null)
        {
            _audioSource.PlayOneShot(clip);
        }
    }
    #endregion

    #region Movement System
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isMoving)
        {
            HandleClick();
        }
    }

    private void HandleClick()
    {
        // Если меню открыто - игнорируем клики
        if (_openMenu != null && _openMenu.isMenuOpen) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        var door = hit.collider.GetComponent<CameraDoorPoint>();
        if (door != null && door.CanBeUsed())
        {
            SafeStartCoroutine(ProcessDoorTransition(door));
            return;
        }

        var point = hit.collider.GetComponent<CameraPoint>();
        if (point != null && point != _currentPoint)
        {
            SafeStartCoroutine(MoveToPoint(point));
        }
    }

    private void SafeStartCoroutine(IEnumerator coroutine)
    {
        if (_movementCoroutine != null)
        {
            StopCoroutine(_movementCoroutine);
        }
        _movementCoroutine = StartCoroutine(coroutine);
    }

    private IEnumerator ProcessDoorTransition(CameraDoorPoint door)
    {
        if (door == null || _usedDoors.Contains(door)) yield break;

        _isMoving = true;
        StartFootsteps();
        door.MarkAsUsed();
        _usedDoors.Add(door);

        // Включаем анимацию открытия для всех дверей
        SetAllDoorsOpenState(true);

        yield return MoveToPosition(door.approachPoint);

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

        var doorInteraction = door.GetComponent<DoorEnemyInteraction>();
        doorInteraction?.OnPlayerEntered();

        yield return MoveToPosition(door.exitPoint);

        // Выключаем анимацию открытия для всех дверей
        SetAllDoorsOpenState(false);

        _roomsCounter.RoomCount++;

        if (door._exitStartRoom)
        {
            GameObject startRoom = GameObject.Find("StartRoom(Clone)");
            if (startRoom != null) Destroy(startRoom);
        }

        FindAllPoints();
        UpdateCurrentPointAfterDoor(door);

        StopFootsteps();
        _isMoving = false;
    }

    // Новый метод для управления состоянием всех дверей
    private void SetAllDoorsOpenState(bool isOpen)
    {
        // Находим все аниматоры в сцене
        Animator[] allAnimators = FindObjectsOfType<Animator>();

        foreach (Animator animator in allAnimators)
        {
            // Проверяем есть ли параметр "open" в аниматоре
            if (HasParameter(animator, "open"))
            {
                animator.SetBool("open", isOpen);
            }
        }
    }

    // Вспомогательный метод для проверки параметров аниматора
    private bool HasParameter(Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName && param.type == AnimatorControllerParameterType.Bool)
                return true;
        }
        return false;
    }

    private IEnumerator MoveToPoint(CameraPoint newPoint)
    {
        if (newPoint == null) yield break;

        _isMoving = true;
        StartFootsteps();

        if (_currentPoint != null)
            _currentPoint.SetSelected(false);

        yield return MoveToPosition(newPoint.transform);

        newPoint.SetSelected(true);
        _currentPoint = newPoint;

        StopFootsteps();
        _isMoving = false;
    }

    private IEnumerator MoveToPosition(Transform target)
    {
        if (target == null) yield break;

        _targetPosition = target.position;
        _isMoving = true;

        while (Vector3.Distance(transform.position, _targetPosition) > arrivalThreshold)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _targetPosition,
                moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = _targetPosition;
        _isMoving = false;
    }
    #endregion

    #region Helper Methods
    private void FindAllPoints()
    {
        _allPoints.Clear();
        var foundPoints = FindObjectsOfType<CameraPoint>();

        foreach (var point in foundPoints)
        {
            if (point != null)
            {
                point.OnSelected += OnPointSelected;
                _allPoints.Add(point);
            }
        }
    }

    private void SetInitialPoint()
    {
        if (_allPoints.Count == 0) return;

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

    private void UpdateCurrentPointAfterDoor(CameraDoorPoint door)
    {
        if (_currentPoint != null)
            _currentPoint.SetSelected(false);

        CameraPoint exitPoint = door.exitPoint.GetComponent<CameraPoint>();
        if (exitPoint != null)
        {
            _currentPoint = exitPoint;
            _currentPoint.SetSelected(true);
        }
        else
        {
            SetNewCurrentPointAfterDoor();
        }
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

    private IEnumerator DestroyRoomWithDelay(GameObject room)
    {
        yield return new WaitForSeconds(roomDestroyDelay);

        if (room != null && room != _currentActiveRoom)
        {
            Destroy(room);
        }
    }
    #endregion

    #region Event Handlers
    private void OnPointSelected(CameraPoint point)
    {
        if (_isMoving || point == _currentPoint) return;
        SafeStartCoroutine(MoveToPoint(point));
    }

    private void OnDestroy()
    {
        foreach (var point in _allPoints)
        {
            if (point != null)
                point.OnSelected -= OnPointSelected;
        }

        if (_movementCoroutine != null)
            StopCoroutine(_movementCoroutine);

        StopFootsteps();
    }
    #endregion

    public void StopAllMovement()
    {
        if (_movementCoroutine != null)
        {
            StopCoroutine(_movementCoroutine);
            _movementCoroutine = null;
        }

        StopFootsteps();
        _isMoving = false;
    }
}