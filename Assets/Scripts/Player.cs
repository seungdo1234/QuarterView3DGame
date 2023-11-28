using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("# Stat")]
    public float runSpeed;
    public float walkSpeed;
    public float jumpPower;

    [Header("# Weapon")]
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;
    public int hasGrenades;

    [Header("# Consumable Item")]
    public int ammo;
    public int coin;
    public int health;

    [Header("# Max Consumable Item ")]
    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenades;

    private float hAxis;
    private float vAxis;
    private bool wDown;
    private bool jDown;
    private bool iDown;
    private bool fDown;
    private bool sDown1;
    private bool sDown2;
    private bool sDown3;

    private bool isJump;
    private bool isDodge;
    private bool isSwap;
    private bool isFireReady = true;

    private Vector3 moveDir;
    private Vector3 dodgeVec;

    private Animator anim;
    private Rigidbody rigid;

    private GameObject nearObject;
    private Weapon equipWeapon;
    private int equipWeaponIndex = -1;
    private float fireDelay;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput(); 
        Move();
        Turn();
        Jump();
        Attack();
        Dodge();
        Swap();
        Interaction();
    }

    private void Turn()
    {
        // 지정된 벡터를 향해서 회전시켜주는 함수
        transform.LookAt(transform.position + moveDir);
    }

    private void Move()
    {
        moveDir = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge)
        {
            moveDir = dodgeVec;
        }

        if (isSwap || !isFireReady)
        {
            moveDir = Vector3.zero;
        }

        Vector3 move = moveDir * Time.deltaTime;

        transform.position += wDown ? move * walkSpeed : move * runSpeed;

        anim.SetBool("isRun", moveDir != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    private void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButtonDown("Fire1");
        iDown = Input.GetButtonDown("Interaction");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    private void Jump()
    {
        if (!isJump && moveDir == Vector3.zero && !isDodge && jDown && !isSwap) // 움직이지 않을 때 점프
        {
            isJump = true;
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
        }
    }
    private void Attack()
    {
        if(equipWeapon == null)
        {
            return;
        }

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if(fDown && isFireReady && !isDodge && !isSwap)
        {
            equipWeapon.Use();
            anim.SetTrigger("doSwing");
            fireDelay = 0;
        }
    }
    private void Dodge()
    {
        if (!isJump && moveDir != Vector3.zero && !isDodge && jDown && !isSwap) // 움직이고 있다면 구르기
        {
            dodgeVec = moveDir;

            runSpeed *= 2;
            isDodge = true;
            anim.SetTrigger("doDodge");

            Invoke("DodgeOut", 0.8f);
        }
    }

    private void DodgeOut()
    {
        runSpeed *= 0.5f;
        isDodge = false;
    }
    private void Swap()
    {
        if (isSwap)
        {
            return;
        }

        if(sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
        {
            return;
        }
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
        {
            return;
        }
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
        {
            return;
        }

        int weaponIndex = -1;

        if (sDown1)
        {
            weaponIndex = 0;
        }
        else if (sDown2)
        {
            weaponIndex = 1;
        }
        else if (sDown3)
        {
            weaponIndex = 2;
        }

        if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge )
        {
            if(equipWeapon != null) // 장착한 무기가 있거나, 같은 장비 스왑일때는
            {
                equipWeapon.gameObject.SetActive(false);
            }

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            weapons[weaponIndex].gameObject.SetActive(true);

            anim.SetTrigger("doSwap");

            isSwap = true;

            Invoke("SwapOut", 0.5f);
        }
    }
    private void SwapOut()
    {
        isSwap = false;
    }
    private void Interaction()
    {
        if(iDown && nearObject != null && !isJump && !isDodge) // 아이템 상호작용
        {
            if (nearObject.CompareTag("Weapon"))
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;

                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJump = false;
            anim.SetBool("isJump", false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            nearObject = other.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.itemType)
            {
                case ItemType.Ammo:
                    ammo += item.value;

                    if(ammo > maxAmmo)
                    {
                        ammo = maxAmmo;
                    }
                    break;
                case ItemType.Coin:
                    coin += item.value;
                    if (coin > maxCoin)
                    {
                        coin = maxAmmo;
                    }
                    break;
                case ItemType.Heart:
                    health += item.value;
                    if (health > maxHealth)
                    {
                        health = maxHealth;
                    }
                    break;
                case ItemType.Grenade:
                    if(hasGrenades == maxHasGrenades)
                    {
                        return;
                    }
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    break;
            }
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            nearObject = null;
        }
    }
}
