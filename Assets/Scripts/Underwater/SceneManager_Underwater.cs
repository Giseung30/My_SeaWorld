using UnityEngine;

public class SceneManager_Underwater : MonoBehaviour
{
    [Header("Object")]
    public GameObject leftCloud; //LeftCloud 오브젝트
    public GameObject rightCloud; //RightCloud 오브젝트

    /* Cloud를 활성화/비활성화하는 함수 */
    public void ActivateCloud(bool state)
    {
        leftCloud.SetActive(state);
        rightCloud.SetActive(state);
    }
}