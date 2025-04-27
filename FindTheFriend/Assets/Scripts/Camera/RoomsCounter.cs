using TMPro;
using UnityEngine;
using YG;

public class RoomsCounter : MonoBehaviour
{
    public int RoomCount = 0;
    public TextMeshProUGUI RoomCountTxt;

    [Header("��������� �������")]
    [SerializeField] private int minRoomsBetweenAds = 10;
    [SerializeField] private int maxRoomsBetweenAds = 20;
    [SerializeField] private GameObject yandexPrefab;
   private int nextAdRoom;

    private void Start()
    {
        // �������� �� ������������ ��������� ��������
        if (minRoomsBetweenAds <= 0 || maxRoomsBetweenAds <= 0)
        {
            Debug.LogError("�������� minRoomsBetweenAds � maxRoomsBetweenAds ������ ���� ������ 0!");
            minRoomsBetweenAds = 10;
            maxRoomsBetweenAds = 20;
        }

        if (minRoomsBetweenAds > maxRoomsBetweenAds)
        {
            Debug.LogError("minRoomsBetweenAds �� ����� ���� ������ maxRoomsBetweenAds! �������� ����� �������� �������.");
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

        // ��������� ����� �� ���������� �������
        if (RoomCount == nextAdRoom) // ������� >= �� == ��� ������� ����������
        {
            PlayAd();
            SetNextAdRoom();
        }

        if (RoomCountTxt != null)
        {
            RoomCountTxt.text = "�������: " + RoomCount.ToString();
        }
    }

    public void IncreaseRoomCount()
    {
        RoomCount++;

        UpdateRoomCountText();
    }

    private void SetNextAdRoom()
    {
        // ���������� ��������� ���������� ������ �� ��������� �������
        int roomsToAdd = Random.Range(minRoomsBetweenAds, maxRoomsBetweenAds + 1);

        // ������������� ��������� ������� ��� �������
        nextAdRoom = RoomCount + roomsToAdd;

        Debug.Log($"��������� ������� ����� �� �������: {nextAdRoom} (����� {roomsToAdd} ������)");
    }

    public void PlayAd()
    {
        // ����� ���� ������ ������ �������
        Debug.Log("������������������������������������������������������������� " + RoomCount);
        yandexPrefab.SetActive(true);


        // ������ ������ Yandex �������:
        //YandexGame.RewVideoShow(0);
    }
}