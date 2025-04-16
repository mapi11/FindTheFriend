using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Sensitivity")]
    public float sensitivity = 2.0f;

    [Header("Rotate")]
    public float minVerticalAngle = -80f;
    public float maxVerticalAngle = 80f;

    private float rotationX = 0f;
    private float rotationY = 0f;
    public bool isCameraActive = true; // Флаг активности управления камерой

    FlashlightSystem flashlightSystem;

    void Start()
    {
        flashlightSystem = FindObjectOfType<FlashlightSystem>();

        // Инициализация курсора и углов поворота
        SetCameraActive(true);
        rotationY = transform.eulerAngles.y;
    }

    void Update()
    {
        // Переключение режима по нажатию Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetCameraActive(!isCameraActive);
        }

        // Если управление камерой активно - обрабатываем ввод
        if (isCameraActive)
        {
            HandleCameraRotation();
        }
    }

    void HandleCameraRotation()
    {
        // Получаем ввод от мыши
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Вращение по горизонтали (оси Y)
        rotationY += mouseX;

        // Вращение по вертикали (оси X) с ограничениями
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        // Применяем вращение к камере
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0f);
    }

    void SetCameraActive(bool active)
    {
        isCameraActive = active;

        if (active)
        {
            flashlightSystem._pause = false;
            // Режим управления камерой - курсор скрыт и заблокирован
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            flashlightSystem._pause = true;
            // Режим UI - курсор виден и свободен
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void OnDisable()
    {
        // При отключении скрипта разблокируем курсор
        SetCameraActive(false);
    }
}
