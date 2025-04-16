using UnityEngine;
using UnityEngine.UI;
using YG;

public class AddVideoMenu : MonoBehaviour
{
    [Header("Buttons")]
    //[SerializeField] private Button _btnGlassesAdd;
    //[SerializeField] private Button _btnFlashlightAdd;
    [SerializeField] private Button _btnHealthAdd;

    HealthSystem healthSystem;
    OpenMenu openMenu;


    private void Awake()
    {
        healthSystem = FindAnyObjectByType<HealthSystem>();
        openMenu = FindAnyObjectByType<OpenMenu>();
    }

    private void Start()
    {
        //_btnGlassesAdd.onClick.AddListener(GlassesAdd);
        //_btnFlashlightAdd.onClick.AddListener(FlashlightAdd);
        //_btnHealthAdd.onClick.AddListener(HealthAdd);
    }

    public void AddVideoAdd()
    {
        YandexGame.RewVideoShow(0);
    }

    public void HealthAdd()
    {
        if (healthSystem.currentHealth != healthSystem.maxHealth)
        {


            healthSystem.Heal();

            AddVideoAdd();

            openMenu.CloseMenu();
        }
    }
}