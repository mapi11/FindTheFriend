using System;
using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    public event Action<CameraPoint> OnSelected;
    public event Action<CameraPoint> OnDeselected;  // ����� �������

    [Header("Visuals")]
    public GameObject selectionIndicator;

    public void SetSelected(bool state)
    {
        if (selectionIndicator != null)
            selectionIndicator.SetActive(!state);  // �������������: ���� ������� (state=true), ��������� �����������

        if (state)
            OnSelected?.Invoke(this);
        else
            OnDeselected?.Invoke(this);  // ���������� �������, ��� ����� ������ �� �������
    }
}