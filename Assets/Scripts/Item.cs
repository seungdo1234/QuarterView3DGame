using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Ammo,  Coin, Grenade, Heart, Weapon}
public class Item : MonoBehaviour
{
    public ItemType itemType;
    public int value;

    private Rigidbody rigid;
    private SphereCollider sc;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();
    }
    private void Update()
    {
        transform.Rotate(Vector3.up * 30 * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            rigid.isKinematic = true;
            sc.enabled = false;
        }
    }
}
