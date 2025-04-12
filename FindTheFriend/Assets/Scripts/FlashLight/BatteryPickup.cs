using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    [Header("Settings")]
    public float rechargeAmount = 25f;
    public AudioClip pickupSound;

    private void OnMouseDown()
    {
        FlashlightSystem flashlight = FindObjectOfType<FlashlightSystem>();
        if (flashlight != null)
        {
            flashlight.RechargeBattery(rechargeAmount);

            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            Destroy(gameObject);
        }
    }
}
