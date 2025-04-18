using UnityEngine;
using System.Collections.Generic;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance { get; private set; }

    public System.Action OnSkinChanged;

    [System.Serializable]
    public class SkinData
    {
        public int skinID;
        public GameObject skinPrefab;
        public int price;
        public Transform spawnParent;
    }

    [SerializeField] private List<SkinData> _skins = new List<SkinData>();
    private int _currentSkinID = -1;
    private HashSet<int> _purchasedSkins = new HashSet<int>();

    private const string PREFS_CURRENT_SKIN = "CurrentSkinID";
    private const string PREFS_PURCHASED_SKINS = "PurchasedSkins";

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

    public void SpawnCurrentSkin()
    {
        if (_currentSkinID == -1) return;

        SkinData skinData = _skins.Find(s => s.skinID == _currentSkinID);
        if (skinData != null && skinData.skinPrefab != null && skinData.spawnParent != null)
        {
            // Удаляем все предыдущие скины
            foreach (Transform child in skinData.spawnParent)
            {
                Destroy(child.gameObject);
            }

            Instantiate(skinData.skinPrefab, skinData.spawnParent);
        }
    }

    public bool TryPurchaseSkin(int skinID, MoneyCount moneyCount)
    {
        SkinData skinData = _skins.Find(s => s.skinID == skinID);
        if (skinData == null) return false;

        // Если скин уже куплен
        if (_purchasedSkins.Contains(skinID))
        {
            ApplySkin(skinID);
            return true;
        }

        // Проверяем хватает ли денег
        if (moneyCount.GetMoneyCount() >= skinData.price)
        {
            moneyCount.AddMoney(-skinData.price);
            _purchasedSkins.Add(skinID);
            ApplySkin(skinID);
            SaveData();
            return true;
        }

        return false;
    }

    public void ApplySkin(int skinID)
    {
        _currentSkinID = skinID;
        SaveData();
        SpawnCurrentSkin();
        OnSkinChanged?.Invoke(); // Уведомляем об изменении скина
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

        Debug.Log("Все покупки скинов сброшены!");
    }

}