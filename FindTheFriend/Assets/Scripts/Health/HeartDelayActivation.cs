using UnityEngine;
using System.Collections;

public class HeartDelayActivation : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectsToActivate;
    [SerializeField] private float _delay = 0.2f;
    [SerializeField] private bool _activateOnStart = true;

    private void Start()
    {
        // ������� ��������� ��� ������� � �������
        DeactivateAll();

        // ����� ��������� ���������������� ��������� (���� �����)
        if (_activateOnStart)
        {
            StartActivation();
        }
    }

    public void StartActivation()
    {
        StartCoroutine(ActivateObjectsWithDelay());
    }

    private IEnumerator ActivateObjectsWithDelay()
    {
        // �������� ������� �� ������ � ���������
        for (int i = 0; i < _objectsToActivate.Length; i++)
        {
            if (_objectsToActivate[i] != null)
            {
                _objectsToActivate[i].SetActive(true);
                yield return new WaitForSeconds(_delay);
            }
        }
    }

    // ��������� ��� ������� � �������
    public void DeactivateAll()
    {
        StopAllCoroutines(); // ������������� ���������, ���� ��� ���� ��������

        foreach (GameObject obj in _objectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
