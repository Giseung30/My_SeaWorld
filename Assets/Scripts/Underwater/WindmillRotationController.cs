using UnityEngine;
using System.Collections;

public class WindmillRotationController : MonoBehaviour
{
    [Header("Component")]
    public Transform fanTransform; //Fan의 Transform 정보

    [Header("Variable")]
    public float rotateSpeed; //회전 속도

    private void Start()
    {
        StartCoroutine(RotateWindmill());
    }

    /* 풍차를 회전시키는 코루틴 함수 */
    private IEnumerator RotateWindmill()
    {
        while (true)
        {
            fanTransform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime); //풍차 회전

            yield return null;
        }
    }
}