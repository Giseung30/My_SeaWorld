using UnityEngine;
using System.Collections;

public class JoystickCanvas : MonoBehaviour
{
    [Header("Component")]
    public RectTransform boardTransform; //Board�� Transform ����
    public RectTransform handleTransform; //Handel�� Transform ����
    public RectTransform mouseAnchorPositionTransform; //MouseAnchorPosition�� Transform ����

    [Header("Variable")]
    public int joystickPointerID = -1; //Joystick�� �����ϴ� �������� ID
    public float controlDistance; //���� �Ÿ�
    public float horizontalAxis; //���� ��
    public float verticalAxis; //���� ��

    private void Awake()
    {
        Scripts.joystickCanvas = this;
    }

    private void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor) //PC�̸�
        {
            StartCoroutine(ControlJoystickForPC());
        }
        else //Android�̸�
        {
            StartCoroutine(ControlJoystickForAndroid());
        }
    }

    /* Joystick�� Ŭ�� ���¸� �����ϴ� �Լ� */
    public void SetJoystickClicked(bool state)
    {
        if(Application.platform == RuntimePlatform.WindowsEditor) //PC�̸�
        {
            joystickPointerID = state ? 0 : -1; //������ ID �ʱ�ȭ
        }
        else //Android�̸�
        {
            if (state) //Joystick�� Ŭ���Ǹ�
            {
                joystickPointerID = SearchJoystickPointerID(); //���� ������ �ִ� ������ ID�� ����
            }
            else //Joystick�� Ŭ������ ������
            {
                joystickPointerID = -1; //������ ID �ʱ�ȭ
            }
        }
    }

    /* Joystick�� �����ϴ� �ڷ�ƾ �Լ� - PC */
    private IEnumerator ControlJoystickForPC()
    {
        while (true)
        {
            mouseAnchorPositionTransform.position = Input.mousePosition; //���콺 Anchor ��ġ ����

            if (joystickPointerID == 0) //Ŭ���ϴ� ���̸�
            {
                if (Vector3.Distance(boardTransform.anchoredPosition, mouseAnchorPositionTransform.anchoredPosition) > controlDistance) //���콺�� ��ġ�� ���� ������ �����
                {
                    handleTransform.anchoredPosition = boardTransform.anchoredPosition + (mouseAnchorPositionTransform.anchoredPosition - boardTransform.anchoredPosition).normalized * controlDistance; //�̵� ���� ����
                }
                else //���콺�� ��ġ�� ���� ���� ���̸�
                {
                    handleTransform.anchoredPosition = mouseAnchorPositionTransform.anchoredPosition; //���콺�� ��ġ�� Handle �̵�
                }
            }
            else //Ŭ���ϴ� ���� �ƴϸ�
            {
                handleTransform.anchoredPosition = boardTransform.anchoredPosition; //Handle ��ġ �ʱ�ȭ
            }

            verticalAxis = (handleTransform.anchoredPosition.y - boardTransform.anchoredPosition.y) / controlDistance; //���� �� ����
            horizontalAxis = (handleTransform.anchoredPosition.x - boardTransform.anchoredPosition.x) / controlDistance; //���� �� ����

            yield return null;
        }
    }

    /* Joystick�� �����ϴ� �ڷ�ƾ �Լ� - Android */
    private IEnumerator ControlJoystickForAndroid()
    {
        int preTouchCount = 0; //���� ��ġ ����

        while (true)
        {
            if (joystickPointerID != -1 && Input.touchCount != preTouchCount) //Joystick ������ ID�� �����ϸ鼭 ��ġ ������ ����Ǹ�
            {
                joystickPointerID = SearchJoystickPointerID(); //���� ������ �ִ� ������ ID�� ����
            }

            if (joystickPointerID != -1) //Joystick ������ ID�� �����ϸ�
            {
                mouseAnchorPositionTransform.position = Input.touches[joystickPointerID].position; //���콺 Anchor ��ġ ����

                if (Vector3.Distance(boardTransform.anchoredPosition, mouseAnchorPositionTransform.anchoredPosition) > controlDistance) //���콺�� ��ġ�� ���� ������ �����
                {
                    handleTransform.anchoredPosition = boardTransform.anchoredPosition + (mouseAnchorPositionTransform.anchoredPosition - boardTransform.anchoredPosition).normalized * controlDistance; //�̵� ���� ����
                }
                else //���콺�� ��ġ�� ���� ���� ���̸�
                {
                    handleTransform.anchoredPosition = mouseAnchorPositionTransform.anchoredPosition; //���콺�� ��ġ�� Handle �̵�
                }
            }
            else //Joystick ������ ID�� �������� ������
            {
                handleTransform.anchoredPosition = boardTransform.anchoredPosition; //Handle ��ġ �ʱ�ȭ
            }

            verticalAxis = (handleTransform.anchoredPosition.y - boardTransform.anchoredPosition.y) / controlDistance; //���� �� ����
            horizontalAxis = (handleTransform.anchoredPosition.x - boardTransform.anchoredPosition.x) / controlDistance; //���� �� ����

            preTouchCount = Input.touchCount; //��ġ ���� ����

            yield return null;
        }
    }

    /* Joystick�� ���� ������ ID�� Ž���ϴ� �Լ� */
    private int SearchJoystickPointerID()
    {
        int currentPointerID = -1; //���� ���õ� ������ ID
        float currentPointerMinDistance = Mathf.Infinity; //���� �����Ϳ��� �ּ� �Ÿ�

        for (int i = 0; i < Input.touchCount; ++i) //��ġ ������ŭ �ݺ�
        {
            if (Vector2.Distance(Input.touches[i].position, boardTransform.position) < currentPointerMinDistance) //���� �����Ϳ��� �ּҰŸ����� ª����
            {
                currentPointerID = i; //���� ���õ� ������ ID ����
                currentPointerMinDistance = Vector2.Distance(Input.touches[i].position, boardTransform.position); //���� �����Ϳ��� �ּ� �Ÿ� ����
            }
        }

        return currentPointerID; //���� ���õ� ������ ID ��ȯ
    }
}