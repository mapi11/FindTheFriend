using System.Linq;
using UnityEngine;

public class RandomEnemytActivator : MonoBehaviour
{
    [Header("Настройки")]
    [Tooltip("Минимальное количество включаемых объектов")]
    public int minActiveObjects = 1;
    [Tooltip("Максимальное количество включаемых объектов")]
    public int maxActiveObjects = 5;

    [Header("Массив объектов")]
    [Tooltip("Добавьте сюда объекты, которые нужно включать случайным образом")]
    public GameObject[] objectsToActivate;

    private void Start()
    {
        // Проверка на валидность входных данных
        if (objectsToActivate == null || objectsToActivate.Length == 0)
        {
            Debug.LogError("Массив объектов пуст!", this);
            return;
        }

        if (minActiveObjects < 0 || maxActiveObjects > objectsToActivate.Length || minActiveObjects > maxActiveObjects)
        {
            Debug.LogError("Некорректные значения min/max активных объектов!", this);
            return;
        }

        // Выключаем все объекты в начале
        foreach (var obj in objectsToActivate)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // Определяем сколько объектов включить (случайное число в заданном диапазоне)
        int objectsToEnable = Random.Range(minActiveObjects, maxActiveObjects + 1);

        // Перемешиваем массив
        System.Random rng = new System.Random();
        var shuffledObjects = objectsToActivate.OrderBy(a => rng.Next()).ToArray();

        // Включаем нужное количество объектов
        for (int i = 0; i < objectsToEnable; i++)
        {
            if (shuffledObjects[i] != null)
                shuffledObjects[i].SetActive(true);
        }
    }
}
