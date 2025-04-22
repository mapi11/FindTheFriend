using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyCount : MonoBehaviour
{
    public static MoneyCount Instance { get; private set; }
    public static event System.Action OnMoneyChanged;

    public TextMeshProUGUI MoneyText;

    private const string MONEY_PREFS_KEY = "PlayerMoney";

    [SerializeField] private int _defaultMoney = 0; // Начальное количество денег (если нет сохранений)
    public int _currentMoney;
    private List<MoneyPoint> _allMoneyPoints = new List<MoneyPoint>();

    private void Update()
    {
        MoneyText.text = _currentMoney.ToString();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Делаем объект постоянным между сценами
            LoadMoney();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        FindAllMoneyPoints();
    }

    private void LoadMoney()
    {
        _currentMoney = PlayerPrefs.GetInt(MONEY_PREFS_KEY, _defaultMoney);
        Debug.Log($"Загружено денег: {_currentMoney}");
    }

    private void SaveMoney()
    {
        PlayerPrefs.SetInt(MONEY_PREFS_KEY, _currentMoney);
        PlayerPrefs.Save();
        Debug.Log($"Сохранено денег: {_currentMoney}");
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
        SaveMoney(); // Сохраняем после изменения
        OnMoneyChanged?.Invoke();
        Debug.Log($"Добавлено денег: {amount}. Всего: {_currentMoney}");
    }

    // Добавим метод для вычитания денег с сохранением
    public bool SpendMoney(int amount)
    {
        if (_currentMoney >= amount)
        {
            _currentMoney -= amount;
            SaveMoney();
            OnMoneyChanged?.Invoke();
            Debug.Log($"Потрачено {amount} денег. Осталось: {_currentMoney}");
            return true;
        }
        Debug.LogWarning($"Недостаточно денег: нужно {amount}, есть {_currentMoney}");
        return false;
    }

    public int GetMoneyCount()
    {
        return _currentMoney;
    }

    // Метод для сброса денег (например, для тестирования)
    public void ResetMoney()
    {
        _currentMoney = _defaultMoney;
        SaveMoney();
        OnMoneyChanged?.Invoke();
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

        // Сохраняем при уничтожении (на всякий случай)
        if (Instance == this)
        {
            SaveMoney();
        }
    }

}