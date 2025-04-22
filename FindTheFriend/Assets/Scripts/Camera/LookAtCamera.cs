using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform mainCamera;
    private bool lookAtCamera = true;
    private bool cameraFound;

    void Start()
    {
        //FindMainCamera();
    }

    void Update()
    {
        FindMainCamera();

        // Если камера найдена и флаг включен - поворачиваем объект к камере
        if (lookAtCamera && mainCamera != null)
        {
            transform.LookAt(mainCamera);
        }
    }

    void FindMainCamera()
    {
        if (cameraFound) return;

        GameObject cameraObj = GameObject.FindGameObjectWithTag("MainCamera");

        if (cameraObj != null)
        {
            mainCamera = cameraObj.transform;
            cameraFound = true;
            Debug.Log("MainCamera found and assigned", this);
        }
        else
        {
            Debug.LogWarning("MainCamera not found in scene!", this);
            lookAtCamera = false;
            cameraFound = true;
        }
    }
}
