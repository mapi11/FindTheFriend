using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlassesScript : MonoBehaviour
{
    [Header("Volume Settings")]
    public Volume volumeComponent; // Перетащите сюда ваш Volume

    [Header("Profiles")]
    public VolumeProfile normalProfile; // Обычный профиль
    public VolumeProfile glassesProfile; // Профиль с отключенным Depth of Field

    private void Start()
    {
        // Проверяем настройки
        if (volumeComponent == null)
        {
            Debug.LogError("Volume component not assigned!", this);
            return;
        }

        // Устанавливаем начальный профиль
        if (normalProfile != null)
        {
            volumeComponent.profile = normalProfile;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GetGlasses();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SetGlasses();
        }
    }

    // Метод для "надевания очков" (переключаем на профиль без Depth of Field)
    public void GetGlasses()
    {
        if (volumeComponent != null && glassesProfile != null)
        {
            volumeComponent.profile = glassesProfile;
            Debug.Log("Switched to glasses profile");
        }
    }

    // Метод для "снятия очков" (возвращаем обычный профиль)
    public void SetGlasses()
    {
        if (volumeComponent != null && normalProfile != null)
        {
            volumeComponent.profile = normalProfile;
            Debug.Log("Restored normal profile");
        }
    }

    // Для теста в редакторе
    [ContextMenu("Test GetGlasses")]
    private void TestGetGlasses()
    {
        GetGlasses();
    }

    [ContextMenu("Test SetGlasses")]
    private void TestSetGlasses()
    {
        SetGlasses();
    }
}
