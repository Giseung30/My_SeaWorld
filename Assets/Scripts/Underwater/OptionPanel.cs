using UnityEngine;
using TMPro;

public class OptionPanel : MonoBehaviour
{
    [Header("Component")]
    public GameObject optionPanel; //OptionPanel

    [Header("Resolution")]
    public TextMeshProUGUI resolutionTextTMP; //ResolutionText�� TMP ����
    private int currentResolutionValue; //���� �ػ� ��

    private void Start()
    {
        currentResolutionValue = 1;

        SetResolutionText(currentResolutionValue);
    }

    /* OptionPanel�� Ȱ��ȭ�ϴ� �Լ� */
    public void ActiveOptionPanel(bool state)
    {
        optionPanel.SetActive(state); //OptionPanel Ȱ��ȭ/��Ȱ��ȭ
    }

    /* ResolutionButton�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickResolutionButton()
    {
        currentResolutionValue = (currentResolutionValue + 1) % 3; //���� �ػ� �� ����
        SetResolutionText(currentResolutionValue); //���� �ػ󵵿� ���� ResolutionText ����
    }

    /* ResolutionText�� �����ϴ� �Լ�  */
    private void SetResolutionText(int value)
    {
        switch (value)
        {
            case 0: //�����̸�
                resolutionTextTMP.text = "���ػ�";
                break;
            case 1: //�߿��̸�
                resolutionTextTMP.text = "���ػ�";
                break;
            case 2: //����̸�
                resolutionTextTMP.text = "���ػ�";
                break;
        }

        Scripts.mainCamera_Underwater.SetResolution(value); //�ػ� ����
    }

    /* ExitButton�� Ŭ������ �� ����Ǵ� �Լ� */
    public void OnClickExitButton()
    {
        Application.Quit();
    }
}