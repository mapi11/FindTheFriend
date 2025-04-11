using UnityEngine;

public class FlashLightFollow : MonoBehaviour
{
    [Header("Настройки слежения")]
    [Tooltip("Задержка реакции фонарика в секундах")]
    [Range(0.05f, 1f)] public float followDelay = 0.2f;
    [Tooltip("Скорость поворота фонарика")]
    [Range(3f, 15f)] public float rotationSpeed = 8f;

    [Header("Настройки выравнивания")]
    [Tooltip("Автоматически выравнивать фонарик при старте")]
    public bool alignOnStart = true;
    [Tooltip("Ось фонарика, которая должна смотреть вперед")]
    public Vector3 flashlightForwardAxis = Vector3.forward;

    private Transform _cameraTransform;
    private Quaternion _targetRotation;
    private Quaternion _previousCameraRotation;

    private void Awake()
    {
        // Получаем трансформ камеры
        _cameraTransform = Camera.main.transform;

        // Инициализируем начальное положение
        if (alignOnStart)
        {
            AlignInstantly();
        }
        _previousCameraRotation = _cameraTransform.rotation;
        _targetRotation = transform.rotation;
    }

    private void Update()
    {
        // Получаем текущее вращение камеры
        Quaternion currentCameraRot = _cameraTransform.rotation;

        // Если вращение камеры изменилось
        if (currentCameraRot != _previousCameraRotation)
        {
            // Вычисляем целевое вращение с задержкой
            _targetRotation = Quaternion.Slerp(
                _targetRotation,
                currentCameraRot,
                rotationSpeed * Time.deltaTime
            );

            // Применяем вращение к фонарику
            transform.rotation = _targetRotation * Quaternion.FromToRotation(Vector3.forward, flashlightForwardAxis);

            _previousCameraRotation = currentCameraRot;
        }
    }

    // Метод для мгновенного выравнивания с камерой
    public void AlignInstantly()
    {
        if (_cameraTransform != null)
        {
            transform.rotation = _cameraTransform.rotation * Quaternion.FromToRotation(Vector3.forward, flashlightForwardAxis);
            _targetRotation = transform.rotation;
        }
    }

    // Для отладки
    private void OnDrawGizmosSelected()
    {
        if (_cameraTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + transform.TransformDirection(flashlightForwardAxis) * 2);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_cameraTransform.position, _cameraTransform.position + _cameraTransform.forward * 2);
        }
    }
}
