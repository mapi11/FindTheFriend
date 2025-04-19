using TMPro;
using UnityEngine;
using YG;

public class RoomsCounter : MonoBehaviour
{
    public int RoomCount = 0;
    public TextMeshProUGUI RoomCountTxt;

   [Header("Настройки рекламы")]
    [SerializeField] private int minRoomsBetweenAds = 10;
    [SerializeField] private int maxRoomsBetweenAds = 20;
    private int nextAdRoom;

    private void Start()
    {
        // Устанавливаем первую комнату для показа рекламы
        SetNextAdRoom();
    }

    private void Update()
    {
        UpdateRoomCountText();
    }

    private void UpdateRoomCountText()
    {
        if (RoomCountTxt != null)
        {
            RoomCountTxt.text = "Комната: " + RoomCount.ToString();
        }
    }

    public void IncreaseRoomCount()
    {
        RoomCount++;
        UpdateRoomCountText();
        
        // Проверяем нужно ли показывать рекламу
        if (RoomCount >= nextAdRoom)
        {
            PlayAd(); // Ваш метод для показа рекламы
            SetNextAdRoom(); // Устанавливаем следующую комнату для рекламы
        }
    }

    private void SetNextAdRoom()
    {
        // Случайное значение между min и max
        int roomsToAdd = Random.Range(minRoomsBetweenAds, maxRoomsBetweenAds + 1);
        nextAdRoom = RoomCount + roomsToAdd;
        
        Debug.Log($"Следующая реклама будет на комнате: {nextAdRoom}");
    }

    private void PlayAd()
    {
        // Здесь ваша логика показа рекламы
        Debug.Log($"Показываем рекламу на комнате {RoomCount}");
        
        // Пример вызова Yandex рекламы:
        // YandexGame.RewVideoShow(0);
    }
}
