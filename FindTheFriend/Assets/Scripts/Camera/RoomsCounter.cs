using TMPro;
using UnityEngine;
using YG;

public class RoomsCounter : MonoBehaviour
{
    public int RoomCount = 0;
    public TextMeshProUGUI RoomCountTxt;

    [Header("Ќастройки рекламы")]
    [SerializeField] private int minRoomsBetweenAds = 10;
    [SerializeField] private int maxRoomsBetweenAds = 20;
    [SerializeField] private GameObject yandexPrefab;
   private int nextAdRoom;

    private void Start()
    {
        // ѕроверка на корректность введенных значений
        if (minRoomsBetweenAds <= 0 || maxRoomsBetweenAds <= 0)
        {
            Debug.LogError("«начени€ minRoomsBetweenAds и maxRoomsBetweenAds должны быть больше 0!");
            minRoomsBetweenAds = 10;
            maxRoomsBetweenAds = 20;
        }

        if (minRoomsBetweenAds > maxRoomsBetweenAds)
        {
            Debug.LogError("minRoomsBetweenAds не может быть больше maxRoomsBetweenAds! «начени€ будут помен€ны местами.");
            int temp = minRoomsBetweenAds;
            minRoomsBetweenAds = maxRoomsBetweenAds;
            maxRoomsBetweenAds = temp;
        }

        SetNextAdRoom();
    }

    private void Update()
    {
        UpdateRoomCountText();
    }

    private void UpdateRoomCountText()
    {

        // ѕровер€ем нужно ли показывать рекламу
        if (RoomCount == nextAdRoom) // »зменил >= на == дл€ точного совпадени€
        {
            PlayAd();
            SetNextAdRoom();
        }

        if (RoomCountTxt != null)
        {
            RoomCountTxt.text = " омната: " + RoomCount.ToString();
        }
    }

    public void IncreaseRoomCount()
    {
        RoomCount++;

        UpdateRoomCountText();
    }

    private void SetNextAdRoom()
    {
        // √енерируем случайное количество комнат до следующей рекламы
        int roomsToAdd = Random.Range(minRoomsBetweenAds, maxRoomsBetweenAds + 1);

        // ”станавливаем следующую комнату дл€ рекламы
        nextAdRoom = RoomCount + roomsToAdd;

        Debug.Log($"—ледующа€ реклама будет на комнате: {nextAdRoom} (через {roomsToAdd} комнат)");
    }

    public void PlayAd()
    {
        // «десь ваша логика показа рекламы
        Debug.Log("јјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјјј " + RoomCount);
        yandexPrefab.SetActive(true);


        // ѕример вызова Yandex рекламы:
        //YandexGame.RewVideoShow(0);
    }
}