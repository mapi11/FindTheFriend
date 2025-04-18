using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinShopButton : MonoBehaviour
{
    [SerializeField] private int _skinID;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Button _button;

    [Header("Colors")]
    [SerializeField] private Color _purchasedColor = Color.green;
    [SerializeField] private Color _selectedColor = Color.blue;
    [SerializeField] private Color _normalColor = Color.white;

    private MoneyCount _moneyCount;

    private void Start()
    {
        _moneyCount = FindObjectOfType<MoneyCount>();
        UpdateButtonState();

        // Подписываемся на событие смены скина
        SkinManager.Instance.OnSkinChanged += UpdateButtonState;

        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        // Отписываемся при уничтожении объекта
        if (SkinManager.Instance != null)
        {
            SkinManager.Instance.OnSkinChanged -= UpdateButtonState;
        }
    }

    public void OnButtonClick()
    {
        if (SkinManager.Instance.IsSkinPurchased(_skinID))
        {
            // Если этот скин уже выбран - ничего не делаем
            if (SkinManager.Instance.GetCurrentSkinID() == _skinID)
                return;

            SkinManager.Instance.ApplySkin(_skinID);
        }
        else
        {
            if (_moneyCount != null && SkinManager.Instance.TryPurchaseSkin(_skinID, _moneyCount))
            {
                UpdateButtonState();
            }
        }
    }

    private void UpdateButtonState()
    {
        bool isPurchased = SkinManager.Instance.IsSkinPurchased(_skinID);
        bool isSelected = SkinManager.Instance.GetCurrentSkinID() == _skinID;

        if (isPurchased)
        {
            _priceText.text = "Куплено";
            _buttonText.text = isSelected ? "Selected" : "Select";
            _buttonImage.color = isSelected ? _selectedColor : _purchasedColor;
        }
        else
        {
            _priceText.text = SkinManager.Instance.GetSkinPrice(_skinID).ToString();
            _buttonText.text = "Buy";
            _buttonImage.color = _normalColor;
        }
    }
}