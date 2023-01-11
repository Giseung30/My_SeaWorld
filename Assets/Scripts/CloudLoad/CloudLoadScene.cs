using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CloudLoadScene : MonoBehaviour
{
    [Header("LoadScene")]
    private AsyncOperation async; //���� �ҷ����� ���� ����� �� �ֵ��� �ϴ� ����

    private void Start()
    {
        Screen.SetResolution(Statics.setWidth, (int)(((float)Screen.height / Screen.width) * Statics.setWidth), true); //��� �ػ󵵿� �°� 
        Application.targetFrameRate = Statics.setFrame; //������ ����
        Screen.sleepTimeout = SleepTimeout.NeverSleep; //ȭ�� ���� �� �������� ����

        StartCoroutine(LoadScene());
    }

    /* ���� �ҷ����� �Լ� */
    private IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(Statics.underwaterSceneIndex); //�� �񵿱������ �ҷ�����
        async.allowSceneActivation = false; //�ҷ����� �Ϸ� �� ��� �̵� ����

        while (async.progress < 0.9f) //�ε尡 �Ϸ�� ������
        {
            yield return null; //���
        }

        async.allowSceneActivation = true; //�� �̵� ���
    }
}