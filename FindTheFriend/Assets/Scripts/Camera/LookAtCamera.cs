using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform mainCamera;
    private bool lookAtCamera = true;

    void Start()
    {
        FindMainCamera();
    }

    void Update()
    {
        // ���� ������ ������� � ���� ������� - ������������ ������ � ������
        if (lookAtCamera && mainCamera != null)
        {
            transform.LookAt(mainCamera);
        }
    }

    void FindMainCamera()
    {
        // ���� ������ � ������ "MainCamera"
        GameObject cameraObj = GameObject.Find("MainCamera");

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
