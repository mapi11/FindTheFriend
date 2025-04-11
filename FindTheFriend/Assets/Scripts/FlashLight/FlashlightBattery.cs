using UnityEngine;

public class FlashlightBattery : MonoBehaviour
{
    [Header("Light Settings")]
    public Light spotLight; // Ссылка на компонент света фонарика
    [Range(1f, 6f)] public float minMaxIntensity = 6f;
    [Range(15f, 30f)] public float minMaxRange = 30f; // Диапазон изменения расстояния света

    [Header("Battery Settings")]
    [Range(0, 120)] public float currentCharge = 120f;
    public float drainSpeed = 5f; // Скорость расхода заряда в секунду

    private float _maxIntensity;
    private float _minIntensity = 1f;
    private float _maxRange;
    private float _minRange = 15f;

    private void Start()
    {
        if (spotLight != null)
        {
            _maxIntensity = minMaxIntensity;
            _maxRange = minMaxRange;

            // Инициализируем начальные значения
            spotLight.intensity = _maxIntensity;
            spotLight.range = _maxRange;
        }
    }

    private void Update()
    {
        if (currentCharge > 0)
        {
            currentCharge -= drainSpeed * Time.deltaTime;
            currentCharge = Mathf.Clamp(currentCharge, 0, 100);
            UpdateLightParameters();

            spotLight.gameObject.SetActive(true);
        }
        else
        {
            spotLight.gameObject.SetActive(false);
        }
    }

    private void UpdateLightParameters()
    {
        if (spotLight == null) return;

        // Рассчитываем процент заряда
        float chargePercent = currentCharge / 100f;

        // Обновляем интенсивность
        spotLight.intensity = Mathf.Lerp(_minIntensity, _maxIntensity, chargePercent);

        // Обновляем расстояние света (медленнее чем интенсивность)
        float rangePercent = Mathf.Pow(chargePercent, 0.5f); // Квадратный корень для более медленного уменьшения
        spotLight.range = Mathf.Lerp(_minRange, _maxRange, rangePercent);
    }

    public void AddCharge(float amount)
    {
        currentCharge += amount;
        currentCharge = Mathf.Clamp(currentCharge, 0, 100);
        UpdateLightParameters();
        Debug.Log($"Battery charged! Current: {currentCharge}%");
    }

    // Для отладки в редакторе
    private void OnDrawGizmosSelected()
    {
        if (spotLight != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(spotLight.transform.position, spotLight.range);
        }
    }
}
