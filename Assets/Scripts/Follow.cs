using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target; // ī�޶� ����ٴ� Ÿ�� Ʈ������ ����
    public Vector3 offset; // ī�޶� �ʱ� ��ġ ����


    private void Update()
    {
        transform.position = target.position + offset;
    }

}
