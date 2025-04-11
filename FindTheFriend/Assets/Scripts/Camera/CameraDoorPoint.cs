using UnityEngine;

public class CameraDoorPoint : MonoBehaviour
{
    [Header("Movement Points")]
    public Transform approachPoint;
    public Transform exitPoint;

    [Header("Room Generation")]
    public Transform roomSpawnPoint;
    public GameObject[] roomPrefabs;

    [Header("Visual Feedback")]
    public GameObject doorModel;
    public Material usedDoorMaterial;
    public Collider doorCollider;

    private bool _isUsed = false;

    public bool CanBeUsed() => !_isUsed;

    public void MarkAsUsed()
    {
        _isUsed = true;

        if (doorModel != null && usedDoorMaterial != null)
        {
            doorModel.GetComponent<Renderer>().material = usedDoorMaterial;
        }

        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (approachPoint && exitPoint)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, approachPoint.position);
            Gizmos.DrawLine(approachPoint.position, exitPoint.position);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(approachPoint.position, 0.15f);
            Gizmos.DrawSphere(exitPoint.position, 0.15f);

            if (roomSpawnPoint)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireCube(roomSpawnPoint.position, Vector3.one);
            }
        }
    }

    private void Reset()
    {
        if (doorCollider == null)
        {
            doorCollider = GetComponent<Collider>();
        }
    }
}