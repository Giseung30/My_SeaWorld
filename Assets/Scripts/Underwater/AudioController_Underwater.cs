using UnityEngine;
using System.Collections;

public class AudioController_Underwater : MonoBehaviour
{
    [Header("Variable")]
    public float underwaterStartPosY; //수중 구역이 시작되는 Y축 위치

    [Header("Audio")]
    public AudioSource audioSource2D; //AudioSource 2D의 AudioSource 정보
    public AudioClip enterTheWaterAudioClip; //수중으로 들어가는 소리
    public AudioClip getOutWaterAudioClip; //바다로 나오는 소리
    public AudioSource seaTrack1AudioSource; //Sea Track1의 AudioSource 정보
    public AudioSource seaTrack2AudioSource; //Sea Track2의 AudioSource 정보
    public AudioSource underwaterTrack1AudioSource; //Underwater Track1의 AudioSource 정보
    public AudioSource underwaterTrack2AudioSource; //Underwater Track2의 AudioSource 정보
    public AudioSource seagullsAudioSource; //Seagulls의 AudioSource 정보

    [Header("Coroutine")]
    public Coroutine playSeaAudioCoroutine; //바다 소리 재생하는 코루틴
    public Coroutine playUnderwaterAudioCoroutine; //수중 소리 재생하는 코루틴

    private void Start()
    {
        StartCoroutine(ControlSeaAndUnderwaterAudio());
    }

    /* 수중 소리와 바다 소리를 관리하는 코루틴 함수 */
    private IEnumerator ControlSeaAndUnderwaterAudio()
    {
        while (true)
        {
            if (Scripts.mainCamera_Underwater.mainCameraTransform.position.y > underwaterStartPosY && playSeaAudioCoroutine == null) //바다 높이이면서 코루틴이 실행 중이지 않으면
            {
                if (playUnderwaterAudioCoroutine != null) //수중 소리 코루틴이 존재하면
                {
                    StopCoroutine(playUnderwaterAudioCoroutine); //수중 소리 코루틴 정지
                    playUnderwaterAudioCoroutine = null;
                }
                
                playSeaAudioCoroutine = StartCoroutine(PlaySeaAudio()); //바다 소리 코루틴 실행
            }

            else if (Scripts.mainCamera_Underwater.mainCameraTransform.position.y <= underwaterStartPosY && playUnderwaterAudioCoroutine == null) //수중 높이이면서 코루틴이 실행 중이지 않으면
            {
                if (playSeaAudioCoroutine != null) //바다 소리 코루틴이 존재하면
                {
                    StopCoroutine(playSeaAudioCoroutine); //바다 소리 코루틴 정지
                    playSeaAudioCoroutine = null;
                }

                playUnderwaterAudioCoroutine = StartCoroutine(PlayUnderwaterAudio()); //수중 소리 코루틴 실행
            }

            yield return null;
        }
    }

    /* 바다 소리를 재생하는 코루틴 함수 */
    private IEnumerator PlaySeaAudio()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(seaTrack1AudioSource.clip.length * 0.8f); //바다 소리 끝나기 전까지 대기

        underwaterTrack1AudioSource.Stop(); //수중 소리 정지
        underwaterTrack2AudioSource.Stop();

        seagullsAudioSource.volume = 1f; //갈매기 소리 들리도록 설정

        audioSource2D.PlayOneShot(getOutWaterAudioClip); //바다로 나오는 소리 재생

        while (true)
        {
            seaTrack1AudioSource.Play(); //Sea Track1 재생

            yield return waitForSeconds; //바다 소리 끝날 때까지 대기

            seaTrack2AudioSource.Play(); //Sea Track2 재생

            yield return waitForSeconds; //바다 소리 끝날 때까지 대기
        }
    }

    /* 수중 소리를 재생하는 코루틴 함수 */
    private IEnumerator PlayUnderwaterAudio()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(underwaterTrack1AudioSource.clip.length * 0.8f); //수중 소리 끝나기 전까지 대기

        seaTrack1AudioSource.Stop(); //바다 소리 정지
        seaTrack2AudioSource.Stop();

        seagullsAudioSource.volume = 0.01f; //갈매기 소리 잘 안들리도록 설정

        audioSource2D.PlayOneShot(enterTheWaterAudioClip); //수중으로 들어가는 소리 재생

        while (true)
        {
            underwaterTrack1AudioSource.Play(); //Underwater Track1 재생

            yield return waitForSeconds; //수중 소리 끝날 때까지 대기

            underwaterTrack2AudioSource.Play(); //Underwater Track2 재생

            yield return waitForSeconds; //수중 소리 끝날 때까지 대기
        }
    }
}