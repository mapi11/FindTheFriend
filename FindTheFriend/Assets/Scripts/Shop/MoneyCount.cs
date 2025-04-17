using System.Collections.Generic;
using UnityEngine;

public class MoneyCount : MonoBehaviour
{
    [SerializeField] private int _currentMoney = 0;
    private List<MoneyPoint> _allMoneyPoints = new List<MoneyPoint>();

    private void Awake()
    {
        FindAllMoneyPoints();
    }

    private void FindAllMoneyPoints()
    {
        _allMoneyPoints.Clear();
        MoneyPoint[] foundPoints = FindObjectsOfType<MoneyPoint>();

        foreach (MoneyPoint point in foundPoints)
        {
            if (point != null)
            {
                point.OnCollected += OnMoneyCollected;
                _allMoneyPoints.Add(point);
            }
        }
    }

    private void OnMoneyCollected(MoneyPoint point)
    {
        _allMoneyPoints.Remove(point);
        point.OnCollected -= OnMoneyCollected;
    }

    public void AddMoney(int amount)
    {
        _currentMoney += amount;
        Debug.Log($"Добавлено денег: {amount}. Всего: {_currentMoney}");
    }

    public int GetMoneyCount()
    {
        return _currentMoney;
    }

    private void OnDestroy()
    {
        foreach (MoneyPoint point in _allMoneyPoints)
        {
            if (point != null)
            {
                point.OnCollected -= OnMoneyCollected;
            }
        }
    }
}
