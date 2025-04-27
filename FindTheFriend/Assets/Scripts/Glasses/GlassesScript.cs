using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

using UnityEngine.UI;

public class GlassesScript : MonoBehaviour
{
    [Header("Volume Settings")]
    public Volume volumeComponent;
    public VolumeProfile normalProfile;
    public VolumeProfile glassesProfile;

    [Header("Slider Settings")]
    public Slider glassesSlider;
    public float drainSpeed = 10f; // Скорость уменьшения заряда
    public bool isGlassesActive = false;

    [Header("Slider Colors")]
    public Image fillArea;
    public Color fullChargeColor = Color.green;
    public Color mediumChargeColor = Color.yellow;
    public Color lowChargeColor = Color.red;
    [Range(0, 100)] public int mediumThreshold = 50;
    [Range(0, 100)] public int lowThreshold = 20;

    private List<GlassesPoint> _allPoints = new List<GlassesPoint>();
    private GlassesPoint _currentPoint;

    private void Awake()
    {
        FindAllPoints();
        InitializeSlider();
    }

    private void Start()
    {
        if (volumeComponent == null)
        {
            Debug.LogError("Volume component not assigned!", this);
            return;
        }

        if (normalProfile != null)
        {
            volumeComponent.profile = normalProfile;
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    GetGlasses();
        //}
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    SetGlasses();
        //}

        if (Input.GetMouseButtonDown(0))
            HandleClick();

        // Обновляем заряд, если очки активны
        if (isGlassesActive && glassesSlider != null)
        {
            glassesSlider.value -= drainSpeed * Time.deltaTime;
            UpdateSliderColor();

            if (glassesSlider.value <= 0)
            {
                SetGlasses();
            }
        }
    }

    private void InitializeSlider()
    {
        if (glassesSlider != null)
        {
            glassesSlider.gameObject.SetActive(false);
            glassesSlider.maxValue = 100;
            glassesSlider.minValue = 0;
            glassesSlider.value = 0;
        }
    }

    public void GetGlasses()
    {
        if (volumeComponent != null && glassesProfile != null)
        {
            volumeComponent.profile = glassesProfile;
            isGlassesActive = true;

            if (glassesSlider != null)
            {
                glassesSlider.value = 100;
                glassesSlider.gameObject.SetActive(true);
                UpdateSliderColor();
            }

            Debug.Log("Очки активированы, заряд 100%");
        }
    }

    public void SetGlasses()
    {
        if (volumeComponent != null && normalProfile != null)
        {
            volumeComponent.profile = normalProfile;
            isGlassesActive = false;

            if (glassesSlider != null)
            {
                glassesSlider.gameObject.SetActive(false);
            }

            Debug.Log("Очки деактивированы");
        }
    }

    private void UpdateSliderColor()
    {
        if (fillArea == null) return;

        float currentCharge = glassesSlider.value;
        float normalizedCharge = currentCharge / 100f;

        if (currentCharge <= lowThreshold)
        {
            fillArea.color = Color.Lerp(lowChargeColor, mediumChargeColor, normalizedCharge / lowThreshold);
        }
        else if (currentCharge <= mediumThreshold)
        {
            fillArea.color = Color.Lerp(mediumChargeColor, fullChargeColor,
                                      (normalizedCharge - lowThreshold / 100f) / ((mediumThreshold - lowThreshold) / 100f));
        }
        else
        {
            fillArea.color = fullChargeColor;
        }
    }

    private void FindAllPoints()
    {
        _allPoints.Clear();
        _allPoints.AddRange(FindObjectsOfType<GlassesPoint>());

        foreach (var point in _allPoints)
        {
            point.OnSelected += OnGlassesPointSelected;
        }
    }

    private void HandleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var point = hit.collider.GetComponent<GlassesPoint>();
            if (point != null && point != _currentPoint)
            {
                point.SetSelected(true);
            }
        }
    }

    private void OnGlassesPointSelected(GlassesPoint point)
    {
        _currentPoint = point;
    }

    private void OnDestroy()
    {
        foreach (var point in _allPoints)
        {
            if (point != null)
                point.OnSelected -= OnGlassesPointSelected;
        }
    }
}
