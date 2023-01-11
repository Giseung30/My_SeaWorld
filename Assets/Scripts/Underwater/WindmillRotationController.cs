using UnityEngine;
using System.Collections;

public class WindmillRotationController : MonoBehaviour
{
    [Header("Component")]
    public Transform fanTransform; //Fan�� Transform ����

    [Header("Variable")]
    public float rotateSpeed; //ȸ�� �ӵ�

    private void Start()
    {
        StartCoroutine(RotateWindmill());
    }

    /* ǳ���� ȸ����Ű�� �ڷ�ƾ �Լ� */
    private IEnumerator RotateWindmill()
    {
        while (true)
        {
            fanTransform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime); //ǳ�� ȸ��

            yield return null;
        }
    }
}