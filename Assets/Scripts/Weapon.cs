using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Melee, Range }
public class Weapon : MonoBehaviour
{
    public WeaponType type;
    public int damage;
    public float rate;
    public int maxAmmo;
    public int curAmmo;

    public BoxCollider meleeArea; // °ø°Ý ¹üÀ§
    public TrailRenderer trailEffect; // °ø°Ý½Ã ÀÌÆåÆ®
    public Transform bulletPos; // ÃÑ¾Ë ¹ß»ç À§Ä¡
    public GameObject bullet; // ÃÑ¾Ë ÇÁ¸®ÆÕ ÀúÀå
    public Transform bulletCasePos; // ÅºÇÇ ¹èÃâ À§Ä¡
    public GameObject bulletCase; // ÅºÇÇ ÇÁ¸®ÆÕ ÀúÀå

    public void Use()
    {
        if(type == WeaponType.Melee)
        {
            StopCoroutine(Swing());
            StartCoroutine(Swing());
        }
        else if (type == WeaponType.Range && curAmmo > 0)
        {
            curAmmo--;
            StartCoroutine(Shot());
        }
    }
    private IEnumerator Shot()
    {
        // ÃÑ¾Ë ¹ß»ç
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;

        yield return null;
        // ÅºÇÇ ¹èÃâ
        GameObject intantBulletCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody bulletCaseRigid = intantBullet.GetComponent<Rigidbody>();

        // ÅºÇÇ°¡ ¶³¾îÁö´Â Èû
        Vector3 caseVec = bulletCasePos.forward *  Random.Range(-3,-2) + Vector3.up * Random.Range(2, 3);
        bulletCaseRigid.AddForce(caseVec, ForceMode.Impulse);
        bulletCaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse); // È¸Àü
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
