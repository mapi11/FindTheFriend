using UnityEngine;

public class LookAtHorizontalCamera : MonoBehaviour
{
    private Transform mainCamera;
    private bool lookAtCamera = true;
    private float initialVerticalRotation;

    [Header("Smooth Follow Settings")]
    [SerializeField] private float rotationSpeed = 5f; // Скорость плавного поворота
    [SerializeField] private float maxRotationAngle = 70f; // Максимальный угол поворота (опционально)

    void Start()
    {
        FindMainCamera();
        initialVerticalRotation = transform.eulerAngles.x;
    }

    void Update()
    {
        if (lookAtCamera && mainCamera != null)
        {
            SmoothLookAtHorizontal();
        }
    }

    void SmoothLookAtHorizontal()
    {
        // Получаем направление к камере, игнорируя вертикальную ось
        Vector3 directionToCamera = mainCamera.position - transform.position;
        directionToCamera.y = 0;

        if (directionToCamera != Vector3.zero)
        {
            // Вычисляем целевой поворот
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);

            // Сохраняем начальный вертикальный поворот
            Vector3 targetEuler = targetRotation.eulerAngles;
            targetEuler.x = initialVerticalRotation;

            // Применяем ограничение угла поворота (если нужно)
            if (maxRotationAngle > 0)
            {
                targetEuler.y = ClampAngle(targetEuler.y, -maxRotationAngle, maxRotationAngle);
            }

            // Плавно интерполируем к целевому повороту
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.Euler(targetEuler),
                rotationSpeed * Time.deltaTime
            );
        }
    }

    // Метод для корректного ограничения углов Эйлера
    float ClampAngle(float angle, float min, float max)
    {
        if (angle > 180) angle -= 360;
        angle = Mathf.Clamp(angle, min, max);
        return angle;
    }

    void FindMainCamera()
    {
        // Ищем сначала по тегу, потом по имени
        GameObject cameraObj = GameObject.FindGameObjectWithTag("MainCamera") ?? GameObject.Find("MainCamera");

        if (cameraObj != null)
        {
            mainCamera = cameraObj.transform;
            Debug.Log("MainCamera found and assigned");
        }
        else
        {
            Debug.LogWarning("MainCamera not found in scene!");
            lookAtCamera = false;
        }
    }
}
