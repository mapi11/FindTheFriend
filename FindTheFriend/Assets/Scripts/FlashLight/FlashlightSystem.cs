using UnityEngine;
using UnityEngine.UI;

public class FlashlightSystem : MonoBehaviour
{
    [Header("Light Settings")]
    public Light flashlightLight;
    [Range(0.5f, 2f)] public float maxIntensity = 1.5f;
    [Range(0.1f, 0.5f)] public float minIntensity = 0.3f;
    [Range(15f, 30f)] public float maxRange = 25f;
    [Range(5f, 10f)] public float minRange = 8f;

    [Header("Battery Settings")]
    [Range(0, 100)] public float batteryCharge = 100f;
    public float drainSpeed = 2f;
    public float rechargeSpeed = 10f;

    [Header("UI Settings")]
    public Slider batterySlider;
    [SerializeField] private Image sliderFill; // Теперь приватное с сериализацией
    public Color fullChargeColor = Color.green;
    public Color mediumChargeColor = Color.yellow;
    public Color lowChargeColor = Color.red;
    [Range(0, 100)] public int mediumThreshold = 50;
    [Range(0, 100)] public int lowThreshold = 20;

    private HealthSystem _healthSystem;
    private bool _isOn = true;

    private void Start()
    {
        _healthSystem = FindObjectOfType<HealthSystem>();
        if (_healthSystem == null)
        {
            Debug.LogError("HealthSystem не найден в сцене!");
        }

        if (flashlightLight == null)
        {
            flashlightLight = GetComponent<Light>();
        }

        InitializeBatteryUI();
        UpdateLightParameters();
    }

    private void InitializeBatteryUI()
    {
        if (batterySlider != null)
        {
            // Автоматически находим Fill если не назначен
            if (sliderFill == null)
            {
                sliderFill = batterySlider.fillRect.GetComponent<Image>();
            }

            batterySlider.maxValue = 100;
            batterySlider.minValue = 0;
            batterySlider.value = batteryCharge;
            UpdateSliderColor();
        }
    }

    private void Update()
    {
        if (CanDrainBattery())
        {
            UpdateBatteryCharge();
        }

        UpdateLightParameters();
    }

    private bool CanDrainBattery()
    {
        return _isOn && (_healthSystem == null || _healthSystem.currentHealth > 0);
    }

    private void UpdateBatteryCharge()
    {
        batteryCharge -= drainSpeed * Time.deltaTime;
        batteryCharge = Mathf.Clamp(batteryCharge, 0, 100);
        UpdateBatteryUI();

        if (batteryCharge <= 0)
        {
            BatteryDepleted();
        }
    }

    private void BatteryDepleted()
    {
        Debug.Log("Батарея разряжена!");

        if (_healthSystem != null && _healthSystem.currentHealth > 0)
        {
            _healthSystem.Die();
        }

        ToggleFlashlight(false);
    }

    private void UpdateLightParameters()
    {
        if (flashlightLight == null) return;

        float chargePercent = batteryCharge / 100f;
        flashlightLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, chargePercent);
        flashlightLight.range = Mathf.Lerp(minRange, maxRange, Mathf.Pow(chargePercent, 0.7f));
    }

    private void UpdateBatteryUI()
    {
        if (batterySlider != null)
        {
            batterySlider.value = batteryCharge;
            UpdateSliderColor();
        }
    }

    private void UpdateSliderColor()
    {
        if (sliderFill == null) return;

        float currentCharge = batteryCharge;
        float normalizedCharge = currentCharge / 100f;

        if (currentCharge <= lowThreshold)
        {
            sliderFill.color = Color.Lerp(lowChargeColor, mediumChargeColor, normalizedCharge / lowThreshold);
        }
        else if (currentCharge <= mediumThreshold)
        {
            sliderFill.color = Color.Lerp(mediumChargeColor, fullChargeColor,
                (normalizedCharge - lowThreshold / 100f) / ((mediumThreshold - lowThreshold) / 100f));
        }
        else
        {
            sliderFill.color = fullChargeColor;
        }
    }

    public void RechargeBattery(float amount)
    {
        batteryCharge += amount;
        batteryCharge = Mathf.Clamp(batteryCharge, 0, 100);
        UpdateBatteryUI();
        Debug.Log($"Battery recharged! Current: {batteryCharge}%");
    }

    public void ToggleFlashlight()
    {
        ToggleFlashlight(!_isOn);
    }

    public void ToggleFlashlight(bool state)
    {
        _isOn = state;
        if (flashlightLight != null)
        {
            flashlightLight.enabled = _isOn;
        }
    }

    // Для автоматической настройки в редакторе
    private void OnValidate()
    {
        if (batterySlider != null && sliderFill == null)
        {
            sliderFill = batterySlider.fillRect?.GetComponent<Image>();
        }
    }

    [ContextMenu("Test Recharge 25%")]
    private void TestRecharge()
    {
        RechargeBattery(25f);
    }
}
