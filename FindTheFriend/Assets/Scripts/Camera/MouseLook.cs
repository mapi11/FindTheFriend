using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Sensitivity")]
    public float sensitivity = 2.0f;

    [Header("Rotate")]
    public float minVerticalAngle = -80f; // Минимальный угол наклона вниз
    public float maxVerticalAngle = 80f;  // Максимальный угол наклона вверх

    private float rotationX = 0f; // Текущий угол вращения по вертикали
    private float rotationY = 0f; // Текущий угол вращения по горизонтали

    void Start()
    {
        // Заблокировать и скрыть курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Инициализация текущих углов поворота
        rotationY = transform.eulerAngles.y;
    }

    void Update()
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

    void OnDisable()
    {
        // При отключении скрипта разблокируем курсор
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
