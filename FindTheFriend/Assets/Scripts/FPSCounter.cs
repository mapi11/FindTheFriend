using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText; // Ссылка на UI Text для вывода FPS
    public int targetFPS = 120; // Желаемый лимит FPS (можно менять в инспекторе)

    private float deltaTime = 0.0f;

    void Start()
    {
        // Устанавливаем лимит FPS
        Application.targetFrameRate = targetFPS;

        // Если текстовое поле не задано в инспекторе, ищем его автоматически
        if (fpsText == null)
        {
            fpsText = GetComponent<TextMeshProUGUI>();
            if (fpsText == null)
            {
                Debug.LogWarning("FPSCounter: Не найден UI Text для отображения FPS!");
            }
        }
    }

    void Update()
    {
        // Вычисляем FPS
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        // Обновляем текст (если есть куда выводить)
        if (fpsText != null)
        {
            fpsText.text = $"FPS: {Mathf.Round(fps)}";
        }
    }
}
