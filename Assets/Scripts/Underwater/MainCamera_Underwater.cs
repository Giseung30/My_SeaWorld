using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCamera_Underwater : MonoBehaviour
{
    [Header("Component")]
    public Camera mainCamera; //메인 카메라
    public Transform mainCameraTransform; //메인 카메라의 Transform 정보
    public EventSystem eventSystem; //EventSystem

    [Header("Variable")]
    public float moveSpeed; //움직이는 속도
    public float rotateSpeed; //회전하는 속도
    public float maxColliderDistance; //최대 Collider 거리

    private void Awake()
    {
        Scripts.mainCamera_Underwater = this;
    }

    private void Start()
    {
        Application.targetFrameRate = Statics.setFrame; //프레임 설정
        Screen.sleepTimeout = SleepTimeout.NeverSleep; //화면 절대 안 꺼지도록 설정

        StartCoroutine(MoveCamera());

        if (Application.platform == RuntimePlatform.WindowsEditor) //PC이면
        {
            StartCoroutine(RotateCameraForPC());
        }
        else //Android이면
        {
            StartCoroutine(RotateCameraForAndroid());
        }
    }

    /* 해상도 설정하는 함수 */
    public void SetResolution(int value)
    {
        switch (value) //인자에 따른 해상도 설정
        {
            case 0: //하옵
                Statics.setWidth = 1280;
                Statics.setHeight = 720;
                break;
            case 1: //중옵
                Statics.setWidth = 1600;
                Statics.setHeight = 900;
                break;
            case 2: //상옵
                Statics.setWidth = 1920;
                Statics.setHeight = 1080;
                break;
        }

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

    /* MainCamera를 움직이는 코루틴 함수 */
    private IEnumerator MoveCamera()
    {
        RaycastHit raycastHit; //Raycast 결과

        while (true)
        {
            Vector3 moveDirection = mainCameraTransform.forward * Scripts.joystickCanvas.verticalAxis + mainCameraTransform.right * Scripts.joystickCanvas.horizontalAxis; //움직이는 방향

            if (moveDirection.magnitude != 0f) //방향벡터의 크기가 존재하면
            {
                if (Physics.Raycast(mainCameraTransform.position, moveDirection, out raycastHit, maxColliderDistance, ~(1 << LayerMask.NameToLayer("PostProcessing")))) //충돌체에 접근하면
                {
                    if (!Physics.Raycast(raycastHit.point, Vector3.Reflect(moveDirection, raycastHit.normal), maxColliderDistance, ~(1 << LayerMask.NameToLayer("PostProcessing")))) //반사 경로에 충돌체가 없으면
                    {
                        mainCameraTransform.position += Vector3.Normalize((raycastHit.point + Vector3.Reflect(moveDirection, raycastHit.normal) * maxColliderDistance) - mainCameraTransform.position) * moveSpeed * Time.deltaTime; //반사 경로로 이동
                    }
                }
                else //충돌체에 접근하지 않으면
                {
                    mainCameraTransform.position += moveDirection * Time.deltaTime * moveSpeed; //Joystick의 Axis를 기준으로 이동
                }
            }

            yield return null;
        }
    }

    /* 카메라를 회전하는 함수 - PC */
    private IEnumerator RotateCameraForPC()
    {
        Vector3 currentMousePointer = Vector3.zero; //현재 마우스 포인터 초기화
        Vector3 tracingPointer = Vector3.zero; //추적 포인터 초기화
        float tracingSpeed = 7.5f; //추적 속도
        float degreeX = mainCameraTransform.localEulerAngles.x; //X축 각도 초기화
        float degreeY = mainCameraTransform.localEulerAngles.y; //Y축 각도 초기화
        float inverseSetWidth = 1f / Statics.setWidth; //화면 너비 역수
        bool notOnUIPointer = false; //UI 상의 포인터 여부를 담는 변수

        while (true)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) //클릭하거나 클릭에서 벗어나면
            {
                notOnUIPointer = false; //UI 상의 포인터 여부 초기화

                if (Input.GetMouseButtonDown(0) && !eventSystem.IsPointerOverGameObject()) //포인터가 UI 상에 없으면
                {
                    notOnUIPointer = true; //포인터 상태 저장

                    currentMousePointer = Input.mousePosition; //현재 마우스 포인터 갱신
                    tracingPointer = Input.mousePosition; //추적 포인터 갱신
                }
            }

            if (notOnUIPointer) //포인터가 UI 상에 없으면
            {
                currentMousePointer = Input.mousePosition; //현재 마우스 포인터 갱신
            }

            Vector3 preTracingPosition = tracingPointer; //이전 추적 포인터
            tracingPointer += (currentMousePointer - tracingPointer) * Time.deltaTime * tracingSpeed; //마우스 포인터 추적

            degreeX += (tracingPointer.y - preTracingPosition.y) * inverseSetWidth * rotateSpeed; //X축 각도 조정
            degreeX = Mathf.Clamp(degreeX, -89f, 89f); //각도 제한
            degreeY += (preTracingPosition.x - tracingPointer.x) * inverseSetWidth * rotateSpeed; //Y축 각도 조정
            mainCameraTransform.localEulerAngles = new Vector3(degreeX, degreeY, 0f); //메인 카메라 각도 지정

            yield return null;
        }
    }

    /* 카메라를 회전하는 함수 - Android */
    private IEnumerator RotateCameraForAndroid()
    {
        Vector3 currentMousePointer = Vector3.zero; //현재 마우스 포인터 초기화
        Vector3 tracingPointer = Vector3.zero; //추적 포인터 초기화
        float tracingSpeed = 7.5f; //추적 속도
        float degreeX = mainCameraTransform.localEulerAngles.x; //X축 각도 초기화
        float degreeY = mainCameraTransform.localEulerAngles.y; //Y축 각도 초기화
        float inverseSetWidth = 1f / Statics.setWidth; //화면 너비 역수
        int notOnUIPointerID = -1; //UI 상에 없는 포인터를 담는 변수
        int preTouchCount = 0; //이전 터치 개수

        while (true)
        {
            if (Input.GetMouseButtonDown(0) || preTouchCount != Input.touchCount) //터치 개수가 변경되면
            {
                notOnUIPointerID = -1; //UI 상에 없는 포인터 초기화

                for (int i = 0; i < Input.touchCount; ++i) //포인터의 개수만큼 반복
                {
                    if (!eventSystem.IsPointerOverGameObject(Input.touches[i].fingerId) && i != Scripts.joystickCanvas.joystickPointerID) //해당 포인터가 UI 상에 없으면서 Joystick 포인터가 아니면
                    {
                        notOnUIPointerID = i; //포인터 ID 저장

                        currentMousePointer = Input.touches[notOnUIPointerID].position; //현재 마우스 포인터 갱신
                        tracingPointer = Input.touches[notOnUIPointerID].position; //추적 포인터 갱신

                        break;
                    }
                }
            }

            if (notOnUIPointerID != -1) //UI 상에 없는 포인터가 있으면
            {
                currentMousePointer = Input.touches[notOnUIPointerID].position; //현재 마우스 포인터 갱신
            }

            Vector3 preTracingPosition = tracingPointer; //이전 추적 포인터
            tracingPointer += (currentMousePointer - tracingPointer) * Time.deltaTime * tracingSpeed; //마우스 포인터 추적

            degreeX += (tracingPointer.y - preTracingPosition.y) * inverseSetWidth * rotateSpeed; //X축 각도 조정
            degreeX = Mathf.Clamp(degreeX, -89f, 89f); //각도 제한
            degreeY += (preTracingPosition.x - tracingPointer.x) * inverseSetWidth * rotateSpeed; //Y축 각도 조정
            mainCameraTransform.localEulerAngles = new Vector3(degreeX, degreeY, 0f); //메인 카메라 각도 지정

            preTouchCount = Input.touchCount; //이전 터치 개수 갱신

            yield return null;
        }
    }
}