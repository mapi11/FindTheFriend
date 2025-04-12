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
    public Slider batterySlider; // Слайдер для отображения заряда
    public Image fillArea; // Область заполнения слайдера
    public Color fullChargeColor = Color.green;
    public Color mediumChargeColor = Color.yellow;
    public Color lowChargeColor = Color.red;
    [Range(0, 50)] public int mediumThreshold = 50; // Порог среднего заряда
    [Range(0, 20)] public int lowThreshold = 20; // Порог низкого заряда

    private bool _isOn = true;

    private void Start()
    {
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
            batterySlider.maxValue = 100;
            batterySlider.minValue = 0;
            batterySlider.value = batteryCharge;
            UpdateSliderColor();
        }
    }

    private void Update()
    {
        if (_isOn)
        {
            batteryCharge -= drainSpeed * Time.deltaTime;
            batteryCharge = Mathf.Clamp(batteryCharge, 0, 100);
            UpdateBatteryUI();
        }

        UpdateLightParameters();
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
        if (fillArea == null) return;

        if (batteryCharge <= lowThreshold)
        {
            fillArea.color = lowChargeColor;
        }
        else if (batteryCharge <= mediumThreshold)
        {
            fillArea.color = mediumChargeColor;
        }
        else
        {
            fillArea.color = fullChargeColor;
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
        _isOn = !_isOn;
        flashlightLight.enabled = _isOn;
    }

    [ContextMenu("Test Recharge 25%")]
    private void TestRecharge()
    {
        RechargeBattery(25f);
    }
}
