using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject menuPrefab;      // Префаб меню
    public Transform spawnPosition;    // Позиция для спавна меню

    [Header("Menu State")]
    [SerializeField] private bool isMenuOpen = false; // Текущее состояние меню

    private GameObject currentMenuInstance;  // Ссылка на экземпляр мен

    MouseLook mouseLook;

    private void Awake()
    {
        mouseLook = FindAnyObjectByType<MouseLook>();
    }

    void Update()
    {
        // Опционально: управление через Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    // Открывает меню
    public void MenuOpen()
    {
        if (!isMenuOpen)
        {
            isMenuOpen = true;
            UpdateMenuInstance();

            mouseLook.isCameraActive = false;
        }
    }

    // Закрывает меню
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

    // Переключает состояние меню
    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        UpdateMenuInstance();
    }

    // Обновляет видимое состояние меню
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

    // Возвращает текущее состояние меню
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