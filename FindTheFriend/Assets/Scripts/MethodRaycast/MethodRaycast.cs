using UnityEngine;

public class MethodRaycast : MonoBehaviour
{
    [Header("������ ���������")]
    public bool watchAdd = false;
    public bool reloadScene = false;
    public bool customAction = false;

    [Header("��������� Raycast")]
    public float raycastDistance = 3f;
    public LayerMask interactableLayer;

    private HealthSystem healthSystem;

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

        GameObject secondPoint = GameObject.Find("secondPoint");
        if (secondPoint == null)
        {
            Debug.LogError("����� secondPoint �� �������!");
            return;
        }

        character.transform.position = secondPoint.transform.position;
        Debug.Log("Character �������������� � secondPoint");
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
        if (watchAdd)
        {
            WatchAdd(targetObject);
        }
        else if (reloadScene)
        {
            ReloadScene();
        }
        else if (customAction)
        {
            CustomAction(targetObject);
        }
    }

    private void WatchAdd(GameObject target)
    {
        Debug.Log("������ " + target.name + " �������� � ������ ���������");

        TeleportToSecondPoint();

        if (healthSystem != null)
        {
            healthSystem.FullHeal();
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
        watchAdd = false;
        reloadScene = false;
        customAction = false;

        switch (actionType)
        {
            case "watch": watchAdd = true; break;
            case "reload": reloadScene = true; break;
            case "custom": customAction = true; break;
        }
    }
}
