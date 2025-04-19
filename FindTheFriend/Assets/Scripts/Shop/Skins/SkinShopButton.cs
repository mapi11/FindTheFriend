using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class SkinShopButton : MonoBehaviour
{
    [SerializeField] private int _skinID;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _statusText;

    [Header("Colors")]
    [SerializeField] private Color _purchasedColor = new Color(0.2f, 0.8f, 0.2f); // ������
    [SerializeField] private Color _selectedColor = new Color(0.2f, 0.4f, 0.8f);  // �����
    [SerializeField] private Color _normalColor = Color.white;

    private Button _button;
    private Image _buttonImage;

    private void Awake()
    {
        // �������� ������������ ����������
        _button = GetComponent<Button>();
        _buttonImage = GetComponent<Image>();

        // ��������� ���������� �����
        _button.onClick.AddListener(OnButtonClick);

        // ��������� ������
        if (_priceText == null || _statusText == null)
        {
            Debug.LogError($"�� ��������� ��������� �������� ��� ������ ����� {_skinID}", this);
        }
    }

    private void Start()
    {
        UpdateButtonUI();
        SkinManager.Instance.OnSkinChanged += UpdateButtonUI;
    }

    private void OnButtonClick()
    {
        if (IsPurchased())
        {
            if (!IsSelected())
            {
                SkinManager.Instance.ApplySkin(_skinID);
            }
        }
        else
        {
            TryPurchaseSkin();
        }
    }

    private void UpdateButtonUI()
    {
        if (_priceText == null || _statusText == null) return;

        bool isPurchased = IsPurchased();
        bool isSelected = IsSelected();
        bool isDefault = _skinID == SkinManager.Instance.GetDefaultSkinID();

        // ���������� ������ ����
        if (isDefault)
        {
            _priceText.text = "��������";
            _priceText.color = Color.gray;
        }
        else if (isPurchased)
        {
            _priceText.text = "�������";
            _priceText.color = _purchasedColor;
        }
        else
        {
            _priceText.text = SkinManager.Instance.GetSkinPrice(_skinID).ToString();
            _priceText.color = Color.white;
        }

        // ���������� �������
        _statusText.text = isSelected ? "�������" : "�������";

        // ���������� ����� ������
        _buttonImage.color = isSelected ? _selectedColor :
                           isPurchased ? _purchasedColor : _normalColor;
    }

    private bool TryPurchaseSkin()
    {
        MoneyCount moneyCount = MoneyCount.Instance;
        if (moneyCount == null) return false;

        return SkinManager.Instance.TryPurchaseSkin(_skinID, moneyCount);
    }

    private bool IsPurchased()
    {
        return SkinManager.Instance.IsSkinPurchased(_skinID) ||
               _skinID == SkinManager.Instance.GetDefaultSkinID();
    }

    private bool IsSelected()
    {
        return SkinManager.Instance.GetCurrentSkinID() == _skinID;
    }

    private void OnDestroy()
    {
        if (SkinManager.Instance != null)
        {
            SkinManager.Instance.OnSkinChanged -= UpdateButtonUI;
        }
        _button.onClick.RemoveListener(OnButtonClick);
    }
}