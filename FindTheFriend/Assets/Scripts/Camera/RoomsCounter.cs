using TMPro;
using UnityEngine;
using YG;

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
            RoomCountTxt.text = "�������: " + RoomCount.ToString();
        }
    }

    // ����� ��� ���������� ��������
    public void IncreaseRoomCount()
    {
        RoomCount++;
        UpdateRoomCountText();
    }
}
