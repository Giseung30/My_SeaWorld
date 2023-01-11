using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainCamera_Underwater : MonoBehaviour
{
    [Header("Component")]
    public Camera mainCamera; //���� ī�޶�
    public Transform mainCameraTransform; //���� ī�޶��� Transform ����
    public EventSystem eventSystem; //EventSystem

    [Header("Variable")]
    public float moveSpeed; //�����̴� �ӵ�
    public float rotateSpeed; //ȸ���ϴ� �ӵ�
    public float maxColliderDistance; //�ִ� Collider �Ÿ�

    private void Awake()
    {
        Scripts.mainCamera_Underwater = this;
    }

    private void Start()
    {
        Application.targetFrameRate = Statics.setFrame; //������ ����
        Screen.sleepTimeout = SleepTimeout.NeverSleep; //ȭ�� ���� �� �������� ����

        StartCoroutine(MoveCamera());

        if (Application.platform == RuntimePlatform.WindowsEditor) //PC�̸�
        {
            StartCoroutine(RotateCameraForPC());
        }
        else //Android�̸�
        {
            StartCoroutine(RotateCameraForAndroid());
        }
    }

    /* �ػ� �����ϴ� �Լ� */
    public void SetResolution(int value)
    {
        switch (value) //���ڿ� ���� �ػ� ����
        {
            case 0: //�Ͽ�
                Statics.setWidth = 1280;
                Statics.setHeight = 720;
                break;
            case 1: //�߿�
                Statics.setWidth = 1600;
                Statics.setHeight = 900;
                break;
            case 2: //���
                Statics.setWidth = 1920;
                Statics.setHeight = 1080;
                break;
        }

        Screen.SetResolution(Statics.setWidth, (int)(((float)Screen.height / Screen.width) * Statics.setWidth), true); //��� �ػ󵵿� �°� 

        if ((float)Statics.setWidth / Statics.setHeight < (float)Screen.width / Screen.height) //����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)Statics.setWidth / Statics.setHeight) / ((float)Screen.width / Screen.height); //���ο� �ʺ�
            mainCamera.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); //���ο� Rect ����
        }
        else //������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)Screen.width / Screen.height) / ((float)Statics.setWidth / Statics.setHeight); //���ο� ����
            mainCamera.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); //���ο� Rect ����
        }
    }

    /* MainCamera�� �����̴� �ڷ�ƾ �Լ� */
    private IEnumerator MoveCamera()
    {
        RaycastHit raycastHit; //Raycast ���

        while (true)
        {
            Vector3 moveDirection = mainCameraTransform.forward * Scripts.joystickCanvas.verticalAxis + mainCameraTransform.right * Scripts.joystickCanvas.horizontalAxis; //�����̴� ����

            if (moveDirection.magnitude != 0f) //���⺤���� ũ�Ⱑ �����ϸ�
            {
                if (Physics.Raycast(mainCameraTransform.position, moveDirection, out raycastHit, maxColliderDistance, ~(1 << LayerMask.NameToLayer("PostProcessing")))) //�浹ü�� �����ϸ�
                {
                    if (!Physics.Raycast(raycastHit.point, Vector3.Reflect(moveDirection, raycastHit.normal), maxColliderDistance, ~(1 << LayerMask.NameToLayer("PostProcessing")))) //�ݻ� ��ο� �浹ü�� ������
                    {
                        mainCameraTransform.position += Vector3.Normalize((raycastHit.point + Vector3.Reflect(moveDirection, raycastHit.normal) * maxColliderDistance) - mainCameraTransform.position) * moveSpeed * Time.deltaTime; //�ݻ� ��η� �̵�
                    }
                }
                else //�浹ü�� �������� ������
                {
                    mainCameraTransform.position += moveDirection * Time.deltaTime * moveSpeed; //Joystick�� Axis�� �������� �̵�
                }
            }

            yield return null;
        }
    }

    /* ī�޶� ȸ���ϴ� �Լ� - PC */
    private IEnumerator RotateCameraForPC()
    {
        Vector3 currentMousePointer = Vector3.zero; //���� ���콺 ������ �ʱ�ȭ
        Vector3 tracingPointer = Vector3.zero; //���� ������ �ʱ�ȭ
        float tracingSpeed = 7.5f; //���� �ӵ�
        float degreeX = mainCameraTransform.localEulerAngles.x; //X�� ���� �ʱ�ȭ
        float degreeY = mainCameraTransform.localEulerAngles.y; //Y�� ���� �ʱ�ȭ
        float inverseSetWidth = 1f / Statics.setWidth; //ȭ�� �ʺ� ����
        bool notOnUIPointer = false; //UI ���� ������ ���θ� ��� ����

        while (true)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) //Ŭ���ϰų� Ŭ������ �����
            {
                notOnUIPointer = false; //UI ���� ������ ���� �ʱ�ȭ

                if (Input.GetMouseButtonDown(0) && !eventSystem.IsPointerOverGameObject()) //�����Ͱ� UI �� ������
                {
                    notOnUIPointer = true; //������ ���� ����

                    currentMousePointer = Input.mousePosition; //���� ���콺 ������ ����
                    tracingPointer = Input.mousePosition; //���� ������ ����
                }
            }

            if (notOnUIPointer) //�����Ͱ� UI �� ������
            {
                currentMousePointer = Input.mousePosition; //���� ���콺 ������ ����
            }

            Vector3 preTracingPosition = tracingPointer; //���� ���� ������
            tracingPointer += (currentMousePointer - tracingPointer) * Time.deltaTime * tracingSpeed; //���콺 ������ ����

            degreeX += (tracingPointer.y - preTracingPosition.y) * inverseSetWidth * rotateSpeed; //X�� ���� ����
            degreeX = Mathf.Clamp(degreeX, -89f, 89f); //���� ����
            degreeY += (preTracingPosition.x - tracingPointer.x) * inverseSetWidth * rotateSpeed; //Y�� ���� ����
            mainCameraTransform.localEulerAngles = new Vector3(degreeX, degreeY, 0f); //���� ī�޶� ���� ����

            yield return null;
        }
    }

    /* ī�޶� ȸ���ϴ� �Լ� - Android */
    private IEnumerator RotateCameraForAndroid()
    {
        Vector3 currentMousePointer = Vector3.zero; //���� ���콺 ������ �ʱ�ȭ
        Vector3 tracingPointer = Vector3.zero; //���� ������ �ʱ�ȭ
        float tracingSpeed = 7.5f; //���� �ӵ�
        float degreeX = mainCameraTransform.localEulerAngles.x; //X�� ���� �ʱ�ȭ
        float degreeY = mainCameraTransform.localEulerAngles.y; //Y�� ���� �ʱ�ȭ
        float inverseSetWidth = 1f / Statics.setWidth; //ȭ�� �ʺ� ����
        int notOnUIPointerID = -1; //UI �� ���� �����͸� ��� ����
        int preTouchCount = 0; //���� ��ġ ����

        while (true)
        {
            if (Input.GetMouseButtonDown(0) || preTouchCount != Input.touchCount) //��ġ ������ ����Ǹ�
            {
                notOnUIPointerID = -1; //UI �� ���� ������ �ʱ�ȭ

                for (int i = 0; i < Input.touchCount; ++i) //�������� ������ŭ �ݺ�
                {
                    if (!eventSystem.IsPointerOverGameObject(Input.touches[i].fingerId) && i != Scripts.joystickCanvas.joystickPointerID) //�ش� �����Ͱ� UI �� �����鼭 Joystick �����Ͱ� �ƴϸ�
                    {
                        notOnUIPointerID = i; //������ ID ����

                        currentMousePointer = Input.touches[notOnUIPointerID].position; //���� ���콺 ������ ����
                        tracingPointer = Input.touches[notOnUIPointerID].position; //���� ������ ����

                        break;
                    }
                }
            }

            if (notOnUIPointerID != -1) //UI �� ���� �����Ͱ� ������
            {
                currentMousePointer = Input.touches[notOnUIPointerID].position; //���� ���콺 ������ ����
            }

            Vector3 preTracingPosition = tracingPointer; //���� ���� ������
            tracingPointer += (currentMousePointer - tracingPointer) * Time.deltaTime * tracingSpeed; //���콺 ������ ����

            degreeX += (tracingPointer.y - preTracingPosition.y) * inverseSetWidth * rotateSpeed; //X�� ���� ����
            degreeX = Mathf.Clamp(degreeX, -89f, 89f); //���� ����
            degreeY += (preTracingPosition.x - tracingPointer.x) * inverseSetWidth * rotateSpeed; //Y�� ���� ����
            mainCameraTransform.localEulerAngles = new Vector3(degreeX, degreeY, 0f); //���� ī�޶� ���� ����

            preTouchCount = Input.touchCount; //���� ��ġ ���� ����

            yield return null;
        }
    }
}