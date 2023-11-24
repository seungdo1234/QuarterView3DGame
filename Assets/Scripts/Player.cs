using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float runSpeed;
    public float walkSpeed;
    public float jumpPower;

    private float hAxis;
    private float vAxis;
    private bool wDown;
    private bool jDown;

    private bool isJump;
    private bool isDodge;

    private Vector3 moveDir;
    private Vector3 dodgeVec;
    private Animator anim;

    private Rigidbody rigid;
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
        Dodge();
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

        Vector3 move = moveDir * Time.deltaTime;

        transform.position += wDown ? move * walkSpeed : move * runSpeed;

        anim.SetBool("isRun", moveDir != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    private void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetKey(KeyCode.LeftShift);
        jDown = Input.GetKeyDown(KeyCode.Space);
    }

    private void Jump()
    {
        if (!isJump && moveDir == Vector3.zero && !isDodge && jDown) // 움직이지 않을 때 점프
        {
            isJump = true;
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
        }
    }
    private void Dodge()
    {
        if (!isJump && moveDir != Vector3.zero && !isDodge && jDown) // 움직이고 있다면 구르기
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJump = false;
            anim.SetBool("isJump", false);
        }
    }
}
