using TMPro;
using UnityEngine;

public class RoomsCounter : MonoBehaviour
{
    public int RoomCount = 0;
    public TextMeshProUGUI RoomCountTxt;

    private void Update()
    {
        UpdateRoomCountText();
    }

    // ����� ��� ���������� ������
    private void UpdateRoomCountText()
    {
        if (RoomCountTxt != null)
        {
            RoomCountTxt.text = "Room: " + RoomCount.ToString();
        }
    }

    // ����� ��� ���������� ��������
    public void IncreaseRoomCount()
    {
        RoomCount++;
        UpdateRoomCountText();
    }
}
