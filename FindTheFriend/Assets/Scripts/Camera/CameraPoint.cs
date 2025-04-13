using System;
using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    public event Action<CameraPoint> OnSelected;
    public event Action<CameraPoint> OnDeselected;  // Новое событие

    [Header("Visuals")]
    public GameObject selectionIndicator;

    public void SetSelected(bool state)
    {
        if (selectionIndicator != null)
            selectionIndicator.SetActive(!state);  // Инвертировано: если выбрана (state=true), индикатор выключается

        if (state)
            OnSelected?.Invoke(this);
        else
            OnDeselected?.Invoke(this);  // Уведомляем систему, что точка больше не активна
    }
}