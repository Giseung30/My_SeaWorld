using UnityEngine;
using System.Collections;

public class AudioController_Underwater : MonoBehaviour
{
    [Header("Variable")]
    public float underwaterStartPosY; //���� ������ ���۵Ǵ� Y�� ��ġ

    [Header("Audio")]
    public AudioSource audioSource2D; //AudioSource 2D�� AudioSource ����
    public AudioClip enterTheWaterAudioClip; //�������� ���� �Ҹ�
    public AudioClip getOutWaterAudioClip; //�ٴٷ� ������ �Ҹ�
    public AudioSource seaTrack1AudioSource; //Sea Track1�� AudioSource ����
    public AudioSource seaTrack2AudioSource; //Sea Track2�� AudioSource ����
    public AudioSource underwaterTrack1AudioSource; //Underwater Track1�� AudioSource ����
    public AudioSource underwaterTrack2AudioSource; //Underwater Track2�� AudioSource ����
    public AudioSource seagullsAudioSource; //Seagulls�� AudioSource ����

    [Header("Coroutine")]
    public Coroutine playSeaAudioCoroutine; //�ٴ� �Ҹ� ����ϴ� �ڷ�ƾ
    public Coroutine playUnderwaterAudioCoroutine; //���� �Ҹ� ����ϴ� �ڷ�ƾ

    private void Start()
    {
        StartCoroutine(ControlSeaAndUnderwaterAudio());
    }

    /* ���� �Ҹ��� �ٴ� �Ҹ��� �����ϴ� �ڷ�ƾ �Լ� */
    private IEnumerator ControlSeaAndUnderwaterAudio()
    {
        while (true)
        {
            if (Scripts.mainCamera_Underwater.mainCameraTransform.position.y > underwaterStartPosY && playSeaAudioCoroutine == null) //�ٴ� �����̸鼭 �ڷ�ƾ�� ���� ������ ������
            {
                if (playUnderwaterAudioCoroutine != null) //���� �Ҹ� �ڷ�ƾ�� �����ϸ�
                {
                    StopCoroutine(playUnderwaterAudioCoroutine); //���� �Ҹ� �ڷ�ƾ ����
                    playUnderwaterAudioCoroutine = null;
                }
                
                playSeaAudioCoroutine = StartCoroutine(PlaySeaAudio()); //�ٴ� �Ҹ� �ڷ�ƾ ����
            }

            else if (Scripts.mainCamera_Underwater.mainCameraTransform.position.y <= underwaterStartPosY && playUnderwaterAudioCoroutine == null) //���� �����̸鼭 �ڷ�ƾ�� ���� ������ ������
            {
                if (playSeaAudioCoroutine != null) //�ٴ� �Ҹ� �ڷ�ƾ�� �����ϸ�
                {
                    StopCoroutine(playSeaAudioCoroutine); //�ٴ� �Ҹ� �ڷ�ƾ ����
                    playSeaAudioCoroutine = null;
                }

                playUnderwaterAudioCoroutine = StartCoroutine(PlayUnderwaterAudio()); //���� �Ҹ� �ڷ�ƾ ����
            }

            yield return null;
        }
    }

    /* �ٴ� �Ҹ��� ����ϴ� �ڷ�ƾ �Լ� */
    private IEnumerator PlaySeaAudio()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(seaTrack1AudioSource.clip.length * 0.8f); //�ٴ� �Ҹ� ������ ������ ���

        underwaterTrack1AudioSource.Stop(); //���� �Ҹ� ����
        underwaterTrack2AudioSource.Stop();

        seagullsAudioSource.volume = 1f; //���ű� �Ҹ� �鸮���� ����

        audioSource2D.PlayOneShot(getOutWaterAudioClip); //�ٴٷ� ������ �Ҹ� ���

        while (true)
        {
            seaTrack1AudioSource.Play(); //Sea Track1 ���

            yield return waitForSeconds; //�ٴ� �Ҹ� ���� ������ ���

            seaTrack2AudioSource.Play(); //Sea Track2 ���

            yield return waitForSeconds; //�ٴ� �Ҹ� ���� ������ ���
        }
    }

    /* ���� �Ҹ��� ����ϴ� �ڷ�ƾ �Լ� */
    private IEnumerator PlayUnderwaterAudio()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(underwaterTrack1AudioSource.clip.length * 0.8f); //���� �Ҹ� ������ ������ ���

        seaTrack1AudioSource.Stop(); //�ٴ� �Ҹ� ����
        seaTrack2AudioSource.Stop();

        seagullsAudioSource.volume = 0.01f; //���ű� �Ҹ� �� �ȵ鸮���� ����

        audioSource2D.PlayOneShot(enterTheWaterAudioClip); //�������� ���� �Ҹ� ���

        while (true)
        {
            underwaterTrack1AudioSource.Play(); //Underwater Track1 ���

            yield return waitForSeconds; //���� �Ҹ� ���� ������ ���

            underwaterTrack2AudioSource.Play(); //Underwater Track2 ���

            yield return waitForSeconds; //���� �Ҹ� ���� ������ ���
        }
    }
}