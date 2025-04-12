using System;
using UnityEngine;

public class GlassesPoint : MonoBehaviour
{
    public event Action<GlassesPoint> OnSelected;

    [Header("Visuals")]
    public GameObject selectionIndicator;
    public float destroyDelay = 0.5f; // Задержка перед удалением

    private GlassesScript _glassesScript;
    private bool _wasUsed = false;

    private void Start()
    {
        // Находим скрипт GlassesScript на сцене
        _glassesScript = FindObjectOfType<GlassesScript>();
        if (_glassesScript == null)
        {
            Debug.LogError("GlassesScript не найден на сцене!");
        }
    }

    public void SetSelected(bool state)
    {
        if (selectionIndicator != null)
            selectionIndicator.SetActive(state);

        if (state && !_wasUsed)
        {
            _wasUsed = true;
            OnSelected?.Invoke(this);
            ActivateGlasses();
        }
    }

    private void ActivateGlasses()
    {
        if (_glassesScript != null)
        {
            _glassesScript.GetGlasses();
        }

        // Удаляем объект после задержки
        Destroy(gameObject, destroyDelay);
    }
}
