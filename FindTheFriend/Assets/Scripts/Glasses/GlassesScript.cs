using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlassesScript : MonoBehaviour
{
    [Header("Volume Settings")]
    public Volume volumeComponent; // ���������� ���� ��� Volume

    [Header("Profiles")]
    public VolumeProfile normalProfile; // ������� �������
    public VolumeProfile glassesProfile; // ������� � ����������� Depth of Field

    private void Start()
    {
        // ��������� ���������
        if (volumeComponent == null)
        {
            Debug.LogError("Volume component not assigned!", this);
            return;
        }

        // ������������� ��������� �������
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

    // ����� ��� "��������� �����" (����������� �� ������� ��� Depth of Field)
    public void GetGlasses()
    {
        if (volumeComponent != null && glassesProfile != null)
        {
            volumeComponent.profile = glassesProfile;
            Debug.Log("Switched to glasses profile");
        }
    }

    // ����� ��� "������ �����" (���������� ������� �������)
    public void SetGlasses()
    {
        if (volumeComponent != null && normalProfile != null)
        {
            volumeComponent.profile = normalProfile;
            Debug.Log("Restored normal profile");
        }
    }

    // ��� ����� � ���������
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
