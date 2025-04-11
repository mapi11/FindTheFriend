using UnityEngine;

public class FlashLightFollow : MonoBehaviour
{
    [Header("��������� ��������")]
    [Tooltip("�������� ������� �������� � ��������")]
    [Range(0.05f, 1f)] public float followDelay = 0.2f;
    [Tooltip("�������� �������� ��������")]
    [Range(3f, 15f)] public float rotationSpeed = 8f;

    [Header("��������� ������������")]
    [Tooltip("������������� ����������� ������� ��� ������")]
    public bool alignOnStart = true;
    [Tooltip("��� ��������, ������� ������ �������� ������")]
    public Vector3 flashlightForwardAxis = Vector3.forward;

    private Transform _cameraTransform;
    private Quaternion _targetRotation;
    private Quaternion _previousCameraRotation;

    private void Awake()
    {
        // �������� ��������� ������
        _cameraTransform = Camera.main.transform;

        // �������������� ��������� ���������
        if (alignOnStart)
        {
            AlignInstantly();
        }
        _previousCameraRotation = _cameraTransform.rotation;
        _targetRotation = transform.rotation;
    }

    private void Update()
    {
        // �������� ������� �������� ������
        Quaternion currentCameraRot = _cameraTransform.rotation;

        // ���� �������� ������ ����������
        if (currentCameraRot != _previousCameraRotation)
        {
            // ��������� ������� �������� � ���������
            _targetRotation = Quaternion.Slerp(
                _targetRotation,
                currentCameraRot,
                rotationSpeed * Time.deltaTime
            );

            // ��������� �������� � ��������
            transform.rotation = _targetRotation * Quaternion.FromToRotation(Vector3.forward, flashlightForwardAxis);

            _previousCameraRotation = currentCameraRot;
        }
    }

    // ����� ��� ����������� ������������ � �������
    public void AlignInstantly()
    {
        if (_cameraTransform != null)
        {
            transform.rotation = _cameraTransform.rotation * Quaternion.FromToRotation(Vector3.forward, flashlightForwardAxis);
            _targetRotation = transform.rotation;
        }
    }

    // ��� �������
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
