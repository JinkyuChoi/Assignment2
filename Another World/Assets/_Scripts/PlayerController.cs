using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class PlayerController : MonoBehaviour
{

    public PlayerAnimState playerAnimState;

    [Header("Properties")]
    public Animator playerAnimator;
    public SpriteRenderer playerSpriteRenderer;
    public Rigidbody2D playerRigidBody;

    [Header("Movement")]
    public float moveForce;
    public float jumpForce;

    public bool isGrounded;
    public Transform groundTarget;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimState = PlayerAnimState.IDLE;
        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.Linecast(
            transform.position,
            groundTarget.position,
            1 << LayerMask.NameToLayer("Ground"));


        //Stop
        if (Input.GetAxis("Horizontal") == 0)
        {
            playerAnimState = PlayerAnimState.IDLE;
            playerAnimator.SetInteger("AnimState", (int)PlayerAnimState.IDLE);
        }

        //Right
        if (Input.GetAxis("Horizontal") > 0)
        {
            playerSpriteRenderer.flipX = false;

            playerAnimState = PlayerAnimState.WALK;
            playerAnimator.SetInteger("AnimState", (int)PlayerAnimState.WALK);
            playerRigidBody.AddForce(Vector2.right * moveForce);

        }

        //Left
        if (Input.GetAxis("Horizontal") < 0)
        {
            playerSpriteRenderer.flipX = true;
            playerAnimState = PlayerAnimState.WALK;
            playerAnimator.SetInteger("AnimState", (int)PlayerAnimState.WALK);
            playerRigidBody.AddForce(Vector2.left * moveForce);

        }

        //Jump)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerAnimState = PlayerAnimState.JUMP;
            playerAnimator.SetInteger("AnimState", (int)PlayerAnimState.JUMP);
            playerRigidBody.AddForce(Vector2.up * jumpForce);
            isGrounded = false;
        }

        //Not Jump
        //if (Input.GetAxis("Jump") == 0)
        //{
        //    playerAnimState = PlayerAnimState.IDLE;
        //}
    }
}
