using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject menuPrefab;      // ������ ����
    public Transform spawnPosition;    // ������� ��� ������ ����

    [Header("Menu State")]
    [SerializeField] private bool isMenuOpen = false; // ������� ��������� ����

    private GameObject currentMenuInstance;  // ������ �� ��������� ���

    MouseLook mouseLook;

    private void Awake()
    {
        mouseLook = FindAnyObjectByType<MouseLook>();
    }

    void Update()
    {
        // �����������: ���������� ����� Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    // ��������� ����
    public void MenuOpen()
    {
        if (!isMenuOpen)
        {
            isMenuOpen = true;
            UpdateMenuInstance();

            mouseLook.isCameraActive = false;
        }
    }

    // ��������� ����
    public void CloseMenu()
    {
        if (isMenuOpen)
        {
            isMenuOpen = false;
            UpdateMenuInstance();

            mouseLook.isCameraActive = true;

            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }
    }

    // ����������� ��������� ����
    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        UpdateMenuInstance();
    }

    // ��������� ������� ��������� ����
    private void UpdateMenuInstance()
    {
        if (isMenuOpen)
        {
            if (currentMenuInstance == null)
            {
                currentMenuInstance = Instantiate(menuPrefab, spawnPosition);
            }
        }
        else
        {
            if (currentMenuInstance != null)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                Destroy(currentMenuInstance);
                currentMenuInstance = null;

            }
        }
    }

    // ���������� ������� ��������� ����
    public bool IsMenuOpen()
    {
        return isMenuOpen;
    }

    void OnDestroy()
    {
        if (currentMenuInstance != null)
        {
            Destroy(currentMenuInstance);
        }
    }
}