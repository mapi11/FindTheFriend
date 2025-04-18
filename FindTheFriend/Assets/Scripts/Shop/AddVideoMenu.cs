using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YG;

public class AddVideoMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _btnGlassesAdd;
    [SerializeField] private Button _btnFlashlightAdd;
    [SerializeField] private Button _btnHealthAdd;

    [Header("Open shop")]
    [SerializeField] private Button _btnOpenShop;
    [SerializeField] private Button _btnCloseShop;
    [SerializeField] private GameObject _shopWindow;
    [SerializeField] private TextMeshProUGUI _txtMoneyCount;

    GlassesScript _glassesScript;
    FlashlightSystem _flashlightSystem;
    HealthSystem healthSystem;
    OpenMenu openMenu;
    MoneyCount moneyCount;


    private void Awake()
    {
        _glassesScript = FindAnyObjectByType<GlassesScript>();
        _flashlightSystem = FindAnyObjectByType<FlashlightSystem>();
        healthSystem = FindAnyObjectByType<HealthSystem>();
        openMenu = FindAnyObjectByType<OpenMenu>();
        moneyCount = FindAnyObjectByType<MoneyCount>();
    }

    private void Start()
    {
        _btnGlassesAdd.onClick.AddListener(GlassesAdd);
        _btnFlashlightAdd.onClick.AddListener(FlashlightAdd);
        _btnOpenShop.onClick.AddListener(OpenShop);
        _btnCloseShop.onClick.AddListener(CloseShop);
    }

    private void Update()
    {
        _txtMoneyCount.text = moneyCount._currentMoney.ToString();
    }

    public void AddVideoAdd()
    {
        YandexGame.RewVideoShow(0);
    }

    public void GlassesAdd()
    {

            _glassesScript.GetGlasses();

            AddVideoAdd();

            openMenu.CloseMenu();
        
    }

    public void FlashlightAdd()
    {

            _flashlightSystem.batteryCharge = 100;

            AddVideoAdd();

            openMenu.CloseMenu();
        
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

    private void OpenShop()
    {
        _shopWindow.SetActive(true);
    }
    private void CloseShop()
    {
        _shopWindow.SetActive(false);
    }

    public void DelSave()
    {
        PlayerPrefs.DeleteKey("CurrentSkinID");
        PlayerPrefs.DeleteKey("PurchasedSkins");
        PlayerPrefs.Save();
    }
}