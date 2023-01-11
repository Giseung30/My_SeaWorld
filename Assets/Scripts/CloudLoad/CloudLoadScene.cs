using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CloudLoadScene : MonoBehaviour
{
    [Header("LoadScene")]
    private AsyncOperation async; //씬을 불러오는 동안 대기할 수 있도록 하는 변수

    private void Start()
    {
        Screen.SetResolution(Statics.setWidth, (int)(((float)Screen.height / Screen.width) * Statics.setWidth), true); //기기 해상도에 맞게 
        Application.targetFrameRate = Statics.setFrame; //프레임 설정
        Screen.sleepTimeout = SleepTimeout.NeverSleep; //화면 절대 안 꺼지도록 설정

        StartCoroutine(LoadScene());
    }

    /* 씬을 불러오는 함수 */
    private IEnumerator LoadScene()
    {
        async = SceneManager.LoadSceneAsync(Statics.underwaterSceneIndex); //씬 비동기식으로 불러오기
        async.allowSceneActivation = false; //불러오기 완료 후 즉시 이동 금지

        while (async.progress < 0.9f) //로드가 완료될 때까지
        {
            yield return null; //대기
        }

        async.allowSceneActivation = true; //씬 이동 허용
    }
}