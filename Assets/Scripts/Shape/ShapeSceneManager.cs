using UnityEngine;
using UnityEngine.SceneManagement;

public class ShapeSceneManager : MonoBehaviour
{
    /* CloudLoad ���� �ҷ����� �Լ� */
    public void LoadCloudLoadScene()
    {
        SceneManager.LoadScene(Statics.cloudLoadSceneIndex);
    }
}