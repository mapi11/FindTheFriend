using UnityEditor;
using UnityEngine;
using YG;

public class MethodRaycast : MonoBehaviour
{
    [Header("������ ���������")]
    public bool watchAddFullRevive = false;
    public bool watchAddRevive = false;
    public bool reloadScene = false;
    public bool customAction = false;

    [Header("��������� Raycast")]
    public float raycastDistance = 3f;
    public LayerMask interactableLayer;

    [Header("Yandex")]
    public YandexGame yandex;

    private HealthSystem healthSystem;
    RoomsCounter RoomsCounter;

    private void Start()
    {
        // ������� HealthSystem �� ������� Character
        GameObject character = GameObject.Find("Character");
        if (character != null)
        {
            healthSystem = character.GetComponent<HealthSystem>();
            if (healthSystem == null)
            {
                Debug.LogError("HealthSystem �� ������ �� ������� Character!");
            }

            RoomsCounter = GetComponent<RoomsCounter>();
        }
        else
        {
            Debug.LogError("������ Character �� ������ � �����!");
        }
    }

    public void TeleportToSecondPoint()
    {
        GameObject character = GameObject.Find("Character");
        if (character == null)
        {
            Debug.LogError("������ Character �� ������!");
            return;
        }

        //GameObject lastCharacterPosition = GameObject.Find("LastCharacterPosition(Clone)");
        //if (lastCharacterPosition == null)
        //{
        //    Debug.LogError("LastCharacterPosition �� �������!");
        //    return;
        //}

        //character.transform.position = lastCharacterPosition.transform.position;
        //Debug.Log("Character �������������� � LastCharacterPosition");
        //Destroy(lastCharacterPosition);
    }

    private void Update()
    {
        HandleRaycast();
    }

    private void HandleRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


            if (Physics.Raycast(ray, out hit, raycastDistance, interactableLayer))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ExecuteMethod(hit.transform.gameObject);
                }
            }
    }

    private void ExecuteMethod(GameObject targetObject)
    {
        if (watchAddFullRevive)
        {
            WatchAddFullRevive(targetObject);
        }
        else if (watchAddRevive)
        {
            WatchAddRevive(targetObject);
        }
        else if (reloadScene)
        {
            ReloadScene();
            //RoomsCounter.RoomCount = 0;
        }
        else if (customAction)
        {
            CustomAction(targetObject);
        }
    }

    public void WatchAddFullRevive(GameObject target)
    {
        Debug.Log("������ " + target.name + " �������� � ������ ���������");

        //TeleportToSecondPoint();

        if (healthSystem != null)
        {
            yandex._RewardedShow(0);
            ReloadScene();
            healthSystem.FullHealRevive();
        }
        else
        {
            Debug.LogError("�� ������� ��������� ���������: HealthSystem �� ������");
        }
    }

    public void WatchAddRevive(GameObject target)
    {
        Debug.Log("������ " + target.name + " �������� � ������ ���������");

        //TeleportToSecondPoint();

        if (healthSystem != null)
        {
            yandex._RewardedShow(1);
            ReloadScene();
            healthSystem.HealRevive();
        }
        else
        {
            Debug.LogError("�� ������� ��������� ���������: HealthSystem �� ������");
        }
    }

    private void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void CustomAction(GameObject target)
    {
        Debug.Log("��������� �������� � ��������: " + target.name);
    }

    public void SetActiveAction(string actionType)
    {
        watchAddFullRevive = false;
        watchAddRevive = false ;
        reloadScene = false;
        customAction = false;

        switch (actionType)
        {
            case "watchAddFullRevive": watchAddFullRevive = true; break;
            case "watchAddRevive": watchAddRevive = true; break;
            case "reload": reloadScene = true; break;
            case "custom": customAction = true; break;
        }
    }
}
