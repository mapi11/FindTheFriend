using UnityEngine;
using System.Collections.Generic;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance { get; private set; }

    [System.Serializable]
    public class SkinData
    {
        public int skinID;
        public GameObject skinPrefab;
        public int price;
        public Transform spawnParent;
    }

    [SerializeField] private int _defaultSkinID = 0;
    [SerializeField] private List<SkinData> _skins = new List<SkinData>();

    private int _currentSkinID = -1;
    private HashSet<int> _purchasedSkins = new HashSet<int>();

    private const string PREFS_CURRENT_SKIN = "CurrentSkinID";
    private const string PREFS_PURCHASED_SKINS = "PurchasedSkins";

    public System.Action OnSkinChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadData();

        // �����������, ��� ��������� ���� ������ ������
        if (!_purchasedSkins.Contains(_defaultSkinID))
        {
            _purchasedSkins.Add(_defaultSkinID);
        }

        // ���� ���� ��� �� ������, �������� ���������
        if (_currentSkinID == -1)
        {
            _currentSkinID = _defaultSkinID;
            SaveData();
        }

        SpawnCurrentSkin();
    }

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadData();
        SpawnCurrentSkin();
    }

    private void LoadData()
    {
        _currentSkinID = PlayerPrefs.GetInt(PREFS_CURRENT_SKIN, -1);

        string purchasedSkinsString = PlayerPrefs.GetString(PREFS_PURCHASED_SKINS, "");
        if (!string.IsNullOrEmpty(purchasedSkinsString))
        {
            string[] ids = purchasedSkinsString.Split(',');
            foreach (string id in ids)
            {
                if (int.TryParse(id, out int skinID))
                {
                    _purchasedSkins.Add(skinID);
                }
            }
        }
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(PREFS_CURRENT_SKIN, _currentSkinID);

        List<string> ids = new List<string>();
        foreach (int id in _purchasedSkins)
        {
            ids.Add(id.ToString());
        }
        PlayerPrefs.SetString(PREFS_PURCHASED_SKINS, string.Join(",", ids));
        PlayerPrefs.Save();
    }

    private void SpawnCurrentSkin()
    {
        SkinData skinData = GetSkinData(_currentSkinID);
        if (skinData == null || skinData.skinPrefab == null) return;

        // ������� ���������� ����
        foreach (Transform child in skinData.spawnParent)
        {
            Destroy(child.gameObject);
        }

        Instantiate(skinData.skinPrefab, skinData.spawnParent);
    }


    public bool TryPurchaseSkin(int skinID, MoneyCount moneyCount)
    {
        // ��������� ���������� �� ����
        SkinData skinData = GetSkinData(skinID);
        if (skinData == null)
        {
            Debug.LogError($"���� � ID {skinID} �� ������!");
            return false;
        }

        // ��������� �� ������ �� ��� ����
        if (_purchasedSkins.Contains(skinID))
        {
            Debug.Log($"���� {skinID} ��� ������, ��������� ���");
            ApplySkin(skinID);
            return true;
        }

        // ��������� ���������� �� �����
        if (moneyCount.GetMoneyCount() >= skinData.price)
        {
            if (moneyCount.SpendMoney(skinData.price))
            {
                _purchasedSkins.Add(skinID);
                ApplySkin(skinID);
                SaveData();
                Debug.Log($"���� {skinID} ������� ������!");
                return true;
            }
        }

        Debug.LogWarning($"�� ������� ������ ���� {skinID}. ������: {moneyCount.GetMoneyCount()}, ����: {skinData.price}");
        return false;
    }

    public void ApplySkin(int skinID)
    {
        if (!_purchasedSkins.Contains(skinID))
        {
            Debug.LogError($"������� ��������� ����������� ����: {skinID}");
            return;
        }

        _currentSkinID = skinID;
        SpawnCurrentSkin();
        SaveData();
        OnSkinChanged?.Invoke();
    }

    private SkinData GetSkinData(int skinID)
    {
        return _skins.Find(s => s.skinID == skinID);
    }

    public bool IsSkinPurchased(int skinID)
    {
        return _purchasedSkins.Contains(skinID);
    }

    public int GetSkinPrice(int skinID)
    {
        SkinData skinData = _skins.Find(s => s.skinID == skinID);
        return skinData?.price ?? 0;
    }

    public int GetCurrentSkinID()
    {
        return _currentSkinID;
    }

    public void ResetAllPurchases()
    {
        _currentSkinID = -1;
        _purchasedSkins.Clear();

        PlayerPrefs.DeleteKey(PREFS_CURRENT_SKIN);
        PlayerPrefs.DeleteKey(PREFS_PURCHASED_SKINS);
        PlayerPrefs.Save();

        Debug.Log("��� ������� ������ ��������!");
    }

    public int GetDefaultSkinID() => _defaultSkinID;
}