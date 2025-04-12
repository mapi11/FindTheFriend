using System;
using UnityEngine;

public class GlassesPoint : MonoBehaviour
{
    public event Action<GlassesPoint> OnSelected;

    [Header("Visuals")]
    public GameObject selectionIndicator;
    public float destroyDelay = 0.5f; // �������� ����� ���������

    private GlassesScript _glassesScript;
    private bool _wasUsed = false;

    private void Start()
    {
        // ������� ������ GlassesScript �� �����
        _glassesScript = FindObjectOfType<GlassesScript>();
        if (_glassesScript == null)
        {
            Debug.LogError("GlassesScript �� ������ �� �����!");
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

        // ������� ������ ����� ��������
        Destroy(gameObject, destroyDelay);
    }
}
