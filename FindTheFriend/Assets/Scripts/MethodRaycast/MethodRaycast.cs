using UnityEditor;
using UnityEngine;
using YG;

public class MethodRaycast : MonoBehaviour
{
    [Header("Методы активации")]
    public bool watchAddFullRevive = false;
    public bool watchAddRevive = false;
    public bool reloadScene = false;
    public bool customAction = false;

    [Header("Настройки Raycast")]
    public float raycastDistance = 3f;
    public LayerMask interactableLayer;

    [Header("Yandex")]
    public YandexGame yandex;

    private HealthSystem healthSystem;
    RoomsCounter RoomsCounter;

    private void Start()
    {
        // Находим HealthSystem на объекте Character
        GameObject character = GameObject.Find("Character");
        if (character != null)
        {
            healthSystem = character.GetComponent<HealthSystem>();
            if (healthSystem == null)
            {
                Debug.LogError("HealthSystem не найден на объекте Character!");
            }

            RoomsCounter = GetComponent<RoomsCounter>();
        }
        else
        {
            Debug.LogError("Объект Character не найден в сцене!");
        }
    }

    public void TeleportToSecondPoint()
    {
        GameObject character = GameObject.Find("Character");
        if (character == null)
        {
            Debug.LogError("Объект Character не найден!");
            return;
        }

        //GameObject lastCharacterPosition = GameObject.Find("LastCharacterPosition(Clone)");
        //if (lastCharacterPosition == null)
        //{
        //    Debug.LogError("LastCharacterPosition не найдена!");
        //    return;
        //}

        //character.transform.position = lastCharacterPosition.transform.position;
        //Debug.Log("Character телепортирован к LastCharacterPosition");
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
        Debug.Log("Объект " + target.name + " добавлен в список просмотра");

        //TeleportToSecondPoint();

        if (healthSystem != null)
        {
            yandex._RewardedShow(0);
            ReloadScene();
            healthSystem.FullHealRevive();
        }
        else
        {
            Debug.LogError("Не удалось выполнить исцеление: HealthSystem не найден");
        }
    }

    public void WatchAddRevive(GameObject target)
    {
        Debug.Log("Объект " + target.name + " добавлен в список просмотра");

        //TeleportToSecondPoint();

        if (healthSystem != null)
        {
            yandex._RewardedShow(1);
            ReloadScene();
            healthSystem.HealRevive();
        }
        else
        {
            Debug.LogError("Не удалось выполнить исцеление: HealthSystem не найден");
        }
    }

    private void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void CustomAction(GameObject target)
    {
        Debug.Log("Кастомное действие с объектом: " + target.name);
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
