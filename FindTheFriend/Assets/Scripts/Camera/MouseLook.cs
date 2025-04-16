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
    public bool isCameraActive = true; // ���� ���������� ���������� �������

    FlashlightSystem flashlightSystem;

    void Start()
    {
        flashlightSystem = FindObjectOfType<FlashlightSystem>();

        // ������������� ������� � ����� ��������
        SetCameraActive(true);
        rotationY = transform.eulerAngles.y;
    }

    void Update()
    {
        // ������������ ������ �� ������� Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetCameraActive(!isCameraActive);
        }

        // ���� ���������� ������� ������� - ������������ ����
        if (isCameraActive)
        {
            HandleCameraRotation();
        }
    }

    void HandleCameraRotation()
    {
        // �������� ���� �� ����
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // �������� �� ����������� (��� Y)
        rotationY += mouseX;

        // �������� �� ��������� (��� X) � �������������
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);

        // ��������� �������� � ������
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0f);
    }

    void SetCameraActive(bool active)
    {
        isCameraActive = active;

        if (active)
        {
            flashlightSystem._pause = false;
            // ����� ���������� ������� - ������ ����� � ������������
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            flashlightSystem._pause = true;
            // ����� UI - ������ ����� � ��������
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void OnDisable()
    {
        // ��� ���������� ������� ������������ ������
        SetCameraActive(false);
    }
}
