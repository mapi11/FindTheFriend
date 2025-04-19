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

    // Метод для обновления текста
    private void UpdateRoomCountText()
    {
        if (RoomCountTxt != null)
        {
            RoomCountTxt.text = "Комната: " + RoomCount.ToString();
        }
    }

    // Метод для увеличения счетчика
    public void IncreaseRoomCount()
    {
        RoomCount++;
        UpdateRoomCountText();
    }
}
