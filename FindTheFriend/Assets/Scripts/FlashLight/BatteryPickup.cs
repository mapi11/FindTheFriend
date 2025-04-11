using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    [Header("Battery Settings")]
    public float chargeAmount = 25f; // ������� ������ ��������� ��� ���������
    public GameObject pickupEffect; // ������ ��� ������� (�����������)

    private void OnMouseDown() // ���������� ��� ����� ��� �� ������� � �����������
    {
        FlashlightBattery flashlight = FindObjectOfType<FlashlightBattery>();
        if (flashlight != null)
        {
            flashlight.AddCharge(chargeAmount);

            // ������������� ������
            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            // ���������� ���������
            Destroy(gameObject);

            Debug.Log("Battery picked up!");
        }
    }

    // ������������ � ���������
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
