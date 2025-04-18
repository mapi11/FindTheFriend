using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform mainCamera;
    private bool lookAtCamera = true;

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
        // Ищем объект с именем "MainCamera"
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
