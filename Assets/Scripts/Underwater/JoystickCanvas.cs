using UnityEngine;
using System.Collections;

public class JoystickCanvas : MonoBehaviour
{
    [Header("Component")]
    public RectTransform boardTransform; //Board의 Transform 정보
    public RectTransform handleTransform; //Handel의 Transform 정보
    public RectTransform mouseAnchorPositionTransform; //MouseAnchorPosition의 Transform 정보

    [Header("Variable")]
    public int joystickPointerID = -1; //Joystick을 조작하는 포인터의 ID
    public float controlDistance; //조작 거리
    public float horizontalAxis; //수평 축
    public float verticalAxis; //수직 축

    private void Awake()
    {
        Scripts.joystickCanvas = this;
    }

    private void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor) //PC이면
        {
            StartCoroutine(ControlJoystickForPC());
        }
        else //Android이면
        {
            StartCoroutine(ControlJoystickForAndroid());
        }
    }

    /* Joystick의 클릭 상태를 갱신하는 함수 */
    public void SetJoystickClicked(bool state)
    {
        if(Application.platform == RuntimePlatform.WindowsEditor) //PC이면
        {
            joystickPointerID = state ? 0 : -1; //포인터 ID 초기화
        }
        else //Android이면
        {
            if (state) //Joystick이 클릭되면
            {
                joystickPointerID = SearchJoystickPointerID(); //가장 가까이 있는 포인터 ID를 지정
            }
            else //Joystick이 클릭되지 않으면
            {
                joystickPointerID = -1; //포인터 ID 초기화
            }
        }
    }

    /* Joystick을 관리하는 코루틴 함수 - PC */
    private IEnumerator ControlJoystickForPC()
    {
        while (true)
        {
            mouseAnchorPositionTransform.position = Input.mousePosition; //마우스 Anchor 위치 갱신

            if (joystickPointerID == 0) //클릭하는 중이면
            {
                if (Vector3.Distance(boardTransform.anchoredPosition, mouseAnchorPositionTransform.anchoredPosition) > controlDistance) //마우스의 위치가 조작 범위를 벗어나면
                {
                    handleTransform.anchoredPosition = boardTransform.anchoredPosition + (mouseAnchorPositionTransform.anchoredPosition - boardTransform.anchoredPosition).normalized * controlDistance; //이동 범위 제한
                }
                else //마우스의 위치가 조작 범위 내이면
                {
                    handleTransform.anchoredPosition = mouseAnchorPositionTransform.anchoredPosition; //마우스의 위치로 Handle 이동
                }
            }
            else //클릭하는 중이 아니면
            {
                handleTransform.anchoredPosition = boardTransform.anchoredPosition; //Handle 위치 초기화
            }

            verticalAxis = (handleTransform.anchoredPosition.y - boardTransform.anchoredPosition.y) / controlDistance; //수직 축 갱신
            horizontalAxis = (handleTransform.anchoredPosition.x - boardTransform.anchoredPosition.x) / controlDistance; //수평 축 갱신

            yield return null;
        }
    }

    /* Joystick을 관리하는 코루틴 함수 - Android */
    private IEnumerator ControlJoystickForAndroid()
    {
        int preTouchCount = 0; //이전 터치 개수

        while (true)
        {
            if (joystickPointerID != -1 && Input.touchCount != preTouchCount) //Joystick 포인터 ID가 존재하면서 터치 개수가 변경되면
            {
                joystickPointerID = SearchJoystickPointerID(); //가장 가까이 있는 포인터 ID를 지정
            }

            if (joystickPointerID != -1) //Joystick 포인터 ID가 존재하면
            {
                mouseAnchorPositionTransform.position = Input.touches[joystickPointerID].position; //마우스 Anchor 위치 갱신

                if (Vector3.Distance(boardTransform.anchoredPosition, mouseAnchorPositionTransform.anchoredPosition) > controlDistance) //마우스의 위치가 조작 범위를 벗어나면
                {
                    handleTransform.anchoredPosition = boardTransform.anchoredPosition + (mouseAnchorPositionTransform.anchoredPosition - boardTransform.anchoredPosition).normalized * controlDistance; //이동 범위 제한
                }
                else //마우스의 위치가 조작 범위 내이면
                {
                    handleTransform.anchoredPosition = mouseAnchorPositionTransform.anchoredPosition; //마우스의 위치로 Handle 이동
                }
            }
            else //Joystick 포인터 ID가 존재하지 않으면
            {
                handleTransform.anchoredPosition = boardTransform.anchoredPosition; //Handle 위치 초기화
            }

            verticalAxis = (handleTransform.anchoredPosition.y - boardTransform.anchoredPosition.y) / controlDistance; //수직 축 갱신
            horizontalAxis = (handleTransform.anchoredPosition.x - boardTransform.anchoredPosition.x) / controlDistance; //수평 축 갱신

            preTouchCount = Input.touchCount; //터치 개수 갱신

            yield return null;
        }
    }

    /* Joystick에 사용될 포인터 ID를 탐색하는 함수 */
    private int SearchJoystickPointerID()
    {
        int currentPointerID = -1; //현재 선택된 포인터 ID
        float currentPointerMinDistance = Mathf.Infinity; //현재 포인터와의 최소 거리

        for (int i = 0; i < Input.touchCount; ++i) //터치 개수만큼 반복
        {
            if (Vector2.Distance(Input.touches[i].position, boardTransform.position) < currentPointerMinDistance) //현재 포인터와의 최소거리보다 짧으면
            {
                currentPointerID = i; //현재 선택된 포인터 ID 갱신
                currentPointerMinDistance = Vector2.Distance(Input.touches[i].position, boardTransform.position); //현재 포인터와의 최소 거리 갱신
            }
        }

        return currentPointerID; //현재 선택된 포인터 ID 반환
    }
}