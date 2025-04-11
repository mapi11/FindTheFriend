using UnityEngine;
using UnityEngine.UI;

public class AnimatedHeart : MonoBehaviour
{
    [Header("Компоненты")]
    public Animator heartAnimator;
    public Image heartImage;

    [Header("Настройки")]
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(0.3f, 0.3f, 0.3f, 0.7f);

    // Устанавливаем состояние сердечка
    public void SetActive(bool isActive)
    {
        heartAnimator.enabled = isActive;
        heartImage.color = isActive ? activeColor : inactiveColor;

        if (!isActive && heartAnimator.isActiveAndEnabled)
        {
            heartAnimator.Play("Idle"); // Сбрасываем анимацию
        }
    }
}
