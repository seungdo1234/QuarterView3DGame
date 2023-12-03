using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) // ź�� �浹
        {
            Destroy(gameObject, 3);
        }
        else if (collision.gameObject.CompareTag("Wall")) // �Ѿ� �浹
        {
            Destroy(gameObject);
        }
    }
}
