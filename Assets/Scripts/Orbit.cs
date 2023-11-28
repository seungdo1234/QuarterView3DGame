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
        // �߽� ���� �������� ȸ���ϴ� �Լ� (�߽� ��, ����, �ӵ�)
        // RotateAround�� Ÿ���� ��ġ�� ���ϸ� ȸ���� �ϱ׷���.
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);

      //  offset = transform.position - target.position;
    }
}
