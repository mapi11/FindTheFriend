using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Sensitivity")]
    public float sensitivity = 2.0f;

    [Header("Rotate")]
    public float minVerticalAngle = -80f; // ����������� ���� ������� ����
    public float maxVerticalAngle = 80f;  // ������������ ���� ������� �����

    private float rotationX = 0f; // ������� ���� �������� �� ���������
    private float rotationY = 0f; // ������� ���� �������� �� �����������

    void Start()
    {
        // ������������� � ������ ������
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // ������������� ������� ����� ��������
        rotationY = transform.eulerAngles.y;
    }

    void Update()
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

    void OnDisable()
    {
        // ��� ���������� ������� ������������ ������
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
