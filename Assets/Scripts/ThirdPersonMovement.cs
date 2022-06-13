using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController _heroController;
    public Transform cam;
    public float heroSpeed = 200f;
    public float sprintFactor = 2f;

    [SerializeField]
    GameObject heroGraphics;

    Animator anim;

    float inputHorizontal;
    float inputVertical;
    float targetAngle;
    bool facingRight = true;
    Vector3 direction;

    void Start()
    {
        anim = heroGraphics.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
            return;
        }

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        direction = new Vector3(inputHorizontal, 0f, inputVertical);

        HeroMove();
        FaceDirection();
        UpdateAnimation();
    }

    void HeroMove()
    {
        if (direction.magnitude >= 0.1f)
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                sprintFactor = 2f;
            }
            else
            {
                sprintFactor = 1f;
            }

            _heroController.SimpleMove(
                moveDir.normalized * heroSpeed * sprintFactor * Time.deltaTime
            );
        }
        else
        {
            _heroController.SimpleMove(Vector3.zero);
        }
    }

    void FaceDirection()
    {
        if (inputHorizontal < 0 && facingRight)
        {
            Flip();
        }
        if (inputHorizontal >= 0 && !facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        Vector3 currentScale = heroGraphics.transform.localScale;
        currentScale.x *= -1;
        heroGraphics.transform.localScale = currentScale;
        facingRight = !facingRight;
    }

    void Fire()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isSprinting", false);
        anim.SetTrigger("shoot");
    }

    void UpdateAnimation()
    {
        if ((inputHorizontal != 0f || inputVertical != 0f))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("isSprinting", true);
            }
            else if (!anim.GetCurrentAnimatorStateInfo(0).IsName("HeroShootAnimation"))
            {
                anim.SetBool("isWalking", true);
                anim.SetBool("isSprinting", false);
            }
        }
        else
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isSprinting", false);
        }
    }
}
