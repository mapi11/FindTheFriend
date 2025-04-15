using UnityEngine;
using UnityEngine.UI;

public class FlashlightSystem : MonoBehaviour
{
    [Header("Light Settings")]
    public Light flashlightLight;
    [Range(3f, 9f)] public float maxIntensity = 9f;
    [Range(1f, 3f)] public float minIntensity = 0.3f;
    [Range(15f, 50f)] public float maxRange = 25f;
    [Range(5f, 10f)] public float minRange = 8f;

    [Header("Battery Settings")]
    [Range(0, 100)] public float batteryCharge = 100f;
    public float drainSpeed = 2f;
    public float rechargeSpeed = 10f;

    [Header("Room Difficulty Settings")]
    [Tooltip("Base multiplier for drainSpeed increase per room")]
    public float roomDifficultyMultiplier = 0.02f;
    [Tooltip("Maximum drainSpeed increase from rooms (percentage)")]
    public float maxRoomDifficultyEffect = 0.5f;

    [Header("UI Settings")]
    public Slider batterySlider;
    [SerializeField] private Image sliderFill;
    public Color fullChargeColor = Color.green;
    public Color mediumChargeColor = Color.yellow;
    public Color lowChargeColor = Color.red;
    [Range(0, 100)] public int mediumThreshold = 50;
    [Range(0, 100)] public int lowThreshold = 20;

    private HealthSystem _healthSystem;
    private RoomsCounter _roomsCounter;
    private float _baseDrainSpeed;
    private bool _isOn = true;

    private void Start()
    {
        _healthSystem = FindObjectOfType<HealthSystem>();
        _roomsCounter = FindObjectOfType<RoomsCounter>();

        if (_healthSystem == null)
        {
            Debug.LogError("HealthSystem not found in scene!");
        }

        if (_roomsCounter == null)
        {
            Debug.LogError("RoomsCounter not found in scene!");
        }

        if (flashlightLight == null)
        {
            flashlightLight = GetComponent<Light>();
        }

        _baseDrainSpeed = drainSpeed;
        InitializeBatteryUI();
        UpdateLightParameters();
    }

    private void InitializeBatteryUI()
    {
        if (batterySlider != null)
        {
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
            UpdateRoomDifficultyEffect();
            UpdateBatteryCharge();
        }

        UpdateLightParameters();
    }

    private void UpdateRoomDifficultyEffect()
    {
        if (_roomsCounter == null) return;

        float difficultyEffect = Mathf.Min(
            _roomsCounter.RoomCount * roomDifficultyMultiplier,
            maxRoomDifficultyEffect);

        drainSpeed = _baseDrainSpeed * (1 + difficultyEffect);
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
        Debug.Log("Battery depleted!");

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
