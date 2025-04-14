using UnityEngine;
using System.Collections;

public class HeartDelayActivation : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectsToActivate;
    [SerializeField] private float _delay = 0.2f;
    [SerializeField] private bool _activateOnStart = true;

    private void Start()
    {
        // Сначала отключаем ВСЕ объекты в массиве
        DeactivateAll();

        // Затем запускаем последовательное включение (если нужно)
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
        // Включаем объекты по одному с задержкой
        for (int i = 0; i < _objectsToActivate.Length; i++)
        {
            if (_objectsToActivate[i] != null)
            {
                _objectsToActivate[i].SetActive(true);
                yield return new WaitForSeconds(_delay);
            }
        }
    }

    // Отключает ВСЕ объекты в массиве
    public void DeactivateAll()
    {
        StopAllCoroutines(); // Останавливаем активацию, если она была запущена

        foreach (GameObject obj in _objectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
