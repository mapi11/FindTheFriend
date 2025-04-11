using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    [Header("Battery Settings")]
    public float chargeAmount = 25f; // Сколько заряда добавляет эта батарейка
    public GameObject pickupEffect; // Эффект при подборе (опционально)

    private void OnMouseDown() // Вызывается при клике ЛКМ по объекту с коллайдером
    {
        FlashlightBattery flashlight = FindObjectOfType<FlashlightBattery>();
        if (flashlight != null)
        {
            flashlight.AddCharge(chargeAmount);

            // Воспроизводим эффект
            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            // Уничтожаем батарейку
            Destroy(gameObject);

            Debug.Log("Battery picked up!");
        }
    }

    // Визуализация в редакторе
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
