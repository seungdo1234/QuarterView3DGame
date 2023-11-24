using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Ammo,  Coin, Grenade, Heart, Weapon}
public class Item : MonoBehaviour
{
    public ItemType itemType;
    public int value;

    private void Update()
    {
        transform.Rotate(Vector3.up * 30 * Time.deltaTime);
    }
}
