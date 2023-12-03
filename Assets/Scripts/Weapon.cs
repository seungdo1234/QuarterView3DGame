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

    public BoxCollider meleeArea; // ���� ����
    public TrailRenderer trailEffect; // ���ݽ� ����Ʈ
    public Transform bulletPos; // �Ѿ� �߻� ��ġ
    public GameObject bullet; // �Ѿ� ������ ����
    public Transform bulletCasePos; // ź�� ���� ��ġ
    public GameObject bulletCase; // ź�� ������ ����

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
        // �Ѿ� �߻�
        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;

        yield return null;
        // ź�� ����
        GameObject intantBulletCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody bulletCaseRigid = intantBullet.GetComponent<Rigidbody>();

        // ź�ǰ� �������� ��
        Vector3 caseVec = bulletCasePos.forward *  Random.Range(-3,-2) + Vector3.up * Random.Range(2, 3);
        bulletCaseRigid.AddForce(caseVec, ForceMode.Impulse);
        bulletCaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse); // ȸ��
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
