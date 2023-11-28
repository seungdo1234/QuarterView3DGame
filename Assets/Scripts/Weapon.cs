using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Melee, Range }
public class Weapon : MonoBehaviour
{
    public WeaponType type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea; // 공격 범위
    public TrailRenderer trailEffect; // 공격시 이펙트

    public void Use()
    {
        if(type == WeaponType.Melee)
        {
            StopCoroutine(Swing());
            StartCoroutine(Swing());
        }
    }

    private IEnumerator Swing()
    {

        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }
}
