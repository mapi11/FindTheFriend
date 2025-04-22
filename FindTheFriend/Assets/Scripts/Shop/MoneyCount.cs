using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyCount : MonoBehaviour
{
    public static MoneyCount Instance { get; private set; }
    public static event System.Action OnMoneyChanged;

    public TextMeshProUGUI MoneyText;

    private const string MONEY_PREFS_KEY = "PlayerMoney";

    [SerializeField] private int _defaultMoney = 0; // ��������� ���������� ����� (���� ��� ����������)
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
            DontDestroyOnLoad(gameObject); // ������ ������ ���������� ����� �������
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
        Debug.Log($"��������� �����: {_currentMoney}");
    }

    private void SaveMoney()
    {
        PlayerPrefs.SetInt(MONEY_PREFS_KEY, _currentMoney);
        PlayerPrefs.Save();
        Debug.Log($"��������� �����: {_currentMoney}");
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
        SaveMoney(); // ��������� ����� ���������
        OnMoneyChanged?.Invoke();
        Debug.Log($"��������� �����: {amount}. �����: {_currentMoney}");
    }

    // ������� ����� ��� ��������� ����� � �����������
    public bool SpendMoney(int amount)
    {
        if (_currentMoney >= amount)
        {
            _currentMoney -= amount;
            SaveMoney();
            OnMoneyChanged?.Invoke();
            Debug.Log($"��������� {amount} �����. ��������: {_currentMoney}");
            return true;
        }
        Debug.LogWarning($"������������ �����: ����� {amount}, ���� {_currentMoney}");
        return false;
    }

    public int GetMoneyCount()
    {
        return _currentMoney;
    }

    // ����� ��� ������ ����� (��������, ��� ������������)
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

        // ��������� ��� ����������� (�� ������ ������)
        if (Instance == this)
        {
            SaveMoney();
        }
    }

}