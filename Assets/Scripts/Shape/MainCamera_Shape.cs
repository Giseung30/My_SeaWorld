using UnityEngine;

public class MainCamera_Shape : MonoBehaviour
{
    [Header("Component")]
    public Camera mainCamera; //���� ī�޶�

    private void Start()
    {
        Application.targetFrameRate = Statics.setFrame; //������ ����
        Screen.sleepTimeout = SleepTimeout.NeverSleep; //ȭ�� ���� �� �������� ����
        SetResolution();
    }

    /* �ػ� �����ϴ� �Լ� */
    private void SetResolution()
    {
        Screen.SetResolution(Statics.setWidth, (int)(((float)Screen.height / Screen.width) * Statics.setWidth), true); //��� �ػ󵵿� �°� 

        if ((float)Statics.setWidth / Statics.setHeight < (float)Screen.width / Screen.height) //����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)Statics.setWidth / Statics.setHeight) / ((float)Screen.width / Screen.height); //���ο� �ʺ�
            mainCamera.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); //���ο� Rect ����
        }
        else //������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)Screen.width / Screen.height) / ((float)Statics.setWidth / Statics.setHeight); //���ο� ����
            mainCamera.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); //���ο� Rect ����
        }
    }
}