using UnityEngine;

public class SceneManager_Underwater : MonoBehaviour
{
    [Header("Object")]
    public GameObject leftCloud; //LeftCloud ������Ʈ
    public GameObject rightCloud; //RightCloud ������Ʈ

    /* Cloud�� Ȱ��ȭ/��Ȱ��ȭ�ϴ� �Լ� */
    public void ActivateCloud(bool state)
    {
        leftCloud.SetActive(state);
        rightCloud.SetActive(state);
    }
}