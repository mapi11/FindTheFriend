using UnityEngine;

public class LookAtHorizontalCamera : MonoBehaviour
{
    private Transform mainCamera;
    private bool lookAtCamera = true;
    private float initialVerticalRotation;

    [Header("Smooth Follow Settings")]
    [SerializeField] private float rotationSpeed = 5f; // �������� �������� ��������
    [SerializeField] private float maxRotationAngle = 70f; // ������������ ���� �������� (�����������)

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
        // �������� ����������� � ������, ��������� ������������ ���
        Vector3 directionToCamera = mainCamera.position - transform.position;
        directionToCamera.y = 0;

        if (directionToCamera != Vector3.zero)
        {
            // ��������� ������� �������
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);

            // ��������� ��������� ������������ �������
            Vector3 targetEuler = targetRotation.eulerAngles;
            targetEuler.x = initialVerticalRotation;

            // ��������� ����������� ���� �������� (���� �����)
            if (maxRotationAngle > 0)
            {
                targetEuler.y = ClampAngle(targetEuler.y, -maxRotationAngle, maxRotationAngle);
            }

            // ������ ������������� � �������� ��������
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.Euler(targetEuler),
                rotationSpeed * Time.deltaTime
            );
        }
    }

    // ����� ��� ����������� ����������� ����� ������
    float ClampAngle(float angle, float min, float max)
    {
        if (angle > 180) angle -= 360;
        angle = Mathf.Clamp(angle, min, max);
        return angle;
    }

    void FindMainCamera()
    {
        // ���� ������� �� ����, ����� �� �����
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
