using UnityEngine;
using TMPro;

public class OptionPanel : MonoBehaviour
{
    [Header("Component")]
    public GameObject optionPanel; //OptionPanel

    [Header("Resolution")]
    public TextMeshProUGUI resolutionTextTMP; //ResolutionText의 TMP 정보
    private int currentResolutionValue; //현재 해상도 값

    private void Start()
    {
        currentResolutionValue = 1;

        SetResolutionText(currentResolutionValue);
    }

    /* OptionPanel을 활성화하는 함수 */
    public void ActiveOptionPanel(bool state)
    {
        optionPanel.SetActive(state); //OptionPanel 활성화/비활성화
    }

    /* ResolutionButton을 클릭했을 때 실행되는 함수 */
    public void OnClickResolutionButton()
    {
        currentResolutionValue = (currentResolutionValue + 1) % 3; //현재 해상도 값 증가
        SetResolutionText(currentResolutionValue); //현재 해상도에 따라 ResolutionText 변경
    }

    /* ResolutionText를 설정하는 함수  */
    private void SetResolutionText(int value)
    {
        switch (value)
        {
            case 0: //저옵이면
                resolutionTextTMP.text = "저해상도";
                break;
            case 1: //중옵이면
                resolutionTextTMP.text = "중해상도";
                break;
            case 2: //고옵이면
                resolutionTextTMP.text = "고해상도";
                break;
        }

        Scripts.mainCamera_Underwater.SetResolution(value); //해상도 설정
    }

    /* ExitButton을 클릭했을 때 실행되는 함수 */
    public void OnClickExitButton()
    {
        Application.Quit();
    }
}