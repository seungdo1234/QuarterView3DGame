using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target; // 카메라가 따라다닐 타켓 트랜스폼 정보
    public Vector3 offset; // 카메라 초기 위치 저장


    private void Update()
    {
        transform.position = target.position + offset;
    }

}
