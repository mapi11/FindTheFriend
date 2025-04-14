using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText; // ������ �� UI Text ��� ������ FPS
    public int targetFPS = 120; // �������� ����� FPS (����� ������ � ����������)

    private float deltaTime = 0.0f;

    void Start()
    {
        // ������������� ����� FPS
        Application.targetFrameRate = targetFPS;

        // ���� ��������� ���� �� ������ � ����������, ���� ��� �������������
        if (fpsText == null)
        {
            fpsText = GetComponent<TextMeshProUGUI>();
            if (fpsText == null)
            {
                Debug.LogWarning("FPSCounter: �� ������ UI Text ��� ����������� FPS!");
            }
        }
    }

    void Update()
    {
        // ��������� FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        // ��������� ����� (���� ���� ���� ��������)
        if (fpsText != null)
        {
            fpsText.text = $"FPS: {Mathf.Round(fps)}";
        }
    }
}
