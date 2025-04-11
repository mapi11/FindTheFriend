using UnityEngine;
using System;

public class CameraPoint : MonoBehaviour
{
    public event Action<CameraPoint> OnSelected;

    [Header("Visuals")]
    public GameObject selectionIndicator;

    public void SetSelected(bool state)
    {
        if (selectionIndicator != null)
            selectionIndicator.SetActive(state);

        if (state)
            OnSelected?.Invoke(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}