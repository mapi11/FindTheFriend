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
    private int nextAdRoom;

    private void Start()
    {
        // ������������� ������ ������� ��� ������ �������
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
            RoomCountTxt.text = "�������: " + RoomCount.ToString();
        }
    }

    public void IncreaseRoomCount()
    {
        RoomCount++;
        UpdateRoomCountText();
        
        // ��������� ����� �� ���������� �������
        if (RoomCount >= nextAdRoom)
        {
            PlayAd(); // ��� ����� ��� ������ �������
            SetNextAdRoom(); // ������������� ��������� ������� ��� �������
        }
    }

    private void SetNextAdRoom()
    {
        // ��������� �������� ����� min � max
        int roomsToAdd = Random.Range(minRoomsBetweenAds, maxRoomsBetweenAds + 1);
        nextAdRoom = RoomCount + roomsToAdd;
        
        Debug.Log($"��������� ������� ����� �� �������: {nextAdRoom}");
    }

    private void PlayAd()
    {
        // ����� ���� ������ ������ �������
        Debug.Log($"���������� ������� �� ������� {RoomCount}");
        
        // ������ ������ Yandex �������:
        // YandexGame.RewVideoShow(0);
    }
}
