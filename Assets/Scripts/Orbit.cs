using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target;
    public float orbitSpeed;


    private void Awake()
    {


    }
    private void Update()
    {
        //transform.position = target.position + offset;
        // 중심 축을 기준으로 회전하는 함수 (중심 축, 방향, 속도)
        // RotateAround는 타겟의 위치가 변하면 회전이 일그러짐.
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);

      //  offset = transform.position - target.position;
    }
}
