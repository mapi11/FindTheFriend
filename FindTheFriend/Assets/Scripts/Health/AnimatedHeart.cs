using UnityEngine;
using UnityEngine.UI;

public class AnimatedHeart : MonoBehaviour
{
    [Header("����������")]
    public Animator heartAnimator;
    public Image heartImage;

    [Header("���������")]
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(0.3f, 0.3f, 0.3f, 0.7f);

    // ������������� ��������� ��������
    public void SetActive(bool isActive)
    {
        heartAnimator.enabled = isActive;
        heartImage.color = isActive ? activeColor : inactiveColor;

        if (!isActive && heartAnimator.isActiveAndEnabled)
        {
            heartAnimator.Play("Idle"); // ���������� ��������
        }
    }
}
