using UnityEngine;
using UnityEngine.SceneManagement;

public class ShapeSceneManager : MonoBehaviour
{
    /* CloudLoad 씬을 불러오는 함수 */
    public void LoadCloudLoadScene()
    {
        SceneManager.LoadScene(Statics.cloudLoadSceneIndex);
    }
}