using UnityEngine;

public class MainCamera_Shape : MonoBehaviour
{
    [Header("Component")]
    public Camera mainCamera; //메인 카메라

    private void Start()
    {
        Application.targetFrameRate = Statics.setFrame; //프레임 설정
        Screen.sleepTimeout = SleepTimeout.NeverSleep; //화면 절대 안 꺼지도록 설정
        SetResolution();
    }

    /* 해상도 설정하는 함수 */
    private void SetResolution()
    {
        Screen.SetResolution(Statics.setWidth, (int)(((float)Screen.height / Screen.width) * Statics.setWidth), true); //기기 해상도에 맞게 

        if ((float)Statics.setWidth / Statics.setHeight < (float)Screen.width / Screen.height) //기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)Statics.setWidth / Statics.setHeight) / ((float)Screen.width / Screen.height); //새로운 너비
            mainCamera.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); //새로운 Rect 적용
        }
        else //게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)Screen.width / Screen.height) / ((float)Statics.setWidth / Statics.setHeight); //새로운 높이
            mainCamera.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); //새로운 Rect 적용
        }
    }
}